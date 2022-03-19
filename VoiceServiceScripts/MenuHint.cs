using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// 遊戲開始後第一次到menu時，撥放歡迎提示音效
public class MenuHint : MonoBehaviour
{
    AudioSource AudioSource;
    bool Pressed = false;
    public void ChangeToMenu()
    {
        Pressed = true;
        SceneManager.LoadScene("Menu");
        AudioSource.Play();
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);  // Load到下個場景時持續撥放
    }

    // Start is called before the first frame update
    void Start()
    {
        AudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!AudioSource.isPlaying && Pressed == true)  // 若播放完畢或menu的按鈕被按下，則結束撥放(destroy object)
        {            
            Destroy(gameObject);
        }
    }
}
