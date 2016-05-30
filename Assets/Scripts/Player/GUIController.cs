using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public struct Notification
{
    public Notification(string val, float t)
    {
        this.value = val;
        this.timeOn = t;
    }
    public string value;
    public float timeOn;
}

public class GUIController : MonoBehaviour {
    public Transform pausedGUI;
    private bool paused = false;

    public Text guiHealth;
    private Color initialHPColor;
    private int health = 100;

    public Text guiEnemyCount; 
    private int enemyCount = -1;

    public Text guiNotification;
    public AudioSource guiSndNotification;
    private ArrayList notifications;
    private string oldNotification;
    private float lastChangedNotification;

    public Text guiPickup;
    private string pickupOriginalText;

    void Start () {
        initialHPColor = guiHealth.color;
        pausedGUI.gameObject.SetActive(false);
        notifications = new ArrayList();
        guiPickup.gameObject.SetActive(false);
        pickupOriginalText = guiPickup.text + " ";
    }
	
    void Update () {
        guiHealth.text = "HEALTH: " + health.ToString();
        guiHealth.color = Color.Lerp(guiHealth.color, initialHPColor, Time.deltaTime * 2f);

        guiEnemyCount.text = "ENEMIES: " + enemyCount.ToString();
        guiEnemyCount.color = Color.Lerp(guiEnemyCount.color, initialHPColor, Time.deltaTime * 2f);

        if (notifications.Count != 0)
        {
            Notification curr = (Notification)notifications[0];
            guiNotification.text = curr.value;
            curr.timeOn -= Time.deltaTime;

            notifications.RemoveAt(0);
            notifications.Reverse();
            notifications.Add(curr);
            notifications.Reverse();

            if (oldNotification != curr.value)
            {
                guiSndNotification.Play();
                oldNotification = curr.value;
            }

            // @TODO maybe add fade in/out
            if (curr.timeOn <= 0)
            {
                notifications.RemoveAt(0);
            }
        }
        else
        {
            guiNotification.text = "";
            oldNotification = guiNotification.text;
        }
    }

    public void SetPaused(bool p)
    {
        paused = p;
        pausedGUI.gameObject.SetActive(paused);
    }

    public void SetHP(int hp)
    {
        health = Mathf.Clamp(hp, 0, int.MaxValue);
        guiHealth.color = Color.red;
    }

    public void SetEnemyCount(int c)
    {
        if (c != enemyCount)
            guiEnemyCount.color = Color.gray;
        enemyCount = c;
    }

    public void PushNotification(string not)
    {
        notifications.Add(new Notification(not, 1.5f));
    }

    public void OnPickup(PickupWeapon pw, bool onPickup)
    {
        if (!onPickup)
        {
            guiPickup.gameObject.SetActive(false);
            return;
        }

        guiPickup.gameObject.SetActive(true);
        guiPickup.text = pickupOriginalText + pw.GetName();
    }
}
