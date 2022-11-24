using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSetMng : MonoBehaviour {

    const int _ArrayX = 16;
    const int _ArrayY = 9;

    const int _TileSize = 80;

    [SerializeField]
    GameObject _AmpTargetSelectBG;

    public GameObject _ObjectRoot;
    public GameObject _LoadRoot;
    public GameObject _GridObject;
    public GameObject[] _TowerObjectArray = new GameObject[4];

    public GameObject[] _SetPointImage = new GameObject[4];
    int _NowSelectTowerNumber;

    public int[,] _MapTileArray = new int[_ArrayY, _ArrayX];

    bool _WantCreateTower;
    public TowerMng _TowerMng;
    public MapMng _MapMng;

    [SerializeField]
    GameObject _AmpLine;
    

    bool _SetDrumTower;
    [SerializeField]
    GameObject[] _GuitarTower_Gray;
    bool _SetKeyBoardTower;
    [SerializeField]
    GameObject[] _DrumIcon_Gray;
    bool _SetBassTower;
    [SerializeField]
    GameObject[] _BassTower_Gray;
    bool _SetGuitarTower;
    [SerializeField]
    GameObject[] _KeyBoardTower_Gray;
    [SerializeField]
    GameObject[] _AmpTower_Gray;

    [SerializeField]
    GameObject _TowerIconGrid;
    public bool _AmpSelecting;
    Tower_Amp _AmpTemp;
    //int _AmpTempNumber;
    int _AmpTempX;
    int _AmpTempY;

    int _AmpCost;

    //bool _WantDragSetting;

    [SerializeField]
    GameObject[] _TowerSetIconGroups;

    void Start()
    {
        
        if (StaticMng.Instance._GameMode_Infinity)
            _AmpCost = 20;
        else
            _AmpCost = 10;

        if (StaticMng.Instance._TowerSet_Drag)
            _TowerSetIconGroups[0].SetActive(true);
        else
            _TowerSetIconGroups[1].SetActive(true);
    }

    void Update()
    {
        if (StaticMng.Instance._TowerSet_Drag)
        {
            _TowerSetIconGroups[0].SetActive(true);
            _TowerSetIconGroups[1].SetActive(false);
        }
        else
        {
            _TowerSetIconGroups[0].SetActive(false);
            _TowerSetIconGroups[1].SetActive(true);
        }

        bool tempbool = true;

        for(int i=0;i<2;i++)
        {
            if (_SetDrumTower)
                _DrumIcon_Gray[i].SetActive(true);
            if (_SetGuitarTower)
                _GuitarTower_Gray[i].SetActive(true);
            if (_SetBassTower)
                _BassTower_Gray[i].SetActive(true);
            if (_SetKeyBoardTower)
                _KeyBoardTower_Gray[i].SetActive(true);
        }
        
        if (_SetGuitarTower || _SetDrumTower || _SetBassTower || _SetKeyBoardTower)
        {
            tempbool = false;
        }

        if (StageMng.Data._NowEnegy >= _AmpCost && !tempbool)
            tempbool = false;
        else
            tempbool = true;

        _AmpTower_Gray[0].SetActive(tempbool);
        _AmpTower_Gray[1].SetActive(tempbool);


        if (_WantCreateTower)
        {
            if(StaticMng.Instance._TowerSet_Drag)
            {
                Vector2 touchPos = new Vector2((1280.0f / Screen.width) * Input.mousePosition.x, (720.0f / Screen.height) * Input.mousePosition.y);
                //touchPos -= new Vector2(90, 55);
                int X1 = (int)(touchPos.x / _TileSize);
                int Y1 = (int)(touchPos.y / _TileSize);
                if (X1 >= _ArrayX)
                    X1 = _ArrayX - 1;
                if (Y1 >= _ArrayY)
                    Y1 = _ArrayY - 1;
                if (X1 < 0)
                    X1 = 0;
                if (Y1 < 0)
                    Y1 = 0;

                _SetPointImage[_NowSelectTowerNumber].transform.localPosition = new Vector3(X1 * _TileSize + _TileSize / 2, Y1 * _TileSize + _TileSize / 2, 0);



                if (Input.GetMouseButtonUp(0))
                {
                    //touchPos += new Vector2(90, 55);

                    //touchPos -= new Vector2(90, 55);
                    int X = (int)(touchPos.x / _TileSize);
                    int Y = (int)(touchPos.y / _TileSize);
                    if (X >= _ArrayX)
                        X = _ArrayX - 1;
                    if (Y >= _ArrayY)
                        Y = _ArrayY - 1;

                    if (_MapTileArray[Y, X] == 0)
                    {
                        _MapTileArray[Y, X] = 1;
                        //Debug.Log(X.ToString() + " , " + Y.ToString());

                        CreateTower(_NowSelectTowerNumber, X, Y);
                    }
                    else
                    {
                        _TowerIconGrid.SetActive(true);
                        //Debug.Log("fail");
                    }
                    _WantCreateTower = false;
                    _GridObject.SetActive(false);
                    _SetPointImage[_NowSelectTowerNumber].SetActive(false);
                }
            }
            else
            {
                Vector2 touchPos = new Vector2((1280.0f / Screen.width) * Input.mousePosition.x, (720.0f / Screen.height) * Input.mousePosition.y);
                //touchPos -= new Vector2(90, 55);
                //int X1 = (int)(touchPos.x / _TileSize);
                //int Y1 = (int)(touchPos.y / _TileSize);
                //if (X1 >= _ArrayX)
                //    X1 = _ArrayX - 1;
                //if (Y1 >= _ArrayY)
                //    Y1 = _ArrayY - 1;
                //if (X1 < 0)
                //    X1 = 0;
                //if (Y1 < 0)
                //    Y1 = 0;

                //_SetPointImage[_NowSelectTowerNumber].transform.localPosition = new Vector3(X1 * _TileSize + _TileSize / 2, Y1 * _TileSize + _TileSize / 2, 0);



                if (Input.GetMouseButtonDown(0))
                {
                    int X = (int)(touchPos.x / _TileSize);
                    int Y = (int)(touchPos.y / _TileSize);
                    if (X >= _ArrayX)
                        X = _ArrayX - 1;
                    if (Y >= _ArrayY)
                        Y = _ArrayY - 1;

                    if (_MapTileArray[Y, X] == 0)
                    {
                        _MapTileArray[Y, X] = 1;
                        CreateTower(_NowSelectTowerNumber, X, Y);
                    }
                    else
                    {
                        _TowerIconGrid.SetActive(true);
                    }
                    _WantCreateTower = false;
                    _GridObject.SetActive(false);
                }
            }
        }

        if(_AmpSelecting)
        {
            for(int i=0;i<_TowerMng._TowerList.Count;i++)
            {
                if (_TowerMng._TowerList[i]._AmpTarget)
                {
                    SetAmpTowerInit(_TowerMng._TowerList[i]);
                    break;
                }
            }
            for (int i = 0; i < _TowerMng._TowerList.Count; i++)
            {
                if (!_TowerMng._TowerList[i]._Amp)
                    _TowerMng._TowerList[i]._MyAnimation.SetDepth(740+ 730 - (int)(_TowerMng._TowerList[i].transform.localPosition.y));
            }
        }
        else
        {
            for(int i=0;i<_TowerMng._TowerList.Count;i++)
            {
                _TowerMng._TowerList[i]._MyAnimation.SetDepth(730 - (int)(_TowerMng._TowerList[i].transform.localPosition.y));
            }
        }
    }

    void CreateTower(int number, int x, int y)
    {
        if (number == 4)//AMP
        {
            if (_TowerMng._TowerList.Count >= 1)
            {
                for (int i = 0; i < _TowerMng._TowerList.Count; i++)
                {
                    _TowerMng._TowerList[i]._AmpTarget = false;
                }
                GameObject obj = NGUITools.AddChild(_ObjectRoot, _TowerObjectArray[number]);
                obj.transform.localPosition = new Vector3(x * _TileSize + _TileSize / 2, y * _TileSize + _TileSize / 2, 0);// + new Vector3(140, 105, 0);
                obj.GetComponent<Tower>()._Amp = true;
                obj.GetComponent<Tower>()._MyAnimation.Init("amp");

                //float range = Vector2.Distance(obj.transform.localPosition, _TowerMng._TowerList[0].transform.localPosition);
                //int tnum = 0;
                //for (int i = 1; i < _TowerMng._TowerList.Count; i++)
                //{
                //    if (range > Vector2.Distance(obj.transform.localPosition, _TowerMng._TowerList[i].transform.localPosition) && 
                //        !_TowerMng._TowerList[i]._Amp)
                //    {
                //        tnum = i;
                //        range = Vector2.Distance(obj.transform.localPosition, _TowerMng._TowerList[i].transform.localPosition);
                //    }
                //}
                _AmpTemp = obj.GetComponent<Tower_Amp>();

                //_AmpTempNumber = tnum;
                _AmpTempX = x;
                _AmpTempY = y;
                _AmpTargetSelectBG.SetActive(true);
                _AmpSelecting = true;
            }
        }
        else
        {
            float[] Rank_DamagePercent = {75,85,100,115,125 };

            if (number == 0 && !_SetGuitarTower)
            {
                GameObject obj = NGUITools.AddChild(_ObjectRoot, _TowerObjectArray[number]);
                obj.transform.localPosition = new Vector3(x * _TileSize + _TileSize / 2, y * _TileSize + _TileSize / 2, 0);// + new Vector3(140, 105, 0);
                obj.GetComponent<Tower>().Init("guitar", 1, 1.3f, _ObjectRoot, 250, (8.0f + (4.0f * StaticMng.Instance._GuitarTowerLevel))*(Rank_DamagePercent[StaticMng.Instance._GuitarTowerRank-1]/100.0f));
                _SetGuitarTower = true;
                _TowerMng._TowerList.Add(obj.GetComponent<Tower>());
                _MapMng.RedTileSet(x, y);
                StageMng.Data._Towers[0] = obj.GetComponent<Tower>();
                StageMng.Data._TowerSet[0] = true;
            }
            else if (number == 1 && !_SetDrumTower)
            {
                GameObject obj = NGUITools.AddChild(_ObjectRoot, _TowerObjectArray[number]);
                obj.transform.localPosition = new Vector3(x * _TileSize + _TileSize / 2, y * _TileSize + _TileSize / 2, 0);// + new Vector3(140, 105, 0);
                obj.GetComponent<Tower>().Init("drum", 2, 1.4f, (1.0f + (0.5f * StaticMng.Instance._DrumTowerLevel)) * (Rank_DamagePercent[StaticMng.Instance._DrumTowerRank - 1] / 100.0f));
                _SetDrumTower = true;
                _TowerMng._TowerList.Add(obj.GetComponent<Tower>());
                _MapMng.RedTileSet(x, y);
                StageMng.Data._Towers[1] = obj.GetComponent<Tower>();
                StageMng.Data._TowerSet[1] = true;
            }
            else if (number == 2 && !_SetBassTower)
            {
                GameObject obj = NGUITools.AddChild(_ObjectRoot, _TowerObjectArray[number]);
                obj.transform.localPosition = new Vector3(x * _TileSize + _TileSize / 2, y * _TileSize + _TileSize / 2, 0);// + new Vector3(140, 105, 0);
                obj.GetComponent<Tower>().Init("bass",3, 1.5f, _ObjectRoot, 250, (3.0f + (1.5f * StaticMng.Instance._BassTowerLevel)) * (Rank_DamagePercent[StaticMng.Instance._BassTowerRank - 1] / 100.0f));
                _SetBassTower = true;
                _TowerMng._TowerList.Add(obj.GetComponent<Tower>());
                _MapMng.RedTileSet(x, y);
                StageMng.Data._Towers[2] = obj.GetComponent<Tower>();
                StageMng.Data._TowerSet[2] = true;
            }
            else if (number == 3 && !_SetKeyBoardTower)
            {
                GameObject obj = NGUITools.AddChild(_ObjectRoot, _TowerObjectArray[number]);
                obj.transform.localPosition = new Vector3(x * _TileSize + _TileSize / 2, y * _TileSize + _TileSize / 2, 0);// + new Vector3(140, 105, 0);
                obj.GetComponent<Tower>().Init("keyboard",4,2.0f, _ObjectRoot, 250, (4.0f + (2.0f * StaticMng.Instance._KeyBoardTowerLevel)) * (Rank_DamagePercent[StaticMng.Instance._KeyBoardTowerRank - 1] / 100.0f));
                _SetKeyBoardTower = true;
                _TowerMng._TowerList.Add(obj.GetComponent<Tower>());
                _MapMng.RedTileSet(x, y);
                StageMng.Data._Towers[3] = obj.GetComponent<Tower>();
                StageMng.Data._TowerSet[3] = true;
            }


            _TowerIconGrid.SetActive(true);
        }
        
    }
        
    public void SetAmpTowerInit(Tower tower)
    {
        for (int i = 0; i < _TowerMng._TowerList.Count; i++)
        {
            _TowerMng._TowerList[i]._AmpTarget = false;
        }
        _AmpSelecting = false;
        _AmpTargetSelectBG.SetActive(false);
        _AmpTemp.InitAmp( _TowerMng, _MapMng, _AmpTempX, _AmpTempY, _LoadRoot, _AmpLine,_ObjectRoot, tower);
        _TowerIconGrid.SetActive(true);
        StageMng.Data._NowEnegy -= _AmpCost;


    }

    public void WantCreateTower(int num)
    {
        _TowerIconGrid.SetActive(false);
        if (!_AmpSelecting)
        {
            _NowSelectTowerNumber = num;
            _SetPointImage[num].SetActive(true);
            _WantCreateTower = true;
            _GridObject.SetActive(true);

        }
    }
    public void WantCreateTowerTouch(int num)
    {
        _TowerIconGrid.SetActive(false);
        if (!_AmpSelecting)
        {
            _NowSelectTowerNumber = num;
            //_SetPointImage[num].SetActive(true);
            _WantCreateTower = true;
            _GridObject.SetActive(true);
        }
    }
}
