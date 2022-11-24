using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class J_FadeAnimation : MonoBehaviour {

    [SerializeField]
    float _Time;
    float _NowTime;

    [SerializeField]
    bool _FadeIn;
    [SerializeField]
    bool _2DSprites;
    UI2DSprite _2DSprite;
    UISprite _Sprite;

    void Awake()
    {
        _2DSprite = GetComponent<UI2DSprite>();
        _Sprite = GetComponent<UISprite>();
    }
    public void Restart()
    {
        if(_2DSprites)
        {
            _2DSprite = GetComponent<UI2DSprite>();
            _2DSprite.alpha = 1.0f;
        }
        else
        {
            _Sprite = GetComponent<UISprite>();
            _Sprite.alpha = 1.0f;
        }
        
        _NowTime = 0.0f;
    }
    void Update()
    {
        _NowTime += Time.smoothDeltaTime;
        if(_2DSprites)
        {
            if (_FadeIn)
                _2DSprite.alpha = _NowTime / _Time;
            else
                _2DSprite.alpha = 1.0f - (_NowTime / _Time);
        }
        else
        {
            if (_FadeIn)
                _Sprite.alpha = _NowTime / _Time;
            else
                _Sprite.alpha = 1.0f - (_NowTime / _Time);
        }
    }
}
