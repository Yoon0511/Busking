using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfinityRankBlock : MonoBehaviour {
    [SerializeField]
    UILabel _Rank;
    [SerializeField]
    UILabel _UserName;
    [SerializeField]
    UILabel _ScoreLabel;
    [SerializeField]
    UI2DSprite _Table;
    
    [SerializeField]
    UI2DSprite _RankIcon;

    [SerializeField]
    Sprite _MyTable;
    [SerializeField]
    Sprite[] _RankIconImage;

    public void Init(int rank,string username,string score,int idennum)
    {
        if(rank<4)
        {
            _RankIcon.gameObject.SetActive(true);
            _RankIcon.sprite2D = _RankIconImage[rank - 1];
        }
        else
        {
            _Rank.gameObject.SetActive(true);
            _Rank.text = rank.ToString();
        }
        _UserName.text = username;
        _ScoreLabel.text = score;
        if (idennum == StaticMng.Instance._InfinityScoreIdentity)
            _Table.sprite2D = _MyTable;
    }


}
