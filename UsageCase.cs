using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using RemptyTool.ES_MessageSystem;

[RequireComponent(typeof(ES_MessageSystem))]
public class UsageCase : MonoBehaviour
{
    int num = 0;
    private ES_MessageSystem msgSys;
    public UnityEngine.UI.Text uiText;
    public TextAsset textAsset;
    private List<string> textList = new List<string>();
    private int textIndex = 0;
    public AudioSource AudioSource;  //  存放故事內容的語音

    void Start()
    {        
        msgSys = this.GetComponent<ES_MessageSystem>();
        if (uiText == null)
        {
            Debug.LogError("UIText Component not assign.");
        }
        else
            ReadTextDataFromAsset(textAsset);

        //add special chars and functions in other component.
        //Debug.Log(textList.)
        msgSys.AddSpecialCharToFuncMap("UsageCase", CustomizedFunction);
        AudioSource = GameObject.Find("故事內容語音").GetComponent<AudioSource>();
    }

    private void CustomizedFunction()
    {
        Debug.Log("Hi! This is called by CustomizedFunction!");
    }

    private void ReadTextDataFromAsset(TextAsset _textAsset)
    {
        textList.Clear();
        textList = new List<string>();
        textIndex = 0;
        var lineTextData = _textAsset.text.Split('\n');
        foreach (string line in lineTextData)
        {
            textList.Add(line);
        }
        //Debug.Log(lineTextData.Length);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            //You can sending the messages from strings or text-based files.
            if (msgSys.IsCompleted)
            {
                msgSys.SetText("Send the messages![lr] HelloWorld![w]");
            }
        }

        //If the message is complete, stop updating text.
        if (msgSys.IsCompleted == false)
        {
            uiText.text = msgSys.text;
        }

        //Auto update from textList.
        if (msgSys.IsCompleted == true && textIndex < textList.Count)
        {
            msgSys.audioSource.Stop();
            AudioSource.Stop();  // 停止前一個撥放的語音
            string filename = textIndex.ToString();  
            AudioSource.clip = Resources.Load<AudioClip>(filename);  // 將語音撥放順序與textfile.txt的各行index對上
            AudioSource.Play();
            //Debug.Log("textIndex : " + textIndex);
            msgSys.SetText(textList[textIndex]);
            textIndex++;
        }
        
        if (msgSys.InMission == true && Input.GetMouseButton(0))
        {
            msgSys.Next();
            //Debug.Log("EndOne");
            
        }        
    }

    public void ClickToNext()
    {
        //string filename = string.Format();
        msgSys.Next();
        num++;        
    }
}
