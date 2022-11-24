using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MySql.Data.MySqlClient;    //클라이언트 기능을사용하기 위해서 사용
using System.Threading;
using System;

public class TitleMng : MonoBehaviour {

    [SerializeField]
    LoginMng _LoginMng;

    MySqlConnection conn;
    string connectdb = "Server=" + StaticMng.Instance._IpAdress + ";Database=BuskingServer;UserId=root;Password=Jsysb0p5pN1k6wI7;";
    DBSupport _DBSupport;
    [SerializeField]
    GameObject _LoadingTable;
    bool _AcceptReady;

    [SerializeField]
    GameObject _ErrorPopup;
    [SerializeField]
    Animator _ErrorPopupAni;
    [SerializeField]
    UILabel _ErrorLog;

    [SerializeField]
    GameObject _ErrorPopup_HyperLink;
    [SerializeField]
    Animator _ErrorPopupAni_HyperLink;
    [SerializeField]
    UILabel _ErrorLog_HyperLink;

    void Start()
    {
        conn = new MySqlConnection(connectdb);
        _DBSupport = new DBSupport();
        _DBSupport.Init();
        //PlayerPrefs.SetInt("Initiate", 0);
        
        StaticMng.Instance._Stage_Chapter = 1;

        StartCoroutine(StartLoading());

        Debug.Log(StaticMng.Instance._AppVersion);
    }

    IEnumerator StartLoading()
    {
        yield return new WaitForSeconds(0.75f);

        _LoadingTable.SetActive(true);
        _AcceptReady = true;
        _DBSupport.Start();
    }

    void Update()
    {
        if (_AcceptReady && _DBSupport.GetCanConnect())
        {
            //succese
            _LoadingTable.SetActive(false);
            _AcceptReady = false;
            _LoginMng.Init();
            _DBSupport.SetCanConnectFalse();
            _DBSupport.SetConnectFailFalse();
        }
        if (_AcceptReady && _DBSupport.GetConnectFail())
        {
            //Fail
            _AcceptReady = false;
            _LoadingTable.SetActive(false);
            ExportError("서버와의 접속이\r\n원활하지 않습니다");
            _DBSupport.SetConnectFailFalse();
        }
        if(_AcceptReady && _DBSupport.GetNotCurrentVersion())
        {
            _LoadingTable.SetActive(false);
            ExportError_HyperLink("최신버전이 아닙니다\r\n업데이트하여 주십시오");
            _DBSupport.SetFalse();
        }
        if(_AcceptReady && _DBSupport.GetServerClose())
        {
            _LoadingTable.SetActive(false);
            ExportError("서버 점검중입니다");
            _DBSupport.SetFalse();
        }
    }
    
    void ExportError(string log)
    {
        _ErrorPopup.SetActive(true);
        _ErrorPopupAni.SetTrigger("open");
        _ErrorLog.text = log;
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
}
