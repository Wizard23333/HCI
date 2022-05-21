using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class ScreenShoot : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnScreenShootClick()
    {
        // 截屏文件的日期
        System.DateTime now = System.DateTime.Now;
        string times = now.ToString();
        times = times.Trim();
        times = times.Replace("/", "-");

        // 文件名称
        string fileName = "ARScreenShoot" + times + ".png";

        if (Application.platform == RuntimePlatform.Android)
        {
            // 按钮在截屏的时候设置不可见
            this.GetComponentInChildren<Image>().enabled = false;
            this.GetComponentInChildren<Text>().enabled = false;
            
            // 截图到材质上
            Texture2D texture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
            texture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
            texture.Apply();

            // 转化成字节数组
            byte[] bytes = texture.EncodeToPNG();
            //存储到截屏的目录下
            string des = "/sdcard/DCIM/Screenshots";

            if (!Directory.Exists(des))
            {
                Directory.CreateDirectory(des);
            }

            string pathSave = des + "/" + fileName;
            File.WriteAllBytes(pathSave, bytes);

            this.GetComponentInChildren<Image>().enabled = true;
            this.GetComponentInChildren<Text>().enabled = true;
        }

    }
}
