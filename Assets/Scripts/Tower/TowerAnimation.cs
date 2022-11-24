using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerAnimation : MonoBehaviour {

     UISprite _MySprite;
     string _TowerName;
     string _TowerState;
     float _NowFrameTime;
     public float _MaxFrameTime;
     int _NowFrame;
     int _MaxFrame;

    public bool _Transforming;
    public bool _NowChange;

    public void Init(string name)
    {
        _TowerName = name;
        _TowerState = "stay";
        _MaxFrameTime = 0.125f;
        _MaxFrame = 1;
    }

    void Awake()
    {
        _MySprite = GetComponent<UISprite>();
    }

    void Update()
    {
        if(!StaticMng.Instance._PauseGame)
        {
            _NowFrameTime += Time.smoothDeltaTime;
            if (_NowFrameTime >= _MaxFrameTime)
            {
                _NowFrameTime -= _MaxFrameTime;
                _NowFrame++;
                if (_NowFrame >= _MaxFrame)
                {
                    if (_Transforming)
                        _Transforming = false;
                    if(_NowChange)
                    {
                        _TowerState = "changestay";
                        _NowFrame = 0;
                        if (_TowerName == "guitar")
                            _MaxFrame = 2;
                        else if (_TowerName == "drum")
                            _MaxFrame = 3;
                        else if (_TowerName == "bass")
                            _MaxFrame = 1;
                        else if (_TowerName == "keyboard")
                            _MaxFrame = 3;
                    }
                    else
                    {
                        _TowerState = "stay";
                        _MaxFrame = 1;
                        _NowFrame = 0;
                    }
                    
                }
                _MySprite.spriteName = "tower_" + _TowerName + "_" + _TowerState + "_" + _NowFrame.ToString();
                _MySprite.MakePixelPerfect();
            }
        }
    }
    public void SetDepth(int num)
    {
        _MySprite.depth = num;
    }
    public void SetAnimation(string state,int maxframe)
    {
        _TowerState = state;
        _MaxFrame = maxframe;
        _NowFrameTime = 0.0f;
        _NowFrame = 0;
    }

    public bool getTransforming() { return _Transforming; }
}
