using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;
using RemptyTool.ES_MessageSystem;

public class PutItemIn : MonoBehaviour, IDropHandler
{
    /// <summary>
    /// CHAPTER 1 : 任務三
    /// </summary>
    public bool good_ingredient_click = false;
    public bool bad_ingredient_click = false;
    public int enoughnum;
    public GameObject EventSys;
    public GameObject WrongImg;
    public GameObject RightImg;
    public GameObject FinishImg;

    ES_MessageSystem es_messageSystem;

    // Start is called before the first frame update
    void Start()
    {
        es_messageSystem = this.EventSys.GetComponent<ES_MessageSystem>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
            es_messageSystem.MissionThreeHint_AS.Stop();
            if (good_ingredient_click == true)
            {
                es_messageSystem.MissionThreeWhatElse.Play();
                es_messageSystem.rightState = true;
                RightImg.SetActive(true);
                es_messageSystem.enough += 1;
                EventSystem.current.currentSelectedGameObject.SetActive(false);
                if (es_messageSystem.enough >= 2)
                {
                    es_messageSystem.MissionThreeWhatElse.Stop();
                    es_messageSystem.MissionThreeHint_AS.Stop();
                    es_messageSystem.MissionThreeRightIngredient.Play();
                    FinishImg.SetActive(true);
                    es_messageSystem.flag_MissionThree = true;
                }
            }
            else if (bad_ingredient_click == true)
            {
                es_messageSystem.MissionThreeWrongIngredient.Play();
                es_messageSystem.wrongState = true;
                WrongImg.SetActive(true);
            }
        }
    }
    
    public void MissionThreeClickRight()
    {
        good_ingredient_click = true;
        bad_ingredient_click = false;        
    }
    public void MissionThreeClickWrong()
    {
        bad_ingredient_click = true;
        good_ingredient_click = false;        
    }    
}
