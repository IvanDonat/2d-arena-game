using UnityEngine;
using System.Collections;

public class MusicPlayer : MonoBehaviour {

    void Start()
    {
        gameObject.tag = "MusicPlayer";
        GameObject[] musicPlayers = GameObject.FindGameObjectsWithTag("MusicPlayer");

        if (musicPlayers.Length > 1)
        {
            foreach (GameObject player in musicPlayers)
            {
                if (player != gameObject)
                {
                    if (player.GetComponent<AudioSource>().clip == GetComponent<AudioSource>().clip)
                    {
                        Destroy(gameObject);
                    }
                    else
                    {
                        Destroy(player);
                    }
                }
            }
        }

        DontDestroyOnLoad(gameObject);
    }

}
