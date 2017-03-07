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
        _boardController.OnLevelComplete += OnLevelCompleteHandler;
        _boardController.OnTileClicked += OnTIleClickedHandler;

        _gameplayUI.SetLevel(_selectedLevel);
    }


    #region Private

    private IEnumerator FinishedGameSequance()
    {
        yield return new WaitForSeconds(1.0f);

        _gameplayUI.gameObject.SetActive(false);
        _boardController.gameObject.SetActive(false);

        PopupsManager.Instance.DisplayPopup<LevelCompletedPopupController>();
    }

    #endregion


    #region Events

    private void OnLevelCompleteHandler()
    {
        StartCoroutine(FinishedGameSequance());
    }

    private void OnTIleClickedHandler(TileController tileController)
    {
        _movesCount++;

        _gameplayUI.SetMovesCount(_movesCount);
    }

    #endregion

}
