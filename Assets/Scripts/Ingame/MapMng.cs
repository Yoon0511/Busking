using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MapMng : MonoBehaviour {

    [SerializeField]
    GameObject _LoadTile;
    public TowerSetMng _TowerSetMng;

    [SerializeField]
    GameObject[] _ChapterBGArray;

    [SerializeField]
    GameObject[] _InfinityBGArray;

    [SerializeField]
    GameObject _Chapter2_Dark;
    [SerializeField]
    GameObject _Chapter4_Light;

    [SerializeField]
    List<GameObject> _StageLine_1 = new List<GameObject>();
    [SerializeField]
    List<GameObject> _StageLine_2 = new List<GameObject>();
    [SerializeField]
    List<GameObject> _StageLine_3 = new List<GameObject>();
    [SerializeField]
    List<GameObject> _StageLine_4 = new List<GameObject>();

    List<List<GameObject>> _StageLineList = new List<List<GameObject>>();

    bool _GameMode;

    void Start()
    {
        _GameMode = StaticMng.Instance._GameMode_Infinity;
        if(_GameMode)
        {
            int stage = Random.Range(0, 4);
            _InfinityBGArray[stage].SetActive(true);

            List<string> linedata = new List<string>();
            TextAsset file = Resources.Load<TextAsset>("stage_infinity");//temp
            StreamReader sr = new StreamReader(new MemoryStream(file.bytes));
            while (sr.Peek() >= 0)
                linedata.Add(sr.ReadLine());
            for (int i = 0; i < 4; i++)
            {
                string temp = linedata[i];
                linedata[i] = linedata[8 - i];
                linedata[8 - i] = temp;
            }

            int[,] maparr = new int[9, 16];
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 16; j++)
                {
                    maparr[i, j] = linedata[i][j] - 48;
                    if (maparr[i, j] == 1)
                        RedTileSet(j, i);
                    //Debug.Log(maparr[i, j]);
                }
            }
            _TowerSetMng._MapTileArray = maparr;

            if (stage == 1)
                _Chapter2_Dark.SetActive(true);
            if (stage == 3)
                _Chapter4_Light.SetActive(true);
            StaticMng.Instance._NowInfinityBgNumber = stage;
        }
        else
        {
            _StageLineList.Add(_StageLine_1);
            _StageLineList.Add(_StageLine_2);
            _StageLineList.Add(_StageLine_3);
            _StageLineList.Add(_StageLine_4);
            _ChapterBGArray[StaticMng.Instance._Stage_Chapter - 1].SetActive(true);

            List<string> linedata = new List<string>();
            
            int stage = StaticMng.Instance._Stage_Chapter;

            TextAsset file = Resources.Load<TextAsset>("stage" + stage.ToString() + "_" + StaticMng.Instance._Stage_Sector.ToString());//temp
            StreamReader sr = new StreamReader(new MemoryStream(file.bytes));
            while (sr.Peek() >= 0)
                linedata.Add(sr.ReadLine());
            for (int i = 0; i < 4; i++)
            {
                string temp = linedata[i];
                linedata[i] = linedata[8 - i];
                linedata[8 - i] = temp;
            }

            int[,] maparr = new int[9, 16];
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 16; j++)
                {
                    maparr[i, j] = linedata[i][j] - 48;
                    if (maparr[i, j] == 1)
                        RedTileSet(j, i);
                    //Debug.Log(maparr[i, j]);
                }
            }
            _TowerSetMng._MapTileArray = maparr;
            _StageLineList[StaticMng.Instance._Stage_Chapter - 1][StaticMng.Instance._Stage_Sector - 1].SetActive(true);

            if (StaticMng.Instance._Stage_Chapter == 2)
                _Chapter2_Dark.SetActive(true);
            if (StaticMng.Instance._Stage_Chapter == 4)
                _Chapter4_Light.SetActive(true);
        }
    }

    void Update()
    {
        
    }

    public void RedTileSet(int x,int y)
    {
        GameObject obj = NGUITools.AddChild(_TowerSetMng._GridObject, _LoadTile);
        obj.transform.localPosition = new Vector3(x * 80, ((y+1) * 80) - 720, 0);
        _TowerSetMng._MapTileArray[y, x] = -1;
    }
}