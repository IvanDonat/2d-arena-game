using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LevelSelectListItem : MonoBehaviour {
    public Text txtName;
    private Level lv;

    public void Clicked()
    {
        GameObject.FindGameObjectWithTag("GUI").GetComponent<LevelSelectManager>().SelectLevel(lv);
    }

    public void SetLevel(Level l)
    {
        this.lv = l;
    }
}
