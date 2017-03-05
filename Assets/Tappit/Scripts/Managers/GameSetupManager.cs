using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSetupManager : Kobapps.Singleton<GameSetupManager> {

    #region Private Properties

    [SerializeField]
    private  LevelDefenition _selectedLevel;

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

    #endregion

}
