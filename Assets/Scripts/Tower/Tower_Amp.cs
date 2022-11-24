using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower_Amp : Tower {
    Tower _MainBody;

    [SerializeField]
    GameObject _Guitar_Projectile;
    [SerializeField]
    GameObject _Bass_Projectile;
    [SerializeField]
    GameObject _Drum_CircleAttack;
    [SerializeField]
    GameObject _KeyBoard_AttackObject;
    [SerializeField]
    GameObject _LinkIcon;
    [SerializeField]
    GameObject[] _LinkIconArray;
    //float _NowTime;
    //float _DelayTime;
    public bool _ParentSet;

    int _AmpType;

    void Start()
    {
        //_AttackDelayTime = 1.0f;
        
    }

    public void InitAmp(TowerMng _TowerMng,MapMng _MapMng,int x,int y,GameObject _LoadRoot,GameObject _AmpLine,GameObject ObjRoot,Tower tower)
    {
        //Tower target = new Tower();
        //for(int i=0;i<_TowerMng._TowerList.Count;i++)
        //{
        //    if (_TowerMng._TowerList[i]._AmpTarget)
        //    {
        //        target = _TowerMng._TowerList[i];
        //        target._AmpTarget = false;
        //    }
        //}
        
        if (tower._AmpType_Hero==1)//Guitar
        {
            GetComponent<Tower>().Init("amp", 5, 1.5f, ObjRoot, 250, 0);
            _AmpType = 1;
            _MainBody = StageMng.Data._Towers[0];
        }
        else if(tower._AmpType_Hero == 2)//Drum
        {
            GetComponent<Tower>().Init("amp", 5, 1.5f, 0);
            _MainBody = StageMng.Data._Towers[1];
            _AmpType = 2;
        }
        else if (tower._AmpType_Hero == 3)//Bass
        {
            _AmpType = 3;
            GetComponent<Tower>().Init("amp", 5, 1.5f, ObjRoot, 250, 0);
            _MainBody = StageMng.Data._Towers[2];
        }
        else if (tower._AmpType_Hero == 4)//KeyBoard
        {
            _AmpType = 4;
            GetComponent<Tower>().Init("amp", 5, 1.5f, ObjRoot, 250, 0);
            _MainBody = StageMng.Data._Towers[3];
        }

        _LinkIcon.SetActive(true);
        _LinkIconArray[_AmpType - 1].SetActive(true);
        _TowerMng._TowerList.Add(GetComponent<Tower>());
        _MapMng.RedTileSet(x, y);

        GameObject line = NGUITools.AddChild(_LoadRoot, _AmpLine);
        line.transform.localPosition = transform.localPosition + new Vector3(-640,-360,0);
        line.transform.localEulerAngles = new Vector3(0, 0,
            Mathf.Atan2(transform.localPosition.y - tower.transform.localPosition.y,
            transform.localPosition.x - tower.transform.localPosition.x) * Mathf.Rad2Deg);
        line.GetComponent<UI2DSprite>().width = (int)Vector2.Distance(transform.localPosition, tower.transform.localPosition);

        

        _ParentSet = true;
        StartCoroutine(Update1());
    }
    IEnumerator Update1()
    {
        yield return null;
        if(!StaticMng.Instance._PauseGame && StaticMng.Instance._StartGame)
        {
            _NowTime += Time.smoothDeltaTime;
            if (_NowTime >= _MainBody.getNowAttackDelayTime())
            {
                if (_AmpType == 1)
                {
                    Attack();
                }
                else if (_AmpType == 2)
                {

                    bool check = false;
                    for (int i = 0; i < StageMng.Data._MonsterList.Count; i++)
                    {
                        if (Vector2.Distance(StageMng.Data._MonsterList[i].transform.localPosition, transform.localPosition) < 200)
                        {
                            check = true;
                            break;
                        }
                    }
                    if (check)
                    {
                        _NowTime = _MainBody.getNowAttackDelayTime();
                        Attack_Drum();
                    }
                }
                else if (_AmpType == 3)
                {
                    Attack();
                }
                else if (_AmpType == 4)
                {
                    Attack_KeyBoard();
                }
            }
            if (_NowTime >= _MainBody.getNowAttackDelayTime())
                _NowTime = _MainBody.getNowAttackDelayTime();
            
        }
        StartCoroutine(Update1());
    }

    void Attack()
    {
        
        if(_AmpType == 1)
        {
            //_MyAnimation.SetAnimation("attack", 4);
            _NowTime -= _MainBody.getNowAttackDelayTime();
            StartCoroutine(AttackDelay_Guitar());
        }
        else if(_AmpType ==3)
        {
            _NowTime -= _MainBody.getNowAttackDelayTime();
            StartCoroutine(AttackDelay_Bass());
        }
    }
    void Attack_Drum()
    {
        _NowTime -= _MainBody.getNowAttackDelayTime();
        //_MyAnimation.SetAnimation("attack", 7);
        StartCoroutine(AttackDelay_Drum());
    }
    void Attack_KeyBoard()
    {
        //Debug.Log("Shoot");
        //Debug.Log(_NowTime);
        _NowTime -= _MainBody.getNowAttackDelayTime();
        //_MyAnimation.SetAnimation("attack", 7);
        StartCoroutine(AttackDelay_KeyBoard());
    }

    IEnumerator AttackDelay_Guitar()
    {
        yield return new WaitForSeconds(_MainBody.getAttackFrame() * _MyAnimation._MaxFrameTime);

        if (_TargetMonster != null)
        {
            float angle = Mathf.Atan2(transform.localPosition.y - _TargetMonster.NowPosition().y, transform.localPosition.x - _TargetMonster.NowPosition().x);
            angle *= Mathf.Rad2Deg;

            //GameObject shoote = NGUITools.AddChild(_ObjectRoot, _Guitar_ShootEffect);
            GameObject shoote = ObjectPoolingMng.Data._GuitarShootEffect[ObjectPoolingMng.Data._GuitarShootEffect_Count];
            shoote.SetActive(true);
            shoote.GetComponent<J_UI2DSpriteAnimation>().ReStart();
            ObjectPoolingMng.Data.CountUp_GuitarShoot();
            shoote.transform.localPosition = transform.localPosition;
            shoote.transform.localEulerAngles = new Vector3(0, 0, angle+180);

            GameObject obj = NGUITools.AddChild(_ObjectRoot, _Guitar_Projectile);
            obj.transform.localPosition = transform.localPosition;
            obj.GetComponent<AttackProjectile>().Init(_MainBody.getNowDamage() / 2.0f, _TargetMonster, 800,1);
        }
        else
            _NowTime = _MainBody.getNowAttackDelayTime();
        
    }
    IEnumerator AttackDelay_Bass()
    {
        yield return new WaitForSeconds(_MainBody.getAttackFrame() * _MyAnimation._MaxFrameTime);

        if (_TargetMonster != null)
        {
            float angle = Mathf.Atan2(transform.localPosition.y - _TargetMonster.NowPosition().y, transform.localPosition.x - _TargetMonster.NowPosition().x);
            angle *= Mathf.Rad2Deg;

            //GameObject shoote = NGUITools.AddChild(_ObjectRoot, _Bass_ShootEffect);
            GameObject shoote = ObjectPoolingMng.Data._BassShootEffect[ObjectPoolingMng.Data._BassShootEffect_Count];
            shoote.SetActive(true);
            shoote.GetComponent<J_UI2DSpriteAnimation>().ReStart();
            ObjectPoolingMng.Data.CountUp_BassShoot();
            shoote.transform.localPosition = transform.localPosition;
            shoote.transform.localEulerAngles = new Vector3(0, 0, angle + 180);

            GameObject obj = NGUITools.AddChild(_ObjectRoot, _Bass_Projectile);
            obj.transform.localPosition = transform.localPosition;
            obj.GetComponent<AttackProjectile>().Init(_MainBody.getNowDamage() / 2.0f, _TargetMonster, 800,3);
        }
        else
            _NowTime = _MainBody.getNowAttackDelayTime();

    }
    IEnumerator AttackDelay_Drum()
    {
        yield return new WaitForSeconds(_MainBody.getAttackFrame() * _MyAnimation._MaxFrameTime);
        GameObject obj = NGUITools.AddChild(gameObject, _Drum_CircleAttack);
        obj.GetComponent<TowerAttackObject>().Init(_MainBody.getNowDamage()/2.0f);
    }
    IEnumerator AttackDelay_KeyBoard()
    {
        yield return new WaitForSeconds(_MainBody.getAttackFrame() * _MyAnimation._MaxFrameTime);

        if (_TargetMonster != null)
        {
            float angle = Mathf.Atan2(transform.localPosition.y - _TargetMonster.NowPosition().y, transform.localPosition.x - _TargetMonster.NowPosition().x);
            angle *= Mathf.Rad2Deg;

            //GameObject shoote = NGUITools.AddChild(_ObjectRoot, _KeyBoard_ShootEffect);
            GameObject shoote = ObjectPoolingMng.Data._KeyBoardShootEffect[ObjectPoolingMng.Data._KeyBoardShootEffect_Count];
            shoote.SetActive(true);
            shoote.transform.GetChild(0).GetComponent<J_UI2DSpriteAnimation>().ReStart();
            ObjectPoolingMng.Data.CountUp_KeyBoardShoot();
            shoote.transform.localPosition = transform.localPosition;
            shoote.transform.localEulerAngles = new Vector3(0, 0, angle + 180);

            GameObject obj = NGUITools.AddChild(_ObjectRoot, _KeyBoard_AttackObject);
            obj.transform.localPosition = transform.localPosition;
            obj.transform.localEulerAngles = new Vector3(0, 0, angle + 180);
            obj.transform.GetChild(0).GetComponent<TowerAttackObject>().Init(_MainBody.getNowDamage() / 2.0f);
        }
        else
            _NowTime = _MainBody.getNowAttackDelayTime();

    }
}
