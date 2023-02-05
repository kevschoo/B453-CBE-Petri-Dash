using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    void Awake()
    {
        Application.targetFrameRate = 60;
        AudioManager.Instance().PlayMusic(AudioManager.Music.Game);
    }


    public void Play()
    {
        SceneManager.LoadScene("Game");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
