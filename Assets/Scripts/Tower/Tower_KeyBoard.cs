﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower_KeyBoard : Tower {

    [SerializeField]
    GameObject _AttackObject;
    [SerializeField]
    GameObject _ShootEffect;

    //float _NowTime;
    //float _DelayTime;

    void Start()
    {
        //_AttackDelayTime = 1.0f;
        StartCoroutine(Update1());
    }
    IEnumerator Update1()
    {
        yield return null;

        if (!StaticMng.Instance._PauseGame && StaticMng.Instance._StartGame)
        {
            if (!_MyAnimation.getTransforming())
            {
                _NowTime += Time.smoothDeltaTime;
                if (_NowTime >= getNowAttackDelayTime())
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
                        _NowTime -= getNowAttackDelayTime();
                        Attack();
                    }

                }
                if (_NowTime >= getNowAttackDelayTime())
                    _NowTime = getNowAttackDelayTime();
            }
        }


        StartCoroutine(Update1());
    }

    void Attack()
    {
        
        if(_TargetMonster!=null)
        {
            if (_NowChanging)
                _MyAnimation.SetAnimation("changeattack", 9);
            else
                _MyAnimation.SetAnimation("attack", 9);
            StartCoroutine(AttackDelay());
        }//캐릭터마다 다르게
        
    }

    IEnumerator AttackDelay()
    {
        yield return new WaitForSeconds(_AttackStartFrame*_MyAnimation._MaxFrameTime);

        if (_TargetMonster != null)
        {
            float angle = Mathf.Atan2(transform.localPosition.y - _TargetMonster.NowPosition().y, transform.localPosition.x - _TargetMonster.NowPosition().x);
            angle *= Mathf.Rad2Deg;

            GameObject shoote = NGUITools.AddChild(_ObjectRoot, _ShootEffect);
            
            shoote.transform.localPosition = transform.localPosition;
            shoote.transform.localEulerAngles = new Vector3(0, 0, angle+180);

            GameObject obj = NGUITools.AddChild(_ObjectRoot, _AttackObject);
            obj.transform.localPosition = transform.localPosition;
            obj.transform.localEulerAngles = new Vector3(0, 0, angle + 180);
            obj.transform.GetChild(0).GetComponent<TowerAttackObject>().Init(getNowDamage());
        }
        else
            _NowTime = getNowAttackDelayTime();
        
    }
}
