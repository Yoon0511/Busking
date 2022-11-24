using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldShopMng : MonoBehaviour {

    [SerializeField]
    DataSaveMng _DataSaveMng;

    [SerializeField]
    GameObject _GoldShopPopup;
    [SerializeField]
    GameObject _ChoicePopup;
    [SerializeField]
    Animator _ChoicePopupAni;
    [SerializeField]
    UILabel _ChoiceLabel;
    [SerializeField]
    UILabel _ChoiceButtonLabel;

    [SerializeField]
    GameObject _ErrorPopup;
    [SerializeField]
    Animator _ErrorPopupAni;
    [SerializeField]
    UILabel _ErrorLog;

    int _NowWantGoldValue;

    public void BuyGold(int num)
    {
        _NowWantGoldValue = num;
        _ChoicePopup.SetActive(true);
        _ChoicePopupAni.SetTrigger("open");
        _ChoiceLabel.text = "젬을 사용해서 골드를\r\n구매하시겠습니까?";
        _ChoiceButtonLabel.text = StaticMng.Instance._GoldShop_Price[_NowWantGoldValue].ToString();
    }

    public void BuyGoldSelect()
    {
        if(StaticMng.Instance._Gem>=StaticMng.Instance._GoldShop_Price[_NowWantGoldValue])
        {
            StaticMng.Instance._Gem -= StaticMng.Instance._GoldShop_Price[_NowWantGoldValue];
            StaticMng.Instance._Gold += StaticMng.Instance._GoldShop_Reward[_NowWantGoldValue];
            ExportError(StaticMng.Instance._GoldShop_Reward[_NowWantGoldValue].ToString() + " 골드를\r\n구매하였습니다");
            _DataSaveMng.WantDataSave();
        }
        else
            ExportError("젬이 부족합니다");
        CloseChoicePopup();
    }
    public void CloseChoicePopup()
    {
        _ChoicePopup.SetActive(false);
    }

    void ExportError(string log)
    {
        _ErrorPopup.SetActive(true);
        _ErrorPopupAni.SetTrigger("open");
        _ErrorLog.text = log;
        //Debug.Log(log);
    }
    public void OpenGoldShopPopup()
    {
        _GoldShopPopup.SetActive(true);
    }
    public void CloseGoldShopPopup()
    {
        _GoldShopPopup.SetActive(false);
    }
}
