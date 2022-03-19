using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using RemptyTool.ES_MessageSystem;

[RequireComponent(typeof(ES_MessageSystem))]
public class CalculateChOne : MonoBehaviour
{
    private ES_MessageSystem msgSys;

    public UnityEngine.UI.Text MissionOne;
    public UnityEngine.UI.Text MissionTwo;
    public UnityEngine.UI.Text MissionThree;
    public TextAsset GradeOne;
    private string Mytxt;

    public GameObject S_Grade;
    public GameObject A_Grade;
    public GameObject B_Grade;
    public GameObject F_Grade;

    // Start is called before the first frame update
    void Start()
    {
        msgSys = this.GetComponent<ES_MessageSystem>();
        int monewrong = 2;
        int moneall = 3;
        int mtwowrong = 1;
        int mtwoall = 2;
        int mthreewrong = 1;
        int mthreeall = 3;
        Debug.Log("2II");
        Debug.Log(mtwoall);
        Debug.Log(moneall);
        Debug.Log(mthreeall);

        string MOne =  monewrong.ToString() + "/" + moneall.ToString();;
        string MTwo = mtwowrong.ToString() + "/" + mtwoall.ToString();
        string MThree = mthreewrong.ToString() + "/" + mthreeall.ToString();
        MissionOne.text = MOne;
        MissionTwo.text = MTwo;
        MissionThree.text = MThree;

        var wrong = (float)monewrong + (float)mtwowrong + (float)mthreewrong;
        var all = (float)moneall + (float)mtwoall + (float)mthreeall;
        float grade = wrong / all;

        Debug.Log(wrong);
        Debug.Log(all);
        Debug.Log(grade);

        if (grade > 0.75)
        {
            F_Grade.SetActive(true);
        }
        else if (grade > 0.50)
        {
            B_Grade.SetActive(true);
        }
        else if (grade > 0.25)
        {
            A_Grade.SetActive(true);
        }
        else
        {
            S_Grade.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}