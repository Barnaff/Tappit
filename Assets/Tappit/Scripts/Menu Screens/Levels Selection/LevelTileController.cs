using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelTileController : MonoBehaviour {


    #region Public Properties

    public delegate void LevelTileSelectedDelegate(LevelTileController levelTile);

    public event LevelTileSelectedDelegate OnLevelTileSelected;

    #endregion


    #region Private Properties

    [SerializeField]
    private TextMeshPro _levelLabel;

    [SerializeField]
    private LevelDefenition _levelDefenition;

    [SerializeField]
    private GameObject[] _stars;

    #endregion


    #region Public

    public void SetLevel(LevelDefenition levelDefenition)
    {
        _levelDefenition = levelDefenition;
        _levelLabel.text = _levelDefenition.LevelID.ToString();
    }

    public LevelDefenition LevelDefenition
    {
        get
        {
            return _levelDefenition;
        }
    }

    #endregion


    #region Events

    void OnMouseDown()
    {
        if (OnLevelTileSelected != null)
        {
            OnLevelTileSelected(this);
        }
    }

    #endregion

}
