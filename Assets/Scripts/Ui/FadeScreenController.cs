using static UEventHandler;

using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using NaughtyAttributes;
using System.Threading.Tasks;

[RequireComponent(typeof(Canvas))]
public class FadeScreenController : MonoBehaviour
{
    public static FadeScreenController instance;

    public enum FadeType
    {
        Opacity,
        Grow,
        SlideVertical,
        SlideHorizontal,
    }

    public bool fadeOnAwake = true;

    [ShowIf("fadeOnAwake")]
    public int fadeAwakeDelay = 200;

    [Header("General Parameters")]
    public FadeType fadeType;
    public float fadeInDuration;
    public float fadeOutDuration;
    public Ease fadeInEase;
    public Ease fadeOutEase;

    [Header("Grow Parameters")]
    [ShowIf("IsGrow")]
    public float growInitScale = 0.01f;
    [ShowIf("IsGrow")]
    public float growFinalScale = 10.9f;

    [ShowIf("IsSlide")]
    public float slideOffset = 150f;

    public UEvent OnFadeIn = new UEvent();
    public UEvent OnFadeOut = new UEvent();

    private Image fadeShapeRenderer;


    UEventHandler eventHandler = new UEventHandler();



    private void OnDestroy()
    {
        eventHandler.UnsubcribeAll();
    }

    private void Awake()
    {
        instance = this;
    }


    private async void Start()
    {
        fadeShapeRenderer = transform.Find("Fade Shape").GetComponent<Image>();
        fadeShapeRenderer.gameObject.SetActive(true);

        OnFadeIn.Subscribe(eventHandler, FadeIn);
        OnFadeOut.Subscribe(eventHandler, FadeOut);

        switch (fadeType)
        {
            case FadeType.Opacity:
                fadeShapeRenderer.sprite = null;
                fadeShapeRenderer.GetComponent<Mask>().enabled = false;
                fadeShapeRenderer.rectTransform.localScale = Vector3.one * growFinalScale;
                break;
            case FadeType.Grow:
                fadeShapeRenderer.color = Color.black;
                fadeShapeRenderer.GetComponent<Mask>().enabled = true;
                break;
            default:
                fadeShapeRenderer.color = Color.black;
                fadeShapeRenderer.GetComponent<Mask>().enabled = false;
                break;

        }

        if (!fadeOnAwake) return;

        await Task.Delay(fadeAwakeDelay);
        FadeIn();
    }

    //[ContextMenu("Disable In Editor")]
    //[Button("Disable In Editor", enabledMode: EButtonEnableMode.Editor)]
    //public void DisableInEditor()
    //{
    //    var fadeShape = transform.Find("Fade Shape").gameObject;
    //    fadeShape.SetActive(false);
    //    //EditorSceneManager.SaveOpenScenes();
    //    EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
    //    PrefabUtility.RecordPrefabInstancePropertyModifications(transform);
    //    PrefabUtility.RecordPrefabInstancePropertyModifications(fadeShape);
    //}

    [ContextMenu("Fade In")]
    [Button("Fade In")]
    public void FadeIn()
    {
        switch (fadeType)
        {
            case FadeType.Opacity:
                fadeShapeRenderer.color = Color.black;
                fadeShapeRenderer.DOFade(0, fadeInDuration).SetEase(fadeInEase);
                break;
            case FadeType.Grow:
                fadeShapeRenderer.rectTransform.localScale = Vector3.one * growInitScale;
                fadeShapeRenderer.rectTransform.DOScale(Vector3.one * growFinalScale, fadeInDuration).SetEase(fadeInEase);
                break;
            default:
                fadeShapeRenderer.rectTransform.localPosition = Vector3.zero;
                Vector2 slide;
                if (fadeType == FadeType.SlideHorizontal)
                    slide = new Vector2(slideOffset, 0);
                else
                    slide = new Vector2(0, slideOffset);

                fadeShapeRenderer.rectTransform.DOLocalMove(slide, fadeInDuration).SetEase(fadeInEase);
                break;
        }
    }


    [ContextMenu("Fade Out")]
    [Button("Fade Out")]
    public void FadeOut()
    {
        switch (fadeType)
        {
            case FadeType.Opacity:
                fadeShapeRenderer.color = new Color(0, 0, 0, 0);
                fadeShapeRenderer.DOFade(1, fadeOutDuration).SetEase(fadeOutEase);
                break;
            case FadeType.Grow:
                fadeShapeRenderer.rectTransform.localScale = Vector3.one * growFinalScale;
                fadeShapeRenderer.rectTransform.DOScale(Vector3.one * growInitScale, fadeOutDuration).SetEase(fadeOutEase);
                break;
            default:
                fadeShapeRenderer.rectTransform.localPosition = fadeType == FadeType.SlideHorizontal ? new Vector2(slideOffset, 0) : new Vector2(0, slideOffset);
                fadeShapeRenderer.rectTransform.DOLocalMove(Vector2.zero, fadeOutDuration).SetEase(fadeOutEase);
                break;
        }
    }


    public bool IsGrow() { return fadeType == FadeType.Grow; }
    public bool IsSlide() { return fadeType == FadeType.SlideVertical || fadeType == FadeType.SlideHorizontal; }

}
