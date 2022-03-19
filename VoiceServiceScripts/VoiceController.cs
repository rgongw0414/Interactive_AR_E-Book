using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Threading;
//using Ai.Olami.Example;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using RemptyTool.ES_MessageSystem;
using UnityEngine.SceneManagement;

/// <summary>
/// Interactive Argumented Reality e-Book 
/// Natrural Language Interaction System based on Olami service,
/// </summary>
public class VoiceController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler  // 將此腳本掛在作為錄音按鍵的物件
{   
    ES_MessageSystem ES_MessageSystem;
    /// <summary>
    /// 語音辨識按鍵處理 
    /// reference: https://medium.com/@j2a0r0e0d0/unity-%E9%9A%A8%E6%89%8B%E8%A8%98-%E5%AF%A6%E7%94%A8%E6%8A%80%E5%B7%A701-%E9%95%B7%E6%8C%89button-cb7473d5bc1
    /// </summary>
    private bool PressDown; //按下
    public UnityEvent onLongClick; //開啟Inspector觸發事件        
    GameObject MenuHint;

    [SerializeField]
    AudioSource Btn_Sound_Effect;
    //按下按鈕
    public void OnPointerDown(PointerEventData eventData)  // 掛著腳本的object被持續按著時，Press
    {
        GameObject.Find("BGM").GetComponent<AudioSource>().volume = 0.2f;
        if (SceneManager.GetActiveScene().name == "CharactarInformation")
        {
            //GameObject.Find("Background_Music_AS").GetComponent<AudioSource>().Stop();  // 暫停角色介紹頁的背景音樂
        }

        if (SceneManager.GetActiveScene().name == "ChapterOne")
        {
            ES_MessageSystem.audioSource.Stop();
            ES_MessageSystem.Microphone_Button_Pressed(); // 麥克風按鈕被按下，將任務一的AudioSource停止
            if (ES_MessageSystem.MissionThreeHint_AS != null)
            {
                ES_MessageSystem.MissionThreeHint_AS.Stop();
                ES_MessageSystem.MissionThreeRightIngredient.Stop();
                ES_MessageSystem.MissionThreeWrongIngredient.Stop();
            }
        }
        
        if (MenuHint != null)  // 摧毀首頁提示物件，進而停止音檔撥放。
        {
            Destroy(MenuHint);
        }
        Btn_Sound_Effect.Play();
        PressDown = true;  
    }

    //按鈕彈起
    public void OnPointerUp(PointerEventData eventData)
    {
        if (SceneManager.GetActiveScene().name == "ChapterOne")
            GameObject.Find("BGM").GetComponent<AudioSource>().volume = 0.3f;
        else
            GameObject.Find("BGM").GetComponent<AudioSource>().volume = 1.0f;

        if (SceneManager.GetActiveScene().name == "CharactarInformation")
        {
            //GameObject.Find("Background_Music_AS").GetComponent<AudioSource>().Play();  // 恢復角色介紹頁的背景音樂
        }
        Reset();
    }    
    
    //當PressUp的時候重製計算時間
    private void Reset()
    {
        PressDown = false;
    }
    // EOP

    // some variable declaration
    string[] micArray = null;  // microphone devices' name of the current environment
    int maxFreq;  // the capability of max/min frequency, unused.
    int minFreq;

      
    bool recording = false;
    // bool button_pressed = false;  // 若想要錄音鍵是分兩次點擊，則使用此變數 和 Button_pressed() function
    /*public void Button_pressed()
    {
        button_pressed = !button_pressed;
    }*/


    /// <summary>
    /// WavUtility Script, Wav Utility for Unity (using WavUtility.cs)
    /// WAV utility for recording and audio playback functions in Unity.
    /// reference : https://github.com/deadlyfingers/UnityWav
    /// </summary>
    private AudioSource audioSource; // hook up with scene's AudioSource in Unity Editor inspector    
    AudioClip audioclip;    

    public string SaveWavFile()  // 儲存讀取或錄下的AudioClip，存成wav檔，並return file path
    {
        string filepath;
        byte[] bytes = WavUtility.FromAudioClip(audioSource.clip, out filepath, true);
        return filepath;
    }
    // EOP

    void Start()
    {
        if (SceneManager.GetActiveScene().name == "ChapterOne")
            ES_MessageSystem = GameObject.Find("EventSystem").GetComponent<ES_MessageSystem>();
        MenuHint = GameObject.Find("MenuHint");  // 首頁提示語音之物件

        micArray = Microphone.devices;
        if (micArray.Length == 0)
        {
            Debug.LogError("Microphone.devices is null");
        }
        foreach (string deviceStr in Microphone.devices)
        {
            Microphone.GetDeviceCaps(deviceStr, out minFreq, out maxFreq);
            Debug.Log("device name = " + deviceStr + "\nmaxFreq: " + maxFreq + ", minFreq: " + minFreq);
        }
        if (micArray.Length == 0)
        {
            Debug.LogError("no mic device");
        }
    }

    [SerializeField]
    AudioSource Dont_Understand;
    bool dont_understand = false;

    [SerializeField]
    AudioSource Failed_to_Connect;
    bool failed_to_connect = false;
    private void Update()
    {        
        // 當按下按鈕 PressDown = true 時計時
        if (PressDown == true)
        {
            if (onLongClick != null)
            {
                onLongClick.Invoke();
            }            
        }
        if (dont_understand == true) { Dont_Understand.Play(); dont_understand = false; }
        if (failed_to_connect == true) { Failed_to_Connect.Play(); failed_to_connect = false; }        
    }

    private const int sampleRate = 16000; // sample rate for recording speech (OLAMI指定格式)
    void LateUpdate()
    {
        if (PressDown)
        {
            if (!Microphone.IsRecording(null))
            {
                Debug.Log("START RECORDING!");
                audioclip = Microphone.Start(Microphone.devices[0], false, 5, sampleRate);
            }
        }
        else
        {
            if (Microphone.IsRecording(null))
            {
                Microphone.End(null);
                if (audioclip != null)
                {
                    Debug.Log("FINISH RECORDING.");
                    byte[] audiodata = WavUtility.FromAudioClip(audioclip);
                    Thread thread = new Thread(new ParameterizedThreadStart(process));
                    thread.Start((object)audiodata);
                    //thread.Start((object) result);
                }
                else
                {
                    Debug.Log("AudioClip is EMPTY!");
                }
            }
        }        
    }

    // *** REMEMBER to assign a Scene to this component in Unity Inspector, else "null reference" would happen. ***
    [SerializeField]
    VoiceService voiceService;

    [SerializeField]
    PlayerVoiceControl voiceControl; // using PlayerVoiceControl.cs,     
    void process(object obj)
    {
        byte[] audioFile = (byte[])obj;

        string result = voiceService.sendSpeech(audioFile);
        //string result = speechApi.SendAudioFile(SpeechApiSample.API_NAME_ASR, "nli,seg", true, audioFile, false);
        audioclip = null;
        //Debug.Log(result);

        VoiceResult voiceResult = JsonUtility.FromJson<VoiceResult>(result);
        if (voiceResult.status.Equals("ok"))
        {
            Nli[] nlis = voiceResult.data.nli;  // nli is an array object
            if (nlis != null && nlis.Length != 0)
            {
                foreach (Nli nli in nlis)
                {
                    string type = nli.type;
                    if (type == "menu" || type == "chapter_choose" || type == "chapter_choose" || type == "mission_one" || type == "mission_two" || type == "mission_three") // NLI系統的Module Name
                    {
                        foreach (Semantic sem in nli.semantic) // semantic is an array object
                        {
                            voiceControl.ProcessSemantic(sem);
                        }
                    }
                    else
                    {
                        dont_understand = true;
                        Debug.Log("\"" + type + "\" is not Olami module");
                    }
                }
            }
            else
            {
                dont_understand = true;
                Debug.Log("ERROR! nlis is NULL Reference");
            }
        }
        else
        {
            failed_to_connect = true;
            Debug.Log("Request Failed, plz check out your apiUrl, appKey or appSecret.");
        }
    }
}


// for Json File Access
[Serializable]
public class VoiceResult
{
    public VoiceData data;
    public string status;
}

[Serializable]
public class VoiceData
{
    public Nli[] nli;
    public Asr asr;
}

[Serializable]
public class Asr
{
    public string result;
    public int speech_status;
    public Boolean final;
    public int status;
}

[Serializable]
public class Nli
{
    public DescObj desc;
    public Semantic[] semantic;
    public string type;
}

[Serializable]
public class DescObj
{
    public string result;
    public int status;
}

[Serializable]
public class Semantic
{
    public string app;
    public string input;
    public Slot[] slots;
    public string[] modifier;
    public string customer;
}

[Serializable]
public class Slot
{
    public string name;
    public string value;
    public string[] modifier;
}