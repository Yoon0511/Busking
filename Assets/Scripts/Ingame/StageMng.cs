using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageMng : MonoBehaviour {
    

    float _GameTimer;
    int _Score;
    
    public Animator _LowUITable;
    [SerializeField]
    GameObject _LowUITable_Blind;
    
    public GameObject _ObjectRoot;
    [SerializeField]
    GameObject _UIRoot;
    [SerializeField]
    GameObject _FadeIn;

    [SerializeField]
    GameObject _StageStartEffect;
    [SerializeField]
    GameObject _StageClearEffect;
    [SerializeField]
    GameObject _StageFailEffect;

    [SerializeField]
    GameObject _StageClearPopup;
    [SerializeField]
    GameObject _StageClear_NextStageButton_Gray;
    [SerializeField]
    GameObject _StageClear_Light;

    [SerializeField]
    MonsterGenerateMng _MonsterGenerater;

    [SerializeField]
    IngameSoundMng _SoundMng;
    public TowerSkillMng _TowerSkillMng;

    public Tower[] _Towers = new Tower[4];
    public bool[] _TowerSet = new bool[4];

    public int _MaxLife;
    public int _NowEnegy;
    public int _MaxEnegy;
    const int _PlusEnegy = 1;
    float _EnegyTimer;
    const float _EnegyTimerDelay = 2.0f / 3.0f;

    public DataSaveMng _DataSaveMng;

    public GameObject _Dead_Note8;
    public GameObject _Dead_Note8_2;
    public GameObject _Dead_Note44;
    public GameObject _Dead_GClef;
    public GameObject _Dead_BassClef;
    public GameObject _Dead_Comma4;
    public GameObject _Dead_Comma8;
    public GameObject _Dead_RMark;
    public GameObject _Dead_Sharp;

    public List<Monster> _MonsterList = new List<Monster>();

    [SerializeField]
    GameObject _InfinityFastButton;

    [SerializeField]
    GameObject[] _AmpCost1;
    [SerializeField]
    GameObject[] _AmpCost2;

    //ClearPopup
    [SerializeField]
    GameObject[] _ClearPeeks;
    [SerializeField]
    UILabel _ClearGoldLabel;
    [SerializeField]
    UILabel _ClearStageLabel;
    [SerializeField]
    GameObject _ClearGoldIcon;


    [SerializeField]
    UILabel _ScoreLabel;

    public TowerSetMng _TowerSetMng;

    bool _MissionClear;
    bool _GameMode;

    private static StageMng instance = null;

    public static StageMng Data
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType(typeof(StageMng)) as StageMng;
                if (instance == null)
                {
                    Debug.Log("no instance");
                }
            }
            return instance;
        }
    }

    void Start()
    {
        _GameMode = StaticMng.Instance._GameMode_Infinity;
        StaticMng.Instance._CanPlay = false;
        for (int i = 0; i < 4; i++)
            _TowerSet[i] = false;


        _Score = 0;
        StaticMng.Instance._PauseGame = false;
        if (StaticMng.Instance._GameMode_Infinity)
        {
            StaticMng.Instance._PlayerLife = 70;
            _MaxLife = 70;
            _MaxEnegy = 10000;
            _NowEnegy = 0;
            _ScoreLabel.gameObject.SetActive(true);
            _ScoreLabel.text = "Score\r\n0";
            for (int i = 0; i < 4; i++)
                StaticMng.Instance._InfinityGameTowerLevel[i] = 1;
            if (StaticMng.Instance._Infinity_FastValue > 1)
            {
                _InfinityFastButton.SetActive(true);
                _InfinityFastButton.GetComponent<UISprite>().spriteName = "ingame_fast"+StaticMng.Instance._Infinity_FastValue.ToString();
            }
            _AmpCost1[1].SetActive(true);
            _AmpCost2[1].SetActive(true);
        }
        else
        {
            StaticMng.Instance._PlayerLife = 5;
            _MaxLife = 5;
            _MaxEnegy = 30;
            _NowEnegy = _MaxEnegy;
            _AmpCost1[0].SetActive(true);
            _AmpCost2[0].SetActive(true);
        }
        _GameTimer = 0.0f;

        WaveStartAnimation(1);
        StartCoroutine(StageStart(4.0f));
        
        
    }

 

    void Update()
    {

        if(StaticMng.Instance._StartGame && !StaticMng.Instance._PauseGame && !StaticMng.Instance._Tutorialing)
        {
            _GameTimer += Time.smoothDeltaTime;
            _EnegyTimer += Time.smoothDeltaTime;
            if (_EnegyTimer >= _EnegyTimerDelay)
            {
                _EnegyTimer -= _EnegyTimerDelay;
                _NowEnegy += _PlusEnegy;
                if (_NowEnegy >= _MaxEnegy)
                    _NowEnegy = _MaxEnegy;
            }

            if (!_MonsterGenerater.getGenerating())
            {
                if (_MonsterList.Count == 0 && StaticMng.Instance._PlayerLife>0)
                    StageClear();
            }
            if (_GameMode)
            {
                _ScoreLabel.text = "Score\r\n"+((int)_GameTimer).ToString();
                StaticMng.Instance._PlayerLife = _MaxLife - StaticMng.Instance._MonsterCount;
            }
            if(StaticMng.Instance._PlayerLife<=0)
            {
                FailStage();
            }

            if (Input.GetKeyDown(KeyCode.P))
                AdminTest_FastButton();
        }
    }

    public void WaveStartAnimation(int num)
    {
        GameObject obj = NGUITools.AddChild(_UIRoot, _StageStartEffect);
        obj.GetComponent<WaveEffect>().Init(num);
        
    }
    public void FadeInAnimation()
    {
        GameObject obj = NGUITools.AddChild(_UIRoot, _FadeIn);
    }
    void FailStage()
    {
        StaticMng.Instance._StartGame = false;
        GameObject obj = NGUITools.AddChild(_UIRoot, _StageFailEffect);
        StartCoroutine(EndStage_C(2.0f));
    }
    IEnumerator StageStart(float time)
    {
        yield return new WaitForSeconds(time);


        _SoundMng.PlayBgm();
        StaticMng.Instance._CanPlay = true;
        StaticMng.Instance._StartGame = true;
        StaticMng.Instance._LowUI_Down = false;
    }
    void StageClear()
    {
        _MissionClear = true;
        StaticMng.Instance._StartGame = false;
        GameObject obj = NGUITools.AddChild(_UIRoot, _StageClearEffect);
        //스테이지 클리어 시 섹터 늘려주는 곳
        if(!(StaticMng.Instance._Stage_Chapter==4&&StaticMng.Instance._Stage_Sector==10))
        {
            //Debug.Log(StaticMng.Instance._Stage_Chapter);
            if (StaticMng.Instance._Stage_Sector == StaticMng.Instance._UnLock_Sector[StaticMng.Instance._Stage_Chapter - 1] && StaticMng.Instance._Stage_Chapter == StaticMng.Instance._UnLock_Chapter)
            {
                if (StaticMng.Instance._UnLock_Sector[StaticMng.Instance._Stage_Chapter - 1] == 10)
                {
                    StaticMng.Instance._UnLock_Chapter++;
                    StaticMng.Instance._UnLock_Sector[StaticMng.Instance._UnLock_Chapter - 1]++;
                }
                else
                    StaticMng.Instance._UnLock_Sector[StaticMng.Instance._Stage_Chapter - 1]++;
            }
            _StageClear_NextStageButton_Gray.SetActive(false);
        }

        if (StaticMng.Instance._Stage_Sector == 10)
            _StageClear_NextStageButton_Gray.SetActive(true);
        

        StartCoroutine(EndStage_C(2.0f));
    }
    IEnumerator EndStage_C(float time)
    {
        yield return new WaitForSeconds(time);

        Time.timeScale = 1.0f;//temp

        if (_GameMode)
        {
            _Score = 3;
            _ClearStageLabel.text = "Infinity";
            _ClearGoldIcon.SetActive(false);
        }
        else
        {
            _Score = -2 + StaticMng.Instance._PlayerLife;
            if (_Score < 0) _Score = 0;

            if (StaticMng.Instance._StagePeakCount[StaticMng.Instance._Stage_Chapter - 1, StaticMng.Instance._Stage_Sector - 1] < _Score)
                StaticMng.Instance._StagePeakCount[StaticMng.Instance._Stage_Chapter - 1, StaticMng.Instance._Stage_Sector - 1] = _Score;
            _ClearStageLabel.text = StaticMng.Instance._Stage_Chapter.ToString() + "-" + StaticMng.Instance._Stage_Sector.ToString();
        }
        
        int tempgold = 0;
        _StageClearPopup.SetActive(true);
        if (_MissionClear)
        {

            if(StaticMng.Instance._Stage_Chapter==1)
            {
                if(StaticMng.Instance._Stage_Sector<6)
                    tempgold = Random.Range(200, 600);
                else
                    tempgold = Random.Range(600, 1000);
            }
            else if (StaticMng.Instance._Stage_Chapter == 2)
            {
                if (StaticMng.Instance._Stage_Sector < 6)
                    tempgold = Random.Range(1000, 1500);
                else
                    tempgold = Random.Range(1500, 2200);
            }
            else if (StaticMng.Instance._Stage_Chapter == 3)
            {
                if (StaticMng.Instance._Stage_Sector < 6)
                    tempgold = Random.Range(2500, 3000);
                else
                    tempgold = Random.Range(3000, 3500);
            }
            else if (StaticMng.Instance._Stage_Chapter == 4)
            {
                if (StaticMng.Instance._Stage_Sector < 6)
                    tempgold = Random.Range(3500, 4000);
                else if (StaticMng.Instance._Stage_Sector < 10)
                    tempgold = Random.Range(4000, 5000);
                else
                    tempgold = Random.Range(5000, 6000);
            }

            _StageClear_Light.SetActive(true);
        }
        if (_GameMode)
        {
            int tempscore = (int)_GameTimer;
            tempgold = 0;
            if (StaticMng.Instance._InfinityGameScore < tempscore)
                StaticMng.Instance._InfinityGameScore = tempscore;
            _ClearGoldLabel.text = tempscore.ToString();
        }
        else
            _ClearGoldLabel.text = "+" + tempgold.ToString();
        
        StaticMng.Instance._Gold += tempgold;
        for (int i=0;i<_Score;i++)
            _ClearPeeks[i].SetActive(true);

        _DataSaveMng.WantDataSave();
    }

    public void SwapSetUI()
    {
        
        StaticMng.Instance._LowUI_Down = !StaticMng.Instance._LowUI_Down;
        if(StaticMng.Instance._LowUI_Down)
        {
            _LowUITable.SetTrigger("down");
            _LowUITable_Blind.SetActive(true);
        }
        else
        {
            _LowUITable.SetTrigger("up");
            _LowUITable_Blind.SetActive(false);
        }
    }


    public void AdminTest_FastButton()
    {
        if (Time.timeScale <= 1.5f)
            Time.timeScale = StaticMng.Instance._Infinity_FastValue;
        else
            Time.timeScale = 1.0f;
    }
}
