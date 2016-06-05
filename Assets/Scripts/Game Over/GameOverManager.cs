using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour {

    // info
    public static bool infoWon = true;
    public static int  infoScore = -1;
    public static int  infoTime = -1;

    // gui
    public Text textStatus;
    public Text textScore;
    public Text textTime;

    void Start()
    {
        ReloadText();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Back();
        }
    }

    void ReloadText()
    {
        if (infoWon) 
            textStatus.text = "You won.";
        else 
            textStatus.text = "You lost.";


        textScore.text = "Score: " + infoScore.ToString();


        string min = ((int) infoTime / (int) 60).ToString();
        string sec = ((int) infoTime % (int) 60).ToString();
        if (min.Length < 2)
            min = "0" + min;
        if (sec.Length < 2)
            sec = "0" + sec;

        textTime.text = "Time: " + min + ":" + sec;
    }

    public void Back()
    {
        // reset everything, not mandatory
        infoWon = true;
        infoScore = -1;
        infoTime = -1;

        SceneManager.LoadScene("MainMenu");
    }
}
