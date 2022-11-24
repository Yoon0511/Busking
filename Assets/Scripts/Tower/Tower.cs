using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour {

    public float _NowTime;
    public float _OriginDamage;
    public float _OriginAttackDelayTime;
    public float _OriginRange;

    public float _NowDamage;
    public float _NowAttackDelayTime;
    public float _NowRange;

    public float _SlowValue;
    public float _OriginSlowValue;
    public float _SlowTime;
    public float _OriginSlowTime;
    public float _ArmorBreakValue;
    public float _OriginArmorBreakValue;
    public float _ArmorBreakTime;
    public float _OriginArmorBreakTime;

    public bool _ProjectileType;
    public GameObject _ObjectRoot;
    public Monster _TargetMonster;

    public bool _Amp;
    public int _AmpType_Hero;
    //public GameObject _AmpSelector;

    public TowerAnimation _MyAnimation;
    public UISprite _RankCircle;
    public UISprite _RankLight;

    public GameObject _UpgradePopup;
    public UILabel _UpgradePopupCostLabel;
    public GameObject _UpgradeButtonGray;

    public bool _AmpTarget;
    public int _AttackStartFrame;

    public bool _NowChanging;
    float _ChangeTime;

    const float _FeverTime = 15.0f;

    bool _GameMode;

    virtual public void Init(string name, int type, float delaytime, float damage)
    {
        AllInit();
        _MyAnimation.Init(name);
        _AmpType_Hero = type;
        _OriginAttackDelayTime = delaytime;
        _OriginDamage = damage;

        _NowDamage = _OriginDamage;
        _NowAttackDelayTime = _OriginAttackDelayTime;
    }
    virtual public void Init(string name, int type, float delaytime, GameObject root, float range, float damage)//투사체형
    {
        AllInit();
        _MyAnimation.Init(name);
        _AmpType_Hero = type;
        _OriginDamage = damage;
        _OriginAttackDelayTime = delaytime;
        _ObjectRoot = root;
        _OriginRange = range;
        _ProjectileType = true;

        _NowDamage = _OriginDamage;
        _NowRange = _OriginRange;
        _NowAttackDelayTime = _OriginAttackDelayTime;
    }

    void AllInit()
    {
        float[] Rank_SlowPercent = { 90, 95, 100, 105, 110 };

        _NowChanging = false;
        _OriginSlowValue = (0.7f - (StaticMng.Instance._DrumTowerLevel * 0.02f)) * (Rank_SlowPercent[5 - StaticMng.Instance._DrumTowerRank] / 100.0f);
        _OriginArmorBreakValue = (0 - (0.6f * StaticMng.Instance._BassTowerLevel)) * (Rank_SlowPercent[StaticMng.Instance._BassTowerRank - 1] / 100.0f);
        _SlowValue = _OriginSlowValue;
        _ArmorBreakValue = _OriginArmorBreakValue;
        _OriginSlowTime = 1.0f;
        _OriginArmorBreakTime = 2.0f;
        _SlowTime = _OriginSlowTime;
        _ArmorBreakTime = _OriginArmorBreakTime;
        _GameMode = StaticMng.Instance._GameMode_Infinity;
    }

    void Update()
    {
        if (!StaticMng.Instance._PauseGame)
        {
            if (_GameMode && _AmpType_Hero != 5)
            {
                if (StaticMng.Instance._InfinityGameTowerLevel[_AmpType_Hero - 1] * 10 <= StageMng.Data._NowEnegy)
                    _UpgradeButtonGray.SetActive(false);
                else
                    _UpgradeButtonGray.SetActive(true);
            }
            if(_AmpType_Hero==1)
            {
                _RankLight.spriteName = "tower_rankeffect_light_" + StaticMng.Instance._GuitarTowerRank;
                _RankCircle.spriteName = "tower_rankeffect_circle_" + StaticMng.Instance._GuitarTowerRank;
            }
            else if (_AmpType_Hero == 2)
            {
                _RankLight.spriteName = "tower_rankeffect_light_" + StaticMng.Instance._DrumTowerRank;
                _RankCircle.spriteName = "tower_rankeffect_circle_" + StaticMng.Instance._DrumTowerRank;
                
            }
            else if (_AmpType_Hero == 3)
            {
                _RankLight.spriteName = "tower_rankeffect_light_" + StaticMng.Instance._BassTowerRank;
                _RankCircle.spriteName = "tower_rankeffect_circle_" + StaticMng.Instance._BassTowerRank;
            }
            else if (_AmpType_Hero == 4)
            {
                _RankLight.spriteName = "tower_rankeffect_light_" + StaticMng.Instance._KeyBoardTowerRank;
                _RankCircle.spriteName = "tower_rankeffect_circle_" + StaticMng.Instance._KeyBoardTowerRank;
            }

            _ChangeTime -= Time.smoothDeltaTime;
            if (_ChangeTime <= 0.0f && _NowChanging)
            {
                _MyAnimation._NowChange = false;
                _NowChanging = false;
            }
            if (_ProjectileType)
            {
                _TargetMonster = null;
                Monster temp = new Monster();
                bool tempb = false;
                for (int i = 0; i < StageMng.Data._MonsterList.Count; i++)
                {
                    if (Vector2.Distance(transform.localPosition, StageMng.Data._MonsterList[i].transform.localPosition) <= _OriginRange)
                    {
                        temp = StageMng.Data._MonsterList[i];
                        tempb = true;
                        break;
                    }
                }
                if(_GameMode)
                {
                    for (int i = 0; i < StageMng.Data._MonsterList.Count; i++)
                    {
                        if (Vector2.Distance(transform.localPosition, StageMng.Data._MonsterList[i].transform.localPosition) <= _OriginRange && StageMng.Data._MonsterList[i].GetIsBoss())
                        {
                            temp = StageMng.Data._MonsterList[i];
                            tempb = true;
                            break;
                        }
                    }
                }
                if (tempb)
                    _TargetMonster = temp;
            }

            BuffUpdate();
        }
    }

    void BuffUpdate()
    {
        _NowDamage = _OriginDamage;
        _NowRange = _OriginRange;
        _NowAttackDelayTime = _OriginAttackDelayTime;
        _SlowValue = _OriginSlowValue;
        _ArmorBreakValue = _OriginArmorBreakValue;
        _SlowTime = _OriginSlowTime;
        _ArmorBreakTime = _OriginArmorBreakTime;

        if (_GameMode && !_Amp)
        {
            _NowDamage *= 0.7f + (StaticMng.Instance._InfinityGameTowerLevel[_AmpType_Hero - 1] * 0.3f);
            if (_AmpType_Hero == 2)
                _SlowValue -= StaticMng.Instance._InfinityGameTowerLevel[_AmpType_Hero - 1] * 0.02f;
            if (_AmpType_Hero == 3)
                _ArmorBreakValue *= 1.0f + (StaticMng.Instance._InfinityGameTowerLevel[_AmpType_Hero - 1] * 0.05f);
        }

        if (_NowChanging)
        {
            switch (_AmpType_Hero)
            {
                case 1:
                    _NowAttackDelayTime *= 0.5f;
                    break;
                case 2:
                    _SlowValue = _OriginSlowValue * 0.6f;
                    _SlowTime = _OriginSlowTime * 2.0f;
                    break;
                case 3:
                    _ArmorBreakValue = _OriginArmorBreakValue * 3.0f;
                    _ArmorBreakTime = _OriginArmorBreakTime * 2.0f;
                    break;
                case 4:
                    _NowDamage = _OriginDamage * 2.0f;
                    break;
            }
        }

    }

    public float getNowDamage() { return _NowDamage; }
    public float getNowAttackDelayTime() { return _NowAttackDelayTime; }
    public float getNowRange() { return _NowRange; }
    public float getNowSlowValue() { return _SlowValue; }
    public float getNowArmorBreakValue() { return _ArmorBreakValue; }
    public float getNowSlowTime() { return _SlowTime; }
    public float getNowArmorBreakTime() { return _ArmorBreakTime; }
    public int getAttackFrame() {return _AttackStartFrame; }

    public void AmpTargetSelect()
    {
        _AmpTarget = true;
    }
    public void OpenUpgradePopup()
    {
        if (_GameMode && !StageMng.Data._TowerSetMng._AmpSelecting)
        {
            for(int i=0;i<4;i++)
            {
                if (StageMng.Data._TowerSet[i])
                    StageMng.Data._Towers[i].CloseUpgradePopup();
            }

            _UpgradePopupCostLabel.text = (StaticMng.Instance._InfinityGameTowerLevel[_AmpType_Hero-1] * 10).ToString();
            _UpgradePopup.SetActive(true);
            _UpgradePopup.GetComponent<Animator>().SetTrigger("open");
            if (transform.localPosition.x > 640.0f && transform.localPosition.y > 360.0f)
                _UpgradePopup.transform.localPosition = new Vector3(-170, -120, 0);
            else if (transform.localPosition.x > 640.0f && transform.localPosition.y < 360.0f)
                _UpgradePopup.transform.localPosition = new Vector3(-170, 120, 0);
            else if (transform.localPosition.x < 640.0f && transform.localPosition.y < 360.0f)
                _UpgradePopup.transform.localPosition = new Vector3(170, 120, 0);
            else if (transform.localPosition.x < 640.0f && transform.localPosition.y > 360.0f)
                _UpgradePopup.transform.localPosition = new Vector3(170, -120, 0);
            else
                _UpgradePopup.transform.localPosition = new Vector3(170, 130, 0);
        }
    }
    public void CloseUpgradePopup()
    {
        _UpgradePopup.SetActive(false);
    }

    public void LevelUpTower()
    {
        if (StageMng.Data._NowEnegy >= StaticMng.Instance._InfinityGameTowerLevel[_AmpType_Hero-1] * 10)
        {
            StageMng.Data._NowEnegy -= StaticMng.Instance._InfinityGameTowerLevel[_AmpType_Hero-1] * 10;
            StaticMng.Instance._InfinityGameTowerLevel[_AmpType_Hero - 1]++;
            CloseUpgradePopup();
        }
    }

    public void SetSkill()
    {
        _NowChanging = true;
        _MyAnimation._NowChange = true;
        _MyAnimation._Transforming = true;
        if (_AmpType_Hero == 1)
            _MyAnimation.SetAnimation("change", 5);
        else if (_AmpType_Hero == 2)
            _MyAnimation.SetAnimation("change", 7);
        else if (_AmpType_Hero == 3)
            _MyAnimation.SetAnimation("change", 8);
        else if (_AmpType_Hero == 4)
            _MyAnimation.SetAnimation("change", 8);
        _ChangeTime = _FeverTime;
    }
    
}
