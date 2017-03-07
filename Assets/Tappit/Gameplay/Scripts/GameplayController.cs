using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private bool _levelCompleted = false;

    #endregion

    // Use this for initialization
    void Start () {

        _selectedLevel = GameSetupManager.Instance.SelectedLevel;

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

        _boardController.InitWithLevel(_selectedLevel);
        _boardController.OnTileClicked += OnTileClickedHandler;

        _gameplayUI.SetLevel(_selectedLevel);
        _gameplayUI.UpdateMovesCount(_movesCount);
    }


    #region Private

    private IEnumerator FinishedGameSequance()
    {
		_levelCompleted = true;

        yield return new WaitForSeconds(1.0f);

        _gameplayUI.gameObject.SetActive(false);
        _boardController.gameObject.SetActive(false);

        LevelCompletedPopupController levelCompletionPopup = PopupsManager.Instance.DisplayPopup<LevelCompletedPopupController>();
        levelCompletionPopup.SetMovesCount(_movesCount);
    }

	private IEnumerator LevelFailedSequance()
	{
		_levelCompleted = true;

		yield return new WaitForSeconds(1.0f);

		_gameplayUI.gameObject.SetActive(false);
		_boardController.gameObject.SetActive(false);

		LevelFailedPopupController levelFailedPopup = PopupsManager.Instance.DisplayPopup<LevelFailedPopupController>();
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

    #endregion


    #region Events

    private void OnTileClickedHandler(TileController tileController)
    {
        _movesCount++;

        _gameplayUI.UpdateMovesCount(_movesCount);

		if (_boardController.IsLevelComplete)
		{
			LevelFinished();
		}

		if (_movesCount > _selectedLevel.Stars1Steps)
		{
			LevelFailed();
		}
    }

    #endregion

}
