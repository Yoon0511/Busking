using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerUpgradeMng : MonoBehaviour {

    //public List<string> _GuitarTowerNameData = new List<string>();
    //public List<string> _DrumTowerNameData = new List<string>();
    //public List<string> _BassTowerNameData = new List<string>();
    //public List<string> _KeyBoardTowerNameData = new List<string>();
    string[] _TowerType = { "기타", "드럼", "베이스", "키보드" };
    string[] _RankName = {  "구식", "흔한", "비싼", "희귀한", "전설의" };

    public List<int> _GuitarTowerUpgradeCost = new List<int>();
    public List<int> _DrumTowerUpgradeCost = new List<int>();
    public List<int> _BassTowerUpgradeCost = new List<int>();
    public List<int> _KeyBoardTowerUpgradeCost = new List<int>();

    [SerializeField]
    DataSaveMng _DataSaveMng;

    [SerializeField]
    GameObject _UpgradePopup;

    [SerializeField]
    UI2DSprite[] _TowerLevelGaze;
    float[] _TowerLevelGazeConst = {0.11f,0.2f,0.3f,0.39f,0.49f,0.58f,0.69f,0.79f,0.885f,1 };
    [SerializeField]
    UILabel[] _TowerLevelLabel;
    [SerializeField]
    UILabel[] _TowerName;
    [SerializeField]
    GameObject[] _TowerUpgradeGray;

    [SerializeField]
    UILabel[] _TowersDamageLabel;
    [SerializeField]
    UILabel[] _TowersAttackDelayLabel;
    [SerializeField]
    UILabel _DrumAbilityValueLabel;
    [SerializeField]
    UILabel _BassAbilityValueLabel;

    int _NowTargetTower;
    [SerializeField]
    GameObject _TowerUpgradeCheckPopup;
    [SerializeField]
    UILabel _TowerUpgradeCheckLabel;
    [SerializeField]
    GameObject _TowerStatResetCheckPopup;
    [SerializeField]
    UILabel _TowerStatResetCheckLabel;
    [SerializeField]
    UILabel _TowerStatResetCheckPriceLabel;

    [SerializeField]
    UILabel[] _TowerUpgradeCostLabel;
    [SerializeField]
    GameObject[] _TowerHelpTable;

    [SerializeField]
    UISprite[] _TowerIconImages;
    [SerializeField]
    UI2DSprite[] _TowerIconImages_Rank;
    [SerializeField]
    GameObject[] _TowerRankImages_Table;
    int _NowTabNumber;

    [SerializeField]
    GameObject[] _Tabs;
    [SerializeField]
    GameObject[] _TabButtonGray;

    [SerializeField]
    GameObject _LogPopup;
    [SerializeField]
    Animator _LogAni;
    [SerializeField]
    UILabel _Log;

    [SerializeField]
    GameObject[] _TowerRankIcon_Guitar;
    [SerializeField]
    GameObject[] _TowerRankIcon_Drum;
    [SerializeField]
    GameObject[] _TowerRankIcon_Bass;
    [SerializeField]
    GameObject[] _TowerRankIcon_KeyBoard;
    List<GameObject[]> _TowerRankIcons = new List<GameObject[]>();

    void Start()
    {
        _TowerRankIcons.Add(_TowerRankIcon_Guitar);
        _TowerRankIcons.Add(_TowerRankIcon_Drum);
        _TowerRankIcons.Add(_TowerRankIcon_Bass);
        _TowerRankIcons.Add(_TowerRankIcon_KeyBoard);

        TabSelect(0);
    }
    void AchieveCheck()
    {
        StaticMng.Instance._Achive_NowValue[9] = StaticMng.Instance._Player_Level;

        bool[] check = { false,false,false,false};

        if (StaticMng.Instance._GuitarTowerLevel == 10 && StaticMng.Instance._GuitarTowerRank == 5)
            check[0] = true;
        if (StaticMng.Instance._DrumTowerLevel == 10 && StaticMng.Instance._DrumTowerRank == 5)
            check[1] = true;
        if (StaticMng.Instance._BassTowerLevel == 10 && StaticMng.Instance._BassTowerRank == 5)
            check[2] = true;
        if (StaticMng.Instance._KeyBoardTowerLevel == 10 && StaticMng.Instance._KeyBoardTowerRank == 5)
            check[3] = true;
        int temp = 0;
        for(int i=0;i<4;i++)
        {
            if (check[i])
                temp++;
        }
        StaticMng.Instance._Achive_NowValue[10] = temp;
    }
    void TowerLevelAndRankImageUpdate()
    {
        _TowerIconImages[0].spriteName = "shop_guitar_" + ((StaticMng.Instance._GuitarTowerLevel + 1) / 2).ToString();
        _TowerIconImages[1].spriteName = "shop_drum_" + ((StaticMng.Instance._DrumTowerLevel + 1) / 2).ToString();
        _TowerIconImages[2].spriteName = "shop_bass_" + ((StaticMng.Instance._BassTowerLevel + 1) / 2).ToString();
        _TowerIconImages[3].spriteName = "shop_keyboard_" + ((StaticMng.Instance._KeyBoardTowerLevel + 1) / 2).ToString();

        Vector3[] RankColor = {new Vector3(0,86,255), new Vector3(127, 18, 139), new Vector3(255, 0, 173), new Vector3(255, 164, 0), new Vector3(255, 237, 0) };

        _TowerIconImages_Rank[0].color = new Color((RankColor[StaticMng.Instance._GuitarTowerRank - 1].x/255.0f), (RankColor[StaticMng.Instance._GuitarTowerRank - 1].y/255.0f), (RankColor[StaticMng.Instance._GuitarTowerRank - 1].z/255.0f));
        _TowerIconImages_Rank[1].color = new Color((RankColor[StaticMng.Instance._DrumTowerRank - 1].x / 255.0f), (RankColor[StaticMng.Instance._DrumTowerRank - 1].y / 255.0f), (RankColor[StaticMng.Instance._DrumTowerRank - 1].z / 255.0f));
        _TowerIconImages_Rank[2].color = new Color((RankColor[StaticMng.Instance._BassTowerRank - 1].x / 255.0f), (RankColor[StaticMng.Instance._BassTowerRank - 1].y / 255.0f), (RankColor[StaticMng.Instance._BassTowerRank - 1].z / 255.0f));
        _TowerIconImages_Rank[3].color = new Color((RankColor[StaticMng.Instance._KeyBoardTowerRank - 1].x / 255.0f), (RankColor[StaticMng.Instance._KeyBoardTowerRank - 1].y / 255.0f), (RankColor[StaticMng.Instance._KeyBoardTowerRank - 1].z / 255.0f));

    }

	void Update()
    {
        float[] Rank_DamagePercent = { 75, 85, 100, 115, 125 };
        float[] Rank_SlowPercent = { 90, 95, 100, 105, 110 };
        //if (Input.GetKeyDown(KeyCode.Space))
        //    StaticMng.Instance._Gold += 1000000;

        AchieveCheck();
        TowerLevelAndRankImageUpdate();

        for(int i=0;i<4;i++)
        {
            for(int j=0;j<5;j++)
            {
                _TowerRankIcons[i][j].SetActive(false);
                _TowerRankImages_Table[j].SetActive(false);
            }
        }

        for(int i=0;i<4;i++)
        {
            

            if(i==0)
            {
                if (i == _NowTabNumber)
                    _TowerRankImages_Table[StaticMng.Instance._GuitarTowerRank-1].SetActive(true);
                _TowerLevelLabel[i].text = "Lv."+StaticMng.Instance._GuitarTowerLevel;
                _TowerRankIcons[i][StaticMng.Instance._GuitarTowerRank - 1].SetActive(true);
                _TowerLevelGaze[i].fillAmount = _TowerLevelGazeConst[StaticMng.Instance._GuitarTowerLevel - 1];
                _TowerName[i].text = _RankName[StaticMng.Instance._GuitarTowerRank - 1] + " 기타";
                _TowerUpgradeCostLabel[i].text = _GuitarTowerUpgradeCost[StaticMng.Instance._GuitarTowerLevel].ToString();
                _TowersDamageLabel[i].text = ((8.0f + (4.0f * StaticMng.Instance._GuitarTowerLevel)) * (Rank_DamagePercent[StaticMng.Instance._GuitarTowerRank - 1] / 100.0f)).ToString();
                _TowersAttackDelayLabel[i].text = "1.3";
                if (StaticMng.Instance._GuitarTowerLevel == 10)
                {
                    _TowerUpgradeGray[i].SetActive(true);
                    _TowerUpgradeCostLabel[i].text = "Max";
                }
                else
                    _TowerUpgradeGray[i].SetActive(false);
            }
            if (i == 1)
            {
                if (i == _NowTabNumber)
                    _TowerRankImages_Table[StaticMng.Instance._DrumTowerRank-1].SetActive(true);
                _TowerLevelLabel[i].text = "Lv." + StaticMng.Instance._DrumTowerLevel;
                _TowerRankIcons[i][StaticMng.Instance._DrumTowerRank - 1].SetActive(true);
                _TowerLevelGaze[i].fillAmount = _TowerLevelGazeConst[StaticMng.Instance._DrumTowerLevel - 1];
                _TowerName[i].text = _RankName[StaticMng.Instance._DrumTowerRank - 1] + " 드럼";
                _TowerUpgradeCostLabel[i].text = _DrumTowerUpgradeCost[StaticMng.Instance._DrumTowerLevel].ToString();
                _TowersDamageLabel[i].text = ((1.0f + (0.5f * StaticMng.Instance._DrumTowerLevel)) * (Rank_DamagePercent[StaticMng.Instance._DrumTowerRank - 1] / 100.0f)).ToString();
                _DrumAbilityValueLabel.text = (100.0f-(((0.7f - (StaticMng.Instance._DrumTowerLevel * 0.02f)) * (Rank_SlowPercent[5 - StaticMng.Instance._DrumTowerRank] / 100.0f))*100.0f)).ToString()+"%";
                _TowersAttackDelayLabel[i].text = "1.4";
                if (StaticMng.Instance._DrumTowerLevel == 10)
                {
                    _TowerUpgradeGray[i].SetActive(true);
                    _TowerUpgradeCostLabel[i].text = "Max";
                }
                else
                    _TowerUpgradeGray[i].SetActive(false);
            }
            if (i == 2)
            {
                if (i == _NowTabNumber)
                    _TowerRankImages_Table[StaticMng.Instance._BassTowerRank-1].SetActive(true);
                _TowerLevelLabel[i].text = "Lv." + StaticMng.Instance._BassTowerLevel;
                _TowerRankIcons[i][StaticMng.Instance._BassTowerRank - 1].SetActive(true);
                _TowerLevelGaze[i].fillAmount = _TowerLevelGazeConst[StaticMng.Instance._BassTowerLevel - 1];
                _TowerName[i].text = _RankName[StaticMng.Instance._BassTowerRank - 1] + " 베이스";
                _TowerUpgradeCostLabel[i].text = _BassTowerUpgradeCost[StaticMng.Instance._BassTowerLevel].ToString();
                _TowersDamageLabel[i].text = ((3.0f + (1.5f * StaticMng.Instance._BassTowerLevel)) * (Rank_DamagePercent[StaticMng.Instance._BassTowerRank - 1] / 100.0f)).ToString();
                _BassAbilityValueLabel.text = (-((0 - (0.6f * StaticMng.Instance._BassTowerLevel)) * (Rank_SlowPercent[StaticMng.Instance._BassTowerRank - 1] / 100.0f))).ToString();
                _TowersAttackDelayLabel[i].text = "1.5";
                if (StaticMng.Instance._BassTowerLevel == 10)
                {
                    _TowerUpgradeGray[i].SetActive(true);
                    _TowerUpgradeCostLabel[i].text = "Max";
                }
                else
                    _TowerUpgradeGray[i].SetActive(false);
            }
            if (i == 3)
            {
                if (i == _NowTabNumber)
                    _TowerRankImages_Table[StaticMng.Instance._KeyBoardTowerRank-1].SetActive(true);
                _TowerLevelLabel[i].text = "Lv." + StaticMng.Instance._KeyBoardTowerLevel;
                _TowerRankIcons[i][StaticMng.Instance._KeyBoardTowerRank - 1].SetActive(true);
                _TowerLevelGaze[i].fillAmount = _TowerLevelGazeConst[StaticMng.Instance._KeyBoardTowerLevel - 1];
                _TowerName[i].text = _RankName[StaticMng.Instance._KeyBoardTowerRank - 1] + " 키보드";
                _TowerUpgradeCostLabel[i].text = _KeyBoardTowerUpgradeCost[StaticMng.Instance._KeyBoardTowerLevel].ToString();
                _TowersDamageLabel[i].text = ((4.0f + (2.0f * StaticMng.Instance._KeyBoardTowerLevel)) * (Rank_DamagePercent[StaticMng.Instance._KeyBoardTowerRank - 1] / 100.0f)).ToString();
                _TowersAttackDelayLabel[i].text = "2";
                if (StaticMng.Instance._KeyBoardTowerLevel == 10)
                {
                    _TowerUpgradeGray[i].SetActive(true);
                    _TowerUpgradeCostLabel[i].text = "Max";
                }
                else
                    _TowerUpgradeGray[i].SetActive(false);
            }
        }
    }
    public void OnHelpTable(int i) {_TowerHelpTable[i].SetActive(true);}
    public void OffHelpTable(int i) {_TowerHelpTable[i].SetActive(false);}

    public void OpenUpgradeCheckTower(int num)
    {
        _NowTargetTower = num;
        _TowerUpgradeCheckPopup.SetActive(true);
        _TowerUpgradeCheckLabel.text = "'" + _TowerType[_NowTargetTower] + 
            "'를 강화\r\n하시겠습니까?";
    }
    public void OpenStatResetCheckTower(int num)
    {
        _NowTargetTower = num;
        _TowerStatResetCheckPopup.SetActive(true);

        int gold = 0;
        if (_NowTargetTower == 0)
            gold = _GuitarTowerUpgradeCost[StaticMng.Instance._GuitarTowerLevel];
        else if (_NowTargetTower == 1)
            gold = _DrumTowerUpgradeCost[StaticMng.Instance._DrumTowerLevel];
        else if (_NowTargetTower == 2)
            gold = _BassTowerUpgradeCost[StaticMng.Instance._BassTowerLevel];
        else if (_NowTargetTower == 3)
            gold = _KeyBoardTowerUpgradeCost[StaticMng.Instance._KeyBoardTowerLevel];
        gold /= 3;


        _TowerStatResetCheckLabel.text = "'" + _TowerType[_NowTargetTower] +
            "'의 등급을\r\n재설정 하시겠습니까?";
        _TowerStatResetCheckPriceLabel.text = gold.ToString();
    }

    public void UpgradeTower()
    {
        int gold = 0;
        if (_NowTargetTower == 0)
            gold = _GuitarTowerUpgradeCost[StaticMng.Instance._GuitarTowerLevel];
        else if(_NowTargetTower==1)
            gold = _DrumTowerUpgradeCost[StaticMng.Instance._DrumTowerLevel];
        else if (_NowTargetTower == 2)
            gold = _BassTowerUpgradeCost[StaticMng.Instance._BassTowerLevel];
        else if (_NowTargetTower == 3)
            gold = _KeyBoardTowerUpgradeCost[StaticMng.Instance._KeyBoardTowerLevel];

        if (StaticMng.Instance._Gold>= gold)
        {
            bool levelcheck = false;
            if (_NowTargetTower == 0 && StaticMng.Instance._GuitarTowerLevel <= StaticMng.Instance._Player_Level)
                levelcheck = true;
            else if (_NowTargetTower == 1 && StaticMng.Instance._DrumTowerLevel <= StaticMng.Instance._Player_Level)
                levelcheck = true;
            else if (_NowTargetTower == 2 && StaticMng.Instance._BassTowerLevel <= StaticMng.Instance._Player_Level)
                levelcheck = true;
            else if (_NowTargetTower == 3 && StaticMng.Instance._KeyBoardTowerLevel <= StaticMng.Instance._Player_Level)
                levelcheck = true;
            if(levelcheck)
            {

                StaticMng.Instance._Gold -= gold;
                _TowerUpgradeCheckPopup.SetActive(false);
                _LogPopup.SetActive(true);
                _LogAni.SetTrigger("open");
                _Log.text = "'" + _TowerType[_NowTargetTower] + "'를 강화\r\n하였습니다";
                if (_NowTargetTower == 0)
                {
                    StaticMng.Instance._GuitarTowerLevel++;
                    StaticMng.Instance._GuitarTowerRank = 3;
                }
                else if (_NowTargetTower == 1)
                {
                    StaticMng.Instance._DrumTowerLevel++;
                    StaticMng.Instance._DrumTowerRank = 3;
                }
                else if (_NowTargetTower == 2)
                {
                    StaticMng.Instance._BassTowerLevel++;
                    StaticMng.Instance._BassTowerRank = 3;
                }
                else if (_NowTargetTower == 3)
                {
                    StaticMng.Instance._KeyBoardTowerLevel++;
                    StaticMng.Instance._KeyBoardTowerRank = 3;
                }
                CheckExp();
                _DataSaveMng.WantDataSave();
            }
            else
            {
                _TowerUpgradeCheckPopup.SetActive(false);
                _LogPopup.SetActive(true);
                _LogAni.SetTrigger("open");
                _Log.text = "업그레이드를 하기위한\r\n레벨이 부족합니다";
            }
        }
        else
        {
            _TowerUpgradeCheckPopup.SetActive(false);
            _LogPopup.SetActive(true);
            _LogAni.SetTrigger("open");
            _Log.text = "골드가 부족합니다";
        }
    }
    public void StatResetTower()
    {

        int gold = 0;
        if (_NowTargetTower == 0)
            gold = _GuitarTowerUpgradeCost[StaticMng.Instance._GuitarTowerLevel];
        else if (_NowTargetTower == 1)
            gold = _DrumTowerUpgradeCost[StaticMng.Instance._DrumTowerLevel];
        else if (_NowTargetTower == 2)
            gold = _BassTowerUpgradeCost[StaticMng.Instance._BassTowerLevel];
        else if (_NowTargetTower == 3)
            gold = _KeyBoardTowerUpgradeCost[StaticMng.Instance._KeyBoardTowerLevel];
        gold /= 3;

        if (StaticMng.Instance._Gold >= gold)
        {
            StaticMng.Instance._Gold -= gold;
            _TowerStatResetCheckPopup.SetActive(false);
            _LogPopup.SetActive(true);
            _LogAni.SetTrigger("open");
            
            int rank = 1;
            int random = Random.Range(0, 100);
            if (random >= 90)
                rank = 5;
            else if (random >= 65)
                rank = 4;
            else if (random >= 35)
                rank = 3;
            else if (random >= 10)
                rank = 2;
            else if (random >= 0)
                rank = 1;
            _Log.text = "'" + _TowerType[_NowTargetTower] + "'의 등급이\r\n'" + _RankName[rank - 1]+ "' 등급으로\r\n재설정 되었습니다";

            if (_NowTargetTower == 0)
                StaticMng.Instance._GuitarTowerRank = rank;
            else if (_NowTargetTower == 1)
                StaticMng.Instance._DrumTowerRank = rank;
            else if (_NowTargetTower == 2)
                StaticMng.Instance._BassTowerRank = rank;
            else if (_NowTargetTower == 3)
                StaticMng.Instance._KeyBoardTowerRank = rank;
            _DataSaveMng.WantDataSave();

        }
        else
        {
            _TowerUpgradeCheckPopup.SetActive(false);
            _LogPopup.SetActive(true);
            _LogAni.SetTrigger("open");
            _Log.text = "골드가 부족합니다";
        }
    }

    void CheckExp()
    {
        bool[] check = { false, false, false, false };
        int templevel = StaticMng.Instance._Player_Level;
        int tempexp = 0;
        while (true)
        {
            for (int i = 0; i < 4; i++)
                check[i] = false;
            tempexp = 0;

            for (int i=0;i<4;i++)
            {
                if(i==0)
                {
                    if (templevel < StaticMng.Instance._GuitarTowerLevel)
                    {
                        check[i] = true;
                        tempexp++;
                    }
                }
                else if (i == 1)
                {
                    if (templevel < StaticMng.Instance._DrumTowerLevel)
                    {
                        check[i] = true;
                        tempexp++;
                    }
                }
                else if (i == 2)
                {
                    if (templevel < StaticMng.Instance._BassTowerLevel)
                    {
                        check[i] = true;
                        tempexp++;
                    }
                }
                else if (i == 3)
                {
                    if (templevel < StaticMng.Instance._KeyBoardTowerLevel)
                    {
                        check[i] = true;
                        tempexp++;
                    }
                }
            }
            StaticMng.Instance._Player_NowExp = tempexp;
            
            bool br = false;
            for(int i=0;i<4;i++)
            {
                if (!check[i])
                    br = true;
            }
            if (br)
                break;
            else
            {
                StaticMng.Instance._Player_Level++;
                templevel++;
                //Debug.Log("Up");
            }
        }
        _DataSaveMng.WantDataSave();
    }

    public void CancelUpgarde()
    {
        _TowerUpgradeCheckPopup.SetActive(false);
        _TowerStatResetCheckPopup.SetActive(false);
    }

    public void TabSelect(int num)
    {
        _NowTabNumber = num;
        for (int i=0;i<4;i++)
        {
            _Tabs[i].SetActive(false);
            _TabButtonGray[i].SetActive(false);
            if(i==num)
            {
                _Tabs[i].SetActive(true);
                _TabButtonGray[i].SetActive(true);
            }
        }
    }

    public void OpenUpgardePopup()
    {
        _UpgradePopup.SetActive(true);
        TutorialMng_Main.Data.CheckTutorialClear(1);
    }
    public void CloseUpgradePopup()
    {
        _UpgradePopup.SetActive(false);
    }
}