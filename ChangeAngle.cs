using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeAngle : MonoBehaviour
{
    public GameObject SceneAngle;
    public Slider slider;

    public void SliderChangeAngle()
    {
        float slidervalue = slider.value ;
        Vector3 Rot = SceneAngle.transform.localRotation.eulerAngles;
        Rot.Set(slidervalue, 0f, 0f);
        SceneAngle.transform.localRotation = Quaternion.Euler(Rot);
        Debug.Log(Rot);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}