using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSkillMng : MonoBehaviour {
    [SerializeField]
    GameObject[] _SkillIconGray;
    [SerializeField]
    GameObject[] _SkillIconFeverAni;

    [SerializeField]
    UI2DSprite _FeverGaze;
    [SerializeField]
    GameObject _EpicFeverEffect;
    bool _SkillOn;
        
    public void UseSkill(int num)
    {
        StaticMng.Instance._Achive_NowValue[0]++;
        StaticMng.Instance._Achive_NowValue[num+1]++;
        _SkillOn = false;
        for (int i = 0; i < 4; i++)
            _SkillIconGray[i].SetActive(true);
        for (int i = 0; i < 4; i++)
            _SkillIconFeverAni[i].SetActive(false);

        if((num == 0 && StaticMng.Instance._GuitarTowerRank == 5) ||
            (num == 1 && StaticMng.Instance._DrumTowerRank == 5) ||
            (num == 2 && StaticMng.Instance._BassTowerRank == 5) ||
            (num == 3 && StaticMng.Instance._KeyBoardTowerRank == 5))
        {
            GameObject obj = NGUITools.AddChild(StageMng.Data._Towers[num]._MyAnimation.transform.GetChild(0).gameObject, _EpicFeverEffect);
        }

        StageMng.Data._Towers[num].SetSkill();
        StopCoroutine(FeverOn());
        _FeverGaze.fillAmount = 0;
    }
    public void CanUseSkillSet()
    {
        if(StageMng.Data._TowerSet[0]&& StageMng.Data._TowerSet[1]&& StageMng.Data._TowerSet[2]&& StageMng.Data._TowerSet[3])
        {
            for (int i = 0; i < 4; i++)
            {
                _SkillIconGray[i].SetActive(false);
                _SkillIconFeverAni[i].SetActive(true);
            }
            _SkillOn = true;
            FeverAnimation();
        }
    }

    void FeverAnimation()
    {
        StartCoroutine(FeverOn());
    }
    IEnumerator FeverOn()
    {
        while(true)
        {
            _FeverGaze.fillAmount += Time.smoothDeltaTime;
            if(_FeverGaze.fillAmount>=1.0f)
            {
                _FeverGaze.fillAmount = 1.0f;
                break;
            }
            if (!_SkillOn)
                break;
            yield return null;
        }
    }
}
