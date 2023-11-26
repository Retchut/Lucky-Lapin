using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    public PlayerInputHandlerPlatformer playerInputHandlerPlatformer;


    public float delayToRestart = 1.5f;
    private void Awake()
    {
        instance = this;
    }

    public async void GameOver()
    {
        if (FadeScreenController.instance != null)
        {
            playerInputHandlerPlatformer.ResetMovement();
            FadeScreenController.instance.FadeOut();
            await Task.Delay(System.TimeSpan.FromSeconds(delayToRestart));
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
