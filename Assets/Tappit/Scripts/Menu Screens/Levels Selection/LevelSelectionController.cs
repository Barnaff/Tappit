using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

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

    [SerializeField]
    private Button _nextPageButton;

    [SerializeField]
    private Button _previusPageButton;


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
        MenuScreensController.Instance.MainMenu();
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


    #region MenuScreenBaseController Subclass

    public override void Show()
    {
        base.Show();
    }

    #endregion


    #region Private

    private void DisplayEnterAnimation()
    {
        foreach (LevelTileController levelTile in _activeLevelsTiles)
        {
            float delay = (levelTile.LevelDefenition.LevelID - 1) % 4;

            Vector3 endPosition = levelTile.transform.position;
            Vector3 endRotation = levelTile.transform.rotation.eulerAngles;

            levelTile.transform.position = endPosition + new Vector3(Screen.width, 0, 0);

            levelTile.transform.DOMove(endPosition, 1f).SetDelay(delay);
        }
    }

    private void GenerateLevelTiles(LevelTileController.eLevelTileAnimation transition)
    {
        if (_currentChepterIndex == 0)
        {
            _nextPageButton.image.DOFade(1f, 0.5f);
            _previusPageButton.image.DOFade(0f, 0.5f);
        }
        else if (_currentChepterIndex * LevelsSettigs.Instance.LevelsInChepter + LevelsSettigs.Instance.LevelsInChepter >= LevelsSettigs.Instance.Levels.Count)
        {
            _nextPageButton.image.DOFade(0f, 0.5f);
            _previusPageButton.image.DOFade(1f, 0.5f);
        }
        else
        {
            _nextPageButton.image.DOFade(1f, 0.5f);
            _previusPageButton.image.DOFade(1f, 0.5f);
        }

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
