using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameScript : MonoBehaviour
{
    public Text highScore;

    public void StartGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void Menu()
    {
        SceneManager.LoadScene("MenuScene");
    }

    public void HSValue()
    {
        highScore.text = PlayerPrefs.GetInt("HighScore").ToString();
    }

    void Start()
    {
        HSValue();
    }
}