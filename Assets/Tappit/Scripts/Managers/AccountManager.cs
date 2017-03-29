using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccountManager : Kobapps.Singleton<AccountManager> {

    #region Public Properties

    public delegate void HintsCountUpdatedDelegate();

    public event HintsCountUpdatedDelegate OnHintsCountUpdated;

    #endregion

    #region Private Properties

    [SerializeField]
    private int _lastLevelID = -1;

    [SerializeField]
    private int _topLevel = 0;

    [SerializeField]
    private int _hintsCount = 0;

    [SerializeField]
    private List<int> _hintsUsed;

    [SerializeField]
    private bool _audioToggle;

    [SerializeField]
    private bool _musicToggle;

    private const string LAST_LEVEL_KEY = "lastLevel";
    private const string TOP_LEVEL_KEY = "topLevel";
    private const string HINTS_COUNT_KEY = "hintsCount";
    private const string HINTS_USED_KEY = "hintsUsed";

    private const string AUDIO_TOGGLE_KEY = "audioToggle";
    private const string MUSIC_TOGGLE_KEY = "musicToggleKey";

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

        if (PlayerPrefsUtil.HasKey(HINTS_COUNT_KEY))
        {
            _hintsCount = PlayerPrefsUtil.GetInt(HINTS_COUNT_KEY);
        }

        if (PlayerPrefsUtil.HasKey(HINTS_USED_KEY))
        {
            _hintsUsed = new List<int>();
            string listString = PlayerPrefsUtil.GetString(HINTS_USED_KEY);
            string[] stringArray = listString.Split(","[0]);
            for (int i=0; i< stringArray.Length; i++)
            {
                int levelIdex = 0;
                if (int.TryParse(stringArray[i], out levelIdex))
                {
                    _hintsUsed.Add(levelIdex);
                }
            }
        }
        else
        {
            _hintsUsed = new List<int>();
        }

        if (PlayerPrefsUtil.HasKey(AUDIO_TOGGLE_KEY))
        {
            _audioToggle = PlayerPrefsUtil.GetBool(AUDIO_TOGGLE_KEY);
        }
        else
        {
            _audioToggle = true;
        }

        if (PlayerPrefsUtil.HasKey(MUSIC_TOGGLE_KEY))
        {
            _musicToggle = PlayerPrefsUtil.GetBool(MUSIC_TOGGLE_KEY);
        }
        else
        {
            _musicToggle = true;
        }

        AudioToggle = _audioToggle;
        MusicToggle = _musicToggle;
    }

    #endregion


    #region Public

    public int HintsCount()
    {
        return _hintsCount;
    }

    public bool HasHintForLevel(LevelDefenition level)
    {
        return _hintsUsed.Contains(level.LevelID);
    }

    public bool UseHint(LevelDefenition level)
    {
        if (HasHintForLevel(level))
        {
            return true;
        }

        if (_hintsCount <= 0)
        {
            Debug.LogError("Player have 0 hints!");
            return false;
        }

        _hintsCount--;
        PlayerPrefsUtil.SetInt(HINTS_COUNT_KEY, _hintsCount);

        _hintsUsed.Add(level.LevelID);
        string hintsUsedOutput = "";
        foreach (int levelIndex in _hintsUsed)
        {
            hintsUsedOutput += levelIndex.ToString() + ",";
        }
        PlayerPrefsUtil.SetString(HINTS_USED_KEY, hintsUsedOutput);

        if (OnHintsCountUpdated != null)
        {
            OnHintsCountUpdated();
        }

        return true;
    }

    public void AddHints(int hintsToAdd)
    {
        _hintsCount += hintsToAdd;
        PlayerPrefsUtil.SetInt(HINTS_COUNT_KEY, _hintsCount);

        if (OnHintsCountUpdated != null)
        {
            OnHintsCountUpdated();
        }
    }

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

    public bool AudioToggle
    {
        get
        {
            return _audioToggle;
        }
        set
        {
            _audioToggle = value;
            PlayerPrefsUtil.SetBool(AUDIO_TOGGLE_KEY, _audioToggle);
            SoundManager.MuteSFX(!_audioToggle);
        }
    }

    public bool MusicToggle
    {
        get
        {
            return _musicToggle;
        }
        set
        {
            _musicToggle = value;
            PlayerPrefsUtil.SetBool(MUSIC_TOGGLE_KEY, _musicToggle);
            SoundManager.MuteMusic(!_musicToggle);
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



