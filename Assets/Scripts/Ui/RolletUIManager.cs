using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Threading.Tasks;

public class RolletUIManager : MonoBehaviour
{
    public AudioSource source;
    public SoundUtils.Sound reloadSound;
    public static RolletUIManager instance;
    public RectTransform rollet;

    public Image[] bullets;

    public int numberUnits = 13;

    public int startingIndex = 0;
    private int currentIndex;


    [Header("Reload")]
    public float singleRotation;
    public float singleDuration = 0.2f;
    public AnimationCurve singleEase;

    [Header("Reload")]
    public float reloadDuration = 0.2f;
    public AnimationCurve reloadEase;
    public float singleBulletReloadOffset = 0.1f;
    public Vector2 rotationRandomTimes = new Vector2(3, 4);

    private Tween roationTween;
    private Tween reloadTween;

    public Color initColor;
    private void Awake()
    {
        instance = this;

        singleRotation = 360f / numberUnits;
        initColor = bullets[startingIndex].color;

        for (int i = 0; i < bullets.Length; i++)
        {
            bullets[i].color = Color.gray;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        currentIndex = startingIndex;


        Reload();

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetRandomTags()
    {
        var random = Random.Range(0, bullets.Length);

        for (int i = 0; i < bullets.Length; i++)
        {
            bullets[i].gameObject.tag = i == random ? "ExplosiveBullet" : "Untagged";
        }
    }

    public bool TestShoot()
    {
        if (reloadTween != null && reloadTween.IsActive())
            return false;


        return true;
    }
    [Button]
    public void Next()
    {
        if (currentIndex >= bullets.Length)
            return;
        if (reloadTween != null && reloadTween.IsActive())
            return;

        if (roationTween != null && roationTween.IsActive())
            roationTween.Kill();

        roationTween = rollet.DORotate(Vector3.forward * singleRotation * (currentIndex + 1), singleDuration).SetEase(singleEase).OnComplete(() =>
        {
            if (currentIndex + 1 > numberUnits)
                Reload();
        });


        UseBullet();
    }

    [Button]
    public async void Reload()
    {

        SetRandomTags();

        if (reloadTween != null && reloadTween.IsActive())
            reloadTween.Kill();

        var times = Random.Range(rotationRandomTimes.x, rotationRandomTimes.y);
        var rotation = 360 * times;

        rotation *= singleRotation;
        rotation = Mathf.Round(rotation);
        rotation /= singleRotation;

        reloadTween = rollet.DORotate(Vector3.forward * rotation, reloadDuration * times, RotateMode.LocalAxisAdd).SetEase(reloadEase);

        currentIndex = 0;

        source.PlaySound(reloadSound);
        for (int i = 0; i < bullets.Length; i++)
        {
            bullets[i].color = initColor;
            await Task.Delay(System.TimeSpan.FromSeconds(singleBulletReloadOffset));
        }
    }

    void UseBullet()
    {
        bullets[currentIndex].color = Color.gray;

        if (bullets[currentIndex].tag == "ExplosiveBullet")
        {
            PlayerShootController.instance.BlowUp();
        }

        currentIndex++;


    }
}
