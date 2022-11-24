using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainSceneMng : MonoBehaviour {

    [SerializeField]
    UILabel _Player_GoldLabel;
    [SerializeField]
    UILabel _Player_GemLabel;
    [SerializeField]
    UILabel _Player_LevelLabel;
    [SerializeField]
    UILabel _Player_UserNameLabel;
    [SerializeField]
    UI2DSprite _Player_Exp_Gaze;
    [SerializeField]
    GameObject _OptionPopup;
    [SerializeField]
    Animator _OptionPopup_Ani;
    [SerializeField]
    GameObject _CreditPopup;
    [SerializeField]
    GameObject _DataPopup;
    [SerializeField]
    Animator _DataPopup_Ani;

    [SerializeField]
    UISprite _SoundButton;
    [SerializeField]
    UISprite _TowerSetButton;

    [SerializeField]
    GameObject _LogOutPopup;
    [SerializeField]
    Animator _LogOutPopupAni;

    [SerializeField]
    DataSaveMng _DataSaveMng;

    [SerializeField]
    GameObject _ErrorPopup;
    [SerializeField]
    Animator _ErrorPopupAni;
    [SerializeField]
    UILabel _ErrorLog;


    bool _OpenChapterGroup;
    [SerializeField]
    UISprite _ChapterLabelSprite;
    [SerializeField]
    GameObject _ChapterSelectEffect_Root;
    [SerializeField]
    GameObject _ChapterSelectEffect;
    [SerializeField]
    UISprite _ChapterOpenButton;
    [SerializeField]
    GameObject _ChapterGroup;
    [SerializeField]
    GameObject _StageGroupTable;

    [SerializeField]
    GameObject _UserNameResetPopup;

    [SerializeField]
    GameObject[] _SectorLineGrayArray;
    [SerializeField]
    GameObject[] _SectorCiecleGrayArray;

    List<GameObject[]> _StagePeakArrayList_Gray = new List<GameObject[]>();
    List<GameObject[]> _StagePeakArrayList_Dark = new List<GameObject[]>();
    [SerializeField]
    GameObject[] _StagePeakArray_Gray_1;
    [SerializeField]
    GameObject[] _StagePeakArray_Gray_2;
    [SerializeField]
    GameObject[] _StagePeakArray_Gray_3;
    [SerializeField]
    GameObject[] _StagePeakArray_Dark_1;
    [SerializeField]
    GameObject[] _StagePeakArray_Dark_2;
    [SerializeField]
    GameObject[] _StagePeakArray_Dark_3;

    [SerializeField]
    UISprite _BusStopSprite;


    [SerializeField]
    GameObject _StageInfoTable;
    [SerializeField]
    UILabel _StageInfoLabel;
    [SerializeField]
    UISprite[] _MonsterList;
    [SerializeField]
    GameObject[] _NowStagePeaks;
    [SerializeField]
    Animator _StageInfoTableAni;

    [SerializeField]
    GameObject _AchievementPopup;

    [SerializeField]
    StageBackgroundDecoMng _BackgroundMng;


    [SerializeField]
    GameObject _GameOffPopup;

    [SerializeField]
    GameObject _UIRoot;
    [SerializeField]
    GameObject _FadeIn;
    

    public int _NowSelectChapter;
    int _NowSelectSector;

    //void Start()
    //{
    //    _DataSaveMng.Init();
    //    
    //}
    void Awake()
    {
        
        bool temp = false;
        if (PlayerPrefs.GetInt("TowerSetSwap")==1)
        {
            temp = true;
            _TowerSetButton.spriteName = "option_towerdrag";
        }
        else
        {
            temp = false;
            _TowerSetButton.spriteName = "option_towertouch";
        }
        StaticMng.Instance._TowerSet_Drag = temp;
    }
    
    void Start()
    {
        //PlayerPrefs.SetInt("Initiate", 0);//temp
        _DataSaveMng.WantDataSave();
        //_DataSaveMng.WantDataLoad();
        _StagePeakArrayList_Gray.Add(_StagePeakArray_Gray_1);
        _StagePeakArrayList_Gray.Add(_StagePeakArray_Gray_2);
        _StagePeakArrayList_Gray.Add(_StagePeakArray_Gray_3);
        _StagePeakArrayList_Dark.Add(_StagePeakArray_Dark_1);
        _StagePeakArrayList_Dark.Add(_StagePeakArray_Dark_2);
        _StagePeakArrayList_Dark.Add(_StagePeakArray_Dark_3);

        TutorialMng_Main.Data.CheckTutorialClear(0);
    }

    public void SceneStart()
    {
        _NowSelectChapter = StaticMng.Instance._Stage_Chapter;
        _ChapterLabelSprite.spriteName = "chaptername_"+ StaticMng.Instance._Stage_Chapter.ToString();
        _OpenChapterGroup = true;
        OpenChapterPopup();
    }
    void Update()
    {
        _BusStopSprite.spriteName = "main_infinitybutton_"+_NowSelectChapter.ToString();
        _Player_Exp_Gaze.fillAmount = (float)StaticMng.Instance._Player_NowExp / (float)StaticMng.Instance._Player_MaxExp;
        _Player_UserNameLabel.text = StaticMng.Instance._UserName;
        _Player_GoldLabel.text = StaticMng.Instance._Gold.ToString();
        _Player_GemLabel.text = StaticMng.Instance._Gem.ToString();
        _Player_LevelLabel.text = "Lv. "+StaticMng.Instance._Player_Level.ToString();

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (_GameOffPopup.activeSelf)
                CloseGameOffPopup();
            else
                OpenGameOffPopup();
        }

        //if (Input.GetKeyDown(KeyCode.G))
        //    StaticMng.Instance._Gem += 10;
    }

    public void ChapterSelect(int number)
    {
        _NowSelectChapter = number;
        GameObject obj = NGUITools.AddChild(_ChapterSelectEffect_Root, _ChapterSelectEffect);
        StartCoroutine(ChapterChange());

        _OpenChapterGroup = false;
        _ChapterGroup.SetActive(false);
        _ChapterOpenButton.spriteName = "chapterclose_button";
    }
    IEnumerator ChapterChange()
    {
        yield return new WaitForSeconds(0.5f);

        _ChapterLabelSprite.spriteName = "chaptername_" + _NowSelectChapter.ToString();
        _ChapterLabelSprite.MakePixelPerfect();
        //UnlockSector();
        SetPeak(_NowSelectChapter);
        _BackgroundMng.StageChange(_NowSelectChapter);
        
        _StageGroupTable.SetActive(true);
    }
    void UnlockSector(int nowChapter)
    {
        for(int i=0;i<StaticMng.Instance._MaximumSector[nowChapter - 1];i++)
        {
            _SectorCiecleGrayArray[i].SetActive(true);
            if (StaticMng.Instance._UnLock_Sector[nowChapter - 1]>i)
                _SectorCiecleGrayArray[i].SetActive(false);
        }

        for(int i=1;i<StaticMng.Instance._MaximumSector[nowChapter - 1];i++)
        {
            _SectorLineGrayArray[i-1].SetActive(true);
            if (i < StaticMng.Instance._UnLock_Sector[nowChapter - 1])
                _SectorLineGrayArray[i - 1].SetActive(false);
        }
    }
    public void SetPeak(int nowChapter)
    {
        UnlockSector(nowChapter);
        for (int i=0;i<StaticMng.Instance._MaximumSector[nowChapter - 1];i++)
        {
            
            if (StaticMng.Instance._UnLock_Sector[nowChapter - 1]<=i)
            {
                for (int j = 0; j < 3; j++)
                    _StagePeakArrayList_Dark[j][i].SetActive(true);
            }
            else
            {
                for (int j = 0; j < 3; j++)
                    _StagePeakArrayList_Dark[j][i].SetActive(false);
                for (int j = 0; j < 3; j++)
                    _StagePeakArrayList_Gray[j][i].SetActive(true);
                for (int j=0;j<StaticMng.Instance._StagePeakCount[nowChapter - 1,i];j++)
                    _StagePeakArrayList_Gray[j][i].SetActive(false);
            }
            
        }
    }
    public void OpenChapterPopup()
    {
        _OpenChapterGroup = !_OpenChapterGroup;
        if(_OpenChapterGroup)
        {
            _ChapterGroup.SetActive(true);
            _ChapterOpenButton.spriteName = "chapteropen_button";
            _StageGroupTable.SetActive(false);
        }
        else
        {
            _ChapterGroup.SetActive(false);
            _ChapterOpenButton.spriteName = "chapterclose_button";
            _StageGroupTable.SetActive(true);
        }
    }
    public void SelectSector(int num)//작은 아이콘 눌럿을때
    {
        int[,] minfo = { { 2, 3, 4 }, { 2, 3, 5 }, { 6, 7, 8 }, { 6, 7, 9 } };

        _NowSelectSector = num;
        _StageInfoTable.SetActive(true);
        _StageInfoTableAni.SetTrigger("open");
        _StageInfoLabel.text = _NowSelectChapter + "-" + _NowSelectSector;

        for (int i = 0; i < 3; i++)
            _NowStagePeaks[i].SetActive(false);
        for (int i=0;i<StaticMng.Instance._StagePeakCount[_NowSelectChapter-1,_NowSelectSector-1];i++)
            _NowStagePeaks[i].SetActive(true);

        for (int i = 0; i < 3; i++)
        {
            string mname="";
            if (minfo[_NowSelectChapter - 1, i] == 2)
                mname = "note8_2";
            else if (minfo[_NowSelectChapter - 1, i] == 3)
                mname = "note44";
            else if (minfo[_NowSelectChapter - 1, i] == 4)
                mname = "gclef";
            else if (minfo[_NowSelectChapter - 1, i] == 5)
                mname = "bassclef";
            else if (minfo[_NowSelectChapter - 1, i] == 6)
                mname = "comma4";
            else if (minfo[_NowSelectChapter - 1, i] == 7)
                mname = "comma8";
            else if (minfo[_NowSelectChapter - 1, i] == 8)
                mname = "rmark";
            else if (minfo[_NowSelectChapter - 1, i] == 9)
                mname = "sharp";
            _MonsterList[i].spriteName = "m_" + mname + "_walk_0";
            _MonsterList[i].keepAspectRatio = UIWidget.AspectRatioSource.Free;
            _MonsterList[i].MakePixelPerfect();
            _MonsterList[i].keepAspectRatio = UIWidget.AspectRatioSource.BasedOnHeight;
            _MonsterList[i].height = 75;
        }

    }



    public void StageStart()//인포에서 스타트 눌럿을 때
    {
        GameObject obj = NGUITools.AddChild(_UIRoot,_FadeIn);
        StartCoroutine(StageStart_C());
    }
    IEnumerator StageStart_C()
    {
        yield return new WaitForSeconds(1.5f);

        while(_DataSaveMng.GetDataSending())
            yield return null;
        StaticMng.Instance._Stage_Chapter = _NowSelectChapter;
        StaticMng.Instance._Stage_Sector = _NowSelectSector;
        StaticMng.Instance._GameMode_Infinity = false;
        StaticMng.Instance._NowWantScene = "GameScene";
        SceneManager.LoadScene("LoadingScene");
    }
    public void CancelStage()//인포에서 취소 눌럿을 때
    {
        _StageInfoTable.SetActive(false);
    }

    public void OpenOptionPopup()
    {
        if (StaticMng.Instance._Option_Volume_Bool)
        {
            StaticMng.Instance._Option_Volume = 1.0f;
            _SoundButton.spriteName = "soundbutton_on";
        }
        else
        {
            StaticMng.Instance._Option_Volume = 0.0f;
            _SoundButton.spriteName = "soundbutton_off";
        }
        _OptionPopup.SetActive(true);
        _OptionPopup_Ani.SetTrigger("open");
        TutorialMng_Main.Data.CheckTutorialClear(2);
    }
    public void CloseOptionPopup()
    {
        _OptionPopup.SetActive(false);
    }

    public void OpenDataPopup()
    {
        _DataPopup.SetActive(true);
        _DataPopup_Ani.SetTrigger("open");
    }
    public void CloseDataPopup()
    {
        _DataPopup.SetActive(false);
    }

    public void OpenAchievementPopup()
    {
        _AchievementPopup.SetActive(true);
    }
    public void CloseAchievementPopup()
    {
        _AchievementPopup.SetActive(false);
    }

    public void OpenUserNameResetPopup()
    {
        _UserNameResetPopup.SetActive(true);
    }
    public void CloseUserNameResetPopup()
    {
        _UserNameResetPopup.SetActive(false);
    }
    public void GoUserNameResetScene()
    {
        if(StaticMng.Instance._Gem>=50)
        {
            GameObject obj = NGUITools.AddChild(_UIRoot, _FadeIn);
            StartCoroutine(GoUserNameReset_C());
        }
        else
        {
            CloseUserNameResetPopup();
            ExportError("자금이 부족합니다");
        }
    }
    IEnumerator GoUserNameReset_C()
    {
        yield return new WaitForSeconds(1.5f);
        while (_DataSaveMng.GetDataSending())
            yield return null;
        SceneManager.LoadScene("UserNameScene");
    }
    void ExportError(string log)
    {
        _ErrorPopup.SetActive(true);
        _ErrorPopupAni.SetTrigger("open");
        _ErrorLog.text = log;
        //Debug.Log(log);
    }

    public void Option_SoundChange()
    {
        StaticMng.Instance._Option_Volume_Bool = !StaticMng.Instance._Option_Volume_Bool;
        if (StaticMng.Instance._Option_Volume_Bool)
        {
            StaticMng.Instance._Option_Volume = 1.0f;
            _SoundButton.spriteName = "soundbutton_on";
        }
        else
        {
            StaticMng.Instance._Option_Volume = 0.0f;
            _SoundButton.spriteName = "soundbutton_off";
        }
        //_DataSaveMng.WantDataSave();
    }

    public void Option_TowerSetButtonSwap()
    {
        StaticMng.Instance._TowerSet_Drag = !StaticMng.Instance._TowerSet_Drag;
        int temp = 0;
        if (StaticMng.Instance._TowerSet_Drag)
        {
            temp = 1;
            _TowerSetButton.spriteName = "option_towerdrag";
        }
        else
        {
            temp = 0;
            _TowerSetButton.spriteName = "option_towertouch";
        }
        PlayerPrefs.SetInt("TowerSetSwap", temp);
    }
   

    public void LoadDataNext()
    {
        _ChapterGroup.SetActive(false);
        _StageGroupTable.SetActive(false);
        _ChapterOpenButton.spriteName = "chapterclose_button";
    }

    void OpenGameOffPopup()
    {
        _GameOffPopup.SetActive(true);
    }
    public void CloseGameOffPopup()
    {
        _GameOffPopup.SetActive(false);
    }


    public void OpenLogOutPopup()
    {
        _LogOutPopup.SetActive(true);
        _LogOutPopupAni.SetTrigger("open");
    }
    public void CloseLogOutPopup()
    {
        _LogOutPopup.SetActive(false);
    }
    public void LogOut()
    {
        StartCoroutine(LogOut_C());
        
    }
    IEnumerator LogOut_C()
    {
        yield return null;
        while (_DataSaveMng.GetDataSending())
            yield return null;
        StaticMng.Instance._UserId = "";
        StaticMng.Instance._UserPW = "";
        PlayerPrefs.SetInt("Logining", 0);
        PlayerPrefs.SetString("UserID", "");
        PlayerPrefs.SetString("UserPW", "");
        SceneManager.LoadScene("TitleScene");
    }

    public void CloseErrorPopup()
    {
        _ErrorPopup.SetActive(false);
    }

    public void OpenCreditPopup()
    {
        _CreditPopup.SetActive(true);
    }
    public void CloseCreditPopup()
    {
        _CreditPopup.SetActive(false);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
