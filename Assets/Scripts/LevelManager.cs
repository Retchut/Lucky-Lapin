using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    public string nextLvlName;

    public float delayToRestart = 1.5f;
    private void Awake()
    {
        instance = this;
    }

    [DebugCommand("over", "Make Game Over")]
    public async void GameOver()
    {
        PlayerMovControllerFloat.instance.enabled = false;
        PlayerShootController.instance.enabled = false;

        if (FadeScreenController.instance != null)
        {
            FadeScreenController.instance.FadeOut();
            await Task.Delay(System.TimeSpan.FromSeconds(delayToRestart));
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    [DebugCommand("nextLvl", "To next lvl")]
    public async Task NextLevel()
    {
        if (FadeScreenController.instance != null)
        {
            FadeScreenController.instance.FadeOut();
            await Task.Delay(System.TimeSpan.FromSeconds(delayToRestart));
        }

        SceneManager.LoadScene(nextLvlName);

    }
}
