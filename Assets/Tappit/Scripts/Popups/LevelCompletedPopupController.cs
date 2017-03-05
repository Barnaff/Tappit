using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCompletedPopupController : PopupBaseController {


    #region User Interactions

    public void NextLevelButtonAction()
    {
        FlowManager.Instance.NextLevel();

        ClosePopup();
    }

    public void MenuButtonAction()
    {
        FlowManager.Instance.LevelsSelectionScreen();
    }

    public void PlayAgainButtonAction()
    {
        FlowManager.Instance.StartLevel(GameSetupManager.Instance.SelectedLevel);
    }

    #endregion
}
