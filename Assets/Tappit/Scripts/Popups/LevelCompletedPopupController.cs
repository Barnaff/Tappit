using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

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

    [SerializeField]
    private int _starsCount = 0;

    [SerializeField]
    private CanvasGroup[] _fadeGroups;

    [SerializeField]
    private GameObject _starExplosionEffectPrefab;

    [SerializeField]
    private SoundResource _starShineSouneEffect;

    [SerializeField]
    private GameObject _nextLevelButton;

    #endregion


    #region Initialization

    void Start()
    {
        Canvas canvas = this.GetComponent<Canvas>();
        if (canvas != null)
        {
            canvas.worldCamera = Camera.main;
        }
        _levelLabel.text =  GameSetupManager.Instance.SelectedLevel.LevelID.ToString();

        _nextLevelButton.transform.DOShakeScale(1f, 0.1f).SetDelay(2.0f).SetLoops(-1);
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

        _starsCount = starCount;

        AccountManager.Instance.UpdateLevelStars(currentLevel, starCount);

        DisplayIntroAnimation();
    }


    #endregion


    #region Private

    private void DisplayIntroAnimation()
    {
        CanvasGroup canvasGroup = this.gameObject.GetComponent<CanvasGroup>();

        canvasGroup.alpha = 0f;
        canvasGroup.DOFade(1f, 0.5f);

        float pitch = 1f;
        for (int i = 0; i < _starsImages.Length; i++)
        {
            if (i < _starsCount)
            {
                Image starImage = _starsImages[i];
                starImage.transform.DOPunchScale(Vector3.one * 0.2f, 0.5f).SetDelay((i * 0.5f) + 0.5f).OnStart(() =>
                {
                    starImage.sprite = _fullStarSprite;

                    GameObject explosionEffect = Instantiate(_starExplosionEffectPrefab, starImage.transform.position, Quaternion.identity);
                    explosionEffect.transform.SetParent(starImage.gameObject.transform);

                    pitch += 0.1f;
                    _starShineSouneEffect.Play(1f, pitch);
                });
            }
            else
            {
                _starsImages[i].sprite = _emptyStarSprite;

            }
        }

        for (int i=0; i< _fadeGroups.Length; i++)
        {
            CanvasGroup fadeItem = _fadeGroups[i];
            fadeItem.alpha = 0;
            fadeItem.DOFade(1.0f, 0.7f).SetDelay(1.0f + (i * 0.2f));
        }
    }

    private void DisplayCloseAnimation(System.Action completionAction)
    {
        CanvasGroup canvasGroup = this.gameObject.GetComponent<CanvasGroup>();
        canvasGroup.DOFade(0f, 0.5f).OnComplete(()=>
        {
            if (completionAction != null)
            {
                completionAction();
            }
        });
    }

    #endregion


    #region User Interactions

    public void NextLevelButtonAction()
    {
        ClosePopup(()=>
        {
            FlowManager.Instance.NextLevel();
        });
    }

    public void MenuButtonAction()
    {
        FlowManager.Instance.LevelsSelectionScreen();
    }

    public void PlayAgainButtonAction()
    {
        ClosePopup(()=>
        {
            FlowManager.Instance.StartLevel(GameSetupManager.Instance.SelectedLevel);
        });
    }

    #endregion

    #region PopupBaseController Subclassing

    protected override void DisplayPopupCloseAnimation(System.Action completionAction)
    {
        DisplayCloseAnimation(() =>
        {
            if (completionAction != null)
            {
                completionAction();
            }
        });
    }

    #endregion


}
