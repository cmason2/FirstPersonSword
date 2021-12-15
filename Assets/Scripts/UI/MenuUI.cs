using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuUI : MonoBehaviour
{
    public Button start;
    public Button difficulty;
    public Button quit;
    public Button easy;
    public Button medium;
    public Button hard;
    public Button back;
    public Button back1;
    public Button leaderboard;

    public void startGame()
    {
        Debug.Log("Load the first scene");
        SceneManager.LoadScene(1);
    }

    public void quitGame()
    {
        Debug.Log("Quit Game");
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
        Application.Quit();
        Debug.Log("Quit Game");
    }

    public void clickDifficulty()
    {
        start.gameObject.SetActive(false);
        difficulty.gameObject.SetActive(false);
        quit.gameObject.SetActive(false);
        leaderboard.gameObject.SetActive(false);


        easy.gameObject.SetActive(true);
        medium.gameObject.SetActive(true);
        hard.gameObject.SetActive(true);
        back.gameObject.SetActive(true);
    }

    public void leaderboardClick()
    {
        back1.gameObject.SetActive(true);

        leaderboard.gameObject.SetActive(false);
        start.gameObject.SetActive(false);
        difficulty.gameObject.SetActive(false);
        quit.gameObject.SetActive(false);

    }

    public void backButton()
    {
        start.gameObject.SetActive(true);
        difficulty.gameObject.SetActive(true);
        quit.gameObject.SetActive(true);
        leaderboard.gameObject.SetActive(true);

        back.gameObject.SetActive(false);
        easy.gameObject.SetActive(false);
        medium.gameObject.SetActive(false);
        hard.gameObject.SetActive(false);
    }

    public void backButton1()
    {
        start.gameObject.SetActive(true);
        difficulty.gameObject.SetActive(true);
        quit.gameObject.SetActive(true);
        leaderboard.gameObject.SetActive(true);

        back1.gameObject.SetActive(false);
    }


    public void easyDifficulty()
    {
        Debug.Log("Change to easy");
        easy.interactable = false;
        medium.interactable = true;
        hard.interactable = true;
    }

    public void mediumDifficulty()
    {
        Debug.Log("Change to medium");
        easy.interactable = true;
        medium.interactable = false;
        hard.interactable = true;
    }

    public void hardDifficulty()
    {
        Debug.Log("Change to hard");
        easy.interactable = true;
        medium.interactable = true;
        hard.interactable = false;
    }
}
