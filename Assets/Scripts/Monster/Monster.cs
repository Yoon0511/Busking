using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour {

    public float _HP;
    float _HPMax;
    public float _Speed;
    float _SpeedDebuff;
    float _Armor;
    List<Vector2> _DestinationPosition = new List<Vector2>();
    int _NowTargetDestination;

    List<MonsterDebuff> _DebuffList = new List<MonsterDebuff>();

    //[SerializeField]
    //GameObject _DrumHitEffect;
    int _MonsterCode;
    //[SerializeField]
    //GameObject _KeyBoardHitEffect;
    bool _Boss;

    [SerializeField]
    GameObject _HPBar;
    [SerializeField]
    UI2DSprite _HPGaze;
    [SerializeField]
    UI2DSprite _HPGaze_Back;
    float _TargetHPValue;

    [SerializeField]
    GameObject _SlowIcon;
    [SerializeField]
    GameObject _ArmorBreakIcon;

    GameObject _DeadEffect;

    [SerializeField]
    UISprite _MySprite;
    [SerializeField]
    J_UISpriteAnimation _MyAnimationMng;

    bool _GameMode;

    public void Init(float hp,float speed,List<Vector2> poslist,int num)
    {
        _SpeedDebuff = 1.0f;
        _TargetHPValue = 1;
        _HP = hp;
        _HPMax = hp;
        _Speed = speed;
        _DestinationPosition = poslist;
        gameObject.transform.localPosition = _DestinationPosition[0];
        _NowTargetDestination = 1;
        _MonsterCode = num;
        SetSpriteAnimation(_MonsterCode);
        SetHPBarPosition();
        _GameMode = StaticMng.Instance._GameMode_Infinity;
        BossCheck();
    }
    void BossCheck()
    {
        switch(_MonsterCode)
        {
            case 1:
                _Boss = false;
                break;
            case 2:
                _Boss = false;
                break;
            case 3:
                _Boss = false;
                break;
            case 4:
                _Boss = true;
                break;
            case 5:
                _Boss = true;
                break;
            case 6:
                _Boss = false;
                break;
            case 7:
                _Boss = false;
                break;
            case 8:
                _Boss = true;
                break;
            case 9:
                _Boss = true;
                break;

        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "drum_attackcircle")
        {
            HitMonster(other.GetComponent<TowerAttackObject>()._Damage);

            
            DebuffAdd(StageMng.Data._Towers[1].getNowSlowTime(), 1);
            DrumHitEffectCreate();
        }
        else if(other.tag == "keyboard_attackline")
        {
            HitMonster(other.GetComponent<TowerAttackObject>()._Damage);
            KeyBoardHitEffectCreate(other.transform.parent.localEulerAngles.z);
        }
        else if (other.tag == "bass_attackcircle")
        {
            DebuffAdd(StageMng.Data._Towers[2].getNowArmorBreakTime(), 2);
        }
    }
    void SetHPBarPosition()
    {
        if (_MonsterCode == 1 || _MonsterCode == 2 || _MonsterCode == 3 || _MonsterCode == 6 || _MonsterCode == 7)
            _HPBar.transform.localPosition = new Vector3(0, 122, 0);
        else if (_MonsterCode == 4)
            _HPBar.transform.localPosition = new Vector3(0, 230, 0);
        else if (_MonsterCode == 5 || _MonsterCode == 8)
            _HPBar.transform.localPosition = new Vector3(0, 190, 0);
        else if (_MonsterCode == 9)
            _HPBar.transform.localPosition = new Vector3(0, 200, 0);
    }

    public void HitMonster(float dmg)
    {
        _HP -= dmg - _Armor;
        _TargetHPValue = ((float)_HP / (float)_HPMax);
    }

    IEnumerator HPGazeEffect_C()
    {
        yield return null;
    }
    void DrumHitEffectCreate()
    {
        //GameObject obj = NGUITools.AddChild(gameObject, _DrumHitEffect);
        GameObject obj = ObjectPoolingMng.Data._DrumHitEffect[ObjectPoolingMng.Data._DrumHitEffect_Count];
        obj.SetActive(true);
        obj.GetComponent<J_FadeAnimation>().Restart();
        ObjectPoolingMng.Data.CountUp_DrumHit();
        obj.transform.localPosition = transform.localPosition + new Vector3(0, 10, 0);
        UISprite objs = obj.GetComponent<UISprite>();
        if (_MonsterCode == 1 || _MonsterCode == 2)
            objs.spriteName = "m_note8_" + "hiteffect_drum_" + _MyAnimationMng.GetNowFrame();
        else if (_MonsterCode == 3)
            objs.spriteName = "m_note44_" + "hiteffect_drum_" + _MyAnimationMng.GetNowFrame();
        else if (_MonsterCode == 4)
            objs.spriteName = "m_gclef_" + "hiteffect_drum_" + _MyAnimationMng.GetNowFrame();
        else if (_MonsterCode == 5)
            objs.spriteName = "m_bassclef_" + "hiteffect_drum_" + _MyAnimationMng.GetNowFrame();
        else if (_MonsterCode == 6)
            objs.spriteName = "m_comma4_" + "hiteffect_drum_" + _MyAnimationMng.GetNowFrame();
        else if (_MonsterCode == 7)
            objs.spriteName = "m_comma8_" + "hiteffect_drum_" + _MyAnimationMng.GetNowFrame();
        else if (_MonsterCode == 8)
            objs.spriteName = "m_rmark_" + "hiteffect_drum_" + _MyAnimationMng.GetNowFrame();
        else if (_MonsterCode == 9)
            objs.spriteName = "m_sharp_" + "hiteffect_drum_" + _MyAnimationMng.GetNowFrame();

        objs.flip = _MySprite.flip;
        objs.MakePixelPerfect();
    }
    void KeyBoardHitEffectCreate(float angle)
    {
        //GameObject obj = NGUITools.AddChild(gameObject, _KeyBoardHitEffect);
        GameObject obj = ObjectPoolingMng.Data._KeyBoardHitEffect[ObjectPoolingMng.Data._KeyBoardHitEffect_Count];
        obj.SetActive(true);
        obj.GetComponent<J_UI2DSpriteAnimation>().ReStart();
        ObjectPoolingMng.Data.CountUp_KeyBoardHit();
        obj.transform.localPosition = transform.localPosition + new Vector3(0, 50, 0);
        obj.transform.localEulerAngles = new Vector3(0, 0, angle);
    }
    public void DebuffAdd(float time,int type)
    {
        MonsterDebuff buff = new MonsterDebuff();
        buff.Init(time, type);
        _DebuffList.Add(buff);
    }
    void DebuffCheck()
    {
        bool slow = false;
        bool armor = false;
        for (int i = 0; i < _DebuffList.Count; i++)
        {
            if (_DebuffList[i].GetBuffType() == 1)
                slow = true;
            else
                armor = true;

            if (_DebuffList[i].BuffUpdate())
            {
                _DebuffList.RemoveAt(i);
                i--;
            }
        }
        if (slow)
        {
            _SpeedDebuff = StageMng.Data._Towers[1].getNowSlowValue();
            _SlowIcon.SetActive(true);
        }
        else
        {
            _SpeedDebuff = 1.0f;
            _SlowIcon.SetActive(false);
        }
        if (armor)
        {
            _Armor = StageMng.Data._Towers[2].getNowArmorBreakValue();
            _ArmorBreakIcon.SetActive(true);
        }
        else
        {
            _Armor = 0;
            _ArmorBreakIcon.SetActive(false);
        }
    }

    void Update()
    {
        if(!StaticMng.Instance._PauseGame && StaticMng.Instance._StartGame)
        {
            _MyAnimationMng.SetDepth(730 - (int)(transform.localPosition.y));
            DebuffCheck();
            _HPGaze_Back.fillAmount -= Time.smoothDeltaTime / 2;
            if (_HPGaze_Back.fillAmount <= _TargetHPValue)
                _HPGaze_Back.fillAmount = _TargetHPValue;

            _HPGaze.fillAmount = ((float)_HP / (float)_HPMax);

            transform.localPosition = Vector3.MoveTowards(transform.localPosition, _DestinationPosition[_NowTargetDestination], Time.smoothDeltaTime * _Speed * _SpeedDebuff);
            if (Vector2.Distance(transform.localPosition, _DestinationPosition[_NowTargetDestination]) < 2.0f)
            {
                if (_DestinationPosition.Count - 1 > _NowTargetDestination)
                    _NowTargetDestination++;
                else
                    _NowTargetDestination = 0;
                
            }
            if(!_GameMode)
            {
                if (Vector2.Distance(transform.localPosition, _DestinationPosition[_DestinationPosition.Count - 1]) < 2.0f)
                {
                    StaticMng.Instance._PlayerLife--;
                    DeadMonster();
                }
            }
           


            if (_HP <= 0)
                DeadMonster();

            if (_DestinationPosition[_NowTargetDestination].x - transform.localPosition.x >= 0)
                _MySprite.flip = UIBasicSprite.Flip.Horizontally;
            else
                _MySprite.flip = UIBasicSprite.Flip.Nothing;
        }
    }

    void DeadMonster()
    {
        GameObject obj = NGUITools.AddChild(StageMng.Data._ObjectRoot, _DeadEffect);
        obj.transform.localPosition = transform.localPosition + new Vector3(0,10,0);
        obj.GetComponent<UI2DSprite>().flip = _MySprite.flip;
        StaticMng.Instance._MonsterCount--;
        StageMng.Data._MonsterList.Remove(this);
        gameObject.SetActive(false);
        //Destroy(gameObject);
    }



    void SetSpriteAnimation(int num)
    {
        if (num == 1)//note8
        {
            _MyAnimationMng.Init(6, "m_note8_walk_", 0.125f, true, true);
            _DeadEffect = StageMng.Data._Dead_Note8;
        }
        else if (num == 2) //note8_2
        {
            _MyAnimationMng.Init(6, "m_note8_2_walk_", 0.125f, true, true);
            _DeadEffect = StageMng.Data._Dead_Note8_2;
        }
        else if (num == 3) //note44
        {
            _MyAnimationMng.Init(6, "m_note44_walk_", 0.125f, true, true);
            _DeadEffect = StageMng.Data._Dead_Note44;
        }
        else if (num == 4)
        {
            _MyAnimationMng.Init(4, "m_gclef_walk_", 0.125f, true, true);
            _DeadEffect = StageMng.Data._Dead_GClef;
        }
        else if (num == 5)
        {
            _MyAnimationMng.Init(9, "m_bassclef_walk_", 0.125f, true, true);
            _DeadEffect = StageMng.Data._Dead_BassClef;
        }
        else if (num == 6)
        {
            _MyAnimationMng.Init(12, "m_comma4_walk_", 0.125f, true, true);
            _DeadEffect = StageMng.Data._Dead_Comma4;
        }
        else if (num == 7)
        {
            _MyAnimationMng.Init(4, "m_comma8_walk_", 0.125f, true, true);
            _DeadEffect = StageMng.Data._Dead_Comma8;
        }
        else if (num == 8)
        {
            _MyAnimationMng.Init(6, "m_rmark_walk_", 0.125f, true, true);
            _DeadEffect = StageMng.Data._Dead_RMark;
        }
        else if (num == 9)
        {
            _MyAnimationMng.Init(13, "m_sharp_walk_", 0.125f, true, true);
            _DeadEffect = StageMng.Data._Dead_Sharp;
        }
    }
    public bool GetIsBoss() { return _Boss; }
    public Vector2 NowPosition() { return transform.localPosition + new Vector3(0, 40, 0); }
}


public class MonsterDebuff
{
    float _BuffTime;
    /// <summary>
    /// 1 - 슬로우
    /// 2 - 방어력감소
    /// </summary>
    int _BuffType;
    public void Init(float bufftime,int bufftype)
    {
        _BuffTime = bufftime;
        _BuffType = bufftype;
    }

    public bool BuffUpdate()
    {
        _BuffTime -= Time.smoothDeltaTime;
        if (_BuffTime <= 0)
            return true;
        return false;
    }
    public int GetBuffType() { return _BuffType; }
   
}