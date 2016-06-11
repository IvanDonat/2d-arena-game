using UnityEngine;
using System.Collections;

public class SaveManagement : MonoBehaviour {

    // returns true if saved a new highscore, else does nothing and returns false
    public static bool SaveScore(string scene, int score, int time)
    {
        if (PlayerPrefs.GetInt("PB" + scene) < score)
        {
            PlayerPrefs.SetInt("PB" + scene, score);
            PlayerPrefs.SetInt("PB" + scene + "t", time);
            return true;
        }

        return false;
    }

    public static bool HasPersonalBest(string scene)
    {
        if (PlayerPrefs.GetInt("PB" + scene) != 0)
            return true;
        return false;
    }

    public static string GetPersonalBestScore(string scene)
    {        
        int sc = PlayerPrefs.GetInt("PB" + scene);
        if (sc == 0)
            return "NA";
        else
            return sc.ToString();
    }

    public static string GetPersonalBestTime(string scene)
    {
        int time =  PlayerPrefs.GetInt("PB" + scene + "t");
        if (time == 0)
            return "NA";
        
        string min = ((int) time / (int) 60).ToString();
        string sec = ((int) time % (int) 60).ToString();
        if (min.Length < 2)
            min = "0" + min;
        if (sec.Length < 2)
            sec = "0" + sec;
        return min + ":" + sec;
    }

    public static int GetPersonalBestTimeAsInt(string scene)
    {
        return PlayerPrefs.GetInt("PB" + scene + "t");
    }

}
