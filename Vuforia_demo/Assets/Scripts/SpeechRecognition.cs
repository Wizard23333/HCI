using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;
using System.Text.RegularExpressions;

[RequireComponent(typeof(AudioListener)), RequireComponent(typeof(AudioSource))]
public class SpeechRecognition : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    // 百度API
    private string accessToken;
    private string API_KEY = "MagNe67l7giGsL8AvAuaeSRq";
    private string SECRET_KEY = "Rol8GH8PjMb0OA3jrMuyWDhfnbvO2fWZ";
    
    // 识别结果
    private string result;

    //标记是否有麦克风
    private bool isHaveMic = false;

    //当前录音设备名称
    string currentDeviceName = string.Empty;

    //录音频率,控制录音质量(8000,16000)
    int recordFrequency = 16000;

    //上次按下时间戳
    double lastPressTimestamp = 0;

    //表示录音的最大时长
    int recordMaxLength = 10;

    //实际录音长度(由于unity的录音需先指定长度,导致识别上传时候会上传多余的无效字节)
    //通过该字段,获取有效录音长度,上传时候剪切到无效的字节数据即可
    int trueLength = 0;

    //存储录音的片段
    [HideInInspector]
    public AudioClip saveAudioClip;

    //当前按钮下的文本
    Text textBtn;

    //显示结果的文本
    Text textResult;

    //音源
    AudioSource audioSource;

    void Start()
    {
        //获取麦克风设备，判断是否有麦克风设备
        if (Microphone.devices.Length > 0)
        {
            isHaveMic = true;
            currentDeviceName = Microphone.devices[0];
        }

        //获取相关组件
        textBtn = this.transform.GetChild(0).GetComponent<Text>();
        audioSource = this.GetComponent<AudioSource>();
        textResult = this.transform.GetChild(1).GetComponent<Text>();
        textResult.text = "识别结果";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 开始录音
    public bool StartRecording(bool isLoop = false) //8000,16000
    {
        if (isHaveMic == false || Microphone.IsRecording(currentDeviceName))
        {
            return false;
        }

        //开始录音
        /*
         * public static AudioClip Start(string deviceName, bool loop, int lengthSec, int frequency);
         * deviceName   录音设备名称.
         * loop         如果达到长度,是否继续记录
         * lengthSec    指定录音的长度.
         * frequency    音频采样率   
         */

        lastPressTimestamp = GetTimestampOfNowWithMillisecond();

        saveAudioClip = Microphone.Start(currentDeviceName, isLoop, recordMaxLength, recordFrequency);

        return true;
    }

    // 录音结束,返回实际的录音时长
    public int EndRecording()
    {
        if (isHaveMic == false || !Microphone.IsRecording(currentDeviceName))
        {
            return 0;
        }

        //结束录音
        Microphone.End(currentDeviceName);

        //向上取整,避免遗漏录音末尾
        return Mathf.CeilToInt((float)(GetTimestampOfNowWithMillisecond() - lastPressTimestamp) / 1000f);
    }

    // 获取毫秒级别的时间戳,用于计算按下录音时长
    public double GetTimestampOfNowWithMillisecond()
    {
        return (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000;
    }

    // 按下录音按钮
    public void OnPointerDown(PointerEventData eventData)
    {
        textBtn.text = "松开识别";
        StartRecording();
    }

    // 放开录音按钮
    public void OnPointerUp(PointerEventData eventData)
    {
        textBtn.text = "按住说话";
        trueLength = EndRecording();
        if (trueLength > 1)
        {
            audioSource.PlayOneShot(saveAudioClip);
            StartCoroutine(_StartBaiduYuYin());
        }
        else
        {
            textResult.text = "录音时长过短";
        }
    }

    // 获取accessToken请求令牌
    IEnumerator _GetAccessToken()
    {
        var uri =
            string.Format("https://aip.baidubce.com/oauth/2.0/token?grant_type=client_credentials&client_id={0}&client_secret={1}", API_KEY, SECRET_KEY);


        UnityWebRequest unityWebRequest = UnityWebRequest.Get(uri);
        yield return unityWebRequest.SendWebRequest();

        if (unityWebRequest.isDone)
        {
            //这里可以考虑用Json,本人比较懒所以用正则匹配出accessToken
            Match match = Regex.Match(unityWebRequest.downloadHandler.text, @"access_token.:.(.*?).,");
            if (match.Success)
            {
                //表示正则匹配到了accessToken
                accessToken = match.Groups[1].ToString();
                Debug.Log("accessToken : " + accessToken);
            }
            else
            {
                textResult.text = "验证错误,获取AccessToken失败!!!";
            }
        }
    }

    // 发起语音识别请求
    IEnumerator _StartBaiduYuYin()
    {
        if (string.IsNullOrEmpty(accessToken))
        {
            yield return _GetAccessToken();
        }

        result = string.Empty;

        //处理当前录音数据为PCM16
        float[] samples = new float[recordFrequency * trueLength * saveAudioClip.channels];
        saveAudioClip.GetData(samples, 0);
        var samplesShort = new short[samples.Length];
        for (var index = 0; index < samples.Length; index++)
        {
            samplesShort[index] = (short)(samples[index] * short.MaxValue);
        }
        byte[] datas = new byte[samplesShort.Length * 2];
        Buffer.BlockCopy(samplesShort, 0, datas, 0, datas.Length);

        string url = string.Format("{0}?cuid={1}&token={2}", "https://vop.baidu.com/server_api", SystemInfo.deviceUniqueIdentifier, accessToken);

        WWWForm wwwForm = new WWWForm();
        wwwForm.AddBinaryData("audio", datas);

        UnityWebRequest unityWebRequest = UnityWebRequest.Post(url, wwwForm);

        unityWebRequest.SetRequestHeader("Content-Type", "audio/pcm;rate=" + recordFrequency);

        yield return unityWebRequest.SendWebRequest();

        if (string.IsNullOrEmpty(unityWebRequest.error))
        {
            result = unityWebRequest.downloadHandler.text;
            if (Regex.IsMatch(result, @"err_msg.:.success"))
            {
                Match match = Regex.Match(result, "result.:..(.*?)..]");
                if (match.Success)
                {
                    result = match.Groups[1].ToString();
                }
            }
            else
            {
                result = "识别结果为空";
            }
            textResult.text = result;
        }
    }


}
