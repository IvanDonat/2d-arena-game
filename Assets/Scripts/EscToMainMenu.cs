using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class EscToMainMenu : MonoBehaviour {
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            SceneManager.LoadScene("MainMenu");
    }
}
