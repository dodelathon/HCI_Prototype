using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Button_Functions : MonoBehaviour
{
    private GameObject start;

    public void LoadLogin()
    {
        SceneManager.LoadScene("Login_Menu");
    }

    public void LoadReg()
    {
        SceneManager.LoadScene("Registration_Menu");
    }

    public void LoadMain()
    {
        SceneManager.LoadScene("Main_Menu");
    }

    public void LoadTrainingMenu()
    {
        SceneManager.LoadScene("Training_Menu");
    }

    public void LoadTrainingAreaMenu()
    {
        SceneManager.LoadScene("TrainingArea_Menu");
    }

    public void LoadSettings()
    {
        SceneManager.LoadScene("Settings_Menu");
    }

    public void LoadStats()
    {
        SceneManager.LoadScene("Stats_Menu");
    }

    public void LoadPerf()
    {
        SceneManager.LoadScene("Perf_Menu");
    }

    public void LoadShare()
    {
        SceneManager.LoadScene("Share_Menu");
    }

    public void LoadLeaderboard()
    {
        SceneManager.LoadScene("Leaderboard_Menu");
    }

    public void LoadStimuli()
    {
        SceneManager.LoadScene("Stimuli_Menu");
    }

    public void LoadChallengeArea()
    {
        SceneManager.LoadScene("Challenge_Menu");
    }

    public void LoadPersonal()
    {
        SceneManager.LoadScene("Personal_Menu");
    }

    public void LoadReactionTime()
    {
        SceneManager.LoadScene("ReactionTime_Game");
    }

    public void Register()
    {
        InputField UName = GameObject.Find("Username_Field").GetComponent<InputField>();
        InputField Pass = GameObject.Find("Password_Field").GetComponent<InputField>();
        InputField ConfPass = GameObject.Find("ConfPassword_Field").GetComponent<InputField>();

        if (!UName.text.Equals(""))
        {
            if (Pass.text.Equals(ConfPass.text))
            {
                GameController conn = GameObject.Find("GameController").GetComponent<GameController>();
                conn.Reg(UName.text, Pass.text);
                ShowSucc();
            }
            else
            {
                ShowError();
            }
        }
        else
        {
            ShowError();
        }
    }

    public void Login()
    {
        InputField UName = GameObject.Find("Username_Field").GetComponent<InputField>();
        InputField Pass = GameObject.Find("Password_Field").GetComponent<InputField>();

        string NameData = UName.text;
        string PassData = Pass.text;
        GameController conn = GameObject.Find("GameController").GetComponent<GameController>();

        if (conn.Login(NameData, PassData))
        {
            LoadMain();
        }
        else
        {
            ShowError();
        }
    }

    public void ShowError()
    {
        GameObject[] temp = Resources.FindObjectsOfTypeAll<GameObject>();
        GameObject Error = null;
        foreach (GameObject x in temp)
        {
            if (x.name.Equals("RegError"))
            {
                Error = x;
            }
        }
        Error.SetActive(true);
    }

    public void ShowSucc()
    {
        GameObject Success = null;
        GameObject[] temp = Resources.FindObjectsOfTypeAll<GameObject>();
        foreach (GameObject x in temp)
        {
            if (x.name.Equals("RegSuccess"))
            {
                Success = x;
            }
        }
        Success.SetActive(true);
    }

    public void HideError()
    {
        GameObject Error = GameObject.Find("RegError");
        Error.SetActive(false);
    }

    public void SuccessCont()
    {
        LoadLogin();
    }

    public void StartPress()
    {
        Debug.Log(GameObject.Find("GameController"));
        GameController conn = GameObject.Find("GameController").GetComponent<GameController>();
        GameObject panel = GameObject.Find("GamePanel");
        start = GameObject.Find("StartButton");
        start.SetActive(false);
        conn.Game(panel);
    }

    public void ContPress()
    {
        GameController conn = GameObject.Find("GameController").GetComponent<GameController>();
        conn.ScoreAdder();
        LoadTrainingAreaMenu();
    }

}
