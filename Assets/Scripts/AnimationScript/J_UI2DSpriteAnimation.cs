using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class J_UI2DSpriteAnimation : MonoBehaviour {
    UI2DSprite _MySprite;
    [SerializeField]
    Sprite[] _Sprites;
    [SerializeField]
    bool _Loop;
    [SerializeField]
    bool _Snap;
    [SerializeField]
    bool _OnePlayEffect;

    int _NowFrame;
    bool _Play;

    float _NowTime;
    public float _DelayTime;
    [SerializeField]
    bool _ObjectPooler;

    void Start()
    {
        
        _Play = true;
        _MySprite = GetComponent<UI2DSprite>();
        _NowFrame = 0;
        _MySprite.sprite2D = _Sprites[_NowFrame];
    }
    public void ReStart()
    {
        if (_MySprite == null)
            _MySprite = GetComponent<UI2DSprite>();
        _Play = true;
        _NowFrame = 0;
        _NowTime = 0.0f;
        _MySprite.sprite2D = _Sprites[_NowFrame];
        gameObject.SetActive(true);
    }

    void Update()
    {
        if(!StaticMng.Instance._PauseGame)
        {
            _NowTime += Time.smoothDeltaTime;

            if (_NowTime >= _DelayTime)
            {
                _NowTime -= _DelayTime;
                ChangeSprite();
            }
        }
    }
    void ChangeSprite()
    {
        _NowFrame++;
        if (_NowFrame >= _Sprites.Length)
        {
            _NowFrame = 0;
            if (!_Loop && _OnePlayEffect)
            {
                if (_ObjectPooler)
                    gameObject.SetActive(false);
                else
                    Destroy(gameObject);
            }
            if (!_Loop)
                _Play = false;
        }
        if (_Play)
            _MySprite.sprite2D = _Sprites[_NowFrame];
        if (_Snap)
            _MySprite.MakePixelPerfect();

       
    }
}
