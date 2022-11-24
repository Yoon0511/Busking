using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MySql.Data.MySqlClient;    //클라이언트 기능을사용하기 위해서 사용
using System.Threading;

public class LoginMng : MonoBehaviour {

    MySqlConnection conn;
    string connectdb = "Server=" + StaticMng.Instance._IpAdress + 
        ";Database=BuskingServer;UserId=root;Password=Jsysb0p5pN1k6wI7;";
    

    [SerializeField]
    FadeMng _FadeMng;

    [SerializeField]
    GameObject _BigTable;
    [SerializeField]
    Animator _BigTableAni;
    [SerializeField]
    GameObject _LoginPopup;

    [SerializeField]
    GameObject _AccountPopup;


    [SerializeField]
    GameObject _ErrorPopup;
    [SerializeField]
    Animator _ErrorPopupAni;
    [SerializeField]
    UILabel _ErrorLog;


    [SerializeField]
    UIInput _IDInput_Login;
    [SerializeField]
    UIInput _PWInput_Login;
    [SerializeField]
    UIInput _IDInput_NewAccount;
    [SerializeField]
    UIInput _PWInput_NewAccount;

    [SerializeField]
    DataSaveMng _DataSaveMng;

    void Awake()
    {
        conn = new MySqlConnection(connectdb);
    }

    public void Init()
    {
        //PlayerPrefs.SetInt("Logining", 0);//temp
        if(PlayerPrefs.GetInt("Logining")==0)
        {
            _BigTable.SetActive(true);
            _BigTableAni.SetTrigger("open");
        }
        else
        {
            if (Login(PlayerPrefs.GetString("UserID"), PlayerPrefs.GetString("UserPW")))
            {
                StaticMng.Instance._UserId = PlayerPrefs.GetString("UserID");
                StaticMng.Instance._UserPW = PlayerPrefs.GetString("UserPW");
                NextSceneGo();
            }
            else
            {
                //loginfail
                ExportError("로그인에\r\n실패하였습니다");
                _BigTable.SetActive(true);
                _BigTableAni.SetTrigger("open");
            }
        }
    }

    bool Login(string id,string pw)
    {
        try
        {
            bool returnvalue = false;
            string tempq = "select count(ID) from BuskingTest where ID = '"
                + id + "' and PW = PASSWORD('" + pw + "');";
            conn.Open();
            MySqlCommand cmd1 = new MySqlCommand(tempq, conn);
            MySqlDataReader reader1 = cmd1.ExecuteReader();
            reader1.Read();
            if (reader1[0].ToString() == "1")
            {
                //login
                conn.Close();
                returnvalue = true;
            }
            else
            {
                //fail
                reader1.Close();
                conn.Close();
                returnvalue = false;
            }
            return returnvalue;
        }
        catch(Exception log)
        {
            Debug.Log(log);

            return false;
        }
    }

    public void EndLogin()
    {
        if (Login(_IDInput_Login.value, _PWInput_Login.value))
        {
            StaticMng.Instance._UserId = _IDInput_Login.value;
            StaticMng.Instance._UserPW = _PWInput_Login.value;

            PlayerPrefs.SetInt("Logining", 1);
            PlayerPrefs.SetString("UserID", _IDInput_Login.value);
            PlayerPrefs.SetString("UserPW", _PWInput_Login.value);

            _BigTable.SetActive(false);

            NextSceneGo();
        }
        else
        {
            _IDInput_Login.value = "";
            _PWInput_Login.value = "";
            ExportError("로그인에\r\n실패하였습니다");
        }
    }

    void NextSceneGo()
    {
        string q = "select UserName from BuskingTest where ID = '" + StaticMng.Instance._UserId + "';";
        conn.Open();
        MySqlCommand cmd1 = new MySqlCommand(q, conn);
        MySqlDataReader reader1 = cmd1.ExecuteReader();
        reader1.Read();
        Debug.Log(reader1[0].ToString());
        if (reader1[0].ToString() == "")
        {
            _FadeMng._NextSceneName = "UserNameScene";
            _FadeMng.Init();
        }
        else
        {
            _DataSaveMng.FirstOnLoad();
            _FadeMng._NextSceneName = "MainScene";
            _FadeMng.Init();
        }
        reader1.Close();
        conn.Close();
    }

    public void EndNewAccount()
    {
        if (!Checking(_IDInput_NewAccount.value) && !Checking(_PWInput_NewAccount.value))
        {
            if (_IDInput_NewAccount.value.Length >= 5 && _PWInput_NewAccount.value.Length >= 4)
            {
                string q = "insert into BuskingTest(ID,PW) values('" + _IDInput_NewAccount.value + "',PASSWORD('" + _PWInput_NewAccount.value + "'))";
                //insert

                try
                {
                    string tempq = "select count(ID) from BuskingTest where ID = '" + _IDInput_NewAccount.value + "'";
                    conn.Open();
                    MySqlCommand cmd1 = new MySqlCommand(tempq, conn);
                    MySqlDataReader reader1 = cmd1.ExecuteReader();
                    reader1.Read();
                    if (reader1[0].ToString() == "1")
                    {
                        ExportError("중복된 아이디입니다");
                        conn.Close();
                    }
                    else
                    {
                        _IDInput_NewAccount.value = "";
                        _PWInput_NewAccount.value = "";
                        ExportError("회원가입에\r\n성공했습니다");
                        reader1.Close();
                        MySqlCommand cmd = new MySqlCommand(q, conn);
                        MySqlDataReader reader = cmd.ExecuteReader();
                        conn.Close();
                        OpenLoginPopup();
                    }
                }
                catch (Exception ex)
                {
                    ExportError("인터넷이 원활하지 않습니다");
                    Debug.Log(ex);
                }
            }
            else
            {
                ExportError("ID는 5자 이상, 비밀번호는 4자 이상이어야 합니다");
            }
        }
        else
        {
            ExportError("ID,PW에 특수문자, 한글을 사용할 수 없습니다");
            _IDInput_NewAccount.value = "";
            _PWInput_NewAccount.value = "";
        }

        
    }

    void ExportError(string log)
    {
        _ErrorPopup.SetActive(true);
        _ErrorPopupAni.SetTrigger("open");
        _ErrorLog.text = log;
        //Debug.Log(log);
    }
    public void CloseErrorPopup()
    {
        _ErrorPopup.SetActive(false);
    }

    public void OpenLoginPopup()
    {
        _AccountPopup.SetActive(false);
        _LoginPopup.SetActive(true);
    }
    public void OpenAccountPopup()
    {
        Debug.Log("open");
        _AccountPopup.SetActive(true);
        _LoginPopup.SetActive(false);
    }
    bool Checking(string value)
    {
        for (int i = 0; i < value.Length; i++)
        {
            //if(value[i]==',' || value[i] == '.'||
            //    value[i] == '/'||value[i] == '\\'
            //    || value[i] == '=' || value[i] == '+'
            //    || value[i] == '')
            if ((value[i] >= 32 && value[i] < 48) ||
                (value[i] >= 58 && value[i] < 65) ||
                (value[i] >= 91 && value[i] < 97) ||
                (value[i] >= 123))
            {
                return true;
            }

        }
        return false;
    }
}
