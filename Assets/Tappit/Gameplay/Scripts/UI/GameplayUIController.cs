using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class GameplayUIController : MonoBehaviour, IBackButtonListener {

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

    [SerializeField]
    private TextMeshProUGUI _hintsCountLabel;

    [SerializeField]
    private GameObject _hintsAvalableMark;


    private LevelDefenition _currentLevel;

    #endregion


    #region Initialization

    void Awake()
    {
        CanvasGroup canvasGroup = this.gameObject.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0f;

        AccountManager.Instance.OnHintsCountUpdated += OnHintsCountUpdatedHandler;

        BackButtonManager.Instance.RegisterListener(this);
    }

    void OnDestroy()
    {
        AccountManager.Instance.OnHintsCountUpdated -= OnHintsCountUpdatedHandler;

        BackButtonManager.Instance.RemoveListener(this);
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
        _currentLevel = levelDefenition;
        if (_currentLevel != null)
        {
            _levelLabel.text = _currentLevel.ChecpterID.ToString() + " - " + _currentLevel.LevelID.ToString();

            if (!string.IsNullOrEmpty(_currentLevel.TopTutorialTitle))
            {
                _topTutorialLabel.gameObject.SetActive(true);
                _topTutorialLabel.text = _currentLevel.TopTutorialTitle;
            }
            else
            {
                _topTutorialLabel.gameObject.SetActive(false);
            }

            if (!string.IsNullOrEmpty(_currentLevel.BottomTutorialTitle))
            {
                _bottomTutorialLabel.gameObject.SetActive(true);
                _bottomTutorialLabel.text = _currentLevel.BottomTutorialTitle;
            }
            else
            {
                _bottomTutorialLabel.gameObject.SetActive(false);
            }

            UpdateHintsView();

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
                BackButtonManager.Instance.RegisterListener(this);
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
            if (AccountManager.Instance.HintsCount() > 0 || AccountManager.Instance.HasHintForLevel(_currentLevel))
            {
                if (AccountManager.Instance.UseHint(_currentLevel))
                {
                    GameSetupManager.Instance.UseHint = true;
                    FlowManager.Instance.StartLevel(GameSetupManager.Instance.SelectedLevel);
                }
            }
            else
            {
                if (OnGamePaused != null)
                {
                    OnGamePaused();
                }
                PopupsManager.Instance.DisplayPopup<HintsShopPopupController>(() =>
                {
                    if (OnGameResumed != null)
                    {
                        OnGameResumed();
                    }
                });
            }
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

    private void UpdateHintsView()
    {
        if (AccountManager.Instance.HasHintForLevel(_currentLevel))
        {
            _hintsAvalableMark.SetActive(true);
            _hintsCountLabel.gameObject.SetActive(false);
        }
        else
        {
            _hintsAvalableMark.SetActive(false);
            int hintsCount = AccountManager.Instance.HintsCount();
            if (hintsCount > 0)
            {
                _hintsCountLabel.gameObject.SetActive(true);
                _hintsCountLabel.text = hintsCount.ToString();
            }
            else
            {
                _hintsCountLabel.gameObject.SetActive(false);
            }
        }
    }

    #endregion


    #region Events

    private void OnHintsCountUpdatedHandler()
    {
        UpdateHintsView();
    }

    #endregion


    #region IBackButtonListener Implementation

    public void BackButtonCallback()
    {
        PauseButtonAction();
    }

    #endregion

}
