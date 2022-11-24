using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolingMng : MonoBehaviour {
    private static ObjectPoolingMng instance = null;

    public static ObjectPoolingMng Data
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType(typeof(ObjectPoolingMng)) as ObjectPoolingMng;
                if (instance == null)
                {
                    Debug.Log("no instance");
                }
            }
            return instance;
        }
    }


    public GameObject _GameRoot;

    public GameObject _GuitarShootEffect_Origin;
    public List<GameObject> _GuitarShootEffect = new List<GameObject>();
    public int _GuitarShootEffect_Count;
    public int _GuitarShootEffect_MaxCount;

    public GameObject _GuitarHitEffect_Origin;
    public List<GameObject> _GuitarHitEffect = new List<GameObject>();
    public int _GuitarHitEffect_Count;
    public int _GuitarHitEffect_MaxCount;

    public GameObject _DrumHitEffect_Origin;
    public List<GameObject> _DrumHitEffect = new List<GameObject>();
    public int _DrumHitEffect_Count;
    public int _DrumHitEffect_MaxCount;

    public GameObject _BassShootEffect_Origin;
    public List<GameObject> _BassShootEffect = new List<GameObject>();
    public int _BassShootEffect_Count;
    public int _BassShootEffect_MaxCount;

    public GameObject _BassHitEffect_Origin;
    public List<GameObject> _BassHitEffect = new List<GameObject>();
    public int _BassHitEffect_Count;
    public int _BassHitEffect_MaxCount;

    public GameObject _KeyBoardShootEffect_Origin;
    public List<GameObject> _KeyBoardShootEffect = new List<GameObject>();
    public int _KeyBoardShootEffect_Count;
    public int _KeyBoardShootEffect_MaxCount;

    public GameObject _KeyBoardHitEffect_Origin;
    public List<GameObject> _KeyBoardHitEffect = new List<GameObject>();
    public int _KeyBoardHitEffect_Count;
    public int _KeyBoardHitEffect_MaxCount;

    public GameObject _Monster_Origin;
    public List<GameObject> _Monster = new List<GameObject>();
    public int _Monster_Count;
    public int _Monster_MaxCount;

    void Awake()
    {
        _GuitarShootEffect_MaxCount = 100;
        _GuitarHitEffect_MaxCount = 100;
        _DrumHitEffect_MaxCount = 100;
        _BassShootEffect_MaxCount = 100;
        _BassHitEffect_MaxCount = 100;
        _KeyBoardShootEffect_MaxCount = 100;
        _KeyBoardHitEffect_MaxCount = 100;
        _Monster_MaxCount = 75;

        _GuitarShootEffect_Count = 0;
        for(int i=0;i<_GuitarShootEffect_MaxCount;i++)
        {
            GameObject obj = NGUITools.AddChild(_GameRoot, _GuitarShootEffect_Origin);
            obj.SetActive(false);
            _GuitarShootEffect.Add(obj);
        }

        _GuitarHitEffect_Count = 0;
        for (int i = 0; i < _GuitarHitEffect_MaxCount; i++)
        {
            GameObject obj = NGUITools.AddChild(_GameRoot, _GuitarHitEffect_Origin);
            obj.SetActive(false);
            _GuitarHitEffect.Add(obj);
        }

        _DrumHitEffect_Count = 0;
        for (int i = 0; i < _DrumHitEffect_MaxCount; i++)
        {
            GameObject obj = NGUITools.AddChild(_GameRoot, _DrumHitEffect_Origin);
            obj.SetActive(false);
            _DrumHitEffect.Add(obj);
        }

        _BassShootEffect_Count = 0;
        for (int i = 0; i < _BassShootEffect_MaxCount; i++)
        {
            GameObject obj = NGUITools.AddChild(_GameRoot, _BassShootEffect_Origin);
            obj.SetActive(false);
            _BassShootEffect.Add(obj);
        }

        _BassHitEffect_Count = 0;
        for (int i = 0; i < _BassHitEffect_MaxCount; i++)
        {
            GameObject obj = NGUITools.AddChild(_GameRoot, _BassHitEffect_Origin);
            obj.SetActive(false);
            _BassHitEffect.Add(obj);
        }

        _KeyBoardShootEffect_Count = 0;
        for (int i = 0; i < _KeyBoardShootEffect_MaxCount; i++)
        {
            GameObject obj = NGUITools.AddChild(_GameRoot, _KeyBoardShootEffect_Origin);
            obj.SetActive(false);
            _KeyBoardShootEffect.Add(obj);
        }

        _KeyBoardHitEffect_Count = 0;
        for (int i = 0; i < _KeyBoardHitEffect_MaxCount; i++)
        {
            GameObject obj = NGUITools.AddChild(_GameRoot, _KeyBoardHitEffect_Origin);
            obj.SetActive(false);
            _KeyBoardHitEffect.Add(obj);
        }

        _Monster_Count = 0;
        for (int i = 0; i < _Monster_MaxCount; i++)
        {
            GameObject obj = NGUITools.AddChild(_GameRoot, _Monster_Origin);
            obj.SetActive(false);
            _Monster.Add(obj);
        }
    }

    public void CountUp_GuitarShoot()
    {
        _GuitarShootEffect_Count++;
        if (_GuitarShootEffect_Count >= _GuitarShootEffect_MaxCount)
            _GuitarShootEffect_Count = 0;
    }
    public void CountUp_GuitarHit()
    {
        _GuitarHitEffect_Count++;
        if (_GuitarHitEffect_Count >= _GuitarHitEffect_MaxCount)
            _GuitarHitEffect_Count = 0;
    }
    public void CountUp_DrumHit()
    {
        _DrumHitEffect_Count++;
        if (_DrumHitEffect_Count >= _DrumHitEffect_MaxCount)
            _DrumHitEffect_Count = 0;
    }
    public void CountUp_BassShoot()
    {
        _BassShootEffect_Count++;
        if (_BassShootEffect_Count >= _BassShootEffect_MaxCount)
            _BassShootEffect_Count = 0;
    }
    public void CountUp_BassHit()
    {
        _BassHitEffect_Count++;
        if (_BassHitEffect_Count >= _BassHitEffect_MaxCount)
            _BassHitEffect_Count = 0;
    }
    public void CountUp_KeyBoardShoot()
    {
        _KeyBoardShootEffect_Count++;
        if (_KeyBoardShootEffect_Count >= _KeyBoardShootEffect_MaxCount)
            _KeyBoardShootEffect_Count = 0;
    }
    public void CountUp_KeyBoardHit()
    {
        _KeyBoardHitEffect_Count++;
        if (_KeyBoardHitEffect_Count >= _KeyBoardHitEffect_MaxCount)
            _KeyBoardHitEffect_Count = 0;
    }
    public void CountUp_Monster()
    {
        _Monster_Count++;
        if (_Monster_Count >= _Monster_MaxCount)
            _Monster_Count = 0;
    }
}
