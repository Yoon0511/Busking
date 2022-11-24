using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GetDataExcel : MonoBehaviour
{

    [SerializeField]
    TowerUpgradeMng _TowerUpgradeMng;

    string path = "Assets\\Editor\\excel.xlsx";

    List<string> _DataList = new List<string>();

    Dictionary<string, string> _Dic = new Dictionary<string, string>();

   

    void Awake()
    {
#if UNITY_EDITOR_WIN


        GetTowerNameData();
        GetAchievementData();



#elif UNITY_ANDROID

        GetTowerNameData();
        GetAchievementData();
#else

        GetTowerNameData();
        GetAchievementData();
#endif
    }

    void GetTowerNameData()
    {
        TextAsset file = Resources.Load<TextAsset>("instrumentdata");
        StreamReader sr = new StreamReader(new MemoryStream(file.bytes));
        string data = sr.ReadLine();
        _Dic = JSon.Read(data);
        for (int i = 0; i < 10; i++)
        {

            _TowerUpgradeMng._GuitarTowerUpgradeCost.Add(int.Parse(_Dic["Guitar_Cost_" + i.ToString()]));
            _TowerUpgradeMng._DrumTowerUpgradeCost.Add(int.Parse(_Dic["Bass_Cost_" + i.ToString()]));
            _TowerUpgradeMng._BassTowerUpgradeCost.Add(int.Parse(_Dic["Drum_Cost_" + i.ToString()]));
            _TowerUpgradeMng._KeyBoardTowerUpgradeCost.Add(int.Parse(_Dic["KeyBoard_Cost_" + i.ToString()]));
        }
        _TowerUpgradeMng._GuitarTowerUpgradeCost.Add(33000);
        _TowerUpgradeMng._DrumTowerUpgradeCost.Add(33000);
        _TowerUpgradeMng._BassTowerUpgradeCost.Add(33000);
        _TowerUpgradeMng._KeyBoardTowerUpgradeCost.Add(33000);

    }
    void GetAchievementData()
    {
        TextAsset file = Resources.Load<TextAsset>("achievementdata");
        StreamReader sr = new StreamReader(new MemoryStream(file.bytes));
        string data = sr.ReadLine();
        _Dic = JSon.Read(data);
        for (int i = 0; i < StaticMng.Instance._MaxAchievementCount; i++)
        {
            StaticMng.Instance._Achive_Name.Add(_Dic["Achieve_Name_"+i.ToString()]);
            StaticMng.Instance._Achive_Explanation.Add(_Dic["Achieve_Explain_" + i.ToString()]);
            StaticMng.Instance._Achive_MaxValue.Add(int.Parse(_Dic["Achieve_MaxCount_" + i.ToString()]));
            StaticMng.Instance._Achive_Reward.Add(int.Parse(_Dic["Achieve_Reward_" + i.ToString()]));
            StaticMng.Instance._Achive_RewardInfo.Add(_Dic["Achieve_RewardI_"+ i.ToString()]);
        }
    }
}
