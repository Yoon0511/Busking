using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterGenerateMng : MonoBehaviour {

    bool _Generating;

    [SerializeField]
    GameObject _MonsterRoot;

    //[SerializeField]
    //GameObject _Monster;
    
    List<GameObject> _MonsterLinePos;

    [SerializeField]
    MonsterLineData _MonsterLineData;

    [SerializeField]
    UILabel _WaveLabel;

    List<Vector2> _MonsterLinePos_Value = new List<Vector2>();
    

    float _NowTime;
    float _DelayTime;
    int _NowCreateMonsterCount;
    int _MaxCreateMonsterCount;

    int _ForWaveCount;
    int _NowWave;

    bool _GameMode;

    int _RandomMonster_Infi;
    
    void Start()
    {
        StaticMng.Instance._Tutorialing = true;
        _Generating = true;
        _NowWave = 1;
        _ForWaveCount = 0;

        //Infinity

        _GameMode = StaticMng.Instance._GameMode_Infinity;
        if (_GameMode)
        {
            _DelayTime = 1;
            _RandomMonster_Infi = Random.Range(0, 6);
            StaticMng.Instance._MonsterCount = 0;
            for (int i = 0; i < _MonsterLineData._InfinityModeMap.Count; i++)
                _MonsterLinePos_Value.Add(_MonsterLineData._InfinityModeMap[i].transform.localPosition + new Vector3(40, 0));
            TutorialMng_IG.Data.CheckTutorialClear(1);
        }
        else
        {
            _MaxCreateMonsterCount = 70;
            _MonsterLinePos = _MonsterLineData._MoveLinePosition[StaticMng.Instance._Stage_Chapter - 1][StaticMng.Instance._Stage_Sector - 1];//temp
            for (int i = 0; i < _MonsterLinePos.Count; i++)
                _MonsterLinePos_Value.Add(_MonsterLinePos[i].transform.localPosition + new Vector3(40, 0));
            TutorialMng_IG.Data.CheckTutorialClear(0);
        }
        

        _NowCreateMonsterCount = 0;
        
    }

    void Update()
    {
        if (_GameMode)
            _WaveLabel.text = _NowWave.ToString() + " Wave";
        else
            _WaveLabel.text = _NowWave.ToString() + "/10";
        if (_Generating && StaticMng.Instance._StartGame && !StaticMng.Instance._PauseGame && !StaticMng.Instance._Tutorialing)
        {
            _NowTime += Time.smoothDeltaTime;
            if(_GameMode)
            {
                if (_NowTime >= _DelayTime)
                {
                    _NowTime -= _DelayTime;
                    _DelayTime = MonsterCreateInfo._Infinity_DelayTime[_ForWaveCount];
                    if (_NowWave % 5 == 0)
                    {
                        CreateMonster(MonsterCreateInfo._Infinity_Number_Boss[_RandomMonster_Infi,_ForWaveCount]);
                    }
                    else
                        CreateMonster(MonsterCreateInfo._Infinity_Number[_RandomMonster_Infi, _ForWaveCount]);
                    
                    _ForWaveCount++;
                    if (_ForWaveCount >= 7 )
                    {
                        _RandomMonster_Infi = Random.Range(0, 2);
                        _NowWave++;
                        if (_NowWave % 5 == 0)
                            StageMng.Data._TowerSkillMng.CanUseSkillSet();
                        _ForWaveCount -= 7;
                        StageMng.Data.WaveStartAnimation(_NowWave);
                    }
                }
            }
            else
            {
                if (_NowTime >= MonsterCreateInfo._MonsterCreateTime[_NowCreateMonsterCount])
                {
                    if (MonsterCreateInfo._MonsterCreateNumber[StaticMng.Instance._Stage_Chapter - 1, StaticMng.Instance._Stage_Sector - 1, _NowCreateMonsterCount] != 0)
                        CreateMonster(MonsterCreateInfo._MonsterCreateNumber[StaticMng.Instance._Stage_Chapter - 1, StaticMng.Instance._Stage_Sector - 1, _NowCreateMonsterCount]);
                    
                    _NowCreateMonsterCount++;
                    _ForWaveCount++;
                    if (_ForWaveCount >= 7 && _NowWave != 10)
                    {
                        _NowWave++;
                        if (_NowWave == 5 || _NowWave == 10)
                            StageMng.Data._TowerSkillMng.CanUseSkillSet();
                        _ForWaveCount -= 7;
                        StageMng.Data.WaveStartAnimation(_NowWave);
                    }

                    if (_NowCreateMonsterCount >= _MaxCreateMonsterCount)
                        _Generating = false;
                }
            }
        }
    }
    
    void CreateMonster(int num)
    {
        if(_GameMode)//Infinity
        {
            float hp = ((float)(_NowWave*1.5f / 10.0f) * (MonsterCreateInfo._MonsterHPInfo[3,9]));
            if (num == 4 || num == 5)//boss
                hp *= 5;
            //GameObject obj = NGUITools.AddChild(_MonsterRoot, _Monster);
            while(ObjectPoolingMng.Data._Monster[ObjectPoolingMng.Data._Monster_Count].activeSelf)
                ObjectPoolingMng.Data.CountUp_Monster();
            GameObject obj = ObjectPoolingMng.Data._Monster[ObjectPoolingMng.Data._Monster_Count];
            obj.SetActive(true);
            ObjectPoolingMng.Data.CountUp_Monster();
            obj.GetComponent<Monster>().Init(hp, 100.0f, _MonsterLinePos_Value, num);
            StageMng.Data._MonsterList.Add(obj.GetComponent<Monster>());
            StaticMng.Instance._MonsterCount++;
        }
        else
        {
            float hp = ((float)(_NowWave / 10.0f) * (MonsterCreateInfo._MonsterHPInfo[StaticMng.Instance._Stage_Chapter - 1, StaticMng.Instance._Stage_Sector - 1] / 2.0f)) + (MonsterCreateInfo._MonsterHPInfo[StaticMng.Instance._Stage_Chapter - 1, StaticMng.Instance._Stage_Sector - 1] / 2.0f);
            if (num == 4 || num == 5)//boss
                hp *= 3;
            //GameObject obj = NGUITools.AddChild(_MonsterRoot, _Monster);
            GameObject obj = ObjectPoolingMng.Data._Monster[ObjectPoolingMng.Data._Monster_Count];
            obj.SetActive(true);
            ObjectPoolingMng.Data.CountUp_Monster();
            obj.GetComponent<Monster>().Init(hp, 100.0f, _MonsterLinePos_Value, num);
            StageMng.Data._MonsterList.Add(obj.GetComponent<Monster>());
        }
    }

    public bool getGenerating()
    {
        return _Generating;
    }
}
