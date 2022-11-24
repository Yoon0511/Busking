using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterLineData : MonoBehaviour {

    #region Chapter1
    public List<GameObject> _Chapter_1_1 = new List<GameObject>();
    public List<GameObject> _Chapter_1_2 = new List<GameObject>();
    public List<GameObject> _Chapter_1_3 = new List<GameObject>();
    public List<GameObject> _Chapter_1_4 = new List<GameObject>();
    public List<GameObject> _Chapter_1_5 = new List<GameObject>();
    public List<GameObject> _Chapter_1_6 = new List<GameObject>();
    public List<GameObject> _Chapter_1_7 = new List<GameObject>();
    public List<GameObject> _Chapter_1_8 = new List<GameObject>();
    public List<GameObject> _Chapter_1_9 = new List<GameObject>();
    public List<GameObject> _Chapter_1_10 = new List<GameObject>();
    #endregion

    #region Chapter2
    public List<GameObject> _Chapter_2_1 = new List<GameObject>();
    public List<GameObject> _Chapter_2_2 = new List<GameObject>();
    public List<GameObject> _Chapter_2_3 = new List<GameObject>();
    public List<GameObject> _Chapter_2_4 = new List<GameObject>();
    public List<GameObject> _Chapter_2_5 = new List<GameObject>();
    public List<GameObject> _Chapter_2_6 = new List<GameObject>();
    public List<GameObject> _Chapter_2_7 = new List<GameObject>();
    public List<GameObject> _Chapter_2_8 = new List<GameObject>();
    public List<GameObject> _Chapter_2_9 = new List<GameObject>();
    public List<GameObject> _Chapter_2_10 = new List<GameObject>();
    #endregion

    #region Chapter3
    public List<GameObject> _Chapter_3_1 = new List<GameObject>();
    public List<GameObject> _Chapter_3_2 = new List<GameObject>();
    public List<GameObject> _Chapter_3_3 = new List<GameObject>();
    public List<GameObject> _Chapter_3_4 = new List<GameObject>();
    public List<GameObject> _Chapter_3_5 = new List<GameObject>();
    public List<GameObject> _Chapter_3_6 = new List<GameObject>();
    public List<GameObject> _Chapter_3_7 = new List<GameObject>();
    public List<GameObject> _Chapter_3_8 = new List<GameObject>();
    public List<GameObject> _Chapter_3_9 = new List<GameObject>();
    public List<GameObject> _Chapter_3_10 = new List<GameObject>();
    #endregion

    #region Chapter4
    public List<GameObject> _Chapter_4_1 = new List<GameObject>();
    public List<GameObject> _Chapter_4_2 = new List<GameObject>();
    public List<GameObject> _Chapter_4_3 = new List<GameObject>();
    public List<GameObject> _Chapter_4_4 = new List<GameObject>();
    public List<GameObject> _Chapter_4_5 = new List<GameObject>();
    public List<GameObject> _Chapter_4_6 = new List<GameObject>();
    public List<GameObject> _Chapter_4_7 = new List<GameObject>();
    public List<GameObject> _Chapter_4_8 = new List<GameObject>();
    public List<GameObject> _Chapter_4_9 = new List<GameObject>();
    public List<GameObject> _Chapter_4_10 = new List<GameObject>();
    #endregion

    public List<GameObject> _InfinityModeMap = new List<GameObject>();

    List<List<GameObject>> _Chapter_1 = new List<List<GameObject>>();
    List<List<GameObject>> _Chapter_2 = new List<List<GameObject>>();
    List<List<GameObject>> _Chapter_3 = new List<List<GameObject>>();
    List<List<GameObject>> _Chapter_4 = new List<List<GameObject>>();
    

    public List<List<List<GameObject>>> _MoveLinePosition = new List<List<List<GameObject>>>();
    

    void Awake()
    {
        #region Trash
        _Chapter_1.Add(_Chapter_1_1);
        _Chapter_1.Add(_Chapter_1_2);
        _Chapter_1.Add(_Chapter_1_3);
        _Chapter_1.Add(_Chapter_1_4);
        _Chapter_1.Add(_Chapter_1_5);
        _Chapter_1.Add(_Chapter_1_6);
        _Chapter_1.Add(_Chapter_1_7);
        _Chapter_1.Add(_Chapter_1_8);
        _Chapter_1.Add(_Chapter_1_9);
        _Chapter_1.Add(_Chapter_1_10);

        _Chapter_2.Add(_Chapter_2_1);
        _Chapter_2.Add(_Chapter_2_2);
        _Chapter_2.Add(_Chapter_2_3);
        _Chapter_2.Add(_Chapter_2_4);
        _Chapter_2.Add(_Chapter_2_5);
        _Chapter_2.Add(_Chapter_2_6);
        _Chapter_2.Add(_Chapter_2_7);
        _Chapter_2.Add(_Chapter_2_8);
        _Chapter_2.Add(_Chapter_2_9);
        _Chapter_2.Add(_Chapter_2_10);

        _Chapter_3.Add(_Chapter_3_1);
        _Chapter_3.Add(_Chapter_3_2);
        _Chapter_3.Add(_Chapter_3_3);
        _Chapter_3.Add(_Chapter_3_4);
        _Chapter_3.Add(_Chapter_3_5);
        _Chapter_3.Add(_Chapter_3_6);
        _Chapter_3.Add(_Chapter_3_7);
        _Chapter_3.Add(_Chapter_3_8);
        _Chapter_3.Add(_Chapter_3_9);
        _Chapter_3.Add(_Chapter_3_10);

        _Chapter_4.Add(_Chapter_4_1);
        _Chapter_4.Add(_Chapter_4_2);
        _Chapter_4.Add(_Chapter_4_3);
        _Chapter_4.Add(_Chapter_4_4);
        _Chapter_4.Add(_Chapter_4_5);
        _Chapter_4.Add(_Chapter_4_6);
        _Chapter_4.Add(_Chapter_4_7);
        _Chapter_4.Add(_Chapter_4_8);
        _Chapter_4.Add(_Chapter_4_9);
        _Chapter_4.Add(_Chapter_4_10);
        #endregion


        _MoveLinePosition.Add(_Chapter_1);
        _MoveLinePosition.Add(_Chapter_2);
        _MoveLinePosition.Add(_Chapter_3);
        _MoveLinePosition.Add(_Chapter_4);
    }
}
