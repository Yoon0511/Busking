using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadingMng : MonoBehaviour {

    AsyncOperation async_operation;
    [SerializeField]
    UI2DSprite[] _LoadingBar;
    [SerializeField]
    GameObject _UIRoot;
    [SerializeField]
    GameObject _FadeIn;

    bool _Finish;

    void Start()
    {
        LoadingPage();

    }
    void Update()
    {
        //if(!_Finish && async_operation!=null)
        //{
        //    //_LoadingBar[0].fillAmount += Time.smoothDeltaTime / 3.0f;
        //    //_LoadingBar[1].fillAmount += Time.smoothDeltaTime / 3.0f;
        //    if (async_operation.isDone)
        //    {
        //        _Finish = true;
        //        GameObject obj = NGUITools.AddChild(_UIRoot, _FadeIn);
        //        StartCoroutine(SceneChange());
        //    }
        //}
        if(_Finish)
        {
            _LoadingBar[0].fillAmount += Time.smoothDeltaTime;
            _LoadingBar[1].fillAmount += Time.smoothDeltaTime;
        }
            
    }

    public void LoadingPage()
    {
        StartCoroutine(LoadingProgress());
        StartCoroutine(BarFill());
    }
    IEnumerator SceneChange()
    {
        yield return new WaitForSeconds(1.5f);

        SceneManager.LoadScene(StaticMng.Instance._NowWantScene);
    }
    IEnumerator BarFill()
    {
        yield return new WaitForSeconds(1.6f);
        
        while(true)
        {
            
            if (!_Finish)
            {
                _LoadingBar[0].fillAmount = async_operation.progress;
                _LoadingBar[1].fillAmount = async_operation.progress;

                //_LoadingBar[0].fillAmount += Time.smoothDeltaTime / 3.0f;
                //_LoadingBar[1].fillAmount += Time.smoothDeltaTime / 3.0f;
                if (async_operation.progress>=0.9f)
                {
                    _Finish = true;
                    GameObject obj = NGUITools.AddChild(_UIRoot, _FadeIn);
                    StartCoroutine(SceneChange()); 
                }
            }
            yield return null;
        }
    }

    IEnumerator LoadingProgress()
    {
        yield return new WaitForSeconds(1.5f);
        
        async_operation = SceneManager.LoadSceneAsync(StaticMng.Instance._NowWantScene,LoadSceneMode.Single);
        async_operation.allowSceneActivation = false;
    }
}
