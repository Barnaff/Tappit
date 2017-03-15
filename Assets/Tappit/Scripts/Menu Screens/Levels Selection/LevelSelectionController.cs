using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectionController : MenuScreenBaseController {

    #region Private Properties

    [SerializeField]
    private LevelTileController _levelTilePrefab;

    [SerializeField]
    private Transform _levelsTilesContent;

    [SerializeField]
    private List<LevelTileController> _activeLevelsTiles;

    [SerializeField]
    private int _currentChepterIndex;

    [SerializeField]
    private Text _chepterTitleLabel;


    #endregion


    #region Initialization

    void Start()
    {
        if (_levelTilePrefab != null)
        {
            _levelTilePrefab.gameObject.SetActive(false);
        }

        int lastPlayedLevel = AccountManager.Instance.LastPlayedLevelID;
        if (lastPlayedLevel > 0)
        {
            _currentChepterIndex = lastPlayedLevel / LevelsSettigs.Instance.LevelsInChepter;
        }

        GenerateLevelTiles(LevelTileController.eLevelTileAnimation.None);
    }

    #endregion


    #region Public

    public void BackButtonAction()
    {
        
    }

    public void PreviewsChepterButtonAction()
    {
        if (_currentChepterIndex > 0)
        {
            _currentChepterIndex--;
            GenerateLevelTiles(LevelTileController.eLevelTileAnimation.Back);
        }
    }

    public void NextChepterButtonAction()
    {
        if (LevelsSettigs.Instance.Levels.Count > _currentChepterIndex * LevelsSettigs.Instance.LevelsInChepter + LevelsSettigs.Instance.LevelsInChepter)
        {
            _currentChepterIndex++;
            GenerateLevelTiles(LevelTileController.eLevelTileAnimation.Next);
        }
    }

    #endregion


    #region Private

    private void GenerateLevelTiles(LevelTileController.eLevelTileAnimation transition)
    {
        if (_activeLevelsTiles.Count == 0)
        {
            for (int i = 0; i < LevelsSettigs.Instance.LevelsInChepter; i++)
            {
                int levelIndex = _currentChepterIndex * LevelsSettigs.Instance.LevelsInChepter + i;
                LevelTileController tile = CreateTile(LevelsSettigs.Instance.Levels[levelIndex], transition);
                _activeLevelsTiles.Add(tile);
            }
        }
        else
        {
            for (int i = 0; i < LevelsSettigs.Instance.LevelsInChepter; i++)
            {
                LevelTileController tileController = _activeLevelsTiles[i];

                int levelIndex = _currentChepterIndex * LevelsSettigs.Instance.LevelsInChepter + i;
                tileController.SetLevel(LevelsSettigs.Instance.Levels[levelIndex], transition);
            }
        }

    }

	private LevelTileController CreateTile(LevelDefenition levelDefenition, LevelTileController.eLevelTileAnimation transition)
	{
		LevelTileController newTile = Instantiate(_levelTilePrefab);
		newTile.gameObject.SetActive(true);
		newTile.transform.SetParent(_levelsTilesContent);
		newTile.transform.localScale = Vector3.one;

        if (transition == LevelTileController.eLevelTileAnimation.None && _currentChepterIndex % 2 != 0)
        {
            transition = LevelTileController.eLevelTileAnimation.NoneFlipped;
        }
		newTile.SetLevel(levelDefenition, transition);
		newTile.OnLevelTileSelected += LevelTileSelectedHandler;
		return newTile;
	}

    private void LevelTileSelectedHandler(LevelTileController levelTileController)
    {
        bool isUnlocked = AccountManager.Instance.IsLevelUnlocked(levelTileController.LevelDefenition);
        if (isUnlocked)
        {
            FlowManager.Instance.StartLevel(levelTileController.LevelDefenition);
        }
    }

    #endregion
}
