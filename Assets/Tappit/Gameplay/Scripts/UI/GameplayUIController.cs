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

    public void SetMovesCount(int movesCount)
    {
        _movesLabel.text = movesCount.ToString();
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

	}

	#endregion
}
