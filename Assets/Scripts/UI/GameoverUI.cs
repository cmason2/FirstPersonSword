using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameoverUI : MonoBehaviour
{
    public Button start;
    public Button quit;


    public void startGame()
    {
        Debug.Log("Load the first scene");
        SceneManager.LoadScene("Maze");
    }

    public void quitGame()
    {
        //Application.Quit();
        Debug.Log("Menu scene");
        SceneManager.LoadScene("Menu");
    }
}
