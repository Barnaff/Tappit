using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelCompletedPopupController : PopupBaseController {

    #region Private properties

    [SerializeField]
    private Text _levelLabel;

    #endregion


    #region Initialization

    void Start()
    {
        _levelLabel.text = GameSetupManager.Instance.SelectedLevel.ChecpterID.ToString() + " - " + GameSetupManager.Instance.SelectedLevel.LevelID.ToString();
    }

    #endregion


    #region User Interactions

    public void NextLevelButtonAction()
    {
        FlowManager.Instance.NextLevel();

        ClosePopup();
    }

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
