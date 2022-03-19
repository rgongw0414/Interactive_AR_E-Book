using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using RemptyTool.ES_MessageSystem;
using UnityEngine.EventSystems;

public class PlayerVoiceControl : MonoBehaviour
{
    [SerializeField]
    ES_MessageSystem eS_MessageSystem;

    /// <summary>
    /// CHAPTER 1 : 任務三
    /// </summary>    
    bool tomato, potato, corn, banana, onion = false;
    public GameObject EventSys;
    public GameObject WrongImg;
    public GameObject RightImg;
    public GameObject FinishImg;
    public PutItemIn PutItemIn;
    
    public void Mission3JudgementFunction()
    {
        eS_MessageSystem.MissionThreeHint_AS.Stop(); 
        if (PutItemIn.good_ingredient_click == true)
        {
            eS_MessageSystem.MissionThreeWhatElse.Play(); 
            eS_MessageSystem.rightState = true;
            PutItemIn.RightImg.SetActive(true);  
            eS_MessageSystem.enough += 1;
            
            
            if (eS_MessageSystem.enough >= 2)
            {
                eS_MessageSystem.MissionThreeWhatElse.Stop();
                eS_MessageSystem.MissionThreeRightIngredient.Play();
                PutItemIn.FinishImg.SetActive(true); 
                eS_MessageSystem.flag_MissionThree = true;                
            }
        }
        else if (PutItemIn.bad_ingredient_click == true)
        {
            eS_MessageSystem.MissionThreeWrongIngredient.Play();     
            eS_MessageSystem.wrongState = true;
            PutItemIn.WrongImg.SetActive(true);    
        }
    }

    // 任務三選擇錯誤時，搖動物體
    bool shaking = false;
    IEnumerator shakeGameObjectCOR(GameObject objectToShake, float totalShakeDuration, float decreasePoint, bool objectIs2D = false)
    {
        if (decreasePoint >= totalShakeDuration)
        {
            Debug.LogError("decreasePoint must be less than totalShakeDuration...Exiting");
            yield break; //Exit!
        }

        //Get Original Pos and rot
        Transform objTransform = objectToShake.transform;
        Vector3 defaultPos = objTransform.position;
        Quaternion defaultRot = objTransform.rotation;

        float counter = 0f;  // 計時器，用來存放時間

        //Shake Speed
        const float speed = 0.1f;

        //Angle Rotation(Optional)
        const float angleRot = 4;  //震動幅度

        //Do the actual shaking
        while (counter < totalShakeDuration)
        {
            counter += Time.deltaTime;  // 執行了多久，加到計時器
            float decreaseSpeed = speed;
            float decreaseAngle = angleRot;

            //Shake GameObject
            if (objectIs2D)
            {
                //Don't Translate the Z Axis if 2D Object
                Vector3 tempPos = defaultPos + UnityEngine.Random.insideUnitSphere * decreaseSpeed;
                tempPos.z = defaultPos.z;
                objTransform.position = tempPos;

                //Only Rotate the Z axis if 2D
                objTransform.rotation = defaultRot * Quaternion.AngleAxis(UnityEngine.Random.Range(-angleRot, angleRot), new Vector3(0f, 0f, 1f));
            }
            else
            {
                objTransform.position = defaultPos + UnityEngine.Random.insideUnitSphere * decreaseSpeed;
                objTransform.rotation = defaultRot * Quaternion.AngleAxis(UnityEngine.Random.Range(-angleRot, angleRot), new Vector3(1f, 1f, 1f));
            }
            yield return null;


            //Check if we have reached the decreasePoint then start decreasing  decreaseSpeed value
            if (counter >= decreasePoint)
            {
                Debug.Log("Decreasing shake");

                //Reset counter to 0 
                counter = 0f;
                while (counter <= decreasePoint)
                {
                    counter += Time.deltaTime;
                    decreaseSpeed = Mathf.Lerp(speed, 0, counter / decreasePoint);
                    decreaseAngle = Mathf.Lerp(angleRot, 0, counter / decreasePoint);

                    //Debug.Log("Decrease Value: " + decreaseSpeed);

                    //Shake GameObject
                    if (objectIs2D)
                    {
                        //Don't Translate the Z Axis if 2D Object
                        Vector3 tempPos = defaultPos + UnityEngine.Random.insideUnitSphere * decreaseSpeed;
                        tempPos.z = defaultPos.z;
                        objTransform.position = tempPos;

                        //Only Rotate the Z axis if 2D
                        objTransform.rotation = defaultRot * Quaternion.AngleAxis(UnityEngine.Random.Range(-decreaseAngle, decreaseAngle), new Vector3(0f, 0f, 1f));
                    }
                    else
                    {
                        objTransform.position = defaultPos + UnityEngine.Random.insideUnitSphere * decreaseSpeed;
                        objTransform.rotation = defaultRot * Quaternion.AngleAxis(UnityEngine.Random.Range(-decreaseAngle, decreaseAngle), new Vector3(1f, 1f, 1f));
                    }
                    yield return null;
                }

                //Break from the outer loop
                break;
            }
        }
        objTransform.position = defaultPos; //Reset to original postion
        objTransform.rotation = defaultRot;//Reset to original rotation

        shaking = false; //So that we can call this function next time
        Debug.Log("Done!");
    }
    void shakeGameObject(GameObject objectToShake, float shakeDuration, float decreasePoint, bool objectIs2D = false)
    {
        if (shaking)
        {
            return;
        }
        shaking = true;
        StartCoroutine(shakeGameObjectCOR(objectToShake, shakeDuration, decreasePoint, objectIs2D));
    }    

    CharacterChoosing CharacterChoosing;
    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetActiveScene().name == "CharactarInformation") { CharacterChoosing = GameObject.Find("Scroll").GetComponent<CharacterChoosing>(); }            
    }

    // Update is called once per frame
    void Update()
    {
        if (Mission3Judgement == true) { Mission3JudgementFunction(); Mission3Judgement = false; }
        if (menu == true) { SceneManager.LoadScene("Menu"); menu = false; }
        if (chapter_choose == true) { SceneManager.LoadScene("chooseCH"); chapter_choose = false; }
        if (character_introduction == true) { SceneManager.LoadScene("CharactarInformation"); character_introduction = false; }
        if (ch1 == true) { SceneManager.LoadScene("ChapterOne"); ch1 = false; }
        if (ch2 == true) { SceneManager.LoadScene("ChapterTwo"); ch2 = false; }
        if (ch3 == true) { SceneManager.LoadScene("ChapterThree"); ch3 = false; }
        if (ch4 == true) { SceneManager.LoadScene("ChapterFour"); ch4 = false; }
        if (ch5 == true) { SceneManager.LoadScene("ChapterFive"); ch5 = false; }
        if (dont_understand == true) { GameObject.Find("聽不懂").GetComponent<AudioSource>().Play(); dont_understand = false; }        
        if (tomato == true) { tomato = false; GameObject.Find("Tomato").SetActive(false); }
        if (potato == true) { potato = false; GameObject.Find("Potato").SetActive(false); }
        if (corn == true) { corn = false;  shakeGameObject(GameObject.Find("corn"), 2, 1f, false); }
        if (banana == true) { banana = false; shakeGameObject(GameObject.Find("Banana"), 2, 1f, false); }
        if (onion == true) { onion = false; shakeGameObject(GameObject.Find("Onion"), 2, 1f, false); }
    }

    bool menu = false;
    bool chapter_choose = false;
    bool character_introduction = false;
    bool ch1, ch2, ch3, ch4, ch5 = false;
    bool dont_understand = false;  // 聽不懂使用者說什麼(辨識結果不在OLAMI模組裡)
    bool Mission3Judgement = false;
    public void ProcessSemantic(Semantic sem) // 判斷Olami回傳的semantic case
    {
        string modifier = sem.modifier[0]; // modifier is an array object
        string semantic = sem.app;
        Debug.Log("Processing Semantic");
        if (semantic == "mission_one") //自己在OLAMI上的module name
        {
            switch (modifier)
            {
                case "broom":
                    {
                        eS_MessageSystem.broom = true;
                    }
                    break;
                case "bucket":
                    {
                        eS_MessageSystem.bucket = true;

                    }
                    break;
                case "brush":
                    {
                        eS_MessageSystem.brush = true;
                    }
                    break;                
                default:
                    {
                        dont_understand = true;
                    }
                    break;
            }
            return;
        }
        else if (semantic == "mission_two")
        {
            switch (modifier)
            {
                case "logs_3":
                    {
                        eS_MessageSystem.Three = true;
                    }
                    break;
                case "logs_4":
                    {
                        eS_MessageSystem.Four = true;
                    }
                    break;
                case "logs_5":
                    {
                        eS_MessageSystem.Five = true;
                    }
                    break;
                default:
                    {
                        dont_understand = true;
                    }
                    break;
            }
        }
        else if (semantic == "mission_three")
        {
            switch (modifier)
            {                
                case "tomato":
                    {
                        Mission3Judgement = true;
                        tomato = true;
                        PutItemIn.MissionThreeClickRight();
                    }
                    break;
                case "potato":
                    {
                        Mission3Judgement = true;
                        potato = true;
                        PutItemIn.MissionThreeClickRight();
                    }
                    break;
                case "corn":
                    {
                        Mission3Judgement = true;
                        corn = true;
                        PutItemIn.MissionThreeClickWrong();
                    }
                    break;
                case "onion":
                    {
                        Mission3Judgement = true;
                        onion = true;
                        PutItemIn.MissionThreeClickWrong();
                    }
                    break;
                case "banana":
                    {
                        Mission3Judgement = true;
                        banana = true;
                        PutItemIn.MissionThreeClickWrong();
                    }
                    break;
                default:
                    {
                        dont_understand = true;
                    }
                    break;
            }
        }
        else if (semantic == "chapter_choose")
        {
            switch (modifier)
            {
                case "menu":
                    {
                        menu = true;
                    }
                    break;
                case "ch1":
                    {
                        ch1 = true;
                    }
                    break;
                case "ch2":
                    {
                        ch2 = true;
                    }
                    break;
                case "ch3":
                    {
                        ch3 = true;
                    }
                    break;
                case "ch4":
                    {
                        ch4 = true;
                    }
                    break;
                case "ch5":
                    {
                        ch5 = true;
                    }
                    break;
                default:
                    {
                        dont_understand = true;
                    }
                    break;
            }
        }
        else if (semantic == "menu")
        {
            switch (modifier)
            {
                case "chapter_choose":
                    {
                        chapter_choose = true;
                    }
                    break;
                case "character_introduction":
                    {
                        character_introduction = true;
                    }
                    break;
                case "start_reading":
                    {
                        ch1 = true;
                    }
                    break;
                default:
                    {
                        dont_understand = true;
                    }
                    break;
            }
        }
        else if (semantic == "character_introduction")
        {
            switch (modifier)
            {
                case "仙杜蘿蔔":
                    {
                        CharacterChoosing.ChooseCharacterByVoice(0);
                    }
                    break;
                case "白玉蘿蔔":
                    {
                        CharacterChoosing.ChooseCharacterByVoice(1);
                    }
                    break;                
                case "繼母":
                    {
                        CharacterChoosing.ChooseCharacterByVoice(2);
                    }
                    break;
                case "魔法師":
                    {
                        CharacterChoosing.ChooseCharacterByVoice(3);
                    }
                    break;
                case "王子":
                    {
                        CharacterChoosing.ChooseCharacterByVoice(4);
                    }
                    break;                
                default:
                    {
                        dont_understand = true;
                    }
                    break;
            }
        }
        else
        {
            dont_understand = true;
            Debug.Log("modifier is not in Olami module.");
        }
    }
}