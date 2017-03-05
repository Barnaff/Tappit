﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayController : MonoBehaviour {

    #region Private Properties

    [SerializeField]
    private LevelDefenition _selectedLevel;

    [SerializeField]
    private BoardController _boardController;

    #endregion

    // Use this for initialization
    void Start () {

        _selectedLevel = GameSetupManager.Instance.SelectedLevel;

        _boardController = GameObject.FindObjectOfType<BoardController>();
        if (_boardController == null)
        {
            Debug.LogError("ERROR - Cannot find board controller in scene!");
        }

        _boardController.InitWithLevel(_selectedLevel);
        _boardController.OnLevelComplete += OnLevelCompleteHandler;

    }


    #region Private

    private IEnumerator FinishedGameSequance()
    {
        yield return new WaitForSeconds(1.0f);

        PopupsManager.Instance.DisplayPopup<LevelCompletedPopupController>();
    }

    #endregion


    #region Events

    private void OnLevelCompleteHandler()
    {
        StartCoroutine(FinishedGameSequance());
    }

    #endregion

}
