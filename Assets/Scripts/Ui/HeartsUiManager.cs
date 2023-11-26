using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using NaughtyAttributes;

public class HeartsUiManager : MonoBehaviour
{
    public static HeartsUiManager instance;
    public Image[] hearts;

    public float offSetMult = 0.1f;
    public Ease singleEase;
    public float singleRaise = 0.1f;
    public float singleDuration = 0.8f;
    public int vibrato = 10;
    public float elast = 1;

    public Sprite fullImage;
    public Sprite ghostImage;

    public int health;
    // Start is called before the first frame update

    private void Awake()
    {
        instance = this;
        health = hearts.Length;

    }
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {

    }

    [Button]
    public void PutToSoro()
    {

        if (health == 1)
        {
            TakeHit();
            return;
        }
        health = 1;

        SetHearts();
        ShakeAnimation();
    }

    [Button]
    public void TakeHit()
    {
        health--;

        //khealth = 0;
        SetHearts();
        ShakeAnimation();

        if (health == 0)
        {
            LevelManager.instance.GameOver();
        }
    }


    [Button]
    public void GainHeart()
    {
        health++;
        if (health > hearts.Length)
            health = hearts.Length;
        SetHearts();
        ShakeAnimation(false);
    }

    public void SetHearts()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].sprite = i >= health ? ghostImage : fullImage;
        }
    }
    public void ShakeAnimation(bool invert = true)
    {

        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].rectTransform.DOPunchPosition(Vector3.up * singleRaise, singleDuration, vibrato, elast).SetDelay((invert ? (hearts.Length - i) : i) * offSetMult);

        }
    }
}
