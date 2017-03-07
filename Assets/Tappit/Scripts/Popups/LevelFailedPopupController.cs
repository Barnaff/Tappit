using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelFailedPopupController : PopupBaseController {


	#region Private Properties


	#endregion


	#region User Interactions


	public void MenuButtonAction()
	{
		FlowManager.Instance.LevelsSelectionScreen();

		ClosePopup();
	}

	public void PlayAgainButtonAction()
	{
		FlowManager.Instance.StartLevel(GameSetupManager.Instance.SelectedLevel);

		ClosePopup();
	}

	#endregion

}
