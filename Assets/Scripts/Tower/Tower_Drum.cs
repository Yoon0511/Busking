using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower_Drum : Tower {

    [SerializeField]
    GameObject _CircleAttack;
    //float _NowTime;
    //float _DelayTime;

    void Start()
    {
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

    virtual public void Attack()
    {
        if (_NowChanging)
            _MyAnimation.SetAnimation("changeattack", 11);
        else
            _MyAnimation.SetAnimation("attack", 11);
        StartCoroutine(AttackDelay());
    }

    IEnumerator AttackDelay()
    {
        yield return new WaitForSeconds(_AttackStartFrame * _MyAnimation._MaxFrameTime);
        GameObject obj = NGUITools.AddChild(gameObject, _CircleAttack);
        obj.GetComponent<TowerAttackObject>().Init(getNowDamage());
    }
}
