using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class J_RemoveSelf : MonoBehaviour {

    public float _DelayTime;
    float _NowTime;

    void Update()
    {
        if(StaticMng.Instance._StartGame && !StaticMng.Instance._PauseGame)
        {
            _NowTime += Time.smoothDeltaTime;
            if(_NowTime>=_DelayTime)
                Destroy(gameObject);
        }
    }
}
