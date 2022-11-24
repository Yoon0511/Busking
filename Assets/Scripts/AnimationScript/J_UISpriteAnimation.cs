using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class J_UISpriteAnimation : MonoBehaviour {

    UISprite _Sprite;
    [SerializeField]
    int _FrameCount;
    [SerializeField]
    string _SpriteName;

    int _NowFrame;

    float _NowTime;
    [SerializeField]
    float _DelayTime;

    bool _Play;
    [SerializeField]
    bool _Loop;
    [SerializeField]
    bool _Snap;

    public void Init(int framecount,string spritename,float delaytime,bool loop,bool snap)
    {
        _Play = true;
        _NowTime = 0.0f;
        _NowFrame = 0;
        _FrameCount = framecount;
        _SpriteName = spritename;
        _DelayTime = delaytime;
        _Loop = loop;
        _Snap = snap;
        _Sprite = GetComponent<UISprite>();
        _Sprite.spriteName = _SpriteName + _NowFrame.ToString();
    }

    void Start()
    {
        _Play = true;
        _NowTime = 0.0f;
        _NowFrame = 0;
        _Sprite = GetComponent<UISprite>();
        _Sprite.spriteName = _SpriteName + _NowFrame.ToString();
    }

    void Update()
    {
        if (!StaticMng.Instance._PauseGame)
        {
            if (_Play)
            {
                _NowTime += Time.smoothDeltaTime;
                if (_NowTime >= _DelayTime)
                {
                    _NowTime -= _DelayTime;
                    ChangeSprite();
                }
            }
        }
    }
    void ChangeSprite()
    {
        _NowFrame++;
        if (_NowFrame >= _FrameCount)
        {
            _NowFrame = 0;
            if (!_Loop)
                _Play = false;
        }
        if (_Play)
            _Sprite.spriteName = _SpriteName + _NowFrame.ToString();
        if (_Snap)
            _Sprite.MakePixelPerfect();
    }

    public int GetNowFrame() { return _NowFrame; }
    public void SetDepth(int num)
    {
        _Sprite.depth = num;
    }
}
