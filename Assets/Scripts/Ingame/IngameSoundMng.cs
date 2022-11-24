using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngameSoundMng : MonoBehaviour {
    

    public AudioSource[] _InstrumentMusicList_1;
    public AudioSource[] _InstrumentMusicList_2;
    public AudioSource[] _InstrumentMusicList_3;
    public AudioSource[] _InstrumentMusicList_4;

    bool _GameMode;
    int _NowBGMNumber;

    List<AudioSource[]> _MusicList =new List<AudioSource[]>();
    public void Awake()
    {
        _GameMode = StaticMng.Instance._GameMode_Infinity;
        _MusicList.Add(_InstrumentMusicList_1);
        _MusicList.Add(_InstrumentMusicList_2);
        _MusicList.Add(_InstrumentMusicList_3);
        _MusicList.Add(_InstrumentMusicList_4);

        for(int i=0;i<4;i++)
        {
            for (int j = 0; j < 4; j++)
            {
                _MusicList[j][i].volume = 0.0f;
                if (_GameMode)
                    _MusicList[j][i].loop = true;
                else
                    _MusicList[j][i].loop = true;
            }
        }
    }

    void Update()
    {
        if(_GameMode)
        {
            for (int i = 0; i < 5; i++)
            {
                if (i != 4)
                {
                    //StageMng.Data._TowerSet[3] = true;//temp
                    float volume = 0.0f;
                    if (StageMng.Data._TowerSet[i])
                        volume = 1.0f;
                    _MusicList[StaticMng.Instance._NowInfinityBgNumber][i].volume = Mathf.Lerp(_MusicList[StaticMng.Instance._NowInfinityBgNumber][i].volume, volume, Time.smoothDeltaTime / 10);
                }
                else
                    _MusicList[StaticMng.Instance._NowInfinityBgNumber][i].volume = 0.4f;


                if (!StaticMng.Instance._Option_Volume_Bool)
                    _MusicList[StaticMng.Instance._NowInfinityBgNumber][i].mute = true;
                else
                    _MusicList[StaticMng.Instance._NowInfinityBgNumber][i].mute = false;
            }
        }
        else
        {
            for (int i = 0; i < 5; i++)
            {
                if (i != 4)
                {
                    //StageMng.Data._TowerSet[3] = true;//temp
                    float volume = 0.0f;
                    if (StageMng.Data._TowerSet[i])
                        volume = 1.0f;
                    _MusicList[StaticMng.Instance._Stage_Chapter - 1][i].volume = Mathf.Lerp(_MusicList[StaticMng.Instance._Stage_Chapter - 1][i].volume, volume, Time.smoothDeltaTime / 10);
                }
                else
                    _MusicList[StaticMng.Instance._Stage_Chapter - 1][i].volume = 0.4f;


                if (!StaticMng.Instance._Option_Volume_Bool)
                    _MusicList[StaticMng.Instance._Stage_Chapter - 1][i].mute = true;
                else
                    _MusicList[StaticMng.Instance._Stage_Chapter - 1][i].mute = false;
            }
        }
        
    }
    public void PlayBgm()
    {
        if(_GameMode)
        {
            for (int i = 0; i < 5; i++)
                _MusicList[StaticMng.Instance._NowInfinityBgNumber][i].Play();
        }
        else
        {
            for (int i = 0; i < 5; i++)
                _MusicList[StaticMng.Instance._Stage_Chapter - 1][i].Play();
        }
        
    }
    public void PauseBgm()
    {
        if (_GameMode)
        {
            for (int i = 0; i < 5; i++)
                _MusicList[StaticMng.Instance._NowInfinityBgNumber][i].Pause();
        }
        else
        {
            for (int i = 0; i < 5; i++)
                _MusicList[StaticMng.Instance._Stage_Chapter - 1][i].Pause();
        }
    }
}
