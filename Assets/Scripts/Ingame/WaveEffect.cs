using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveEffect : MonoBehaviour {

    [SerializeField]
    UILabel _WaveNumberLabel;
    [SerializeField]
    UI2DSprite _WaveText;

    float _Timer;

    public void Init(int num)
    {
        _WaveNumberLabel.text = num.ToString();
    }
    void Update()
    {
        _Timer += Time.smoothDeltaTime;
        if(_Timer>=4.0f)
        {
            Destroy(gameObject);
        }
        else if(_Timer>=3.0f)
        {
            _WaveNumberLabel.alpha -= Time.smoothDeltaTime * 2;
            _WaveText.alpha -= Time.smoothDeltaTime * 2;
        }
        else if (_Timer >= 1.0f)
        {
            _WaveNumberLabel.gameObject.SetActive(true);
            _WaveNumberLabel.alpha += Time.smoothDeltaTime * 2;
            if (_WaveNumberLabel.alpha >= 1.0f)
                _WaveNumberLabel.alpha = 1.0f;
        }
    }
}
