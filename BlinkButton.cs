using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlinkButton : MonoBehaviour
{
    public GameObject ButtonImg;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("ShowHide", 0.5f, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void ShowHide()
    {
        if (ButtonImg.activeInHierarchy)
        {
            ButtonImg.SetActive(false);
        }
        else
        {
            ButtonImg.SetActive(true);
        }
    }
}