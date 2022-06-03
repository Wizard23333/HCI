using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Display : MonoBehaviour
{

    //等级文本
    Text levelText;

    // Start is called before the first frame update
    void Start()
    {
        levelText = this.transform.GetChild(0).GetComponent<Text>();
        levelText.text = "shishi";
    }

    // Update is called once per frame
    void Update()
    {
        levelText.text = "LEVEL:" + (PlayerPrefs.GetInt("chicken_value") / 100 - 1).ToString();
    }
}
