using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyBoardLazerAnimation : MonoBehaviour {

    [SerializeField]
    float _DelayTime;
    [SerializeField]
    float _DuringTime;

    float _NowTime;

    bool _Start;

    void Update()
    {
        if (StaticMng.Instance._StartGame && StaticMng.Instance._PauseGame)
            return;

        _NowTime += Time.smoothDeltaTime;
        if (_NowTime >= _DelayTime)
            _Start = true;
        if (_Start)
            transform.localScale = new Vector3(1,Mathf.MoveTowards(transform.localScale.y,0,Time.smoothDeltaTime*(1.0f/_DuringTime)),0);
    }
}
