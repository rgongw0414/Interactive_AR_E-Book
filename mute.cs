using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class mute : MonoBehaviour
{
    AudioSource AudioSource;
    GameObject SoundImage;
    public Sprite MuteImage;
    public Sprite SoundOn_Image;
    public void Mute()
    {
        if(AudioSource.isPlaying == true)
        {
            SoundImage.GetComponent<Image>().sprite = MuteImage;
            AudioSource.Pause();
        }
        else
        {
            SoundImage.GetComponent<Image>().sprite = SoundOn_Image;
            AudioSource.UnPause();            
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        SoundImage = GameObject.Find("mute");
        AudioSource = GameObject.Find("BGM").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
