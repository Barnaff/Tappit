using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccountManager : Kobapps.Singleton<AccountManager> {

    #region Private Properties

    [SerializeField]
    private string _lastLevel = null;

    private const string LAST_LEVEL_KEY = "lastLevel";

    #endregion


    #region Initializzation

    public void Init()
    {
        
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
        string levelKey = GetLevelKey(level);
        _lastLevel = levelKey;
        PlayerPrefsUtil.SetString(LAST_LEVEL_KEY, _lastLevel);
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



