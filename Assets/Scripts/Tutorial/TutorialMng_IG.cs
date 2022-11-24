using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialMng_IG : MonoBehaviour {

    private static TutorialMng_IG instance = null;

    public static TutorialMng_IG Data
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType(typeof(TutorialMng_IG)) as TutorialMng_IG;
                if (instance == null)
                {
                    Debug.Log("no instance");
                }
            }
            return instance;
        }
    }

    [SerializeField]
    List<GameObject> _Tutorial_Stage = new List<GameObject>();
    [SerializeField]
    List<GameObject> _Tutorial_Infinity = new List<GameObject>();

    List<List<GameObject>> _Tutorials = new List<List<GameObject>>();

    int _NowTutorialNum;
    int _NowSlideNum;


    void Awake()
    {
        _Tutorials.Add(_Tutorial_Stage);
        _Tutorials.Add(_Tutorial_Infinity);
    }
    public void CheckTutorialClear(int num)
    {
        //PlayerPrefs.SetInt("Tutorial_IG_" + num.ToString(), 0);
        if (PlayerPrefs.GetInt("Tutorial_IG_" + num.ToString()) == 0)
        {
            PlayerPrefs.SetInt("Tutorial_IG_" + num.ToString(), 1);
            StartTutorial(num);
        }
        else
            StaticMng.Instance._Tutorialing = false;
    }

    public void StartTutorial(int num)
    {
        _NowTutorialNum = num;
        _NowSlideNum = 0;
        _Tutorials[_NowTutorialNum][_NowSlideNum].SetActive(true);
        StaticMng.Instance._Tutorialing = true;
    }
    public void NextSlide()
    {
        _NowSlideNum++;
        for (int i = 0; i < _Tutorials[_NowTutorialNum].Count; i++)
        {
            _Tutorials[_NowTutorialNum][i].SetActive(false);
            if (_NowSlideNum == i)
                _Tutorials[_NowTutorialNum][_NowSlideNum].SetActive(true);
        }
        if (_NowSlideNum >= _Tutorials[_NowTutorialNum].Count)
            StaticMng.Instance._Tutorialing = false;

    }
}
