using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour {

    public Color selectedColor;

    public Text btnPlay;
    public Text btnOptions;
    public Text btnExit;

    private int selectedIndex = 0;
    private Color unselectedColor;

    void Awake()
    {
        Cursor.visible = false;
        
        unselectedColor = btnPlay.color;
    }

    void Update()
    {
        UnselectAll();

        bool isEnter = (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return)) == true;

        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            selectedIndex++;
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            selectedIndex--;
        selectedIndex = Mathf.Clamp(selectedIndex, 0, 2);

        if (selectedIndex == 0)
        {
            Select(btnPlay);

            if (isEnter)
            {
                SceneManager.LoadScene("LevelSelect");
            }
        }
        else if(selectedIndex == 1)
        {
            Select(btnOptions);

            if (isEnter)
            {
                Debug.Log("NO OPTIONS IMPLEMENTED");
            }
        }
        else
        {
            Select(btnExit);            

            if (isEnter)
            {
                Application.Quit();
            }
        }
    }

    void UnselectAll()
    {
        btnPlay.color = Color.Lerp(btnPlay.color, unselectedColor, Time.deltaTime * 10);
        btnOptions.color = Color.Lerp(btnOptions.color, unselectedColor, Time.deltaTime * 10);
        btnExit.color = Color.Lerp(btnExit.color, unselectedColor, Time.deltaTime * 10);
    }

    void Select(Text t)
    {
        t.color = Color.Lerp(t.color, selectedColor, Time.deltaTime * 20);
    }
}
