using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelCompletedPopupController : PopupBaseController {

    #region Private properties

    [SerializeField]
    private Text _levelLabel;

    [Header("Stars")]
    [SerializeField]
    private Image[] _starsImages;

    [SerializeField]
    private Sprite _fullStarSprite;

    [SerializeField]
    private Sprite _emptyStarSprite;

    [SerializeField]
    private Sprite _flashingStarSprite;

    #endregion


    #region Initialization

    void Start()
    {
        _levelLabel.text = GameSetupManager.Instance.SelectedLevel.ChecpterID.ToString() + " - " + GameSetupManager.Instance.SelectedLevel.LevelID.ToString();
    }

    #endregion


    #region Public  

    public void SetMovesCount(int movesCount)
    {
        LevelDefenition currentLevel = GameSetupManager.Instance.SelectedLevel;
        int starCount = 0;
        if (movesCount <= currentLevel.Stars3Steps)
        {
            starCount = 3;
        }
        else if (movesCount <= currentLevel.Stars2Steps)
        {
            starCount = 2;
        }
        else if (movesCount <= currentLevel.Stars1Steps)
        {
            starCount = 1;
        }
       else
       {
            starCount = 0;
       }


        for (int i=0; i < _starsImages.Length; i++)
        {
            if (i < starCount)
            {
                _starsImages[i].sprite = _fullStarSprite;
            }
            else
            {
                _starsImages[i].sprite = _emptyStarSprite;
            }
        }

        AccountManager.Instance.UpdateLevelStars(currentLevel, starCount);
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
