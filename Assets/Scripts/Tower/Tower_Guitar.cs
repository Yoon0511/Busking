using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower_Guitar : Tower {

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
                    _NowTime = getNowAttackDelayTime();
                    Attack();
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
                _MyAnimation.SetAnimation("changeattack", 5);
            else
                _MyAnimation.SetAnimation("attack", 5);
            _NowTime -= getNowAttackDelayTime();
            StartCoroutine(AttackDelay());
        }//캐릭터마다 다르게
        
    }

    IEnumerator AttackDelay()
    {
        yield return new WaitForSeconds(_AttackStartFrame*_MyAnimation._MaxFrameTime);

        if (_TargetMonster != null)
        {
            float angle = Mathf.Atan2(transform.localPosition.y - _TargetMonster.transform.localPosition.y, transform.localPosition.x - _TargetMonster.transform.localPosition.x);
            angle *= Mathf.Rad2Deg;

            GameObject shoote = NGUITools.AddChild(_ObjectRoot, _ShootEffect);
            //GameObject shoote = ObjectPoolingMng.Data._GuitarShootEffect[ObjectPoolingMng.Data._GuitarShootEffect_Count];
            //shoote.SetActive(true);
            //shoote.transform.GetChild(0).GetComponent<J_UI2DSpriteAnimation>().ReStart();
            //ObjectPoolingMng.Data.CountUp_GuitarShoot();
            shoote.transform.localPosition = transform.localPosition;
            shoote.transform.localEulerAngles = new Vector3(0, 0, angle+180);

            GameObject obj = NGUITools.AddChild(_ObjectRoot, _AttackObject);
            obj.transform.localPosition = transform.localPosition;
            obj.GetComponent<AttackProjectile>().Init(getNowDamage(), _TargetMonster, 800,1);
        }
        else
            _NowTime = getNowAttackDelayTime();
        
    }
}
