﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.IO;
using System.Text;
using UnityEngine.UI;

namespace RemptyTool.ES_MessageSystem
{
    /// <summary>The messageSystem is made by Rempty EmptyStudio. 
    /// UserFunction
    ///     SetText(string) -> Make the system to print or execute the commands.
    ///     Next()          -> If the system is WaitingForNext, then it will continue the remaining contents.
    ///     AddSpecialCharToFuncMap(string _str, Action _act)   -> You can add your customized special-characters into the function map.
    /// Parameters
    ///     IsCompleted     -> Is the input text parsing completely by the system.
    ///     text            -> The result, witch you can show on your interface as a dialog.
    ///     IsWaitingForNext-> Waiting for user input -> The Next() function.
    ///     textSpeed       -> Setting the updating period of text.
    /// </summary> 
    public class ES_MessageSystem : MonoBehaviour
       // , IDropHandler
    {
        public bool IsCompleted { get { return IsMsgCompleted; } }
        public string text { get { return msgText; } }
        public bool IsWaitingForNext { get { return IsWaitingForNextToGo; } }
        public float textSpeed = 0.01f; //Updating period of text. The actual period may not less than deltaTime.

        private const char SPECIAL_CHAR_STAR = '[';
        private const char SPECIAL_CHAR_END = ']';
        private enum SpecialCharType { StartChar, CmdChar, EndChar, NormalChar }
        private bool IsMsgCompleted = true;
        private bool IsOnSpecialChar = false;
        private bool IsWaitingForNextToGo = false;
        private bool IsOnCmdEvent = false;
        private string specialCmd = "";
        private string msgText;
        private char lastChar = ' ';

        //public Animator moveposition;
        public Animator behavior;
        public Animator pot_ani;
        public Animator broom_ani;

        public Text readText;
        //private string path;
        public int moneall = 0;
        public int monewrong = 0;
        public int mtwoall = 0;
        public int mtwowrong = 0;
        public int mthreeall = 0;
        public int mthreewrong = 0;
        //string str;

        public bool InMission = false;
        public AudioSource ChapterOneClear_AS; // 第一章完成音效AS
        public AudioSource Mission_Clear_AS; // 任務完成音效AS
        public GameObject MissionOne;//任務一菜单
        public GameObject CoverBrush;
        public GameObject CoverBroom;
        public GameObject CoverBucket;
        public GameObject BottonBrush;
        public GameObject BottonBroom;
        public GameObject BottonBucket;
        public GameObject ShockCarrot;
        public GameObject GoodCarrot;
        public GameObject microphone_button; // 麥克風按鈕
        public AudioSource Mission_One_Hint_Speech;
        public UnityEngine.UI.Text OneTalking;
        /*public Sprite CorrectMone;//任務一正確按鈕
        public Sprite WrongI_Mone;//任務一錯誤按鈕一
        public Sprite WrongII_Mone;//任務一錯誤按鈕二*/
        public bool flag_MissionOne = false;
        bool flag_MissionOne_Click = false;
        public bool broom = false;
        public bool bucket = false;
        public bool brush = false;
        public AudioSource Broom_AS;
        public AudioSource Bucket_AS;
        public AudioSource Brush_AS;

        public GameObject Mission_Two_Hint_Speech;
        public GameObject MissionTwo;//任務二菜单
        public GameObject SharkTwo;
        public GameObject GoodTwo;
        public bool flag_MissionTwo = false;
        public bool flag_ClickToContinuteTwo = false;
        public UnityEngine.UI.Text TwoTalking;
        public UnityEngine.UI.Text ThreeLogText;
        public UnityEngine.UI.Text FourLogText;
        public UnityEngine.UI.Text FiveLogText;
        public int LogsNumber = 0;
        public int ThreeLogNum = 0;
        public int FourLogNum = 0;
        public int FiveLogNum = 0;

        public GameObject Mission_Three_Hint_Speech;
        public GameObject MissionThree;//任務三菜单
        public GameObject TalkingBox;
        //public GameObject microphone_button;
        public GameObject Pot;
        public bool flag_MissionThree = false;
        public int enough = 0;
        public GameObject WrongImg;
        public GameObject RightImg;
        
        public bool wrongState = false;
        public bool rightState = false;

        private Dictionary<string, Action> specialCharFuncMap = new Dictionary<string, Action>();
        public AudioSource audioSource; // 用來撥放MissionTWO的木柴還缺多少
        UsageCase UsageCase;
        void Start()
        {
            UsageCase = gameObject.GetComponent<UsageCase>();
            GameObject.Find("BGM").GetComponent<AudioSource>().volume = 0.3f;
            ChapterOneClear_AS = GameObject.Find("第一章結束音效").GetComponent<AudioSource>();
            Mission_Clear_AS = GameObject.Find("任務完成音效").GetComponent<AudioSource>();
            audioSource = GameObject.Find("mission_two_hint_audio").GetComponent<AudioSource>();
            
            specialCharFuncMap.Add("w", () => StartCoroutine(CmdFun_w_Task()));
            specialCharFuncMap.Add("r", () => StartCoroutine(CmdFun_r_Task()));
            specialCharFuncMap.Add("l", () => StartCoroutine(CmdFun_l_Task()));
            specialCharFuncMap.Add("lr", () => StartCoroutine(CmdFun_lr_Task()));
            specialCharFuncMap.Add("mone", () => StartCoroutine(StartMissionOne()));
            specialCharFuncMap.Add("mtwo", () => StartCoroutine(StartMissionTwo()));
            specialCharFuncMap.Add("mthree", () => StartCoroutine(StartMissionThree()));
            specialCharFuncMap.Add("endChI", () => StartCoroutine(EndChapterI()));
        }

        void Update()
        {
            if (Goal_Log_Num >= 0)
            {
                if (clicked == true)
                {
                    GameObject.Find("聽不懂").GetComponent<AudioSource>().Stop();
                    if (Goal_Log_Num != 0) { TwoTalking.text = "還缺" + Goal_Log_Num.ToString() + "根木柴"; }
                    audioSource.clip = Resources.Load<AudioClip>("owe" + Goal_Log_Num.ToString());
                    audioSource.Play();
                    clicked = false;
                }                
            }
            else
            {
                audioSource.clip = Resources.Load<AudioClip>("too_much");
                audioSource.Play();
                clicked = false;
                Goal_Log_Num = 11;
            }

            // 任務一
            if (broom == true) { MissionOneClickBroom(); broom = false; }
            if (bucket == true) { MissionOneClickBucket(); bucket = false; }
            if (brush == true) { MissionOneClickBrush(); brush = false; }

            //任務二
            if (Three == true) { ClickThree(); Three = false; }
            if (Four == true) { ClickFour(); Four = false; }
            if (Five == true) { ClickFive(); Five = false; }

            // 新增動畫資訊
            if (behavior.GetCurrentAnimatorStateInfo(0).IsName("moveToBroom"))
            {
                behavior.SetBool("ToBroom", false);
                behavior.SetBool("ToPickBroom", true);
            }
            if (behavior.GetCurrentAnimatorStateInfo(0).IsName("PickBroom"))
            {
                broom_ani.SetBool("Start", true);
                behavior.SetBool("ToPickBroom", false);
                behavior.SetBool("PickBroom", true);
                behavior.SetBool("ToCenter", true);
            }
            if (behavior.GetCurrentAnimatorStateInfo(0).IsName("moveToCenter"))
            {
                broom_ani.SetBool("ToCenter", true);
                behavior.SetBool("PickBroom", false);
                //behavior.SetBool("ToCenter", false);
                behavior.SetBool("ToStaySweep", true);
            }
            if (behavior.GetCurrentAnimatorStateInfo(0).IsName("staySweep"))
            {
                broom_ani.SetBool("Sweep", true);
                behavior.SetBool("ToStaySweep", false);
                behavior.SetBool("Sweeping", true);
            }
            if (behavior.GetCurrentAnimatorStateInfo(0).IsName("moveToFire"))
            {
                broom_ani.SetBool("ToFire", true);
                behavior.SetBool("Sweeping", false);
                //behavior.SetBool("ToFire", false);
                behavior.SetBool("ToStayFire", true);
            }
            if (behavior.GetCurrentAnimatorStateInfo(1).IsName("something"))
            {
                behavior.SetBool("Firing", false);
            }
            if (behavior.GetCurrentAnimatorStateInfo(0).IsName("moveToTable"))
            {
                behavior.SetBool("ToTable", false);
                behavior.SetBool("ToStayTable", true);
            }
            if (behavior.GetCurrentAnimatorStateInfo(0).IsName("stayTable"))
            {
                behavior.SetBool("ToStayTable", false);
                //behavior.SetBool("", true);
            }
            if (behavior.GetCurrentAnimatorStateInfo(1).IsName("pot_pick"))
            {
                behavior.SetBool("PickPot", false);
                behavior.SetBool("ToCook", true);
            }
            if (behavior.GetCurrentAnimatorStateInfo(0).IsName("moveToCook"))
            {
                pot_ani.SetBool("StartMove", true);
            }
            if (behavior.GetCurrentAnimatorStateInfo(0).IsName("stayCook"))
            {
                pot_ani.SetBool("Tthrow", true);
                behavior.SetBool("Cooking", true);
            }
        }

        public void Microphone_Button_Pressed()  // 若麥克風被按下，則停止第一章其他物件的聲音
        {
            Broom_AS.Stop();
            Brush_AS.Stop();
            Bucket_AS.Stop();
        }

        //任務一按鍵設置
        public void MissionOneClickBroom()
        {
            CoverBroom.SetActive(false);
            GoodCarrot.SetActive(true);
            OneTalking.text = "就是這個!";
            Mission_One_Hint_Speech.Stop();            
            Bucket_AS.Stop();
            Brush_AS.Stop();
            Broom_AS.Play();
            Mission_Clear_AS.Play();
            flag_MissionOne = true;
            moneall++;

        }
        public void MissionOneClickBucket()
        {
            CoverBucket.SetActive(false);
            ShockCarrot.SetActive(true);
            OneTalking.text = "會弄濕地板";
            Mission_One_Hint_Speech.Stop();
            Brush_AS.Stop();
            Broom_AS.Stop();
            Bucket_AS.Play();
            flag_MissionOne_Click = true;
            moneall++;
            monewrong++;
        }
        public void MissionOneClickBrush()
        {
            CoverBrush.SetActive(false);
            ShockCarrot.SetActive(true);
            OneTalking.text = "掃不起來";            
            Mission_One_Hint_Speech.Stop();
            Bucket_AS.Stop();
            Broom_AS.Stop();
            Brush_AS.Play();
            flag_MissionOne_Click = true;
            moneall++;
            monewrong++;
        }

        int Goal_Log_Num = 11;
        bool clicked = false;
        public bool Three, Four, Five = false;  // 對應到下面三個function
        //任務二案件設置
        public void ClickThree()
        {
            UsageCase.AudioSource.Stop();
            ThreeLogNum += 1;
            ThreeLogText.text = ThreeLogNum.ToString();
            LogsNumber += 3;
            Goal_Log_Num -= 3;
            clicked = true;
        }
        public void ClickFour()
        {
            UsageCase.AudioSource.Stop();
            FourLogNum += 1;
            FourLogText.text = FourLogNum.ToString();
            LogsNumber += 4;
            Goal_Log_Num -= 4;
            clicked = true;
        }
        public void ClickFive()
        {
            UsageCase.AudioSource.Stop();
            FiveLogNum += 1;
            FiveLogText.text = FiveLogNum.ToString();
            LogsNumber += 5;
            Goal_Log_Num -= 5;
            clicked = true;
        }

        //任務三案件設置
        
        #region Public Function
        public void AddSpecialCharToFuncMap(string _str, Action _act)
        {
            specialCharFuncMap.Add(_str, _act);
        }
        #endregion

        #region User Function
        public void Next()
        {
            IsWaitingForNextToGo = false;
        }
        public void SetText(string _text)
        {
            StartCoroutine(SetTextTask(_text));
        }
        #endregion

        #region Keywords Function
        private IEnumerator CmdFun_l_Task()
        {
            IsOnCmdEvent = true;
            IsWaitingForNextToGo = true;
            yield return new WaitUntil(() => IsWaitingForNextToGo == false);
            IsOnCmdEvent = false;
            yield return null;
        }
        private IEnumerator CmdFun_r_Task()
        {
            IsOnCmdEvent = true;
            msgText += '\n';
            IsOnCmdEvent = false;
            yield return null;
        }
        private IEnumerator CmdFun_w_Task()
        {
            IsOnCmdEvent = true;
            IsWaitingForNextToGo = true;
            yield return new WaitUntil(() => IsWaitingForNextToGo == false);
            msgText = "";   //Erase the messages.
            IsOnCmdEvent = false;
            yield return null;
        }
        private IEnumerator CmdFun_lr_Task()
        {
            IsOnCmdEvent = true;
            IsWaitingForNextToGo = true;
            yield return new WaitUntil(() => IsWaitingForNextToGo == false);
            msgText += '\n';
            IsOnCmdEvent = false;
            yield return null;
        }
        private IEnumerator StartMissionOne()//開始任務一
        {

            Debug.Log("StartOne");
            InMission = true;
            MissionOne.SetActive(true);
            TalkingBox.SetActive(false);
            microphone_button.SetActive(true);
            Mission_One_Hint_Speech.Play();
            while (flag_MissionOne == false)
            {
                IsOnCmdEvent = true;
                //IsWaitingForNextToGo = true;
                yield return new WaitUntil(() => IsWaitingForNextToGo == false);
                if (flag_MissionOne_Click == true)
                {

                    IsOnCmdEvent = true;
                    IsWaitingForNextToGo = true;
                    yield return new WaitUntil(() => IsWaitingForNextToGo == false);
                    //OneTalking.text = "掃地要用...";
                    flag_MissionOne_Click = false;
                }
            }
            
            IsOnCmdEvent = true;
            IsWaitingForNextToGo = true;
            yield return new WaitUntil(() => IsWaitingForNextToGo == false);            

            TalkingBox.SetActive(true);
            microphone_button.SetActive(false);            
            msgText = "";   //Erase the messages.
            MissionOne.SetActive(false);

            //IsOnCmdEvent = true;
            //IsWaitingForNextToGo = true;
            //yield return new WaitUntil(() => IsWaitingForNextToGo == false);            
            
            behavior.SetBool("ToBroom", true);
            /*behavior.SetBool("ToCenter", true);
            behavior.SetBool("ToStaySweep", true);*/
            //MissionOne.SetActive(false);
            InMission = false;
            IsOnCmdEvent = false;
            yield return null;
        }

        
        private IEnumerator StartMissionTwo()//開始任務二
        {            
            behavior.SetBool("ToFire", true);
            InMission = true;
            /*IsOnCmdEvent = true;
            IsWaitingForNextToGo = true;
            yield return new WaitUntil(() => IsWaitingForNextToGo == false);*/
            MissionTwo.SetActive(true);
            TalkingBox.SetActive(false);
            microphone_button.SetActive(true);
            while (flag_MissionTwo == false)
            {
                IsOnCmdEvent = true;
                //IsWaitingForNextToGo = true;
                yield return new WaitUntil(() => IsWaitingForNextToGo == false);
                if (LogsNumber >11)//重製數據
                {
                    TwoTalking.text = "太多了";
                    SharkTwo.SetActive(true);

                    flag_ClickToContinuteTwo = true;
                    IsOnCmdEvent = true;
                    IsWaitingForNextToGo = true;
                    yield return new WaitUntil(() => IsWaitingForNextToGo == false);

                    ThreeLogNum = 0;
                    FourLogNum = 0;
                    FiveLogNum = 0;
                    LogsNumber = 0;
                    ThreeLogText.text = "0";
                    FourLogText.text = "0";
                    FiveLogText.text = "0";
                    TwoTalking.text = "需要11根木柴";
                    SharkTwo.SetActive(false);

                    mtwoall++;
                    mtwowrong++;
                }
                else if (LogsNumber == 11)
                {
                    Mission_Clear_AS.Play();
                    TwoTalking.text = "這樣剛剛好";
                    GoodTwo.SetActive(true);
                    IsOnCmdEvent = true;
                    IsWaitingForNextToGo = true;
                    yield return new WaitUntil(() => IsWaitingForNextToGo == false);
                    flag_MissionTwo = true;

                    mtwoall++;
                }
            }
            TalkingBox.SetActive(true);
            microphone_button.SetActive(false);
            behavior.SetBool("Firing", true);

            msgText = "";   //Erase the messages.
            /*IsOnCmdEvent = true;
            IsWaitingForNextToGo = true;
            yield return new WaitUntil(() => IsWaitingForNextToGo == false);*/
            MissionTwo.SetActive(false);
            InMission = false;
            IsOnCmdEvent = false;
            yield return null;
        }

        public AudioSource MissionThreeHint_AS;
        public AudioSource MissionThreeRightIngredient;
        public AudioSource MissionThreeWrongIngredient;
        public AudioSource MissionThreeWhatElse;
        private IEnumerator StartMissionThree()//開始任務三
        {
            MissionThreeHint_AS = GameObject.Find("第三關提示語音").GetComponent<AudioSource>();
            MissionThreeRightIngredient = GameObject.Find("第三關正確食材").GetComponent<AudioSource>();
            MissionThreeWrongIngredient = GameObject.Find("第三關錯誤食材").GetComponent<AudioSource>();
            MissionThreeWhatElse = GameObject.Find("第三關還缺什麼").GetComponent<AudioSource>();
            MissionThreeHint_AS.Play();
            InMission = true;
            behavior.SetBool("ToTable", true);
            Pot.SetActive(true);

            MissionThree.SetActive(true);
            TalkingBox.SetActive(false);
            microphone_button.SetActive(true);
            //flag_MissionThree = false;
            //Debug.Log(enough);
            while (flag_MissionThree == false)
            {
                if (wrongState == true)
                {
                    WrongImg.SetActive(true);
                    IsOnCmdEvent = true;
                    IsWaitingForNextToGo = true;
                    yield return new WaitUntil(() => IsWaitingForNextToGo == false);
                    wrongState = false;
                    WrongImg.SetActive(false);

                    mthreeall++;
                    mthreewrong++;
                }
                if (rightState == true)
                {
                    RightImg.SetActive(true);
                    IsOnCmdEvent = true;
                    IsWaitingForNextToGo = true;
                    yield return new WaitUntil(() => IsWaitingForNextToGo == false);
                    rightState = false;
                    RightImg.SetActive(false);
                    mthreeall++;
                }
                //Debug.Log(enough);
                IsOnCmdEvent = true;
                IsWaitingForNextToGo = true;
                yield return new WaitUntil(() => IsWaitingForNextToGo == false);
                //Debug.Log(flag_MissionThree);
                
            }
            //Debug.Log(flag_MissionThree);
            MissionThree.SetActive(false);
            TalkingBox.SetActive(true);
            microphone_button.SetActive(false);
            IsOnCmdEvent = true;
            IsWaitingForNextToGo = true;
            yield return new WaitUntil(() => IsWaitingForNextToGo == false);
            msgText = "羅宋湯完成了";
            ChapterOneClear_AS.Play();
            behavior.SetBool("PickPot", true);

            IsOnCmdEvent = true;
            IsWaitingForNextToGo = true;
            yield return new WaitUntil(() => IsWaitingForNextToGo == false);
                
            msgText = "";   //Erase the messages.
            /*IsOnCmdEvent = true;
            IsWaitingForNextToGo = true;
            yield return new WaitUntil(() => IsWaitingForNextToGo == false);*/

            InMission = false;
            IsOnCmdEvent = false;
            yield return null;
        }
       
        private IEnumerator EndChapterI()
        {
            /*if (!File.Exists(path))
            {
                FileStream fileStream = new FileStream(path, FileMode.OpenOrCreate);
                StreamWriter sw = new StreamWriter(fileStream, Encoding.UTF8);

                string str = monewrong.ToString() + "/" + moneall.ToString() + "\n" +
                            mtwowrong.ToString() + "/" + mtwoall.ToString() + "\n" +
                            mthreewrong.ToString() + "/" + mthreeall.ToString();
                //Debug.Log(str);
                sw.WriteLine(str);
                sw.Close();
                fileStream.Close();
            }
            else
            {
                FileInfo fileInfo = new FileInfo(path);
                fileInfo.Delete();

                FileStream fileStream = new FileStream(path, FileMode.OpenOrCreate);
                StreamWriter sw = new StreamWriter(fileStream, Encoding.UTF8);

                string str = monewrong.ToString() + "/" + moneall.ToString() + "\n" +
                            mtwowrong.ToString() + "/" + mtwoall.ToString() + "\n" +
                            mthreewrong.ToString() + "/" + mthreeall.ToString();
                //Debug.Log(str);
                sw.WriteLine(str);
                sw.Close();
                fileStream.Close();
            }*/

            /*File.AppendAllText(path, monewrong.ToString() + "/" + moneall.ToString() + "\n" +
                            mtwowrong.ToString() + "/" + mtwoall.ToString() + "\n" +
                            mthreewrong.ToString() + "/" + mthreeall.ToString() , Encoding.Default);*/

            IsOnCmdEvent = true;
            IsWaitingForNextToGo = true;
            yield return new WaitUntil(() => IsWaitingForNextToGo == false);
            SceneManager.LoadScene("MessageOne");
        }
        #endregion

        #region Messages Core
        private void AddChar(char _char)
        {
            msgText += _char;
            lastChar = _char;
        }
        private SpecialCharType CheckSpecialChar(char _char)
        {
            if (_char == SPECIAL_CHAR_STAR)
            {
                if (lastChar == SPECIAL_CHAR_STAR)
                {
                    specialCmd = "";
                    IsOnSpecialChar = false;
                    return SpecialCharType.NormalChar;
                }
                IsOnSpecialChar = true;
                return SpecialCharType.CmdChar;
            }
            else if (_char == SPECIAL_CHAR_END && IsOnSpecialChar)
            {
                //exe cmd!
                if (specialCharFuncMap.ContainsKey(specialCmd))
                {
                    specialCharFuncMap[specialCmd]();
                    //Debug.Log("The keyword : [" + specialCmd + "] execute!");
                }
                else
                    Debug.LogError("The keyword : [" + specialCmd + "] is not exist!");
                specialCmd = "";
                IsOnSpecialChar = false;
                return SpecialCharType.EndChar;
            }
            else if (IsOnSpecialChar)
            {
                specialCmd += _char;
                return SpecialCharType.CmdChar;
            }
            return SpecialCharType.NormalChar;
        }
        private IEnumerator SetTextTask(string _text)
        {
            IsOnSpecialChar = false;
            IsMsgCompleted = false;
            specialCmd = "";
            for (int i = 0; i < _text.Length; i++)
            {
                switch (CheckSpecialChar(_text[i]))
                {
                    case SpecialCharType.NormalChar:
                        AddChar(_text[i]);
                        lastChar = _text[i];
                        yield return new WaitForSeconds(textSpeed);
                        break;
                }
                lastChar = _text[i];
                yield return new WaitUntil(() => IsOnCmdEvent == false);
            }
            IsMsgCompleted = true;
            yield return null;
        }
        #endregion
    }
}