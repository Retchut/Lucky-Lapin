using NaughtyAttributes;
using UnityEngine;
using static UEventHandler;

[RequireComponent(typeof(Collider))]
public class TriggerChecker : MonoBehaviour
{
    [InfoBox("Do not forget that either this/other object needs a Rigidbody to TriggerCheck!", EInfoBoxType.Normal)]

    public bool isChecking = true;
    public bool searchInRigidbody;
    public LayerMask layersToCheck;
    public string tagToSearch;

    public bool hasObject { get; private set; }
    public GameObject obj { get; private set; }
    public Rigidbody objRb { get; private set; }

    public UEvent<Transform> OnTriggered = new UEvent<Transform>();


    private void Awake()
    {
        hasObject = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isChecking) return;
        if (!IsObject(other)) return;

        obj = searchInRigidbody ? other.attachedRigidbody.gameObject : other.gameObject;
        objRb = other.attachedRigidbody;
        hasObject = true;

        OnTriggered.TryInvoke(obj.transform);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!isChecking) return;
        if (!IsObject(other)) return;

        obj = null;
        objRb = null;
        hasObject = false;
    }

    private bool IsObject(Collider other)
    {
        if (searchInRigidbody)
        {
            if (!layersToCheck.DoesMaskContainsLayer(other.attachedRigidbody.gameObject.layer)) return false;
            if (!string.IsNullOrEmpty(tagToSearch) && LayerMask.LayerToName(other.attachedRigidbody.gameObject.layer) == tagToSearch) return false;
        }
        else
        {
            if (!layersToCheck.DoesMaskContainsLayer(other.gameObject.layer)) return false;
            if (!string.IsNullOrEmpty(tagToSearch) && LayerMask.LayerToName(other.gameObject.layer) == tagToSearch) return false;
        }

        return true;
    }



}
