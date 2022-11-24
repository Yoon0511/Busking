using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using MySql.Data.MySqlClient;

public class UserNameMng : MonoBehaviour {
    MySqlConnection conn;
    string connectdb = "Server=" + StaticMng.Instance._IpAdress + ";Database=BuskingServer;UserId=root;Password=Jsysb0p5pN1k6wI7;";
    [SerializeField]
    DataSaveMng _DataSaveMng;

    [SerializeField]
    UIInput _InputField;

    [SerializeField]
    GameObject _ErrorPopup;
    [SerializeField]
    Animator _ErrorPopupAni;
    [SerializeField]
    UILabel _Log;
    [SerializeField]
    GameObject _NextButton;
    [SerializeField]
    GameObject _CloseButton;

    void Start()
    {
        conn = new MySqlConnection(connectdb);
    }

    public void PleaseCheck()
    {
        _ErrorPopup.SetActive(true);
        _ErrorPopupAni.SetTrigger("open");
        if (Checking())
        {
            _Log.text = "특수문자, 띄어쓰기등은 포함할 수 없습니다";
            _CloseButton.SetActive(true);
            _NextButton.SetActive(false);
        }
        else
        {



            _Log.text = "밴드명이\r\n저장되었습니다";
            _CloseButton.SetActive(false);
            _NextButton.SetActive(true);
        }
    }
    public void CloseErrorPopup()
    {
        _ErrorPopup.SetActive(false);
    }

    bool Checking()
    {
        string value = _InputField.value;
        for(int i=0;i<value.Length; i++)
        {
            //if(value[i]==',' || value[i] == '.'||
            //    value[i] == '/'||value[i] == '\\'
            //    || value[i] == '=' || value[i] == '+'
            //    || value[i] == '')
            if ((value[i] >= 32 && value[i] < 48) ||
                (value[i] >= 58 && value[i] < 65) ||
                (value[i] >= 91 && value[i] < 97) ||
                (value[i] >= 123 && value[i] < 128)) 
            {
                return true;
            }
            
        }
        return false;
    }

	public void ComleteInput()
    {
        if(_InputField.value!="")
        {
            StaticMng.Instance._UserName = _InputField.value;
            string q = "select UserName from BuskingTest where ID = '" + StaticMng.Instance._UserId + "';";
            conn.Open();
            MySqlCommand cmd1 = new MySqlCommand(q, conn);
            MySqlDataReader reader1 = cmd1.ExecuteReader();
            reader1.Read();
            if (reader1[0].ToString() == "")
            {
                _DataSaveMng.WantDataInit();
                _DataSaveMng.EndAndSceneChange(true, "MainScene");
            }
            else
            {
                StaticMng.Instance._Gem -= 50;
                _DataSaveMng.WantDataSave();
                _DataSaveMng.EndAndSceneChange(true, "MainScene");
            }
            reader1.Close();
            conn.Close();
        }
        
    }
}
