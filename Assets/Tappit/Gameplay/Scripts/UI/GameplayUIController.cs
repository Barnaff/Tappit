using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameplayUIController : MonoBehaviour {

	#region Public Properties

	[SerializeField]
    private TextMeshProUGUI _levelLabel;

    [SerializeField]
    private TextMeshProUGUI _movesLabel;

    [SerializeField]
    private StarsPanelController _starsController;

	#endregion


	#region Public

	public void SetLevel(LevelDefenition levelDefenition)
	{
		_levelLabel.text = levelDefenition.ChecpterID.ToString() + " - " + levelDefenition.LevelID.ToString();
	}

    public void UpdateMovesCount(int movesCount)
    {
        int movesLeft = MovesLeft(movesCount);

        _movesLabel.text = movesLeft.ToString();

    }

    public void BackButtonAction()
	{
		FlowManager.Instance.LevelsSelectionScreen();
	}

	public void ResetButtonAction()
	{
		FlowManager.Instance.StartLevel(GameSetupManager.Instance.SelectedLevel);
	}

	public void HintButtonAction()
	{
        GameSetupManager.Instance.UseHint = true;
        FlowManager.Instance.StartLevel(GameSetupManager.Instance.SelectedLevel);
    }

    #endregion


    #region Private

    private int MovesLeft(int movesCount)
    {
        LevelDefenition currentLevel = GameSetupManager.Instance.SelectedLevel;
        int movesLeft = 0;
        if (movesCount <= currentLevel.Stars3Steps)
        {
            movesLeft = currentLevel.Stars3Steps - movesCount;
            _starsController.SetStars(3);
        }
        else if (movesCount <= currentLevel.Stars2Steps)
        {
            movesLeft = currentLevel.Stars2Steps - movesCount;
            _starsController.SetStars(2);
        }
        else if (movesCount <= currentLevel.Stars1Steps)
        {
            movesLeft = currentLevel.Stars1Steps - movesCount;
            _starsController.SetStars(1);
        }
        return movesLeft;
    }

    #endregion

}
