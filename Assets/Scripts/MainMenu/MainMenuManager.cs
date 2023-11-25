using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using System.Threading.Tasks;
using static SoundUtils;
using NaughtyAttributes;

public class MainMenuManager : MonoBehaviour
{
    [Header("Sounds")]
    public Sound clickSound;
    public Sound fadeInSound;
    public Sound fadeOutSound;

    [Header("References")]
    public CanvasGroup mainGroup;
    public CanvasGroup creditsGroup;
    public CanvasGroup settingsGroup;
    [Scene]
    public string gameScene;

    //int menuState = -1;
    CanvasGroup currentGroup;
    void Awake()
    {
        currentGroup = mainGroup;
    }

    private void Start()
    {
        CursorManager.instance.ShowCursor();
        SoundManager.instance.PlaySFX(fadeInSound);
    }

    public void GoToMain() => ChangeScreen(mainGroup);
    public void GoToSettings() => ChangeScreen(settingsGroup);
    public void GoToCredits() => ChangeScreen(creditsGroup);

    public async void ExitGame()
    {
        CursorManager.instance.HideCursor();
        SoundManager.instance.PlaySFX(fadeOutSound);
        SoundManager.instance.PlaySFX(clickSound);

        FadeScreenController.instance.FadeOut();

        await Task.Delay(800);
        Application.Quit();
    }

    public async void GoToGame()
    {
        CursorManager.instance.HideCursor();
        SoundManager.instance.PlaySFX(fadeOutSound);
        SoundManager.instance.PlaySFX(clickSound);
        FadeScreenController.instance.FadeOut();

        await Task.Delay(800);
        SceneManager.LoadScene(gameScene);
    }

    private async void ChangeScreen(CanvasGroup outGroup)
    {
        SoundManager.instance.PlaySFX(fadeOutSound);
        SoundManager.instance.PlaySFX(clickSound);
        FadeScreenController.instance.FadeOut();

        await Task.Delay(800);
        currentGroup.gameObject.SetActive(false);
        outGroup.gameObject.SetActive(true);
        currentGroup = outGroup;

        SoundManager.instance.PlaySFX(fadeInSound);
        FadeScreenController.instance.FadeIn();
    }


}
