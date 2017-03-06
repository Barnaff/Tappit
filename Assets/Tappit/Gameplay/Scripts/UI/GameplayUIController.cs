using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameplayUIController : MonoBehaviour {

	#region Public Properties

	[SerializeField]
	public TextMeshProUGUI _levelLabel;


	#endregion


	#region Public

	public void SetLevel(LevelDefenition levelDefenition)
	{
		_levelLabel.text = levelDefenition.ChecpterID.ToString() + " - " + levelDefenition.LevelID.ToString();
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
