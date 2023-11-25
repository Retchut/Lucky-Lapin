using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using NaughtyAttributes;
using System.Threading.Tasks;
using static SoundUtils;

public class ScoreMenu : MonoBehaviour
{
    [Scene]
    public string backScene;

    [Header("Text Fields")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI durationText;
    public TextMeshProUGUI coinsText;
    public TextMeshProUGUI collectiblesText;

    [Header("Sounds")]
    public Sound fadeInSound;

    void Start()
    {
        scoreText.text = GetFinalScore();
        durationText.text = $"Duration: {1}";
        coinsText.text = $"Coins collected: {128}";
        collectiblesText.text = $"Items stolen: {1}/{2}";

        CursorManager.instance.ShowCursor();
        SoundManager.instance.PlaySFX(fadeInSound);
    }

    string GetFinalScore()
    {
        return "A";
    }

    public async void GoBack()
    {
        FadeScreenController.instance.OnFadeOut.TryInvoke();
        await Task.Delay(3000);
        SceneManager.LoadScene(backScene);
    }
}
