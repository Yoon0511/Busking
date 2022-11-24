using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialMng_Main : MonoBehaviour {

    private static TutorialMng_Main instance = null;

    public static TutorialMng_Main Data
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType(typeof(TutorialMng_Main)) as TutorialMng_Main;
                if (instance == null)
                {
                    Debug.Log("no instance");
                }
            }
            return instance;
        }
    }

    [SerializeField]
    List<GameObject> _Tutorial_MainUI = new List<GameObject>();
    [SerializeField]
    List<GameObject> _Tutorial_UpgradeUI = new List<GameObject>();
    [SerializeField]
    List<GameObject> _Tutorial_OptionUI = new List<GameObject>();
    [SerializeField]
    List<GameObject> _Tutorial_InfinityUI = new List<GameObject>();

    List<List<GameObject>> _Tutorials = new List<List<GameObject>>();

    int _NowTutorialNum;
    int _NowSlideNum;
   

    void Awake()
    {
        _Tutorials.Add(_Tutorial_MainUI);
        _Tutorials.Add(_Tutorial_UpgradeUI);
        _Tutorials.Add(_Tutorial_OptionUI);
        _Tutorials.Add(_Tutorial_InfinityUI);
    }
    public void CheckTutorialClear(int num)
    {
        //PlayerPrefs.SetInt("Tutorial_UI_" + num.ToString(), 0);
        if(PlayerPrefs.GetInt("Tutorial_UI_"+num.ToString())==0)
        {
            PlayerPrefs.SetInt("Tutorial_UI_" + num.ToString(), 1);
            StartTutorial(num);
        }
    }

    public void StartTutorial(int num)
    {
        _NowTutorialNum = num;
        _NowSlideNum = 0;
        _Tutorials[_NowTutorialNum][_NowSlideNum].SetActive(true);
    }
    public void NextSlide()
    {
        _NowSlideNum++;
        for(int i=0;i<_Tutorials[_NowTutorialNum].Count;i++)
        {
            _Tutorials[_NowTutorialNum][i].SetActive(false);
            if (_NowSlideNum == i)
                _Tutorials[_NowTutorialNum][_NowSlideNum].SetActive(true);
        }
    }
}
