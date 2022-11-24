using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageBackgroundDecoMng : MonoBehaviour {

    public int _NowChapter;

    [SerializeField]
    MainSceneMng _MainMng;

    public GameObject[] _StageBackgroundImage;
    [SerializeField]
    GameObject[] _StageGrayIcon;
    [SerializeField]
    GameObject _NowChapterIcon;
    [SerializeField]
    UI2DSprite _ChapterLineGray;
    [SerializeField]
    UILabel[] _NeedPeakLabel;


    [SerializeField]
    GameObject _Stage2_Star;
    [SerializeField]
    Vector2[] _Stage2_Star_Pos;

    const int _NeedNestStagePeak = 25;

    void Start()
    {
        //StageChange(2);
#if UNITY_EDITOR_WIN
        _NowChapter = 1;
        StaticMng.Instance._Stage_Chapter = _NowChapter;
#endif
        StageChange(StaticMng.Instance._Stage_Chapter);
        _MainMng.SceneStart();
    }
    void Update()
    {
        _ChapterLineGray.fillAmount = 1.0f - ((StaticMng.Instance._UnLock_Chapter-1)*0.33f);
        _NowChapterIcon.transform.localPosition = new Vector3(-600+(240* _NowChapter),44);

        bool[] peakcheck = { false, false, false };

        for (int i = 0; i < StaticMng.Instance._MaximumChapter - 1; i++)
        {
            int num = 0;
            for (int j = 0; j < 10; j++)
                num += StaticMng.Instance._StagePeakCount[i, j];
            _NeedPeakLabel[i].text = num.ToString() + "/" + StaticMng.Instance._NeedPassPeakCount[i].ToString();

            if (num >= StaticMng.Instance._NeedPassPeakCount[i])
                peakcheck[i] = true;
        }

        for (int i = 0; i < 3; i++)
            _StageGrayIcon[i].SetActive(true);
        for (int i = 0; i < StaticMng.Instance._UnLock_Chapter-1; i++)
        {
            if (peakcheck[i])
                _StageGrayIcon[i].SetActive(false);
        }
        for (int i=0;i<StaticMng.Instance._MaximumChapter;i++)//Achievement
        {
            int num = 0;
            for (int j = 0; j < 10; j++)
                num += StaticMng.Instance._StagePeakCount[i, j];
            StaticMng.Instance._Achive_NowValue[i + 5] = num;
        }
        
        
    }

    public void StageChange(int num)
    {
        _NowChapter = num;
        for (int i = 0; i < StaticMng.Instance._MaximumChapter; i++)
        {
            if (i == _NowChapter - 1)
                _StageBackgroundImage[i].SetActive(true);
            else
                _StageBackgroundImage[i].SetActive(false);
        }
        if (_NowChapter == 1)
        {

        }
        else if (_NowChapter == 2)
        {
            for(int i=0;i<4;i++)
                StartCoroutine(Stage2_StarMaker(i));
        }
        else if (_NowChapter == 3)
        {

        }
        else if (_NowChapter == 4)
        {

        }
        _MainMng.SetPeak(_NowChapter);
    }

    IEnumerator Stage2_StarMaker(int i)
    {
        yield return new WaitForSeconds(Random.Range(2.0f, 5.0f));

        if (_NowChapter == 2)
        {
            GameObject obj = NGUITools.AddChild(_StageBackgroundImage[1], _Stage2_Star);
            obj.transform.localPosition = _Stage2_Star_Pos[i];
            StartCoroutine(Stage2_StarMaker(i));
        }
    }
}
