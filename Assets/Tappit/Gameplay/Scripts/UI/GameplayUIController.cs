using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class GameplayUIController : MonoBehaviour {

    #region Public Properties

    public delegate void GameplayUIDelegate();

    public event GameplayUIDelegate OnGamePaused;

    public event GameplayUIDelegate OnGameResumed;

    #endregion

    #region Private Properties

    [SerializeField]
    private TextMeshProUGUI _levelLabel;

    [SerializeField]
    private TextMeshProUGUI _movesLabel;

    [SerializeField]
    private StarsPanelController _starsController;

    [SerializeField]
    private bool _interactionEnabled;

    [SerializeField]
    private TextMeshProUGUI _topTutorialLabel;

    [SerializeField]
    private TextMeshProUGUI _bottomTutorialLabel;

    #endregion


    #region Initialization

    void Awake()
    {
        CanvasGroup canvasGroup = this.gameObject.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0f;
    }

    #endregion


    #region Public

    public bool InteractionEnabled
    {
        set
        {
            _interactionEnabled = value;
        }
    }

    public void SetLevel(LevelDefenition levelDefenition)
	{
        if (levelDefenition != null)
        {
            _levelLabel.text = levelDefenition.ChecpterID.ToString() + " - " + levelDefenition.LevelID.ToString();

            if (!string.IsNullOrEmpty(levelDefenition.TopTutorialTitle))
            {
                _topTutorialLabel.gameObject.SetActive(true);
                _topTutorialLabel.text = levelDefenition.TopTutorialTitle;
            }
            else
            {
                _topTutorialLabel.gameObject.SetActive(false);
            }

            if (!string.IsNullOrEmpty(levelDefenition.BottomTutorialTitle))
            {
                _bottomTutorialLabel.gameObject.SetActive(true);
                _bottomTutorialLabel.text = levelDefenition.BottomTutorialTitle;
            }
            else
            {
                _bottomTutorialLabel.gameObject.SetActive(false);
            }
        }
    }

    public void UpdateMovesCount(int movesCount)
    {
        int movesLeft = MovesLeft(movesCount);
        _movesLabel.text = movesLeft.ToString();

    }

    public void PauseButtonAction()
	{
        if (_interactionEnabled)
        {
            if (OnGamePaused != null)
            {
                OnGamePaused();
            }
            PopupsManager.Instance.DisplayPopup<GamePausedPopupController>(() =>
            {
                if (OnGameResumed != null)
                {
                    OnGameResumed();
                }
            });
        }
	}

	public void ResetButtonAction()
	{
        if (_interactionEnabled)
        {
            FlowManager.Instance.StartLevel(GameSetupManager.Instance.SelectedLevel);
        }
		
	}

	public void HintButtonAction()
	{
        if (_interactionEnabled)
        {
            if (OnGamePaused != null)
            {
                OnGamePaused();
            }
            PopupsManager.Instance.DisplayPopup<HintsShopPopupController>(()=>
            {
                if (OnGameResumed != null)
                {
                    OnGameResumed();
                }
            });
        }
    }

    public void Show()
    {
        CanvasGroup canvasGroup = this.gameObject.GetComponent<CanvasGroup>();
        canvasGroup.DOFade(1.0f, 0.5f);
    }

    public void Hide()
    {
        CanvasGroup canvasGroup = this.gameObject.GetComponent<CanvasGroup>();
        canvasGroup.DOFade(0.0f, 0.5f);
    }

    #endregion


    #region Private

    private int MovesLeft(int movesCount)
    {
        LevelDefenition currentLevel = GameSetupManager.Instance.SelectedLevel;
        int movesLeft = 0;
        if (currentLevel != null)
        {
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
        }
        return movesLeft;
    }

    #endregion

}
