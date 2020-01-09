using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainSceneController : MonoBehaviour
{
    GameObject exitPanel;

    // Start is called before the first frame update
    void Start()
    {
        exitPanel = GameObject.Find("ExitPanel");
        exitPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void StartGame()
    {
        Data.score = 0;
        Data.level = 1;
        SceneManager.LoadScene("GameScene1");
    }

    public void ExitGame()
    {
        exitPanel.SetActive(true);
    }

    public void ExitConfirmation(Button button)
    {
        if (button.name.Equals("Button_Yes"))
        {
            Application.Quit();
        }

        else if(button.name.Equals("Button_No"))
        {
            exitPanel.SetActive(false);
        }
    }
}