using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccountManager : Kobapps.Singleton<AccountManager> {

    #region Private Properties

    [SerializeField]
    private int _lastLevelID = -1;

    [SerializeField]
    private int _topLevel = 0;

    private const string LAST_LEVEL_KEY = "lastLevel";
    private const string TOP_LEVEL_KEY = "topLevel";

    #endregion


    #region Initializzation

    public void Init()
    {
        if (PlayerPrefsUtil.HasKey(LAST_LEVEL_KEY))
        {
            _lastLevelID = PlayerPrefsUtil.GetInt(LAST_LEVEL_KEY);
        }

        if (PlayerPrefsUtil.HasKey(TOP_LEVEL_KEY))
        {
            _topLevel = PlayerPrefsUtil.GetInt(TOP_LEVEL_KEY);
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

    public bool IsLevelUnlocked(LevelDefenition level)
    {
        return level.LevelID <= _topLevel + 1;
    }

    public void UpdateLevelStars(LevelDefenition level, int stars)
    {
        string levelKey = GetLevelKey(level);
        int currentStars = 0;
        if (PlayerPrefsUtil.HasKey(levelKey))
        {
            currentStars = PlayerPrefsUtil.GetInt(levelKey);
        }
        if (stars > currentStars)
        {
            PlayerPrefsUtil.SetInt(levelKey, stars);
        }
    }

    public void FinishedLevel(LevelDefenition level)
    {
        _lastLevelID = level.LevelID;
        PlayerPrefsUtil.SetInt(LAST_LEVEL_KEY, _lastLevelID);
        if (level.LevelID > _topLevel)
        {
            _topLevel = level.LevelID;
            PlayerPrefsUtil.SetInt(TOP_LEVEL_KEY, _topLevel);
        }
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



