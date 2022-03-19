﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System;

public class ChooseBook : MonoBehaviour, IBeginDragHandler, IEndDragHandler
{
    private ScrollRect scrollrect;
    private float[] pagearray = new float[] { 0, 0.4458776f, 0.8865706f};

    // Start is called before the first frame update
    void Start()
    {
        scrollrect = GetComponent<ScrollRect>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnBeginDrag(PointerEventData eventData)
    {

    }
    public void OnEndDrag(PointerEventData eventData)
    {
        float temp = scrollrect.horizontalNormalizedPosition;//拖曳結果座標
        int index = 0;
        float offset = Math.Abs(pagearray[index] - temp);
        for (int i = 1; i < pagearray.Length; i++)
        {
            float offsetTemp = Math.Abs(pagearray[i] - temp);
            if (offsetTemp < offset)
            {
                index = i;
                offset = offsetTemp;
            }
            scrollrect.horizontalNormalizedPosition = pagearray[index];
        }
        Debug.Log(temp);
        print(temp);

    }
}
