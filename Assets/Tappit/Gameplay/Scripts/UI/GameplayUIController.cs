﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class GameplayUIController : MonoBehaviour {

	#region Public Properties

	[SerializeField]
    private TextMeshProUGUI _levelLabel;

    [SerializeField]
    private TextMeshProUGUI _movesLabel;

    [SerializeField]
    private StarsPanelController _starsController;

    #endregion


    #region Initialization

    void Awake()
    {
        CanvasGroup canvasGroup = this.gameObject.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0f;
    }

    #endregion


    #region Public

    public void SetLevel(LevelDefenition levelDefenition)
	{
        if (levelDefenition != null)
        {
            _levelLabel.text = levelDefenition.ChecpterID.ToString() + " - " + levelDefenition.LevelID.ToString();
        }
	}

    public void UpdateMovesCount(int movesCount)
    {
        int movesLeft = MovesLeft(movesCount);

        _movesLabel.text = movesLeft.ToString();

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
        GameSetupManager.Instance.UseHint = true;
        FlowManager.Instance.StartLevel(GameSetupManager.Instance.SelectedLevel);
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
