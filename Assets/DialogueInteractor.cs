using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueInteractor : MonoBehaviour
{
    [Multiline]
    public string[] dialogue;


    public TextMeshProUGUI hintText;
    public float popDuration = 1;
    public Ease popEase = Ease.InOutBack;

    Vector3 initScale = Vector3.one;
    public TriggerChecker triggerChecker;



    bool playerInZone;

    private void Awake()
    {
        initScale = hintText.rectTransform.localScale;
        hintText.rectTransform.localScale = Vector3.zero;
    }

    void Update()
    {
        if (playerInZone && Input.GetMouseButtonDown(0))
        {
            DialogueWriter.instance.ReadSentences(dialogue);
            HideHint();
            playerInZone = false;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player") return;

        //CameraManager.current.FocusCamera(transform);

        playerInZone = true;
        ShowHint();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag != "Player") return;

        //CameraManager.current.UnfocusCamera();

        playerInZone = false;
        HideHint();
    }

    void ShowHint()
    {
        if (hintText.rectTransform.localScale == initScale) return;

        hintText.rectTransform.DOScale(initScale, popDuration).SetEase(popEase);
    }

    void HideHint()
    {
        if (hintText.rectTransform.localScale == Vector3.zero) return;

        hintText.rectTransform.DOScale(Vector3.zero, popDuration).SetEase(popEase);
    }
}
