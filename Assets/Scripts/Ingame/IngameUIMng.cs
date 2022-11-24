using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IngameUIMng : MonoBehaviour {

    [SerializeField]
    GameObject _PausePopup;
    [SerializeField]
    GameObject _OptionPopup;

    [SerializeField]
    DataSaveMng _DataSaveMng;

    [SerializeField]
    UISprite _SoundButton;
    [SerializeField]
    UISprite _TowerSetButton;
    public IngameSoundMng _SoundMng;
    

    [SerializeField]
    UI2DSprite _LifgGaze;
    [SerializeField]
    UI2DSprite _EnegyGaze;

    [SerializeField]
    UILabel _LifeLabel;
    [SerializeField]
    UILabel _EnegyLabel;

    [SerializeField]
    GameObject _Infinity_TowerLevel;

    [SerializeField]
    UILabel[] _Infinity_TowerLevelLabel;
    bool _GameMode;

    void Start()
    {
        if (StaticMng.Instance._TowerSet_Drag)
            _TowerSetButton.spriteName = "option_towerdrag";
        else
            _TowerSetButton.spriteName = "option_towertouch";

        _GameMode = StaticMng.Instance._GameMode_Infinity;
        _Infinity_TowerLevel.SetActive(_GameMode);
        if (_GameMode)
            _EnegyGaze.fillAmount = 1;
    }

    void Update()
    {
        _LifgGaze.fillAmount = Mathf.Lerp(_LifgGaze.fillAmount, ((float)StaticMng.Instance._PlayerLife / (float)StageMng.Data._MaxLife), Time.smoothDeltaTime * 10.0f);
        _LifeLabel.text = StaticMng.Instance._PlayerLife.ToString() + "/" + StageMng.Data._MaxLife.ToString();
        
        if(_GameMode)
            _EnegyLabel.text = StageMng.Data._NowEnegy.ToString();
        else
        {
            _EnegyGaze.fillAmount = Mathf.Lerp(_EnegyGaze.fillAmount, ((float)StageMng.Data._NowEnegy / (float)StageMng.Data._MaxEnegy), Time.smoothDeltaTime * 10.0f);
            _EnegyLabel.text = StageMng.Data._NowEnegy.ToString() + "/" + StageMng.Data._MaxEnegy.ToString();
        }
       

        if(_GameMode)
        {
            for(int i=0;i<4;i++)
                _Infinity_TowerLevelLabel[i].text = StaticMng.Instance._InfinityGameTowerLevel[i].ToString();
        }

        if (StaticMng.Instance._CanPlay)
        {
            if (StaticMng.Instance._StartGame)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    if (_PausePopup.activeSelf)
                        ClosePausePopup();
                    else
                        OpenPausePopup();
                }
            }
            else if (StaticMng.Instance._PauseGame)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    if (_PausePopup.activeSelf)
                        ClosePausePopup();
                    else
                        OpenPausePopup();
                }
            }
        }
    }
    public void SoundButton()
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
        //StageMng.Data._DataSaveMng.WantDataSave();
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

    public void OpenPausePopup()
    {
        if (StaticMng.Instance._CanPlay)
        {
            _PausePopup.SetActive(true);
            //Time.timeScale = 0.0f;
            StaticMng.Instance._PauseGame = true;
            StaticMng.Instance._StartGame = false;
            _SoundMng.PauseBgm();
        }
    }

    public void ClosePausePopup()
    {
        _PausePopup.SetActive(false);
        //Time.timeScale = 1;
        StaticMng.Instance._PauseGame = false;
        StaticMng.Instance._StartGame = true;
        _OptionPopup.SetActive(false);
        _SoundMng.PlayBgm();
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
    }
    public void CloseOptionPopup()
    {
        _OptionPopup.SetActive(false);
    }

    public void GoMain()
    {
        StaticMng.Instance._PauseGame = false;
        //_SoundMng.PauseBgm();
        //Time.timeScale = 1.0f;
        StageMng.Data.FadeInAnimation();
        StartCoroutine(GoMainScene());
    }

    public void RestartStage()
    {
        //_SoundMng.PauseBgm();
        //StaticMng.Instance._Stage_Chapter++;
        //StaticMng.Instance._Stage_Sector++;
        StageMng.Data.FadeInAnimation();
        StartCoroutine(GoGameScene());
    }
    public void NextStage()
    {
        //_SoundMng.PauseBgm();
        //StaticMng.Instance._Stage_Chapter++;
        StaticMng.Instance._Stage_Sector += 1;
        if(!(StaticMng.Instance._Stage_Chapter==4&&StaticMng.Instance._Stage_Sector==10))
        {
            if (StaticMng.Instance._Stage_Sector == 11)
            {
                StaticMng.Instance._Stage_Chapter++;
                StaticMng.Instance._Stage_Sector = 1;
            }
        }
        StageMng.Data.FadeInAnimation();
        StartCoroutine(GoGameScene());
    }

    IEnumerator GoGameScene()
    {
        yield return new WaitForSeconds(1.5f);
        while (_DataSaveMng.GetDataSending())
            yield return null;
        SceneManager.LoadScene("GameScene");
    }
    IEnumerator GoMainScene()
    {
        yield return new WaitForSeconds(1.5f);
        while (_DataSaveMng.GetDataSending())
            yield return null;
        StaticMng.Instance._NowWantScene = "MainScene";
        SceneManager.LoadScene("LoadingScene");
    }
}