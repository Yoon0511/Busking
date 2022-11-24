using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class J_DoveAnimation : MonoBehaviour {

    UI2DSprite _MySprite;
    const float _EatTime = 0.25f;
    float _MaxTime;
    float _NowTime;
    bool _Eating;

    [SerializeField]
    Sprite _StandSprite;
    [SerializeField]
    Sprite _EatSprite;

    void Awake()
    {
        _MySprite = GetComponent<UI2DSprite>();
        _Eating = false;
        _NowTime = 0.0f;
        _MaxTime = Random.Range(1.0f, 2.5f);
    }

    void Update()
    {
        if(StaticMng.Instance._StartGame&&!StaticMng.Instance._PauseGame)
        {
            _NowTime += Time.smoothDeltaTime;
            if(_NowTime>=_MaxTime)
            {
                _NowTime = 0.0f;
                if(_Eating)
                {
                    _Eating = false;
                    _MySprite.sprite2D = _StandSprite;
                    _MaxTime = Random.Range(1.0f, 2.5f);
                }
                else
                {
                    _Eating = true;
                    _MySprite.sprite2D = _EatSprite;
                    _MaxTime = _EatTime;
                }
            }
        }
    }
}
