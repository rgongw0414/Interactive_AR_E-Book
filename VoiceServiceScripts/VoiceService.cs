using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Ai.Olami.Example;

public class VoiceService : MonoBehaviour
{
    // 繁中版OLAMI
    string url = "https://tw.olami.ai/cloudservice/api";
    string key = "3ed779b0eb9c45d7b5dc5f7699cb42c0"; 
    string secret = "0538cb509a344f289b7dc19ca53f96aa";

    // 簡中版OLAMI
    /*string url = "https://cn.olami.ai/cloudservice/api";
    string key = "bab758d432dd4e4ea73787b5e076de46"; 
    string secret = "bce5362e83834cbdb2a4ee09e90256a1";*/


    //[SerializeField]
    //NluApiSample nluApi;
    [SerializeField]
    SpeechApiSample speechApi;

    private void Start()
    {
        //nluApi.SetLocalization(url);  // for cloud text recognition
        //nluApi.SetAuthorization(key, secret);

        speechApi.SetLocalization(url);  // for cloud speech recognition
        speechApi.SetAuthorization(key, secret);
    }

    private void Update()
    {
        if (Upload_Failed == true) { GameObject.Find("上傳失敗").GetComponent<AudioSource>().Play(); Upload_Failed = false; }  // 連線失敗十，撥放語音提示
        if (Post_Return_Failed == true) { GameObject.Find("回傳失敗").GetComponent<AudioSource>().Play(); Post_Return_Failed = false; }
    }

    /*public string sendText(string text)   // unused
    {
        return nluApi.GetRecognitionResult("nli", text);
    }*/

    public string sendSpeech(string filePath)
    {        
        string result = speechApi.SendAudioFile(SpeechApiSample.API_NAME_ASR, "nli,seg", true, filePath, false);
        VoiceResult resultInJson = JsonUtility.FromJson<VoiceResult>(result);
        if (!resultInJson.status.ToLower().Equals("error"))
        {
            Debug.Log("\n----- Get Recognition Result -----\n");
            //System.Threading.Thread.Sleep(1000);

            // Try to get result until the end of the recognition is complete
            while (true)
            {
                result = speechApi.GetRecognitionResult(SpeechApiSample.API_NAME_ASR, "nli,seg");
                resultInJson = JsonUtility.FromJson<VoiceResult>(result);
                Debug.Log("Result :\n" + result);
                if (!resultInJson.data.asr.final.Equals(true))
                {
                    Debug.Log("*** The recognition is not yet complete. ***\n");
                    if (resultInJson.status.ToLower().Equals("error")) break;
                    //System.Threading.Thread.Sleep(2000);
                }
                else
                {
                    break;
                }
            }
        }
        return result;
    }

    bool Upload_Failed = false;
    bool Post_Return_Failed = false;
    public string sendSpeech(byte[] audioFile)
    {
        string result = speechApi.SendAudioFile(SpeechApiSample.API_NAME_ASR, "nli,seg", true, audioFile, false);
        Debug.Log("POST request returned :\n" + result);
        VoiceResult resultInJson = JsonUtility.FromJson<VoiceResult>(result);
        if (!resultInJson.status.ToLower().Equals("error"))
        {
            Debug.Log("\n----- Get Recognition Result -----\n");
            //System.Threading.Thread.Sleep(1000);

            // Try to get result until the end of the recognition is complete
            while (true)
            {
                result = speechApi.GetRecognitionResult(SpeechApiSample.API_NAME_ASR, "nli,seg");  // send GET request and get recognition
                resultInJson = JsonUtility.FromJson<VoiceResult>(result);
                Debug.Log("Result :\n" + result);
                if (!resultInJson.data.asr.final.Equals(true))
                {
                    Debug.Log("*** The recognition is not yet complete. ***\n");
                    if (resultInJson.status.ToLower().Equals("error"))
                    {
                        Debug.Log("GET returned status : error");
                        Post_Return_Failed = true;
                        break;
                    }
                    //System.Threading.Thread.Sleep(250);
                }
                else
                {
                    break;
                }
            }
        }
        else
        {
            Debug.Log("post return status : error");
            Upload_Failed = true;
        }
        return result;
    }
}