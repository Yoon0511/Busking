using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerAttackObject : MonoBehaviour {

    public float _Damage;
    public float _Speed;
    public Monster _TargetObject;
    public int _ParentTowerType;

    //[SerializeField]
    //GameObject _HitEffect;

    public GameObject _ArmorBreakCircle;

    bool _Projectile;
    virtual public void Init(float damage)
    {
        _Damage = damage;
    }
    virtual public void Init(float damage,Monster target,float speed,int type)
    {
        _Damage = damage;
        _Speed = speed;
        _TargetObject = target;
        _Projectile = true;
        _ParentTowerType = type;
    }

    public void HitEffect()
    {

        if (_Projectile)
        {
            if (_TargetObject != null)
            {
                if(_ParentTowerType==1)
                {
                    //GameObject obj = NGUITools.AddChild(StageMng.Data._ObjectRoot, _HitEffect);
                    GameObject obj = ObjectPoolingMng.Data._GuitarHitEffect[ObjectPoolingMng.Data._GuitarHitEffect_Count];
                    obj.SetActive(true);
                    obj.GetComponent<J_UI2DSpriteAnimation>().ReStart();
                    ObjectPoolingMng.Data.CountUp_GuitarHit();
                    obj.transform.localPosition = _TargetObject.NowPosition();
                    obj.transform.localEulerAngles = transform.localEulerAngles + new Vector3(0, 0, 180);
                }
                else
                {
                    GameObject obj = ObjectPoolingMng.Data._BassHitEffect[ObjectPoolingMng.Data._BassHitEffect_Count];
                    obj.SetActive(true);
                    obj.GetComponent<J_UI2DSpriteAnimation>().ReStart();
                    ObjectPoolingMng.Data.CountUp_BassHit();
                    obj.transform.localPosition = _TargetObject.NowPosition();
                    obj.transform.localEulerAngles = transform.localEulerAngles + new Vector3(0, 0, 180);
                }
                
            }


            Destroy(gameObject);

        }

    }
}
