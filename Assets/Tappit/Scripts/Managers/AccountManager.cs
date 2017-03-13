using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccountManager : Kobapps.Singleton<AccountManager> {

    #region Private Properties

    [SerializeField]
    private int _lastLevelID = -1;

    private const string LAST_LEVEL_KEY = "lastLevel";

    #endregion


    #region Initializzation

    public void Init()
    {
        if (PlayerPrefsUtil.HasKey(LAST_LEVEL_KEY))
        {
            _lastLevelID = PlayerPrefsUtil.GetInt(LAST_LEVEL_KEY);
        }
    }

    #endregion


    #region Public

    public int StarsForLevel(LevelDefenition level)
    {
        string levelKey = GetLevelKey(level);

        if (PlayerPrefsUtil.HasKey(levelKey))
        {
            return PlayerPrefsUtil.GetInt(levelKey);
        }
      
        return 0;
    }

    public void UpdateLevelStars(LevelDefenition level, int stars)
    {
        string levelKey = GetLevelKey(level);

        PlayerPrefsUtil.SetInt(levelKey, stars);
    }

    private void FinishedLevel(LevelDefenition level)
    {
        _lastLevelID = level.LevelID;
        PlayerPrefsUtil.SetInt(LAST_LEVEL_KEY, _lastLevelID);
    }

    public int LastPlayedLevelID
    {
        get
        {
            return _lastLevelID;
        }
        set
        {
            if (value != _lastLevelID)
            {
                PlayerPrefsUtil.SetInt(LAST_LEVEL_KEY, value);
            }
            _lastLevelID = value;
        }
    }
   
    #endregion


    #region Private

    private string GetLevelKey(LevelDefenition level)
    {
        string levelKey = level.ChecpterID + "-" + level.LevelID;
        return levelKey;
    }

    
    #endregion


}



