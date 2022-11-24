using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementBlock : MonoBehaviour {

    public UILabel _A_Name;
    public UILabel _A_Explain;
    public UILabel _A_Count;
    public GameObject[] _A_RewardIcon;
    public UILabel _A_RewardValue;
    public GameObject _A_GetButtonGray;
    public GameObject _A_GetButtonClearCheck;

    int _AchieveCount;
    public int _NowInfo;
    int _RewardInfo;

    AchievementMng _AchieveMng;

    public void Init(int achievecount,UIScrollView scroll,AchievementMng mng)
    {
        _AchieveCount = achievecount;
        _A_Name.text = StaticMng.Instance._Achive_Name[_AchieveCount];
        _A_Explain.text = StaticMng.Instance._Achive_Explanation[_AchieveCount];

        int tempcount = StaticMng.Instance._Achive_NowValue[_AchieveCount];
        if (tempcount > StaticMng.Instance._Achive_MaxValue[_AchieveCount])
            tempcount = StaticMng.Instance._Achive_MaxValue[_AchieveCount];

        _A_Count.text = tempcount.ToString() + "/" + StaticMng.Instance._Achive_MaxValue[_AchieveCount].ToString();
        if (StaticMng.Instance._Achive_RewardInfo[achievecount] == "gold")
            _RewardInfo = 0;
        else if (StaticMng.Instance._Achive_RewardInfo[achievecount] == "gem")
            _RewardInfo = 1;
        _A_RewardIcon[_RewardInfo].SetActive(true);
        _A_RewardValue.text = StaticMng.Instance._Achive_Reward[_AchieveCount].ToString();
        _NowInfo = StaticMng.Instance._Achive_ClearCheck[_AchieveCount];
        GetComponent<UIDragScrollView>().scrollView = scroll;
        _AchieveMng = mng;
    }
    void Update()
    {
        _NowInfo = StaticMng.Instance._Achive_ClearCheck[_AchieveCount];
        int tempcount = StaticMng.Instance._Achive_NowValue[_AchieveCount];
        if (tempcount > StaticMng.Instance._Achive_MaxValue[_AchieveCount])
            tempcount = StaticMng.Instance._Achive_MaxValue[_AchieveCount];

        _A_Count.text = tempcount.ToString() + "/" + StaticMng.Instance._Achive_MaxValue[_AchieveCount].ToString();
        if (_NowInfo == 0 || _NowInfo == 2)
            _A_GetButtonGray.SetActive(true);
        else
            _A_GetButtonGray.SetActive(false);
        if (_NowInfo == 2)
            _A_GetButtonClearCheck.SetActive(true);
        else
            _A_GetButtonClearCheck.SetActive(false);

        
    }

    public void GetGoldPlease()
    {
        _AchieveMng.ClearAchievement(_AchieveCount);
    }
}
