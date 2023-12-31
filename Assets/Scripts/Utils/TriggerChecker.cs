using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UEventHandler;

[RequireComponent(typeof(Collider))]
public class TriggerChecker : MonoBehaviour
{
    public bool searchInRigidbody;
    public LayerMask layersToCheck;
    public string tagToSearch;

    public bool hasObject;
    public GameObject obj { get; private set; }
    public Rigidbody objRb { get; private set; }

    public UEvent<Transform> OnTriggered = new UEvent<Transform>();

    public static bool DoesMaskContainsLayer(LayerMask layermask, int layer)
    {
        return layermask == (layermask | (1 << layer));
    }

    private void Awake()
    {
        hasObject = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!IsObject(other)) return;

        obj = searchInRigidbody ? other.attachedRigidbody.gameObject : other.gameObject;
        objRb = other.attachedRigidbody;
        hasObject = true;

        OnTriggered.TryInvoke(other.transform);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!IsObject(other)) return;

        obj = null;
        objRb = null;
        hasObject = false;
    }

    private bool IsObject(Collider other)
    {
        if (searchInRigidbody)
        {
            if (!DoesMaskContainsLayer(layersToCheck, other.attachedRigidbody.gameObject.layer)) return false;
            if (!string.IsNullOrEmpty(tagToSearch) && other.attachedRigidbody.gameObject.tag == tagToSearch) return false;
        }
        else
        {
            if (!DoesMaskContainsLayer(layersToCheck, other.gameObject.layer)) return false;
            if (!string.IsNullOrEmpty(tagToSearch) && other.gameObject.tag == tagToSearch) return false;
        }

        return true;
    }



}
