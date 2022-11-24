//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using System.Data;     //C#의 데이터 테이블 때문에 사용
using MySql.Data;     //MYSQL함수들을 불러오기 위해서 사용
using MySql.Data.MySqlClient;    //클라이언트 기능을사용하기 위해서 사용
using System.Threading;
using System;

//public class DBConnectMng_My : MonoBehaviour {

//    bool _WantSave;
//    bool _WantLoad;
//    MySqlConnection conn;
//    string connectdb = "Server=" + StaticMng.Instance._IpAdress + ";Database=BuskingServer;UserId=root;Password=Jsysb0p5pN1k6wI7;";
//    [SerializeField]
//    GameObject _IDInputField;
//    [SerializeField]
//    UIInput _IDInput;
//    [SerializeField]
//    UIInput _PWInput;
//    [SerializeField]
//    GameObject _ErrorPopup;
//    [SerializeField]
//    Animator _ErrorPopupAni;
//    [SerializeField]
//    UILabel _ErrorLog;
//    [SerializeField]
//    GameObject _ErrorPopup_Quit;
//    [SerializeField]
//    Animator _ErrorPopupAni_Quit;
//    [SerializeField]
//    UILabel _ErrorLog_Quit;
//    [SerializeField]
//    GameObject _ErrorPopup_Choice;
//    [SerializeField]
//    Animator _ErrorPopupAni_Choice;
//    [SerializeField]
//    UILabel _ErrorLog_Choice;
//    [SerializeField]
//    GameObject _CoverPasswordPopup;
//    [SerializeField]
//    UIInput _CoverPWInput;

//    string _SaveTempId;

//    [SerializeField]
//    DataSaveMng _DataSaveMng;
//    [SerializeField]
//    MainSceneMng _MainUIMng;
//    [SerializeField]
//    StageBackgroundDecoMng _StageBackgroundMng;

//    [SerializeField]
//    GameObject _LoadingTable;

//    DBSupport _DBSupport;

//    bool _AcceptReady;

//    void Start()
//    {
//        conn = new MySqlConnection(connectdb);
//        _DBSupport = new DBSupport();
//        _DBSupport.Init();
//    }

//    public void WantSaveData()
//    {
//        _IDInputField.SetActive(true);
//        _WantSave = true;
//    }
//    public void WantLoadData()
//    {
//        _IDInputField.SetActive(true);
//        _WantLoad = true;
//    }

//    public void CloseInputField()
//    {
//        _IDInputField.SetActive(false);
//        _WantLoad = false;
//        _WantSave = false;
//    }

//    void Update()
//    {
//        //if(_AcceptReady && _DBSupport.GetCanConnect())
//        //{
//        //    //succese
//        //    _AcceptReady = false;
//        //    if (_WantSave)
//        //        SaveData();
//        //    else if (_WantLoad)
//        //        LoadData();
//        //    _WantSave = false;
//        //    _WantLoad = false;
//        //    _IDInputField.SetActive(false);
//        //    _IDInput.value = "";
//        //    _PWInput.value = "";
//        //    _LoadingTable.SetActive(false);
//        //    _DBSupport.SetFalse();
//        //}
//        //if(_AcceptReady && _DBSupport.GetConnectFail())
//        //{
//        //    //Fail
//        //    _AcceptReady = false;
//        //    _LoadingTable.SetActive(false);
//        //    ExportError("서버와의 접속이 원활하지 않습니다.\r\n인터넷 연결을 확인하여 주십시오.");
//        //    _DBSupport.SetFalse();
//        //}
//        //if(_AcceptReady && _DBSupport.GetNotCurrentVersion())
//        //{
//        //    _AcceptReady = false;
//        //    _LoadingTable.SetActive(false);
//        //    ExportError_Quit("최신버전으로 게임을\r\n업데이트해 주십시오");
//        //    _DBSupport.SetFalse();
//        //}
//        //if(_AcceptReady && _DBSupport.GetServerClose())
//        //{
//        //    _AcceptReady = false;
//        //    _LoadingTable.SetActive(false);
//        //    ExportError_Quit("서버가 점검중입니다");
//        //    _DBSupport.SetFalse();
//        //}
//    }

//    public void EndIDInsert()
//    {
//        if (!Checking(_IDInput.value) && !Checking(_PWInput.value))
//        {
//            if (_IDInput.value.Length >= 5 && _PWInput.value.Length >= 4)
//            {
//                _LoadingTable.SetActive(true);
                
//                _DBSupport.Start();
//                _AcceptReady = true;
//                //Debug.Log("ThreadStart And AcceptReady True");

//            }
//            else
//            {
//                ExportError("ID는 5자 이상, 비밀번호는 4자 이상이어야 합니다.");
//            }
//        }
//        else
//        {
//            ExportError("ID,PW에 특수문자, 한글을 사용할 수 없습니다.");
//            _IDInput.value = "";
//            _PWInput.value = "";
//        }
//    }
    
//    //public void SaveData()
//    //{
//    //    Dictionary<string, string> dicJson = new Dictionary<string, string>();
//    //    for(int i=0;i<StaticMng.Instance._MaximumChapter;i++)
//    //    {
//    //        for(int j=0;j<StaticMng.Instance._MaximumSector[i];j++)
//    //        {
//    //            dicJson.Add("sp_" + i.ToString() + "_" + j.ToString(), //StaticMng.Instance._StagePeakCount[i, j].ToString());
//    //        }
//    //    }
//    //    string spdata = JSon.Write(dicJson);
//    //
//    //    Dictionary<string, string> achivejson = new Dictionary<string, string>();
//    //    for(int i=0;i<StaticMng.Instance._MaxAchievementCount;i++)
//    //    {
//    //        achivejson.Add("Achi_NowVal" + i.ToString(), StaticMng.Instance._Achive_NowValue//[i].ToString());
//    //        achivejson.Add("Achi_NowInfo" + i.ToString(), StaticMng.Instance._Achive_ClearCheck//[i].ToString());
//    //    }
//    //    string achivedata = JSon.Write(achivejson);
//    //
//    //    int tempvolume;
//    //    if (StaticMng.Instance._Option_Volume_Bool)
//    //        tempvolume = 1;
//    //    else
//    //        tempvolume = 0;
//    //
//    //    //insert into BuskingTest values('aaaa', PASSWORD('123'), 'admin', 99, 0, 9999999, 1, 1, /1, /1, 1, 1, 1, 0, 0, 0);
//    //    string q = "insert into BuskingTest values('" + _IDInput.value + "',PASSWORD('" + //_PWInput.value + "'),'" + StaticMng.Instance._UserName + "'," + //StaticMng.Instance._Player_Level.ToString() + ","
//    //        + StaticMng.Instance._Player_NowExp.ToString() + "," + /StaticMng.Instance._Gold.ToString/() + ","+StaticMng.Instance._DrumTowerLevel.ToString()//+","+StaticMng.Instance._GuitarTowerLevel.ToString()+
//    //        ","+StaticMng.Instance._BassTowerLevel.ToString()//+","+StaticMng.Instance._KeyBoardTowerLevel.ToString()+"," + tempvolume.ToString()//+","+StaticMng.Instance._UnLock_Chapter.ToString()+","+
//    //        StaticMng.Instance._UnLock_Sector[0].ToString()+","+ StaticMng.Instance._UnLock_Sector//[1].ToString() + ","+ StaticMng.Instance._UnLock_Sector[2].ToString() + ","+ //StaticMng.Instance._UnLock_Sector[3].ToString() + ",'" + spdata+"','"+ achivedata  /+ /"',"+StaticMng.Instance._Gem+ "," + StaticMng.Instance._GuitarTowerRank.ToString() + /"," + /StaticMng.Instance._DrumTowerRank.ToString() + "," + //StaticMng.Instance._BassTowerRank.ToString() + "," + //StaticMng.Instance._KeyBoardTowerRank.ToString() /+ /","+StaticMng.Instance._InfinityGameScore.ToString()//+","+StaticMng.Instance._InfinityScoreIdentity.ToString()//+","+StaticMng.Instance._Infinity_FastValue.ToString()+")";
//    //
//    //    //try
//    //    {
//    //        string tempq = "select count(ID) from BuskingTest where ID = '" + _IDInput.value + "'";
//    //        conn.Open();
//    //        MySqlCommand cmd1 = new MySqlCommand(tempq, conn);
//    //        MySqlDataReader reader1 = cmd1.ExecuteReader();
//    //        reader1.Read();
//    //        if (reader1[0].ToString() == "1")
//    //        {
//    //            //중복
//    //            //Debug.Log("중복");
//    //            _SaveTempId = _IDInput.value;
//    //            ExportError_TwoChoice("이미 같은 ID의\r\n데이터가 있습니다.\r\n덮어씌우시겠습니까?");
//    //            conn.Close();
//    //        }
//    //        else
//    //        {
//    //            ExportError("데이터를\r\n저장하였습니다.");
//    //            reader1.Close();
//    //            MySqlCommand cmd = new MySqlCommand(q, conn);
//    //            MySqlDataReader reader = cmd.ExecuteReader();
//    //            conn.Close();
//    //            _DataSaveMng.SaveData();
//    //        }
//    //
//    //    }
//    //    //catch (System.Exception ex)
//    //    //{
//    //    //    Debug.Log(ex.ToString());
//    //    //}
//    //}
//    //public void LoadData()
//    //{
//    //    string spdata;
//    //    string achidata;
//    //    string q =
//    //        "select //Level,NowExp,Gold,UserName,DrumLevel,GuitarLevel,BassLevel,KeyBoardLevel,Volume,UnlockC//hapter,UnlockSector_1,UnlockSector_2, UnlockSector_3, //UnlockSector_4,StagePeak,Achievement,Gem,GuitarRank,DrumRank,BassRank,KeyBoardRank,Infi//nityGameScore,IdentityNumber,InfinityFastValue from BuskingTest where ID = '" + /_IDInput.value/ + "' and PW = PASSWORD('" + _PWInput.value + "');";
//    //
//    //    try
//    //    {
//    //        bool getdata = false;
//    //        MySqlCommand cmd = new MySqlCommand(q, conn);
//    //        conn.Open();
//    //        MySqlDataReader reader = cmd.ExecuteReader();
//    //        while (reader.Read())
//    //        {
//    //            getdata = true;
//    //            StaticMng.Instance._Player_Level = int.Parse(reader[0].ToString());
//    //            StaticMng.Instance._Player_NowExp = int.Parse(reader[1].ToString());
//    //            StaticMng.Instance._Gold = int.Parse(reader[2].ToString());
//    //            StaticMng.Instance._UserName = reader[3].ToString();
//    //            StaticMng.Instance._DrumTowerLevel = int.Parse(reader[4].ToString());
//    //            StaticMng.Instance._GuitarTowerLevel = int.Parse(reader[5].ToString());
//    //            StaticMng.Instance._BassTowerLevel = int.Parse(reader[6].ToString());
//    //            StaticMng.Instance._KeyBoardTowerLevel = int.Parse(reader[7].ToString());
//    //            StaticMng.Instance._Option_Volume = float.Parse(reader[8].ToString());
//    //            if (StaticMng.Instance._Option_Volume == 1)
//    //                StaticMng.Instance._Option_Volume_Bool = true;
//    //            else
//    //                StaticMng.Instance._Option_Volume_Bool = false;
//    //            StaticMng.Instance._UnLock_Chapter = int.Parse(reader[9].ToString());
//    //            StaticMng.Instance._UnLock_Sector[0] = int.Parse(reader[10].ToString());
//    //            StaticMng.Instance._UnLock_Sector[1] = int.Parse(reader[11].ToString());
//    //            StaticMng.Instance._UnLock_Sector[2] = int.Parse(reader[12].ToString());
//    //            StaticMng.Instance._UnLock_Sector[3] = int.Parse(reader[13].ToString());
//    //            spdata = reader[14].ToString();
//    //            achidata = reader[15].ToString();
//    //            StaticMng.Instance._Gem = int.Parse(reader[16].ToString());
//    //            StaticMng.Instance._GuitarTowerRank = int.Parse(reader[17].ToString());
//    //            StaticMng.Instance._DrumTowerRank = int.Parse(reader[18].ToString());
//    //            StaticMng.Instance._BassTowerRank = int.Parse(reader[19].ToString());
//    //            StaticMng.Instance._KeyBoardTowerRank = int.Parse(reader[20].ToString());
//    //            StaticMng.Instance._InfinityGameScore = int.Parse(reader[21].ToString());
//    //            StaticMng.Instance._InfinityScoreIdentity = int.Parse(reader[22].ToString());
//    //            StaticMng.Instance._Infinity_FastValue = int.Parse(reader[23].ToString());
//    //
//    //            Dictionary<string, string> json = new Dictionary<string, string>();
//    //            json = JSon.Read(spdata);
//    //            for (int i=0;i<StaticMng.Instance._MaximumChapter;i++)
//    //            {
//    //                for(int j=0;j<StaticMng.Instance._MaximumSector[i];j++)
//    //                {
//    //                    StaticMng.Instance._StagePeakCount[i, j] = int.Parse(json["sp_" + /i.ToString/() + "_" + j.ToString()]);
//    //                }
//    //            }
//    //
//    //            Dictionary<string, string> achijson = new Dictionary<string, string>();
//    //            achijson = JSon.Read(achidata);
//    //            for (int i = 0; i < StaticMng.Instance._MaxAchievementCount; i++)
//    //            {
//    //                StaticMng.Instance._Achive_NowValue[i] = int.Parse(achijson//["Achi_NowVal"+i.ToString()]);
//    //                StaticMng.Instance._Achive_ClearCheck[i] = int.Parse(achijson["Achi_NowInfo" + //i.ToString()]);
//    //                Debug.Log("Load_ClearCheck");
//    //            }
//    //        }
//    //        if (getdata)
//    //        {
//    //            ExportError("데이터를 불러오는데\r\n성공했습니다.");
//    //            _MainUIMng.CloseDataPopup();
//    //            _MainUIMng.CloseOptionPopup();
//    //            _MainUIMng.LoadDataNext();
//    //            StaticMng.Instance._Stage_Chapter = 1;
//    //            _StageBackgroundMng.StageChange(1);
//    //            _MainUIMng.SceneStart();
//    //        }
//    //        else
//    //            ExportError("데이터를 불러오는데\r\n실패했습니다.");
//    //        conn.Close();
//    //        _DataSaveMng.SaveData();
//    //
//    //    }
//    //    catch (System.Exception ex)
//    //    {
//    //        Debug.Log(ex.ToString());
//    //        ExportError("데이터를 불러오는데\r\n실패했습니다.");
//    //        conn.Close();
//    //    }
//    //}
//    void ExportError(string log)
//    {
//        _ErrorPopup.SetActive(true);
//        _ErrorPopupAni.SetTrigger("open");
//        _ErrorLog.text = log;
//        //Debug.Log(log);
//    }
//    void ExportError_Quit(string log)
//    {
//        _ErrorPopup_Quit.SetActive(true);
//        _ErrorPopupAni_Quit.SetTrigger("open");
//        _ErrorLog_Quit.text = log;
//        //Debug.Log(log);
//    }
//    void ExportError_TwoChoice(string log)
//    {
//        _ErrorPopup_Choice.SetActive(true);
//        _ErrorPopupAni_Choice.SetTrigger("open");
//        _ErrorLog_Choice.text = log;
//        //Debug.Log(log);
//    }
//    public void Choice_Ok()
//    {
//        _ErrorPopup_Choice.SetActive(false);
//        _CoverPasswordPopup.SetActive(true);
//    }
//    public void Choice_Cancel()
//    {
//        _ErrorPopup_Choice.SetActive(false);
//    }
//    public void CoverPasswordInput()
//    {
//        //_CoverPasswordPopup.SetActive(false);
//        //
//        ////Debug.Log(_SaveTempId);
//        ////Debug.Log(_CoverPWInput.value);
//        //string q = "select count(ID)from BuskingTest where ID = '" + _SaveTempId + "' and PW = //PASSWORD('" + _CoverPWInput.value + "');";
//        //conn.Open();
//        //MySqlCommand cmd1 = new MySqlCommand(q, conn);
//        //MySqlDataReader reader1 = cmd1.ExecuteReader();
//        //reader1.Read();
//        //if (reader1[0].ToString() == "1")
//        //{
//        //    Dictionary<string, string> dicJson = new Dictionary<string, string>();
//        //    for (int i = 0; i < StaticMng.Instance._MaximumChapter; i++)
//        //    {
//        //        for (int j = 0; j < StaticMng.Instance._MaximumSector[i]; j++)
//        //        {
//        //            dicJson.Add("sp_" + i.ToString() + "_" + j.ToString(), //StaticMng.Instance._StagePeakCount[i, j].ToString());
//        //        }
//        //    }
//        //    string spdata = JSon.Write(dicJson);
//        //    Dictionary<string, string> achivejson = new Dictionary<string, string>();
//        //    for (int i = 0; i < StaticMng.Instance._MaxAchievementCount; i++)
//        //    {
//        //        achivejson.Add("Achi_NowVal" + i.ToString(), StaticMng.Instance._Achive_NowValue//[i].ToString());
//        //        achivejson.Add("Achi_NowInfo" + i.ToString(), StaticMng.Instance._Achive_ClearCheck//[i].ToString());
//        //    }
//        //    string achivedata = JSon.Write(achivejson);
//        //
//        //    int tempvolume;
//        //    if (StaticMng.Instance._Option_Volume_Bool)
//        //        tempvolume = 1;
//        //    else
//        //        tempvolume = 0;
//        //
//        //    string q2 = "update BuskingTest set Level=" + StaticMng.Instance._Player_Level.ToString/()/ + ",NowExp=" + StaticMng.Instance._Player_NowExp.ToString() + ",Gold=" + //StaticMng.Instance._Gold.ToString() + ",DrumLevel = " + //StaticMng.Instance._DrumTowerLevel.ToString() + ",GuitarLevel=" + //StaticMng.Instance._GuitarTowerLevel.ToString() + ",BassLevel=" + //StaticMng.Instance._BassTowerLevel.ToString() + ",KeyBoardLevel=" + //StaticMng.Instance._KeyBoardTowerLevel.ToString() + ",Volume=" + tempvolume.ToString() /+ /",UnlockChapter=" + StaticMng.Instance._UnLock_Chapter.ToString() + /",UnlockSector_1=" + /StaticMng.Instance._UnLock_Sector[0].ToString() + /",UnlockSector_2=" + /StaticMng.Instance._UnLock_Sector[1].ToString() + /",UnlockSector_3=" + /StaticMng.Instance._UnLock_Sector[2].ToString() + /",UnlockSector_4=" + /StaticMng.Instance._UnLock_Sector[3].ToString() + ",StagePeak ='" /+ spdata.ToString() + /"',Achievement = '" + achivedata + "',Gem = " + /StaticMng.Instance._Gem.ToString() + /",GuitarRank = /"+StaticMng.Instance._GuitarTowerRank.ToString()+ ",DrumRank = " + //StaticMng.Instance._DrumTowerRank.ToString() + ",BassRank = " + //StaticMng.Instance._BassTowerRank.ToString() + ",KeyBoardRank = " + //StaticMng.Instance._KeyBoardTowerRank.ToString() + ",InfinityGameScore /= /"+StaticMng.Instance._InfinityGameScore.ToString()+",IdentityNumber /= /"+StaticMng.Instance._InfinityScoreIdentity.ToString()+ ",InfinityFastValue /= /"+StaticMng.Instance._Infinity_FastValue.ToString()+" where id='" + //_SaveTempId.ToString() + "';";
//        //
//        //    reader1.Close();
//        //    MySqlCommand cmd2 = new MySqlCommand(q2, conn);
//        //    MySqlDataReader reader2 = cmd2.ExecuteReader();
//        //    ExportError("데이터를\r\n덮어씌웠습니다");
//        //}
//        //else
//        //    ExportError("비밀번호가\r\n일치하지 않습니다");
//        //conn.Close();
//    }
//    public void CloseErrorPopup()
//    {
//        _ErrorPopup.SetActive(false);
//    }
//    bool Checking(string value)
//    {
//        for (int i = 0; i < value.Length; i++)
//        {
//            //if(value[i]==',' || value[i] == '.'||
//            //    value[i] == '/'||value[i] == '\\'
//            //    || value[i] == '=' || value[i] == '+'
//            //    || value[i] == '')
//            if ((value[i] >= 32 && value[i] < 48) ||
//                (value[i] >= 58 && value[i] < 65) ||
//                (value[i] >= 91 && value[i] < 97) ||
//                (value[i] >= 123))
//            {
//                return true;
//            }

//        }
//        return false;
//    }
//}



public class DBSupport
{
    Thread _Thread;
    MySqlConnection conn;
    string connectdb = "Server="+StaticMng.Instance._IpAdress+";Database=BuskingServer;UserId=root;Password=Jsysb0p5pN1k6wI7;";


    bool _WantConnect;
    bool _CanConnect;
    bool _ConnectFail;
    bool _NotCurrentVersion;
    bool _ServerClose;
    public void Init()
    {
        conn = new MySqlConnection(connectdb);
    }
    public void Start()
    {
      
        _Thread = new Thread(new ThreadStart(ThreadUpdate));
        _WantConnect = true;
        _CanConnect = false;
        _ConnectFail = false;
        _NotCurrentVersion = false;
        _ServerClose = false;
        _Thread.Start();
    }

    void ThreadUpdate()
    {
        //Debug.Log("Threading");
        try
        {
            //Debug.Log("ThreadStart");
            if (_WantConnect)
            {
                _WantConnect = false;
                string q = "select * from BuskingVersionChecker";
                try
                {
                    conn.Open();
                    MySqlCommand cmd1 = new MySqlCommand(q, conn);
                    MySqlDataReader reader1 = cmd1.ExecuteReader();
                    reader1.Read();
                    if(reader1[0].ToString()==StaticMng.Instance._AppVersion)
                    {
                        if(reader1[1].ToString() == "1")
                            _CanConnect = true;
                        else
                            _ServerClose = true;
                    }
                    else
                        _NotCurrentVersion = true;

                    reader1.Close();
                  
                    conn.Close();
                  
                }
                catch (Exception ex)
                {
                    Debug.Log(ex.ToString());
                    _ConnectFail = true;

                }
            }
            //Debug.Log("ThreadEnd");
        }
        catch (Exception ex)
        {
            Debug.Log(ex.ToString());
        }
        //Debug.Log("ThreadingEnd");
    }

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