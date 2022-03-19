using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using RemptyTool.ES_MessageSystem;
public class ChangePage : MonoBehaviour
{
    GameObject MenuHint;
    public void ToPrinceCarrot()
    {
        SceneManager.LoadScene("StartMenu");
        DestroyMenuHint();
    }
    public void ToChapterOne()
    {
        SceneManager.LoadScene("ChapterOne");
        DestroyMenuHint();
    }
    public void ToChapterOneEnd()
    {
        SceneManager.LoadScene("MessageOne");
    }
    public void ToChooseChapter()
    {
        SceneManager.LoadScene("ChooseCH");
        DestroyMenuHint();
    }
    public void ToMenu()
    {
        SceneManager.LoadScene("Menu");        
    }
    public void ToIntroduce()
    {
        SceneManager.LoadScene("CharacterInformation");
        DestroyMenuHint();
    }
    public void ToStartPages()
    {
        SceneManager.LoadScene("ChooseBook");
        DestroyMenuHint();
    }

    // 若是MenuHint還未被摧毀，則將其摧毀進而停止音檔撥放。
    public void DestroyMenuHint()
    {
        if (MenuHint != null)
        {
            Destroy(MenuHint);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        MenuHint = GameObject.Find("MenuHint");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}