using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System;
public class CharacterChoosing : MonoBehaviour, IBeginDragHandler, IEndDragHandler
{
    private ScrollRect Scroll;
    private float[] pagearray = new float[] { 0, 0.248267f, 0.4989619f, 0.7459146f, 0.9940633f };
    // Start is called before the first frame update
    void Start()
    {
        Scroll = GetComponent<ScrollRect>();
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
        float temp = Scroll.horizontalNormalizedPosition;//拖曳結果座標
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
            Scroll.horizontalNormalizedPosition = pagearray[index];
        }
        Debug.Log(temp);
        print(temp);

    }

    public void ChooseCharacterByVoice(int index)
    {
        Scroll.horizontalNormalizedPosition = pagearray[index];
    }
}
