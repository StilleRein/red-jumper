using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndSceneController : MonoBehaviour
{
    public Text txtScore, txtHighScore;
    int highscore;

    GameObject soundManager;

    // Start is called before the first frame update
    void Start()
    {
        soundManager = GameObject.Find("SoundManager");

        highscore = PlayerPrefs.GetInt("User_Highscore", 0);

        if (Data.score > highscore )
        {
            highscore = Data.score;
            PlayerPrefs.SetInt("User_Highscore", highscore);
        }

        txtScore.text = "Score : " + Data.score;
        txtHighScore.text = "Highscore : " + highscore;

        Destroy(soundManager);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Replay()
    {
        Data.score = 0;
        Data.level = 1;
        SceneManager.LoadScene("GameScene1");
    }

    public void Home()
    {
        SceneManager.LoadScene("MainScene");
    }
}