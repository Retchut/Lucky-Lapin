using UnityEngine;

public class FollowSimple : MonoBehaviour
{
    [Header("Refs")]
    public Transform followTarget;

    [Header("Mode")]
    public FollowMode followMode = FollowMode.LateUpdate;

    [Header("Position")]
    public bool followPosition = true;
    public bool offsetPosEnabled = true;
    public bool followLerp = false;
    public float followFactor = 1;

    [Header("Rotation")]
    public bool followRotation = false;
    public bool offsetRotEnabled = true;
    public bool rotationLerp = false;
    public float rotationFactor = 1;

    private float timeDif;

    private Quaternion offsetRot;
    public enum FollowMode
    {
        Update, LateUpdate, FixedUpdate
    }

    Vector3 offset;
    void Awake()
    {
        if (followTarget != null)
        {
            offset = offsetPosEnabled ? transform.position - followTarget.position : Vector3.zero;
            offsetRot = transform.rotation;
        }
    }

    private void LateUpdate()
    {
        if (followMode != FollowMode.LateUpdate || followTarget == null) return;
        TimeCalc();
        FollowPosition();
        FollowRotation();
    }


    public void Update()
    {
        if (followMode != FollowMode.Update || followTarget == null) return;
        TimeCalc();
        FollowPosition();
        FollowRotation();

    }

    public void FixedUpdate()
    {
        if (followMode != FollowMode.FixedUpdate || followTarget == null) return;
        TimeCalc();
        FollowPosition();
        FollowRotation();
    }

    public void SetFollowOffset(Vector3 newOffset)
    {
        offsetPosEnabled = true;
        offset = newOffset;
    }
    private void TimeCalc()
    {
        timeDif = followMode switch { FollowMode.FixedUpdate => Time.fixedDeltaTime, _ => Time.deltaTime };
    }


    private void FollowRotation()
    {
        if (!followRotation) return;

        var targetRotation = offsetRotEnabled ? followTarget.rotation * offsetRot : followTarget.rotation;
        if (rotationLerp)
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, timeDif * rotationFactor);
        else
            transform.rotation = targetRotation;
    }



    private void FollowPosition()
    {
        if (!followPosition) return;

        Vector3 dir = transform.position - followTarget.position;
        dir.Normalize();

        //Vector3 finalPos = offsetEnabled && followRotation ? followTarget.position + (followTarget.rotation * offset) : followTarget.position;
        Vector3 finalPos = followTarget.position;

        if (offsetPosEnabled)
            finalPos += offset;

        if (followLerp)
            transform.position = Vector3.Lerp(transform.position, finalPos, timeDif * followFactor);
        else
            transform.position = finalPos;
    }
}
