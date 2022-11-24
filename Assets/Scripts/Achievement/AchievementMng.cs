using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementMng : MonoBehaviour {

    [SerializeField]
    DataSaveMng _SaveMng;
    [SerializeField]
    GameObject _AchieveBlock;
    [SerializeField]
    UIScrollView _ScrollView;


    [SerializeField]
    GameObject _AchieveBlockGrid;

    [SerializeField]
    GameObject _AchievementClearPopupParent;
    public GameObject _ClearAchievePopup;

    [SerializeField]
    GameObject _ScrollSupporter;

    [SerializeField]
    GameObject _AchieveOnPooint;

    //private static AchievementMng instance = null;
    //
    //public static AchievementMng Data
    //{
    //    get
    //    {
    //        if (instance == null)
    //        {
    //            instance = GameObject.FindObjectOfType(typeof(AchievementMng)) as AchievementMng;
    //            if (instance == null)
    //            {
    //                Debug.Log("no instance");
    //            }
    //        }
    //        return instance;
    //    }
    //}
    void Awake()
    {
        

        for (int i = 0; i < StaticMng.Instance._MaxAchievementCount; i++)
        {
            GameObject obj = NGUITools.AddChild(_AchieveBlockGrid, _AchieveBlock);
            obj.GetComponent<AchievementBlock>().Init(i, _ScrollView, this);
            obj.transform.localPosition = new Vector3(0, i * -120, 0);
        }
        float height = ((StaticMng.Instance._MaxAchievementCount) * 120)-20;
        _ScrollSupporter.GetComponent<UI2DSprite>().height = (int)height;
        _ScrollSupporter.GetComponent<BoxCollider>().size = new Vector3(1200, height);
        _ScrollSupporter.GetComponent<BoxCollider>().center = new Vector3(0, -height / 2, 0);
    }

    void Update()
    {
        bool on=false;
        for(int i=0;i<StaticMng.Instance._MaxAchievementCount;i++)
        {
            if (StaticMng.Instance._Achive_NowValue[i] >= StaticMng.Instance._Achive_MaxValue[i] && StaticMng.Instance._Achive_ClearCheck[i]!=2)
            {
                StaticMng.Instance._Achive_ClearCheck[i] = 1;
                on = true;
            }
        }
        _AchieveOnPooint.SetActive(on);
        //if (Input.GetKeyDown(KeyCode.U))
        //{
        //    for (int i = 0; i < StaticMng.Instance._MaxAchievementCount; i++)
        //    {
        //        StaticMng.Instance._Achive_ClearCheck[i] = 0;
        //        StaticMng.Instance._Achive_NowValue[i] = 100;
        //    }
        //}
    }

    public void GetValueCheck(int num)
    {
        StaticMng.Instance._Achive_NowValue[num] += 1;
    }

    public void ClearAchievement(int count)
    {
        GameObject obj = NGUITools.AddChild(_AchievementClearPopupParent, _ClearAchievePopup);
        obj.transform.localPosition = new Vector3();
        obj.transform.GetChild(0).GetChild(0).GetComponent<UILabel>().text = StaticMng.Instance._Achive_Name[count];
        StaticMng.Instance._Achive_ClearCheck[count] = 2;
        if (StaticMng.Instance._Achive_RewardInfo[count] == "gold")
            StaticMng.Instance._Gold += StaticMng.Instance._Achive_Reward[count];
        else if (StaticMng.Instance._Achive_RewardInfo[count] == "gem")
            StaticMng.Instance._Gem += StaticMng.Instance._Achive_Reward[count];
        _SaveMng.WantDataSave();
    }
}
