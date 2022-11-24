using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MySql.Data.MySqlClient;    //클라이언트 기능을사용하기 위해서 사용
using System.Threading;
using System;
using UnityEngine.SceneManagement;

public class DataSaveMng : MonoBehaviour {

    MySqlConnection conn;
    string connectdb = "Server=" + StaticMng.Instance._IpAdress + StaticMng.Instance._DBIDPW;
    [SerializeField]
    GameObject _LoadingTable;
    DBSupport_SaveMng _DBSupport;
    bool _AcceptReady;
    bool _WantSave;
    bool _WantLoad;
    bool _WantInit;

    [SerializeField]
    GameObject _ErrorPopup;
    [SerializeField]
    Animator _ErrorPopupAni;
    [SerializeField]
    UILabel _ErrorLog;

    [SerializeField]
    GameObject _ErrorPopup_Quit;
    [SerializeField]
    Animator _ErrorPopupAni_Quit;
    [SerializeField]
    UILabel _ErrorLog_Quit;

    [SerializeField]
    GameObject _ErrorPopup_HyperLink;
    [SerializeField]
    Animator _ErrorPopupAni_HyperLink;
    [SerializeField]
    UILabel _ErrorLog_HyperLink;


    bool _SceneChange;
    string _SceneName;

    public bool GetDataSending() { return _DBSupport.GetThreading(); }
    void Awake()
    {
        conn = new MySqlConnection(connectdb);
        _DBSupport = new DBSupport_SaveMng();
        _DBSupport.Init();
    }
    public void Init()
    {
        conn = new MySqlConnection(connectdb);
        _DBSupport = new DBSupport_SaveMng();
        _DBSupport.Init();
    }
    void Update()
    {
        if (_AcceptReady && _DBSupport.GetCanConnect())
        {
            //succese
            _AcceptReady = false;
            //if (_WantSave)
            //    SaveData();
            //else if (_WantLoad)
            //    LoadData();
            //else if (_WantInit)
            //    DataInit();
            _WantSave = false;
            _WantLoad = false;
            _WantInit = false;
            _LoadingTable.SetActive(false);
            _DBSupport.SetFalse();
            if(_SceneChange)
            {
                StartCoroutine(SceneChangeLoading_C(""));
                
            }
            
        }
        if (_AcceptReady && _DBSupport.GetConnectFail())
        {
            //Fail
            _AcceptReady = false;
            _LoadingTable.SetActive(false);
            ExportError("서버와의 접속이\r\n원활하지 않습니다");
            _DBSupport.SetFalse();
        }
        if (_AcceptReady && _DBSupport.GetNotCurrentVersion())
        {
            _AcceptReady = false;
            _LoadingTable.SetActive(false);
            ExportError_HyperLink("최신버전으로 게임을\r\n업데이트해 주십시오");
            _DBSupport.SetFalse();
        }
        if (_AcceptReady && _DBSupport.GetServerClose())
        {
            _AcceptReady = false;
            _LoadingTable.SetActive(false);
            ExportError_Quit("서버가 점검중입니다");
            _DBSupport.SetFalse();
        }
    }
    IEnumerator SceneChangeLoading_C(string scenename)
    {
        yield return null;
        while (GetDataSending())
            yield return null;
        StaticMng.Instance._NowWantScene = _SceneName;
        SceneManager.LoadScene("LoadingScene");
    }

    public void WantDataInit()
    {
        _LoadingTable.SetActive(true);
        _DBSupport.Start(1);
        _AcceptReady = true;
        _WantInit = true;
    }
    public void WantDataSave()
    {
        _LoadingTable.SetActive(true);
        _DBSupport.Start(2);
        _AcceptReady = true;
        _WantSave = true;
    }
    public void WantDataLoad()
    {
        _LoadingTable.SetActive(true);
        _DBSupport.Start(3);
        _AcceptReady = true;
        _WantLoad = true;
    }
    public void EndAndSceneChange(bool scenechange, string scenename)
    {
        _SceneChange = scenechange;
        _SceneName = scenename;
    }
    void ChangeScene()
    {
        StaticMng.Instance._NowWantScene = "MainScene";
        SceneManager.LoadScene("LoadingScene");
    }

    void DataInit()
    {
        #region
        //PlayerPrefs.SetInt("Initiate", 1);
        //
        //PlayerPrefs.SetInt("PlayerLevel", 1);
        //PlayerPrefs.SetInt("Gold", 0);
        //PlayerPrefs.SetInt("Gem", 0);
        //PlayerPrefs.SetInt("NowExp", 0);
        //PlayerPrefs.SetInt("TowerLevel_Drum", 1);
        //PlayerPrefs.SetInt("TowerLevel_Guitar", 1);
        //PlayerPrefs.SetInt("TowerLevel_Bass", 1);
        //PlayerPrefs.SetInt("TowerLevel_KeyBoard", 1);
        //PlayerPrefs.SetInt("TowerRank_Drum", 3);
        //PlayerPrefs.SetInt("TowerRank_Guitar", 3);
        //PlayerPrefs.SetInt("TowerRank_Bass", 3);
        //PlayerPrefs.SetInt("TowerRank_KeyBoard", 3);
        //PlayerPrefs.SetInt("VolumeBool", 1);
        //PlayerPrefs.SetFloat("Volume", 1.0f);
        //PlayerPrefs.SetInt("UnlockChapter", 1);
        //PlayerPrefs.SetInt("UnlockSector_1", 1);
        //PlayerPrefs.SetInt("UnlockSector_2", 0);
        //PlayerPrefs.SetInt("UnlockSector_3", 0);
        //PlayerPrefs.SetInt("UnlockSector_4", 0);
        //PlayerPrefs.SetInt("IdentityNum", 0);
        //PlayerPrefs.SetInt("InfinityScore", 0);
        //PlayerPrefs.SetInt("InfinityFastValue", 1);
        //for (int i = 0; i < StaticMng.Instance._MaximumChapter; i++)
        //{
        //    for (int j = 0; j < StaticMng.Instance._MaximumSector[i]; j++)
        //        PlayerPrefs.SetInt("StagePeak_" + i.ToString() + "_" + j.ToString(), 0);
        //}
        //for(int i=0;i<StaticMng.Instance._MaxAchievementCount;i++)
        //{
        //    PlayerPrefs.SetInt("Achieve_NowInfo_" + i.ToString(), 0);
        //    PlayerPrefs.SetInt("Achieve_NowValue_" + i.ToString(), 0);
        //}
        #endregion
        
    }
    public void FirstOnLoad()
    {
        for (int i = 0; i < StaticMng.Instance._MaxAchievementCount; i++)
        {
            StaticMng.Instance._Achive_ClearCheck.Add(0);
            StaticMng.Instance._Achive_NowValue.Add(0);
        }
        try
        {
            string spdata;
            string achidata;
            string q =
                "select Level,NowExp,Gold,UserName,DrumLevel,GuitarLevel,BassLevel,KeyBoardLevel,Volume,UnlockChapter,UnlockSector_1,UnlockSector_2, UnlockSector_3, UnlockSector_4,StagePeak,Achievement,Gem,GuitarRank,DrumRank,BassRank,KeyBoardRank,InfinityGameScore,IdentityNumber,InfinityFastValue from BuskingTest where ID = '" + StaticMng.Instance._UserId + "' and PW = PASSWORD('" + StaticMng.Instance._UserPW + "');";

            try
            {
                bool getdata = false;
                MySqlCommand cmd = new MySqlCommand(q, conn);
                conn.Open();
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    getdata = true;
                    StaticMng.Instance._Player_Level = int.Parse(reader[0].ToString());
                    StaticMng.Instance._Player_NowExp = int.Parse(reader[1].ToString());
                    StaticMng.Instance._Gold = int.Parse(reader[2].ToString());
                    StaticMng.Instance._UserName = reader[3].ToString();
                    StaticMng.Instance._DrumTowerLevel = int.Parse(reader[4].ToString());
                    StaticMng.Instance._GuitarTowerLevel = int.Parse(reader[5].ToString());
                    StaticMng.Instance._BassTowerLevel = int.Parse(reader[6].ToString());
                    StaticMng.Instance._KeyBoardTowerLevel = int.Parse(reader[7].ToString());
                    StaticMng.Instance._Option_Volume = float.Parse(reader[8].ToString());
                    if (StaticMng.Instance._Option_Volume == 1)
                        StaticMng.Instance._Option_Volume_Bool = true;
                    else
                        StaticMng.Instance._Option_Volume_Bool = false;
                    StaticMng.Instance._UnLock_Chapter = int.Parse(reader[9].ToString());
                    StaticMng.Instance._UnLock_Sector[0] = int.Parse(reader[10].ToString());
                    StaticMng.Instance._UnLock_Sector[1] = int.Parse(reader[11].ToString());
                    StaticMng.Instance._UnLock_Sector[2] = int.Parse(reader[12].ToString());
                    StaticMng.Instance._UnLock_Sector[3] = int.Parse(reader[13].ToString());
                    spdata = reader[14].ToString();
                    achidata = reader[15].ToString();
                    StaticMng.Instance._Gem = int.Parse(reader[16].ToString());
                    StaticMng.Instance._GuitarTowerRank = int.Parse(reader[17].ToString());
                    StaticMng.Instance._DrumTowerRank = int.Parse(reader[18].ToString());
                    StaticMng.Instance._BassTowerRank = int.Parse(reader[19].ToString());
                    StaticMng.Instance._KeyBoardTowerRank = int.Parse(reader[20].ToString());
                    StaticMng.Instance._InfinityGameScore = int.Parse(reader[21].ToString());
                    StaticMng.Instance._InfinityScoreIdentity = int.Parse(reader[22].ToString());
                    StaticMng.Instance._Infinity_FastValue = int.Parse(reader[23].ToString());

                    Dictionary<string, string> json = new Dictionary<string, string>();
                    json = JSon.Read(spdata);
                    for (int i = 0; i < StaticMng.Instance._MaximumChapter; i++)
                    {
                        for (int j = 0; j < StaticMng.Instance._MaximumSector[i]; j++)
                        {
                            StaticMng.Instance._StagePeakCount[i, j] = int.Parse(json["sp_" + i.ToString() + "_" + j.ToString()]);
                        }
                    }

                    Dictionary<string, string> achijson = new Dictionary<string, string>();
                    achijson = JSon.Read(achidata);
                    for (int i = 0; i < StaticMng.Instance._MaxAchievementCount; i++)
                    {
                        StaticMng.Instance._Achive_NowValue[i] = int.Parse(achijson["Achi_NowVal" + i.ToString()]);
                        StaticMng.Instance._Achive_ClearCheck[i] = int.Parse(achijson["Achi_NowInfo" + i.ToString()]);
                    }
                }
                //if (getdata)
                //{
                //    Debug.Log("loadsucces");
                //    //StaticMng.Instance._Stage_Chapter = 1;
                //}
                //else
                //    Debug.Log("loadfail");
                conn.Close();

            }
            catch (System.Exception ex)
            {
                Debug.Log(ex.ToString());

                conn.Close();
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex);
        }

    }
    void LoadData()
    {
        #region
        //Debug.Log("Load");
        //if (PlayerPrefs.GetInt("TowerRank_Guitar") == 0)
        //    PlayerPrefs.SetInt("TowerRank_Guitar" , 3);
        //if (PlayerPrefs.GetInt("TowerRank_Drum") == 0)
        //    PlayerPrefs.SetInt("TowerRank_Drum", 3);
        //if (PlayerPrefs.GetInt("TowerRank_Bass") == 0)
        //    PlayerPrefs.SetInt("TowerRank_Bass", 3);
        //if (PlayerPrefs.GetInt("TowerRank_KeyBoard") == 0)
        //    PlayerPrefs.SetInt("TowerRank_KeyBoard", 3);
        //if (PlayerPrefs.GetInt("InfinityFastValue") == 0)
        //    PlayerPrefs.SetInt("InfinityFastValue", 1);
        //
        //
        //StaticMng.Instance._Player_Level = PlayerPrefs.GetInt("PlayerLevel");
        //StaticMng.Instance._Gold = PlayerPrefs.GetInt("Gold");
        //StaticMng.Instance._Gem = PlayerPrefs.GetInt("Gem");
        //StaticMng.Instance._Player_NowExp = PlayerPrefs.GetInt("NowExp");
        //StaticMng.Instance._DrumTowerLevel = PlayerPrefs.GetInt("TowerLevel_Drum");
        //StaticMng.Instance._GuitarTowerLevel = PlayerPrefs.GetInt("TowerLevel_Guitar");
        //StaticMng.Instance._BassTowerLevel = PlayerPrefs.GetInt("TowerLevel_Bass");
        //StaticMng.Instance._KeyBoardTowerLevel = PlayerPrefs.GetInt("TowerLevel_KeyBoard");
        //StaticMng.Instance._GuitarTowerRank = PlayerPrefs.GetInt("TowerRank_Guitar");
        //StaticMng.Instance._DrumTowerRank = PlayerPrefs.GetInt("TowerRank_Drum");
        //StaticMng.Instance._BassTowerRank = PlayerPrefs.GetInt("TowerRank_Bass");
        //StaticMng.Instance._KeyBoardTowerRank = PlayerPrefs.GetInt("TowerRank_KeyBoard");
        //StaticMng.Instance._InfinityScoreIdentity = PlayerPrefs.GetInt("IdentityNum");
        //StaticMng.Instance._InfinityGameScore = PlayerPrefs.GetInt("InfinityScore");
        //StaticMng.Instance._Infinity_FastValue = PlayerPrefs.GetInt("InfinityFastValue");
        //int tempbool = PlayerPrefs.GetInt("VolumeBool");
        //if (tempbool == 1)
        //{
        //    StaticMng.Instance._Option_Volume_Bool = true;
        //    StaticMng.Instance._Option_Volume = 1.0f;
        //}
        //else
        //{
        //    StaticMng.Instance._Option_Volume_Bool = false;
        //    StaticMng.Instance._Option_Volume = 0.0f;
        //}
        //StaticMng.Instance._Option_Volume = PlayerPrefs.GetFloat("Volume");
        //StaticMng.Instance._UnLock_Chapter = PlayerPrefs.GetInt("UnlockChapter");
        //StaticMng.Instance._UnLock_Sector[0] = PlayerPrefs.GetInt("UnlockSector_1");
        //StaticMng.Instance._UnLock_Sector[1] = PlayerPrefs.GetInt("UnlockSector_2");
        //StaticMng.Instance._UnLock_Sector[2] = PlayerPrefs.GetInt("UnlockSector_3");
        //StaticMng.Instance._UnLock_Sector[3] = PlayerPrefs.GetInt("UnlockSector_4");
        //StaticMng.Instance._UserName = PlayerPrefs.GetString("UserName");
        //
        //for (int i = 0; i < StaticMng.Instance._MaximumChapter; i++)
        //{
        //    for (int j = 0; j < StaticMng.Instance._MaximumSector[i]; j++)
        //        StaticMng.Instance._StagePeakCount[i,j] = PlayerPrefs.GetInt("StagePeak_" + //i.ToString() + "_" + j.ToString());
        //}
        
        #endregion
        
        
    }
    void SaveData()
    {
        #region
        //Debug.Log("Save");
        //
        //PlayerPrefs.SetInt("PlayerLevel", StaticMng.Instance._Player_Level);
        //PlayerPrefs.SetInt("Gold", StaticMng.Instance._Gold);
        //PlayerPrefs.SetInt("Gem", StaticMng.Instance._Gem);
        //PlayerPrefs.SetInt("NowExp", StaticMng.Instance._Player_NowExp);
        //PlayerPrefs.SetInt("TowerLevel_Drum", StaticMng.Instance._DrumTowerLevel);
        //PlayerPrefs.SetInt("TowerLevel_Guitar", StaticMng.Instance._GuitarTowerLevel);
        //PlayerPrefs.SetInt("TowerLevel_Bass", StaticMng.Instance._BassTowerLevel);
        //PlayerPrefs.SetInt("TowerLevel_KeyBoard", StaticMng.Instance._KeyBoardTowerLevel);
        //PlayerPrefs.SetInt("TowerRank_Drum", StaticMng.Instance._DrumTowerRank);
        //PlayerPrefs.SetInt("TowerRank_Guitar", StaticMng.Instance._GuitarTowerRank);
        //PlayerPrefs.SetInt("TowerRank_Bass", StaticMng.Instance._BassTowerRank);
        //PlayerPrefs.SetInt("TowerRank_KeyBoard", StaticMng.Instance._KeyBoardTowerRank);
        //PlayerPrefs.SetInt("IdentityNum", StaticMng.Instance._InfinityScoreIdentity);
        //PlayerPrefs.SetInt("InfinityScore", StaticMng.Instance._InfinityGameScore);
        //PlayerPrefs.SetInt("InfinityFastValue", StaticMng.Instance._Infinity_FastValue);
        //
        //if (StaticMng.Instance._Option_Volume_Bool)
        //    PlayerPrefs.SetInt("VolumeBool", 1);
        //else
        //    PlayerPrefs.SetInt("VolumeBool", 0);
        //PlayerPrefs.SetFloat("Volume", StaticMng.Instance._Option_Volume);
        //PlayerPrefs.SetInt("UnlockChapter", StaticMng.Instance._UnLock_Chapter);
        //PlayerPrefs.SetInt("UnlockSector_1", StaticMng.Instance._UnLock_Sector[0]);
        //PlayerPrefs.SetInt("UnlockSector_2", StaticMng.Instance._UnLock_Sector[1]);
        //PlayerPrefs.SetInt("UnlockSector_3", StaticMng.Instance._UnLock_Sector[2]);
        //PlayerPrefs.SetInt("UnlockSector_4", StaticMng.Instance._UnLock_Sector[3]);
        //PlayerPrefs.SetString("UserName", StaticMng.Instance._UserName);
        //
        //for(int i=0;i<StaticMng.Instance._MaximumChapter;i++)
        //{
        //    for(int j=0;j<StaticMng.Instance._MaximumSector[i];j++)
        //        PlayerPrefs.SetInt("StagePeak_"+i.ToString()+"_"+j.ToString(), //StaticMng.Instance._StagePeakCount[i, j]);
        //}
        //for (int i = 0; i < StaticMng.Instance._MaxAchievementCount; i++)
        //{
        //    PlayerPrefs.SetInt("Achieve_NowInfo_" + i.ToString(), //StaticMng.Instance._Achive_ClearCheck[i]);
        //    PlayerPrefs.SetInt("Achieve_NowValue_" + i.ToString(), //StaticMng.Instance._Achive_NowValue[i]);
        //}
        #endregion
        
    }
    void ExportError(string log)
    {
        _ErrorPopup.SetActive(true);
        _ErrorPopupAni.SetTrigger("open");
        _ErrorLog.text = log;
        //Debug.Log(log);
    }
    void ExportError_Quit(string log)
    {
        _ErrorPopup_Quit.SetActive(true);
        _ErrorPopupAni_Quit.SetTrigger("open");
        _ErrorLog_Quit.text = log;
        //Debug.Log(log);
    }
    void ExportError_HyperLink(string log)
    {
        _ErrorPopup_HyperLink.SetActive(true);
        _ErrorPopupAni_HyperLink.SetTrigger("open");
        _ErrorLog_HyperLink.text = log;
        //Debug.Log(log);
    }
    public void DownloadLink()
    {
        Application.OpenURL(StaticMng.Instance._PlayStoreAdress);
        QuitGame();
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void CloseErrorPopup()
    {
        _ErrorPopup.SetActive(false);
    }
}

public class DBSupport_SaveMng
{
    Thread _Thread;
    MySqlConnection conn;
    string connectdb = "Server=" + StaticMng.Instance._IpAdress + StaticMng.Instance._DBIDPW;

    bool _Threading;

    bool _WantConnect;
    bool _CanConnect;
    bool _ConnectFail;
    bool _NotCurrentVersion;
    bool _ServerClose;
    /// <summary>
    /// 1 - init
    /// 2 - save
    /// 3 - load
    /// </summary>
    int _QueryType;

    public void Init()
    {
        conn = new MySqlConnection(connectdb);
    }
    public void Start(int querytype)
    {

        _Thread = new Thread(new ThreadStart(ThreadUpdate));
        _Threading = true;
        _WantConnect = true;
        _CanConnect = false;
        _ConnectFail = false;
        _NotCurrentVersion = false;
        _ServerClose = false;
        _QueryType = querytype;
        _Thread.Start();
    }

    void ThreadUpdate()
    {
        Debug.Log("ThreadStart");
        try
        {
            if (_WantConnect)
            {
                _WantConnect = false;
                string q = "select * from BuskingVersionChecker";
                try
                {
                    bool pass = false;
                    conn.Open();
                    MySqlCommand cmd1 = new MySqlCommand(q, conn);
                    MySqlDataReader reader1 = cmd1.ExecuteReader();
                    reader1.Read();
                    if (reader1[0].ToString() == StaticMng.Instance._AppVersion)
                    {
                        if (reader1[1].ToString() == "1")
                            pass = true;
                        else
                            _ServerClose = true;
                    }
                    else
                        _NotCurrentVersion = true;
                    
                    reader1.Close();

                    conn.Close();
                    if (pass)
                    {
                        if (_QueryType == 1)
                            DataInit();
                        else if (_QueryType == 2)
                            DataSave();
                        else if (_QueryType == 3)
                            DataLoad();
                        _Threading = false;
                        _CanConnect = true;
                    }
                }
                catch (Exception ex)
                {
                    Debug.Log(ex.ToString());
                    _ConnectFail = true;
                    _Threading = false;

                }
            }
            //Debug.Log("ThreadEnd");

        }
        catch (Exception ex)
        {
            Debug.Log(ex.ToString());
            _Threading = false;
        }

        Debug.Log("ThreadingEnd");
    }
    void DataInit()
    {
        for (int i = 0; i < StaticMng.Instance._MaxAchievementCount; i++)
        {
            StaticMng.Instance._Achive_ClearCheck.Add(0);
            StaticMng.Instance._Achive_NowValue.Add(0);
        }
        try
        {
            Dictionary<string, string> dicJson = new Dictionary<string, string>();
            for (int i = 0; i < StaticMng.Instance._MaximumChapter; i++)
            {
                for (int j = 0; j < StaticMng.Instance._MaximumSector[i]; j++)
                {
                    dicJson.Add("sp_" + i.ToString() + "_" + j.ToString(), "0");
                }
            }
            string spdata = JSon.Write(dicJson);

            Dictionary<string, string> achivejson = new Dictionary<string, string>();
            for (int i = 0; i < StaticMng.Instance._MaxAchievementCount; i++)
            {
                achivejson.Add("Achi_NowVal" + i.ToString(), "0");
                achivejson.Add("Achi_NowInfo" + i.ToString(), "0");
            }
            string achivedata = JSon.Write(achivejson);

            string q2 = "update BuskingTest set UserName = '" + StaticMng.Instance._UserName + "',Level=" + "1" + ",NowExp=" + "0" + ",Gold=" + "0" + ",DrumLevel = " + "1" + ",GuitarLevel=" + "1" + ",BassLevel=" + "1" + ",KeyBoardLevel=" + "1" + ",Volume=" + "1" + ",UnlockChapter=" + "1" + ",UnlockSector_1=" + "1" + ",UnlockSector_2=" + "0" + ",UnlockSector_3=" + "0" + ",UnlockSector_4=" + "0" + ",StagePeak ='" + spdata.ToString() + "',Achievement = '" + achivedata + "',Gem = " + "0" + ",GuitarRank = " + "3" + ",DrumRank = " + "3" + ",BassRank = " + "3" + ",KeyBoardRank = " + "3" + ",InfinityGameScore = " + "0" + ",IdentityNumber = " + "0" + ",InfinityFastValue = " + "1" + " where id='" + StaticMng.Instance._UserId + "';";


            conn.Open();

            MySqlCommand cmd = new MySqlCommand(q2, conn);
            MySqlDataReader reader = cmd.ExecuteReader();
            conn.Close();

            DataLoad();
        }
        catch (Exception ex)
        {
            Debug.Log(ex);
        }
        try
        {
            string spdata;
            string achidata;
            string q =
                "select Level,NowExp,Gold,UserName,DrumLevel,GuitarLevel,BassLevel,KeyBoardLevel,Volume,UnlockChapter,UnlockSector_1,UnlockSector_2, UnlockSector_3, UnlockSector_4,StagePeak,Achievement,Gem,GuitarRank,DrumRank,BassRank,KeyBoardRank,InfinityGameScore,IdentityNumber,InfinityFastValue from BuskingTest where ID = '" + StaticMng.Instance._UserId + "' and PW = PASSWORD('" + StaticMng.Instance._UserPW + "');";

            try
            {
                bool getdata = false;
                MySqlCommand cmd = new MySqlCommand(q, conn);
                conn.Open();
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    getdata = true;
                    StaticMng.Instance._Player_Level = int.Parse(reader[0].ToString());
                    StaticMng.Instance._Player_NowExp = int.Parse(reader[1].ToString());
                    StaticMng.Instance._Gold = int.Parse(reader[2].ToString());
                    StaticMng.Instance._UserName = reader[3].ToString();
                    StaticMng.Instance._DrumTowerLevel = int.Parse(reader[4].ToString());
                    StaticMng.Instance._GuitarTowerLevel = int.Parse(reader[5].ToString());
                    StaticMng.Instance._BassTowerLevel = int.Parse(reader[6].ToString());
                    StaticMng.Instance._KeyBoardTowerLevel = int.Parse(reader[7].ToString());
                    StaticMng.Instance._Option_Volume = float.Parse(reader[8].ToString());
                    if (StaticMng.Instance._Option_Volume == 1)
                        StaticMng.Instance._Option_Volume_Bool = true;
                    else
                        StaticMng.Instance._Option_Volume_Bool = false;
                    StaticMng.Instance._UnLock_Chapter = int.Parse(reader[9].ToString());
                    StaticMng.Instance._UnLock_Sector[0] = int.Parse(reader[10].ToString());
                    StaticMng.Instance._UnLock_Sector[1] = int.Parse(reader[11].ToString());
                    StaticMng.Instance._UnLock_Sector[2] = int.Parse(reader[12].ToString());
                    StaticMng.Instance._UnLock_Sector[3] = int.Parse(reader[13].ToString());
                    spdata = reader[14].ToString();
                    achidata = reader[15].ToString();
                    StaticMng.Instance._Gem = int.Parse(reader[16].ToString());
                    StaticMng.Instance._GuitarTowerRank = int.Parse(reader[17].ToString());
                    StaticMng.Instance._DrumTowerRank = int.Parse(reader[18].ToString());
                    StaticMng.Instance._BassTowerRank = int.Parse(reader[19].ToString());
                    StaticMng.Instance._KeyBoardTowerRank = int.Parse(reader[20].ToString());
                    StaticMng.Instance._InfinityGameScore = int.Parse(reader[21].ToString());
                    StaticMng.Instance._InfinityScoreIdentity = int.Parse(reader[22].ToString());
                    StaticMng.Instance._Infinity_FastValue = int.Parse(reader[23].ToString());

                    Dictionary<string, string> json = new Dictionary<string, string>();
                    json = JSon.Read(spdata);
                    for (int i = 0; i < StaticMng.Instance._MaximumChapter; i++)
                    {
                        for (int j = 0; j < StaticMng.Instance._MaximumSector[i]; j++)
                        {
                            StaticMng.Instance._StagePeakCount[i, j] = int.Parse(json["sp_" + i.ToString() + "_" + j.ToString()]);
                        }
                    }

                    Dictionary<string, string> achijson = new Dictionary<string, string>();
                    achijson = JSon.Read(achidata);
                    for (int i = 0; i < StaticMng.Instance._MaxAchievementCount; i++)
                    {
                        StaticMng.Instance._Achive_NowValue[i] = int.Parse(achijson["Achi_NowVal" + i.ToString()]);
                        StaticMng.Instance._Achive_ClearCheck[i] = int.Parse(achijson["Achi_NowInfo" + i.ToString()]);
                        //Debug.Log("Load_ClearCheck");
                    }
                }
                //if (getdata)
                //{
                //    Debug.Log("loadsucces");
                //    //StaticMng.Instance._Stage_Chapter = 1;
                //}
                //else
                //    Debug.Log("loadfail");
                conn.Close();

            }
            catch (System.Exception ex)
            {
                Debug.Log(ex.ToString());

                conn.Close();
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex);
        }
    }

    void DataSave()
    {
        try
        {
            Dictionary<string, string> dicJson = new Dictionary<string, string>();
            for (int i = 0; i < StaticMng.Instance._MaximumChapter; i++)
            {
                for (int j = 0; j < StaticMng.Instance._MaximumSector[i]; j++)
                {
                    dicJson.Add("sp_" + i.ToString() + "_" + j.ToString(), StaticMng.Instance._StagePeakCount[i, j].ToString());
                }
            }
            string spdata = JSon.Write(dicJson);
            Dictionary<string, string> achivejson = new Dictionary<string, string>();
            for (int i = 0; i < StaticMng.Instance._MaxAchievementCount; i++)
            {
                achivejson.Add("Achi_NowVal" + i.ToString(), StaticMng.Instance._Achive_NowValue[i].ToString());
                achivejson.Add("Achi_NowInfo" + i.ToString(), StaticMng.Instance._Achive_ClearCheck[i].ToString());
            }
            string achivedata = JSon.Write(achivejson);

            int tempvolume;
            if (StaticMng.Instance._Option_Volume_Bool)
                tempvolume = 1;
            else
                tempvolume = 0;

            string q2 = "update BuskingTest set UserName = '"+StaticMng.Instance._UserName+"',Level=" + StaticMng.Instance._Player_Level.ToString() + ",NowExp=" + StaticMng.Instance._Player_NowExp.ToString() + ",Gold=" + StaticMng.Instance._Gold.ToString() + ",DrumLevel = " + StaticMng.Instance._DrumTowerLevel.ToString() + ",GuitarLevel=" + StaticMng.Instance._GuitarTowerLevel.ToString() + ",BassLevel=" + StaticMng.Instance._BassTowerLevel.ToString() + ",KeyBoardLevel=" + StaticMng.Instance._KeyBoardTowerLevel.ToString() + ",Volume=" + tempvolume.ToString() + ",UnlockChapter=" + StaticMng.Instance._UnLock_Chapter.ToString() + ",UnlockSector_1=" + StaticMng.Instance._UnLock_Sector[0].ToString() + ",UnlockSector_2=" + StaticMng.Instance._UnLock_Sector[1].ToString() + ",UnlockSector_3=" + StaticMng.Instance._UnLock_Sector[2].ToString() + ",UnlockSector_4=" + StaticMng.Instance._UnLock_Sector[3].ToString() + ",StagePeak ='" + spdata.ToString() + "',Achievement = '" + achivedata + "',Gem = " + StaticMng.Instance._Gem.ToString() + ",GuitarRank = " + StaticMng.Instance._GuitarTowerRank.ToString() + ",DrumRank = " + StaticMng.Instance._DrumTowerRank.ToString() + ",BassRank = " + StaticMng.Instance._BassTowerRank.ToString() + ",KeyBoardRank = " + StaticMng.Instance._KeyBoardTowerRank.ToString() + ",InfinityGameScore = " + StaticMng.Instance._InfinityGameScore.ToString() + ",IdentityNumber = " + StaticMng.Instance._InfinityScoreIdentity.ToString() + ",InfinityFastValue = " + StaticMng.Instance._Infinity_FastValue.ToString() + " where id='" + StaticMng.Instance._UserId + "';";

            conn.Open();
            MySqlCommand cmd2 = new MySqlCommand(q2, conn);
            MySqlDataReader reader2 = cmd2.ExecuteReader();
            //Debug.Log("Save");
            conn.Close();
            DataLoad();
        }
        catch (Exception ex)
        {
            Debug.Log(ex);
            conn.Close();
        }
    }
    void DataLoad()
    {
        try
        {
            string spdata;
            string achidata;
            string q =
                "select Level,NowExp,Gold,UserName,DrumLevel,GuitarLevel,BassLevel,KeyBoardLevel,Volume,UnlockChapter,UnlockSector_1,UnlockSector_2, UnlockSector_3, UnlockSector_4,StagePeak,Achievement,Gem,GuitarRank,DrumRank,BassRank,KeyBoardRank,InfinityGameScore,IdentityNumber,InfinityFastValue from BuskingTest where ID = '" + StaticMng.Instance._UserId + "' and PW = PASSWORD('" + StaticMng.Instance._UserPW + "');";

            try
            {
                bool getdata = false;
                MySqlCommand cmd = new MySqlCommand(q, conn);
                conn.Open();
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    getdata = true;
                    StaticMng.Instance._Player_Level = int.Parse(reader[0].ToString());
                    StaticMng.Instance._Player_NowExp = int.Parse(reader[1].ToString());
                    StaticMng.Instance._Gold = int.Parse(reader[2].ToString());
                    StaticMng.Instance._UserName = reader[3].ToString();
                    StaticMng.Instance._DrumTowerLevel = int.Parse(reader[4].ToString());
                    StaticMng.Instance._GuitarTowerLevel = int.Parse(reader[5].ToString());
                    StaticMng.Instance._BassTowerLevel = int.Parse(reader[6].ToString());
                    StaticMng.Instance._KeyBoardTowerLevel = int.Parse(reader[7].ToString());
                    StaticMng.Instance._Option_Volume = float.Parse(reader[8].ToString());
                    if (StaticMng.Instance._Option_Volume == 1)
                        StaticMng.Instance._Option_Volume_Bool = true;
                    else
                        StaticMng.Instance._Option_Volume_Bool = false;
                    StaticMng.Instance._UnLock_Chapter = int.Parse(reader[9].ToString());
                    StaticMng.Instance._UnLock_Sector[0] = int.Parse(reader[10].ToString());
                    StaticMng.Instance._UnLock_Sector[1] = int.Parse(reader[11].ToString());
                    StaticMng.Instance._UnLock_Sector[2] = int.Parse(reader[12].ToString());
                    StaticMng.Instance._UnLock_Sector[3] = int.Parse(reader[13].ToString());
                    spdata = reader[14].ToString();
                    achidata = reader[15].ToString();
                    StaticMng.Instance._Gem = int.Parse(reader[16].ToString());
                    StaticMng.Instance._GuitarTowerRank = int.Parse(reader[17].ToString());
                    StaticMng.Instance._DrumTowerRank = int.Parse(reader[18].ToString());
                    StaticMng.Instance._BassTowerRank = int.Parse(reader[19].ToString());
                    StaticMng.Instance._KeyBoardTowerRank = int.Parse(reader[20].ToString());
                    StaticMng.Instance._InfinityGameScore = int.Parse(reader[21].ToString());
                    StaticMng.Instance._InfinityScoreIdentity = int.Parse(reader[22].ToString());
                    StaticMng.Instance._Infinity_FastValue = int.Parse(reader[23].ToString());

                    Dictionary<string, string> json = new Dictionary<string, string>();
                    json = JSon.Read(spdata);
                    for (int i = 0; i < StaticMng.Instance._MaximumChapter; i++)
                    {
                        for (int j = 0; j < StaticMng.Instance._MaximumSector[i]; j++)
                        {
                            StaticMng.Instance._StagePeakCount[i, j] = int.Parse(json["sp_" + i.ToString() + "_" + j.ToString()]);
                        }
                    }

                    Dictionary<string, string> achijson = new Dictionary<string, string>();
                    achijson = JSon.Read(achidata);
                    for (int i = 0; i < StaticMng.Instance._MaxAchievementCount; i++)
                    {
                        StaticMng.Instance._Achive_NowValue[i] = int.Parse(achijson["Achi_NowVal" + i.ToString()]);
                        StaticMng.Instance._Achive_ClearCheck[i] = int.Parse(achijson["Achi_NowInfo" + i.ToString()]);
                    }
                }
                //if (getdata)
                //{
                //    //Debug.Log("loadsucces");
                //    //StaticMng.Instance._Stage_Chapter = 1;
                //}
                //else
                //    Debug.Log("loadfail");
                conn.Close();

            }
            catch (System.Exception ex)
            {
                Debug.Log(ex.ToString());

                conn.Close();
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex);
        }
    }
    public bool GetThreading() { return _Threading; }
    public bool GetCanConnect() { return _CanConnect; }
    public bool GetConnectFail() { return _ConnectFail; }
    public bool GetNotCurrentVersion() { return _NotCurrentVersion; }
    public bool GetServerClose() { return _ServerClose; }
    public void SetCanConnectFalse() { _CanConnect = false; }
    public void SetConnectFailFalse() { _ConnectFail = false; }
    public void SetNotCurrentVersion() { _NotCurrentVersion = false; }
    public void SetServerClose() { _ServerClose = false; }
    public void SetFalse()
    {
        _CanConnect = false;
        _ConnectFail = false;
        _NotCurrentVersion = false;
        _ServerClose = false;
    }
}