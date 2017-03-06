using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayUIController : MonoBehaviour {

	#region Public Properties



	#endregion


	#region Public

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
