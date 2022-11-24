//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using System.Data.SqlClient;
//using System.Threading;
//using System;

//public class DBConnectMng : MonoBehaviour {

//    bool _WantSave;
//    bool _WantLoad;
//    SqlConnection conn;
//    string connectdb = "server=" + StaticMng._IpAdress + "; database=TempTable; uid=jhj000213; pwd=guswls88;";
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
//    GameObject _ErrorPopup_Choice;
//    [SerializeField]
//    Animator _ErrorPopupAni_Choice;
//    [SerializeField]
//    UILabel _ErrorLog_Choice;
//    [SerializeField]
//    GameObject _CoverPasswordPopup;
//    [SerializeField]
//    UIInput _CoverPWInput;
//    [SerializeField]
//    UILabel _ErrorLog;

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
//        conn = new SqlConnection(connectdb);
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
//        if(_AcceptReady && _DBSupport.GetCanConnect())
//        {
//            //succese
//            _AcceptReady = false;
//            if (_WantSave)
//                SaveData();
//            else if (_WantLoad)
//                LoadData();
//            _WantSave = false;
//            _WantLoad = false;
//            _IDInputField.SetActive(false);
//            _IDInput.value = "";
//            _PWInput.value = "";
//            _LoadingTable.SetActive(false);
//            _DBSupport.SetCanConnectFalse();
//            _DBSupport.SetConnectFailFalse();
//        }
//        if(_AcceptReady && _DBSupport.GetConnectFail())
//        {
//            //Fail
//            _AcceptReady = false;
//            _LoadingTable.SetActive(false);
//            ExportError("서버와의 접속이 원활하지 않습니다.\r\n인터넷 연결을 확인하여 주십시오.");
//            _DBSupport.SetConnectFailFalse();
//        }
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
    
//    public void SaveData()
//    {
//        Dictionary<string, string> dicJson = new Dictionary<string, string>();
//        for(int i=0;i<StaticMng._MaximumChapter;i++)
//        {
//            for(int j=0;j<StaticMng._MaximumSector[i];j++)
//            {
//                dicJson.Add("sp_" + i.ToString() + "_" + j.ToString(), StaticMng._StagePeakCount[i, j].ToString());
//            }
//        }
//        string spdata = JSon.Write(dicJson);

//        int tempvolume;
//        if (StaticMng._Option_Volume_Bool)
//            tempvolume = 1;
//        else
//            tempvolume = 0;

//        //insert into BuskingTest values('aaaa', PWDENCRYPT('123'), 'admin', 99, 0, 9999999, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0);
//        string q = "insert into BuskingTest values('" + _IDInput.value + "',PWDENCRYPT('" + _PWInput.value + "'),'" + StaticMng._UserName + "'," + StaticMng._Player_Level.ToString() + ","
//            + StaticMng._Player_NowExp.ToString() + "," + StaticMng._Gold.ToString() + ","+StaticMng._DrumTowerLevel.ToString()+","+StaticMng._GuitarTowerLevel.ToString()+
//            ","+StaticMng._BassTowerLevel.ToString()+","+StaticMng._KeyBoardTowerLevel.ToString()+"," + tempvolume.ToString()+","+StaticMng._UnLock_Chapter.ToString()+","+
//            StaticMng._UnLock_Sector[0].ToString()+","+ StaticMng._UnLock_Sector[1].ToString() + ","+ StaticMng._UnLock_Sector[2].ToString() + ","+ StaticMng._UnLock_Sector[3].ToString() + ","+
//            "'" + spdata+"'" + ")";

//        try
//        {
//            string tempq = "select count(ID) from BuskingTest where ID = '" + _IDInput.value + "'";
//            conn.Open();
//            SqlCommand cmd1 = new SqlCommand(tempq, conn);
//            SqlDataReader reader1 = cmd1.ExecuteReader();
//            reader1.Read();
//            if (reader1[0].ToString() == "1")
//            {
//                //중복
//                //Debug.Log("중복");
//                _SaveTempId = _IDInput.value;
//                ExportError_TwoChoice("이미 같은 ID의 데이터가 있습니다.\r\n덮어씌우시겠습니까?");
//                conn.Close();
//            }
//            else
//            {
//                ExportError("데이터를 저장하였습니다.");
//                reader1.Close();
//                SqlCommand cmd = new SqlCommand(q, conn);
//                SqlDataReader reader = cmd.ExecuteReader();
//                conn.Close();
//                _DataSaveMng.SaveData();
//            }

//        }
//        catch (System.Exception ex)
//        {
//            Debug.Log(ex.ToString());
//        }
//    }
//    public void LoadData()
//    {
//        string spdata;
//        string q = 
//            "select Level,NowExp,Gold,UserName,DrumLevel,GuitarLevel,BassLevel,KeyBoardLevel,Volume,UnlockChapter,UnlockSector_1,UnlockSector_2, UnlockSector_3, UnlockSector_4,StagePeak from BuskingTest where ID = '" + _IDInput.value + "' and PWDCOMPARE('" + _PWInput.value + "',PW) = 1;";

//        try
//        {
//            bool getdata = false;
//            SqlCommand cmd = new SqlCommand(q, conn);
//            conn.Open();
//            SqlDataReader reader = cmd.ExecuteReader();
//            while (reader.Read())
//            {
//                getdata = true;
//                StaticMng._Player_Level = int.Parse(reader[0].ToString());
//                StaticMng._Player_NowExp = int.Parse(reader[1].ToString());
//                StaticMng._Gold = int.Parse(reader[2].ToString());
//                StaticMng._UserName = reader[3].ToString();
//                StaticMng._DrumTowerLevel = int.Parse(reader[4].ToString());
//                StaticMng._GuitarTowerLevel = int.Parse(reader[5].ToString());
//                StaticMng._BassTowerLevel = int.Parse(reader[6].ToString());
//                StaticMng._KeyBoardTowerLevel = int.Parse(reader[7].ToString());
//                StaticMng._Option_Volume = float.Parse(reader[8].ToString());
//                if (StaticMng._Option_Volume == 1)
//                    StaticMng._Option_Volume_Bool = true;
//                else
//                    StaticMng._Option_Volume_Bool = false;
//                StaticMng._UnLock_Chapter = int.Parse(reader[9].ToString());
//                StaticMng._UnLock_Sector[0] = int.Parse(reader[10].ToString());
//                StaticMng._UnLock_Sector[1] = int.Parse(reader[11].ToString());
//                StaticMng._UnLock_Sector[2] = int.Parse(reader[12].ToString());
//                StaticMng._UnLock_Sector[3] = int.Parse(reader[13].ToString());
//                spdata = reader[14].ToString();

//                Dictionary<string, string> json = new Dictionary<string, string>();
//                json = JSon.Read(spdata);
//                for (int i=0;i<StaticMng._MaximumChapter;i++)
//                {
//                    for(int j=0;j<StaticMng._MaximumSector[i];j++)
//                    {
//                        StaticMng._StagePeakCount[i, j] = int.Parse(json["sp_" + i.ToString() + "_" + j.ToString()]);
//                    }
//                }

//            }
//            if (getdata)
//            {
//                ExportError("데이터를 불러오는데\r\n성공했습니다.");
//                _MainUIMng.CloseDataPopup();
//                _MainUIMng.CloseOptionPopup();
//                _MainUIMng.LoadDataNext();
//                StaticMng._Stage_Chapter = 1;
//                _StageBackgroundMng.StageChange(1);
//                _MainUIMng.SceneStart();
//            }
//            else
//                ExportError("데이터를 불러오는데\r\n실패했습니다.");
//            conn.Close();
//            _DataSaveMng.SaveData();

//        }
//        catch (System.Exception ex)
//        {
//            Debug.Log(ex.ToString());
//            ExportError("데이터를 불러오는데\r\n실패했습니다.");
//            conn.Close();
//        }
//    }
//    void ExportError(string log)
//    {
//        _ErrorPopup.SetActive(true);
//        _ErrorPopupAni.SetTrigger("open");
//        _ErrorLog.text = log;
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
//        _CoverPasswordPopup.SetActive(false);

//        //Debug.Log(_SaveTempId);
//        //Debug.Log(_CoverPWInput.value);
//        string q = "select count(ID)from BuskingTest where ID = '" + _SaveTempId + "' and PWDCOMPARE('" +_CoverPWInput.value +"', PW) = 1";
//        conn.Open();
//        SqlCommand cmd1 = new SqlCommand(q, conn);
//        SqlDataReader reader1 = cmd1.ExecuteReader();
//        reader1.Read();
//        if (reader1[0].ToString() == "1")
//        {
//            Dictionary<string, string> dicJson = new Dictionary<string, string>();
//            for (int i = 0; i < StaticMng._MaximumChapter; i++)
//            {
//                for (int j = 0; j < StaticMng._MaximumSector[i]; j++)
//                {
//                    dicJson.Add("sp_" + i.ToString() + "_" + j.ToString(), StaticMng._StagePeakCount[i, j].ToString());
//                }
//            }
//            string spdata = JSon.Write(dicJson);

//            int tempvolume;
//            if (StaticMng._Option_Volume_Bool)
//                tempvolume = 1;
//            else
//                tempvolume = 0;

//            string q2 = "update BuskingTest set Level=" + StaticMng._Player_Level.ToString() + ",NowExp=" + StaticMng._Player_NowExp.ToString() + ",Gold=" + StaticMng._Gold.ToString() + ",DrumLevel = " + StaticMng._DrumTowerLevel.ToString() + ",GuitarLevel=" + StaticMng._GuitarTowerLevel.ToString() + ",BassLevel=" + StaticMng._BassTowerLevel.ToString() + ",KeyBoardLevel=" + StaticMng._KeyBoardTowerLevel.ToString() + ",Volume=" + tempvolume.ToString() + ",UnlockChapter=" + StaticMng._UnLock_Chapter.ToString() + ",UnlockSector_1=" + StaticMng._UnLock_Sector[0].ToString() + ",UnlockSector_2=" + StaticMng._UnLock_Sector[1].ToString() + ",UnlockSector_3=" + StaticMng._UnLock_Sector[2].ToString() + ",UnlockSector_4=" + StaticMng._UnLock_Sector[3].ToString() + ",StagePeak ='" + spdata.ToString() + "'where id='" + _SaveTempId.ToString() + "';";

//            reader1.Close();
//            SqlCommand cmd2 = new SqlCommand(q2, conn);
//            SqlDataReader reader2 = cmd2.ExecuteReader();
//            ExportError("데이터를\r\n덮어씌웠습니다");
//        }
//        else
//            ExportError("비밀번호가\r\n일치하지 않습니다");
//        conn.Close();
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



//public class DBSupport
//{
//    Thread _Thread;
//    SqlConnection conn;
//    string connectdb = "server=" + StaticMng._IpAdress + "; database=TempTable; uid=jhj000213; pwd=guswls88;";
    

//    bool _WantConnect;
//    bool _CanConnect;
//    bool _ConnectFail;
//    public void Init()
//    {
//        conn = new SqlConnection(connectdb);
//    }
//    public void Start()
//    {
        
//        _Thread = new Thread(new ThreadStart(ThreadUpdate));
//        _WantConnect = true;
//        _CanConnect = false;
//        _ConnectFail = false;
//        _Thread.Start();
//    }

//    void ThreadUpdate()
//    {
//        //Debug.Log("Threading");
//        try
//        {
//            //Debug.Log("ThreadStart");
//            if (_WantConnect)
//            {
//                _WantConnect = false;
//                string q = "select * from BuskingTest";
//                try
//                {
//                    conn.Open();
//                    SqlCommand cmd1 = new SqlCommand(q, conn);
//                    SqlDataReader reader1 = cmd1.ExecuteReader();
//                    reader1.Read();

//                    reader1.Close();
                    
//                    conn.Close();
//                    _CanConnect = true;
//                }
//                catch (Exception ex)
//                {
//                    Debug.Log(ex.ToString());
//                    _ConnectFail = true;

//                }
//            }
//            //Debug.Log("ThreadEnd");

//        }
//        catch (Exception ex)
//        {
//            Debug.Log(ex.ToString());
//        }
        
//        //Debug.Log("ThreadingEnd");
//    }

//    public bool GetCanConnect() { return _CanConnect; }
//    public bool GetConnectFail() { return _ConnectFail; }
//    public void SetCanConnectFalse() { _CanConnect = false; }
//    public void SetConnectFailFalse() { _ConnectFail = false; }
//}