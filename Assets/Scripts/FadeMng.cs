using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeMng : MonoBehaviour {

    [SerializeField]
    GameObject _FadeRoot;
    [SerializeField]
    GameObject _FadeIn;

    [SerializeField]
    bool _AutoFadeIn;
    [SerializeField]
    float _AutoFadeTime;
    public string _NextSceneName;

    [SerializeField]
    DataSaveMng _DataSaveMng;

    public void Init()
    {
        if (_AutoFadeIn)
            StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        yield return new WaitForSeconds(_AutoFadeTime);

        GameObject obj = NGUITools.AddChild(_FadeRoot, _FadeIn);
        StartCoroutine(SceneChange());

    }
    IEnumerator SceneChange()
    {
        yield return new WaitForSeconds(1.5f);
        while (_DataSaveMng.GetDataSending())
            yield return null;
        SceneManager.LoadScene(_NextSceneName);
    }
}
