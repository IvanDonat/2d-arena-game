using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class OptionsManager : MonoBehaviour {

    void Start()
    {

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            PlayerPrefs.DeleteAll();
            Debug.Log("Deleted PlayerPrefs");
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("MainMenu");
        }
    }

}
