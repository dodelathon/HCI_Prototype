using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using System.Diagnostics;
using System.Text;

public class GameController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Controller;
    public string Registered_Name;
    public string Registered_Pass;
    public GameObject GameButton;
    public double diff;
    public Stopwatch timer;
    public Dictionary<String, String> Users;
    public Dictionary<String, String[]> Scores;
    private string path;
    public bool bob;

    void Start()
    {
        bob = false;
        path = Application.persistentDataPath;
        timer = new Stopwatch();
        Registered_Name = "";
        Registered_Pass = "";
        Users = new Dictionary<string, string>();
        Scores = new Dictionary<string, string[]>();
        ReadUsers();
        ReadScores();
        DontDestroyOnLoad(Controller);
        SceneManager.activeSceneChanged += ChangedActiveScene;
        SceneManager.LoadScene("Registration_Menu");
        SceneManager.LoadScene("Login_Menu");
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name.Equals("Perf_Menu") && bob == false)
        {
            FillChallenges();
            bob = true;
        }
        else if (SceneManager.GetActiveScene().name.Equals("Leaderboard_Menu") && bob == false)
        {
            FillLeaderboard();
            bob = true;
        }
    }

    void ChangedActiveScene(Scene current, Scene next)
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("GameController");
        List<GameObject> temp = new List<GameObject>();
        foreach (GameObject x in objects)
        {
            temp.Add(x);
        }
        objects = null;

        while (temp.Count > 1)
        {
            if (temp[temp.Count - 1].GetComponent<GameController>().Registered_Name.Equals(""))
            {
                Destroy(temp[temp.Count - 1]);
                temp.RemoveAt(temp.Count - 1);
            }
        }

        temp[0].GetComponent<GameController>().bob = false;
    }

    public bool Reg(string uName, string pass)
    {
        if (!Users.ContainsKey(uName))
        {
            Registered_Name = uName;
            Registered_Pass = pass;
            Users.Add(uName, pass);
            WriteUsers();
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool Login(string uName, string Pass)
    {
        if (Users.ContainsKey(uName))
        {
            if (Users[uName].Equals(Pass))
            {
                Registered_Name = uName;
                Registered_Pass = Pass;
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    public void Game(GameObject gamePanel)
    {
        GameController conn = GameObject.Find("GameController").GetComponent<GameController>();
        GameObject[] temp = Resources.FindObjectsOfTypeAll<GameObject>();
        foreach (GameObject t in temp)
        {
            if (t.name.Equals("GameButton"))
            {
                GameButton = t;
            }
        }
        GameButton.transform.SetParent(gamePanel.transform);
        GameObject topleft = GameObject.Find("TopLeft_Marker");
        GameObject bottomright = GameObject.Find("BottomRight_Marker");

        System.Random rand = new System.Random();

        float x = ((float)rand.NextDouble() * (bottomright.transform.position.x - topleft.transform.position.x)) + topleft.transform.position.x;
        float y = ((float)rand.NextDouble() * (topleft.transform.position.y - bottomright.transform.position.y)) + bottomright.transform.position.y;

        GameButton.transform.position = new Vector3(x, y);
        GameButton.SetActive(true);
        conn.timer.Start();
    }

    public void Caught()
    {
        GameController conn = GameObject.Find("GameController").GetComponent<GameController>();
        //UnityEngine.Debug.Log(timer.IsRunning);
        TimeSpan span = conn.timer.Elapsed;
        conn.timer.Stop();
        conn.timer.Reset();
        conn.diff = span.TotalMilliseconds;
        GameObject Success = null;
        GameObject[] temp = Resources.FindObjectsOfTypeAll<GameObject>();
        foreach (GameObject x in temp)
        {
            if (x.name.Equals("RegSuccess"))
            {
                Success = x;
            }
        }
        GameObject text = Success.transform.Find("SDialog").Find("TextBG").Find("MainText").gameObject;
        text.GetComponent<Text>().text = "Time: " + conn.diff + "ms";
        Success.SetActive(true);
    }

    public void ReadUsers()
    {
        try
        {
            string userspath = Path.Combine(path, "users.txt");
            var users = File.ReadLines(userspath);
            foreach (string x in users)
            {
                string[] temp = x.Split(':');
                Users.Add(temp[0], temp[1]);
            }
        }
        catch (IOException)
        {
            string userspath = Path.Combine(path, "users.txt");
            File.Create(userspath);
        }
    }

    public void WriteUsers()
    {
        GameController conn = GameObject.Find("GameController").GetComponent<GameController>();
        try
        {
            string temp = "";
            foreach (KeyValuePair<string, string> x in conn.Users)
            {
                temp += x.Key + ":" + x.Value + "\n";
            }
            string userspath = Path.Combine(path, "users.txt");
            FileStream file = File.Open(userspath, FileMode.Truncate);
            byte[] info = new UTF8Encoding(true).GetBytes(temp);
            file.Write(info, 0, info.Length);
            file.Close();
        }
        catch
        {
            //Something..?
        }
    }

    public void ReadScores()
    {
        try
        {
            string scorespath = Path.Combine(path, "scores.txt");
            var users = File.ReadLines(scorespath);
            foreach (string x in users)
            {
                string[] temp = x.Split(':');
                Scores.Add(temp[0], new string[] { temp[1], temp[2] });
            }
        }
        catch (IOException)
        {
            string scorespath = Path.Combine(path, "scores.txt");
            File.Create(scorespath);
        }
    }

    public void WriteScores()
    {
        try
        {
            UnityEngine.Debug.Log("Write"); ;
            string temp = "";
            foreach (KeyValuePair<string, string[]> x in Scores)
            {
                UnityEngine.Debug.Log(x.Value[0]);
                temp += x.Key + ":" + x.Value[0] + ":" + x.Value[1] + "\n";
            }
            string scorespath = Path.Combine(path, "scores.txt");
            FileStream file = File.Open(scorespath, FileMode.Truncate);
            byte[] info = new UTF8Encoding(true).GetBytes(temp);
            file.Write(info, 0, info.Length);
            file.Close();
            UnityEngine.Debug.Log("End");
        }
        catch (Exception e)
        {
            UnityEngine.Debug.Log("Write Error:\n" + e.Message);
        }
    }

    public void ScoreAdder()
    {

        if (!Scores.ContainsKey(Registered_Name))
        {
            Scores.Add(Registered_Name, new string[] { diff + "", "Reaction" });
        }
        else
        {
            Scores[Registered_Name] = new string[] { diff + "", "Reaction" };
        }
        WriteScores();
    }

    public void FillChallenges()
    {
        //UnityEngine.Debug.Log("Bob");
        GameController conn = GameObject.Find("GameController").GetComponent<GameController>();
        GameObject NameScroll = GameObject.Find("NameScroll");
        GameObject ScoreScroll = GameObject.Find("ScoreScroll");
        GameObject NameContent = NameScroll.transform.Find("List_Mask").Find("List_Content").gameObject;
        GameObject ScoreContent = ScoreScroll.transform.Find("List_Mask").Find("List_Content").gameObject;
        GameObject NameTemp = null;
        GameObject ScoreTemp = null;

        GameObject[] temp = Resources.FindObjectsOfTypeAll<GameObject>();
        foreach (GameObject x in temp)
        {
            if (x.name.Equals("ScoreTemplate"))
            {
                ScoreTemp = x;
            }
            else if (x.name.Equals("NameTemplate"))
            {
                NameTemp = x;
            }
        }

        foreach (KeyValuePair<string, string[]> x in conn.Scores)
        {
            if (x.Key.Equals(conn.Registered_Name))
            {
                UnityEngine.Debug.Log(x.Value[0]);
                UnityEngine.Debug.Log(path);
                GameObject tempor = GameObject.Instantiate(NameTemp, NameContent.transform);
                tempor.transform.Find("Text").gameObject.GetComponent<Text>().text = x.Key;
                tempor.SetActive(true);

                GameObject tem = GameObject.Instantiate(ScoreTemp, ScoreContent.transform);
                tem.transform.Find("Text").gameObject.GetComponent<Text>().text = x.Value[1] + ":" + x.Value[0];
                tem.SetActive(true);
            }
        }
    }

    public void FillLeaderboard()
    {
        //UnityEngine.Debug.Log("Bob");
        GameController conn = GameObject.Find("GameController").GetComponent<GameController>();
        GameObject NameScroll = GameObject.Find("NameScroll");
        GameObject ScoreScroll = GameObject.Find("ScoreScroll");
        GameObject NameContent = NameScroll.transform.Find("List_Mask").Find("List_Content").gameObject;
        GameObject ScoreContent = ScoreScroll.transform.Find("List_Mask").Find("List_Content").gameObject;
        GameObject NameTemp = null;
        GameObject ScoreTemp = null;

        GameObject[] temp = Resources.FindObjectsOfTypeAll<GameObject>();
        foreach (GameObject x in temp)
        {
            if (x.name.Equals("ScoreTemplate"))
            {
                ScoreTemp = x;
            }
            else if (x.name.Equals("NameTemplate"))
            {
                NameTemp = x;
            }
        }

        foreach (KeyValuePair<string, string[]> x in conn.Scores)
        {
            UnityEngine.Debug.Log(x.Value[0]);
            UnityEngine.Debug.Log(path);
            GameObject tempor = GameObject.Instantiate(NameTemp, NameContent.transform);
            tempor.transform.Find("Text").gameObject.GetComponent<Text>().text = x.Key;
            tempor.SetActive(true);
            GameObject tem = GameObject.Instantiate(ScoreTemp, ScoreContent.transform);
            tem.transform.Find("Text").gameObject.GetComponent<Text>().text = x.Value[1] + ":" + x.Value[0];
            tem.SetActive(true);
        }

    }
}

