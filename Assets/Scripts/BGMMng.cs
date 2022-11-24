using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMMng : MonoBehaviour {

    AudioSource _Source;

    void Awake()
    {
        _Source = GetComponent<AudioSource>();
    }

    void Update()
    {
        _Source.volume = StaticMng.Instance._Option_Volume;
    }
}
