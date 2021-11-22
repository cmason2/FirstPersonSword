using System.Collections;
using System.Collections.Generic;
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


    void Start()
    {
        easy.gameObject.SetActive(false);
        medium.gameObject.SetActive(false);
        hard.gameObject.SetActive(false);
        back.gameObject.SetActive(false);
    }

    public void startGame()
    {
        Debug.Log("Load the first scene");
        //SceneManager.LoadScene("")
    }


    public void quitGame()
    {
        Application.Quit();
    }


    public void clickDifficulty()
    {
        start.gameObject.SetActive(false);
        difficulty.gameObject.SetActive(false);
        quit.gameObject.SetActive(false);


        easy.gameObject.SetActive(true);
        medium.gameObject.SetActive(true);
        hard.gameObject.SetActive(true);
        back.gameObject.SetActive(true);

    }


    public void backButton()
    {
        start.gameObject.SetActive(true);
        difficulty.gameObject.SetActive(true);
        quit.gameObject.SetActive(true);

        back.gameObject.SetActive(false);
        easy.gameObject.SetActive(false);
        medium.gameObject.SetActive(false);
        hard.gameObject.SetActive(false);
    }


    public void easyDifficulty()
    {
        Debug.Log("Change to easy");
    }

    public void mediumDifficulty()
    {
        Debug.Log("Change to medium");
    }


    public void hardDifficulty()
    {
        Debug.Log("Change to hard");

    }

}
