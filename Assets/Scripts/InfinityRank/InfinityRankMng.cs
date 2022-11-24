using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;     //C#의 데이터 테이블 때문에 사용
using MySql.Data;     //MYSQL함수들을 불러오기 위해서 사용
using MySql.Data.MySqlClient;    //클라이언트 기능을사용하기 위해서 사용
using System.Threading;
using System;
using UnityEngine.SceneManagement;

public class InfinityRankMng : MonoBehaviour {

    [SerializeField]
    DataSaveMng _DataSaveMng;
    [SerializeField]
    GameObject _UIRoot;
    [SerializeField]
    GameObject _FadeIn;

    MySqlConnection conn;
    string connectdb = "Server=" + StaticMng.Instance._IpAdress + ";Database=BuskingServer;UserId=root;Password=Jsysb0p5pN1k6wI7;";

    DBSupport _DBSupport;

    [SerializeField]
    GameObject _InfinityRankBlock;
    [SerializeField]
    GameObject _ScrollSupporter;
    [SerializeField]
    GameObject _GameStartButtonGray;

    [SerializeField]
    GameObject _ErrorPopup_Quit;
    [SerializeField]
    Animator _ErrorPopupAni_Quit;
    [SerializeField]
    UILabel _ErrorLog_Quit;

    [SerializeField]
    GameObject _BlockGrid;
    [SerializeField]
    GameObject _LoadingTable;
    [SerializeField]
    GameObject _BigLoadingTable;
    [SerializeField]
    GameObject _ErrorTable;

    [SerializeField]
    GameObject _ErrorPopup;
    [SerializeField]
    Animator _ErrorPopupAni;
    [SerializeField]
    UILabel _ErrorLog;

    [SerializeField]
    UILabel _MyScore;

    [SerializeField]
    GameObject _InfinityGamePopup;
    [SerializeField]
    GameObject _InfinityGameInfoPopup;
    [SerializeField]
    UIScrollView _ScrollView;

    bool _AcceptReady;
    bool _WantSave;
    string _SaveTempId;
    void Start()
    {
        conn = new MySqlConnection(connectdb);
        _DBSupport = new DBSupport();
        _DBSupport.Init();
        _WantSave = false;
    }

    void Update()
    {
        _MyScore.text = StaticMng.Instance._InfinityGameScore.ToString();
        if (_AcceptReady && _DBSupport.GetConnectFail())
        {
            _AcceptReady = false;
            if (_WantSave)
            {
                _WantSave = false;
                ExportError("데이터를 저장하는데\r\n실패했습니다");
            }
            else
            {
                _LoadingTable.SetActive(false);
                _ErrorTable.SetActive(true);
            }
            _DBSupport.SetConnectFailFalse();
        }

        if(_AcceptReady && _DBSupport.GetCanConnect())
        {
            _AcceptReady = false;
            if (_WantSave)
            {
                _WantSave = false;
                SaveRank();
            }
            else
            {
                _LoadingTable.SetActive(false);
                GetData();
            }

            _DBSupport.SetFalse();
        }
        if (_AcceptReady && _DBSupport.GetNotCurrentVersion())
        {
            _AcceptReady = false;
            _LoadingTable.SetActive(false);
            ExportError_Quit("최신버전으로 게임을\r\n업데이트해 주십시오");
            _DBSupport.SetFalse();
        }
        if (_AcceptReady && _DBSupport.GetServerClose())
        {
            _AcceptReady = false;
            _LoadingTable.SetActive(false);
            ExportError_Quit("서버가 점검중입니다");
            _DBSupport.SetFalse();
        }
        if (StaticMng.Instance._StagePeakCount[3, 9] >= 1)
            _GameStartButtonGray.SetActive(false);
        else
            _GameStartButtonGray.SetActive(true);
    }

    public void WantSave()
    {
        if(!_AcceptReady)
        {
            _BigLoadingTable.SetActive(true);
            _DBSupport.Start();
            _AcceptReady = true;
            _WantSave = true;
        }
    }
    void SaveRank()
    {
        Debug.Log(StaticMng.Instance._InfinityScoreIdentity);

        string q = "insert into BuskingInfinityGame(UserName,Score) values('" + StaticMng.Instance._UserName + "'," + StaticMng.Instance._InfinityGameScore.ToString() + ")";

        string tempq = "select count(IdentityNum) from BuskingInfinityGame where IdentityNum = " + StaticMng.Instance._InfinityScoreIdentity.ToString();
        conn.Open();
        MySqlCommand cmd1 = new MySqlCommand(tempq, conn);
        MySqlDataReader reader1 = cmd1.ExecuteReader();
        reader1.Read();
        if (reader1[0].ToString() == "1")
        {
            conn.Close();
            UpdateRank();
        }
        else
        {
            _BigLoadingTable.SetActive(false);
            ExportError("랭킹을\r\n등록하였습니다");
            reader1.Close();
            MySqlCommand cmd = new MySqlCommand(q, conn);
            MySqlDataReader reader = cmd.ExecuteReader();
            conn.Close();
            _DataSaveMng.WantDataSave();
            GetIdentityNumber();
            RankingLoading();
        }
    }
    void UpdateRank()
    {
        conn.Open();
        string q2 = "update BuskingInfinityGame set UserName = '" + StaticMng.Instance._UserName + "',Score = " + StaticMng.Instance._InfinityGameScore.ToString() + " where IdentityNum = " + StaticMng.Instance._InfinityScoreIdentity.ToString() + ";";
        MySqlCommand cmd2 = new MySqlCommand(q2, conn);
        MySqlDataReader reader2 = cmd2.ExecuteReader();
        conn.Close();
        _BigLoadingTable.SetActive(false);
        ExportError("랭킹을\r\n갱신하였습니다");
        RankingLoading();
    }
    void GetIdentityNumber()
    {
        string tempq = "select count(IdentityNum) from BuskingInfinityGame";
        conn.Open();
        MySqlCommand cmd1 = new MySqlCommand(tempq, conn);
        MySqlDataReader reader1 = cmd1.ExecuteReader();
        
        if (reader1.Read())
        {
            StaticMng.Instance._InfinityScoreIdentity = int.Parse(reader1[0].ToString());
        }
        conn.Close();
        _DataSaveMng.WantDataSave();
    }
    public void RankingLoading()
    {
        _BlockGrid.transform.DestroyChildren();
        _ScrollView.ResetPosition();
        _LoadingTable.SetActive(true);
        _ErrorTable.SetActive(false);
        _DBSupport.SetCanConnectFalse();
        _DBSupport.SetConnectFailFalse();
        _DBSupport.Start();
        _AcceptReady = true;
    }
    void GetData()
    {
        string q =
            "select UserName,Score,IdentityNum from BuskingInfinityGame order by score desc limit 10;";

        try
        {
            MySqlCommand cmd = new MySqlCommand(q, conn);
            conn.Open();
            MySqlDataReader reader = cmd.ExecuteReader();
            int num = 0;
            while (reader.Read())
            {
                string UserName = reader[0].ToString();
                string Score = reader[1].ToString();
                int IdenNum = int.Parse(reader[2].ToString());
                GameObject obj = NGUITools.AddChild(_BlockGrid, _InfinityRankBlock);
                obj.GetComponent<InfinityRankBlock>().Init(num+1,UserName, Score,IdenNum);
                obj.transform.localPosition = new Vector3(0, -80 * num, 0);
                num++;
            }
            int height = num * 80-20;
            _ScrollSupporter.GetComponent<UI2DSprite>().height = height;
            _ScrollSupporter.GetComponent<BoxCollider>().size = new Vector3(638, height, 1);
            _ScrollSupporter.GetComponent<BoxCollider>().center = new Vector3(319, -height / 2, 0);
            conn.Close();
        }
        catch (System.Exception ex)
        {
            Debug.Log(ex.ToString());
            _ErrorTable.SetActive(true);
            conn.Close();
        }
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
    public void StageStart()
    {
        GameObject obj = NGUITools.AddChild(_UIRoot, _FadeIn);
        StartCoroutine(StageStart_C());
    }
    IEnumerator StageStart_C()
    {
        yield return new WaitForSeconds(1.5f);
        while (_DataSaveMng.GetDataSending())
            yield return null;
        StaticMng.Instance._GameMode_Infinity = true;
        StaticMng.Instance._NowWantScene = "GameScene";
        SceneManager.LoadScene("LoadingScene");
    }

    public void OpenInfinityGamePopup()
    {
        _InfinityGamePopup.SetActive(true);
        RankingLoading();
        TutorialMng_Main.Data.CheckTutorialClear(3);
    }
    public void CloseInfinityGamePopup()
    {
        _InfinityGamePopup.SetActive(false);
    }
    public void OpenInfinityGameInfoPopup()
    {
        _InfinityGameInfoPopup.SetActive(true);
    }
    public void CloseInfinityGameInfoPopup()
    {
        _InfinityGameInfoPopup.SetActive(false);
    }
}
