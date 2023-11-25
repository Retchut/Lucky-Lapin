using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public static class UiUtils
{

    public static void Show(this CanvasGroup group)
    {
        group.interactable = true;
        group.enabled = true;
        group.blocksRaycasts = true;
        group.DOFade(1, .1f).SetEase(Ease.OutQuad);
    }

    public static void Hide(this CanvasGroup group)
    {
        group.interactable = false;
        //group.enabled = false;
        group.blocksRaycasts = false;
        group.DOFade(0, .1f).SetEase(Ease.InQuad);
    }
}
