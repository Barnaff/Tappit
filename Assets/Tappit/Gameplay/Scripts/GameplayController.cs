using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameplayController : MonoBehaviour {

    #region Private Properties

    [SerializeField]
    private LevelDefenition _selectedLevel;

    [SerializeField]
    private BoardController _boardController;

	[SerializeField]
	private GameplayUIController _gameplayUI;

    [SerializeField]
    private int _movesCount = 0;

    [SerializeField]
    private bool _levelCompleted = false;

    [SerializeField]
    private bool _useHints = false;

    [SerializeField]
    private GameObject _hintPrefab;

    private GameObject _hintIndicator = null;

    [SerializeField]
    private Material _skyboxMaterial;

    #endregion

    // Use this for initialization
    void Start () {

        _boardController = GameObject.FindObjectOfType<BoardController>();
        if (_boardController == null)
        {
            Debug.LogError("ERROR - Cannot find board controller in scene!");
        }

		_gameplayUI = GameObject.FindObjectOfType<GameplayUIController>();
		if (_gameplayUI == null)
		{
			Debug.LogError("ERROR - Cannot find gameplay UI in scene!");
		}

        _boardController.OnTileClicked += OnTileClickedHandler;

        ReloadLevel();

    }


    #region Public

    public void ReloadLevel()
    {
        Debug.Log("reload level");

        _useHints = false;
        _movesCount = 0;
        _levelCompleted = false;
        if (_hintIndicator != null)
        {
            _hintIndicator.SetActive(false);
        }
        _gameplayUI.Show();
        _boardController.gameObject.SetActive(true);
        _selectedLevel = GameSetupManager.Instance.SelectedLevel;
        _boardController.BuildLevel(_selectedLevel);
        _gameplayUI.SetLevel(_selectedLevel);
        _gameplayUI.UpdateMovesCount(_movesCount);

        if (GameSetupManager.Instance.UseHint)
        {
            _useHints = true;
            ShowNextHint();
            GameSetupManager.Instance.UseHint = false;

            RenderSettings.skybox.DOBlendableColor(Color.green , "_Tint", 1.0f);
        }
        else
        {
            RenderSettings.skybox.DOBlendableColor(Color.gray, "_Tint", 1.0f);
        }

        _boardController.DisplayIntro();


    }

    #endregion


    #region Private

    private IEnumerator FinishedGameSequance()
    {
		_levelCompleted = true;
        yield return new WaitForSeconds(1.0f);

        _boardController.DisplayOutro();

        yield return new WaitForSeconds(1.2f);

        yield return StartCoroutine(ClearGameplayContents());
        LevelCompletedPopupController levelCompletionPopup = PopupsManager.Instance.DisplayPopup<LevelCompletedPopupController>();
        levelCompletionPopup.SetMovesCount(_movesCount);

        AccountManager.Instance.FinishedLevel(_selectedLevel);
    }

	private IEnumerator LevelFailedSequance()
	{
		_levelCompleted = true;
		yield return new WaitForSeconds(1.0f);
        yield return StartCoroutine(ClearGameplayContents());
		PopupsManager.Instance.DisplayPopup<LevelFailedPopupController>();
	}

    private IEnumerator ClearGameplayContents()
    {
        _gameplayUI.Hide();
        _boardController.gameObject.SetActive(false);

        if (_hintIndicator != null)
        {
            Destroy(_hintIndicator);
            _hintIndicator = null;
        }

        yield return null;
    }

	private void LevelFinished()
	{
		if (!_levelCompleted)
		{
			StartCoroutine(FinishedGameSequance());
		}
	}

	private void LevelFailed()
	{
		if (!_levelCompleted)
		{
			StartCoroutine(LevelFailedSequance());
		}
	}

    private void ShowNextHint()
    {
        if (_selectedLevel.Steps.Count > _movesCount)
        {
            Vector2 hintIndex = _selectedLevel.Steps[_movesCount];

            if (_hintIndicator == null)
            {
                _hintIndicator = Instantiate(_hintPrefab) as GameObject;
            }

            Vector3 tilePosition = _boardController.GetPositionForTileAtIndex(hintIndex);

            Vector3 screenPosition = Camera.main.WorldToScreenPoint(tilePosition);
            tilePosition = Camera.main.ScreenToWorldPoint(new Vector3(screenPosition.x, screenPosition.y, 8f));

            _hintIndicator.SetActive(true);

            _hintIndicator.transform.position = tilePosition;
        }
    }

    #endregion


    #region Events

    private void OnTileClickedHandler(TileController tileController)
    {
        bool canFlip = true;

        if (_useHints)
        {
            Vector2 hintIndex = _selectedLevel.Steps[_movesCount];
            if (tileController.Position != hintIndex)
            {
                canFlip = false;
            }
        }

        if (canFlip)
        {
            _movesCount++;

            _gameplayUI.UpdateMovesCount(_movesCount);

            _boardController.FlipTile(tileController);

            if (_selectedLevel != null)
            {
                if (_boardController.IsLevelComplete)
                {
                    LevelFinished();
                }

                if (_movesCount > _selectedLevel.Stars1Steps)
                {
                    LevelFailed();
                }
            }
           

            if (_useHints)
            {
                ShowNextHint();
            }
        }
    }

    #endregion

}
