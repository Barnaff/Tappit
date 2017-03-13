﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSetupManager : Kobapps.Singleton<GameSetupManager> {

    #region Private Properties

    [SerializeField]
    private  LevelDefenition _selectedLevel;

    [SerializeField]
    private bool _useHint;

    #endregion


    #region Public

    public LevelDefenition SelectedLevel
    {
        get
        {
            return _selectedLevel;
        }
        set
        {
            _selectedLevel = value;
        }
    }

    public bool UseHint
    {
        get
        {
            return _useHint;
        }
        set
        {
            _useHint = value;
        }
    }

    #endregion

}
