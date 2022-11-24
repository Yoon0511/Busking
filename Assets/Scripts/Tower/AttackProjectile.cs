using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackProjectile : TowerAttackObject {



    void Update()
    {
        if (!StaticMng.Instance._PauseGame && StaticMng.Instance._StartGame)
        {
            if (_TargetObject != null)
            {
                float angle = Mathf.Atan2(transform.localPosition.y - _TargetObject.NowPosition().y, transform.localPosition.x - _TargetObject.NowPosition().x);
                transform.localEulerAngles = new Vector3(0, 0, angle * Mathf.Rad2Deg);
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, _TargetObject.NowPosition(), Time.smoothDeltaTime * _Speed);

                if (Vector2.Distance(_TargetObject.NowPosition(), transform.localPosition) < 5.0f)
                {
                    //Debug.Log("1");
                    if (StaticMng.Instance._GameMode_Infinity && _ParentTowerType == 1 && _TargetObject.GetIsBoss())
                        _TargetObject.HitMonster(_Damage * 2.2f);
                    else
                        _TargetObject.HitMonster(_Damage);
                    
                    if (_ParentTowerType == 3)
                    {
                        _TargetObject.DebuffAdd(StageMng.Data._Towers[2].getNowArmorBreakTime(), 2);
                        if (StageMng.Data._Towers[2]._NowChanging)
                        {
                            GameObject obj = NGUITools.AddChild(gameObject.transform.parent.gameObject, _ArmorBreakCircle);
                            obj.transform.localPosition = transform.localPosition;
                        }
                    }
                    HitEffect();
                }
            }
            else
            {
                HitEffect();
            }
        }
    }
}
