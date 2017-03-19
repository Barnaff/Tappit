using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class LevelTileController : MonoBehaviour {


    #region Public Properties

    public delegate void LevelTileSelectedDelegate(LevelTileController levelTile);

    public event LevelTileSelectedDelegate OnLevelTileSelected;

    public enum eLevelTileAnimation
    {
        None,
        NoneFlipped,
        Next,
        Back,
    }

    #endregion


    #region Private Properties

    [SerializeField]
    private LevelDefenition _levelDefenition;

    [SerializeField]
    private LevelTileView _frontView;

    [SerializeField]
    private LevelTileView _backView;

    [SerializeField]
    private LevelTileView _currentLevelView;

    #endregion


    #region Public

    public void SetLevel(LevelDefenition levelDefenition, eLevelTileAnimation tranistion)
    {
        _levelDefenition = levelDefenition;

        DOTween.Complete(this.transform);

        switch (tranistion)
        {
            case eLevelTileAnimation.Next:
                {
                    SwitchView();
                    float delay = (LevelDefenition.LevelID - 1) % 4;
                    delay *= 0.1f;

                    this.transform.DORotate(new Vector3(0, this.transform.rotation.eulerAngles.y + 180f, 0), 0.5f).SetDelay(delay);
                    break;
                }
            case eLevelTileAnimation.Back:
                {
                    SwitchView();
                    float delay = 4 - (LevelDefenition.LevelID - 1) % 4;
                    delay *= 0.1f;

                    this.transform.DORotate(new Vector3(0, this.transform.rotation.eulerAngles.y - 180f, 0), 0.5f).SetDelay(delay);
                    break;
                }
            case eLevelTileAnimation.None:
                {
                    _backView.Disable();
                    _currentLevelView = _frontView;
                    break;
                }
            case eLevelTileAnimation.NoneFlipped:
                {
                    _currentLevelView = _backView;
                    this.transform.rotation = Quaternion.Euler(new Vector3(0, this.transform.rotation.eulerAngles.y - 180f, 0));
                    break;
                }
        }

        int starsCount = AccountManager.Instance.StarsForLevel(_levelDefenition);
        bool isLevelUnlocked = AccountManager.Instance.IsLevelUnlocked(_levelDefenition);

        _currentLevelView.SetLevel(_levelDefenition.LevelID, starsCount, !isLevelUnlocked);

    }

    public LevelDefenition LevelDefenition
    {
        get
        {
            return _levelDefenition;
        }
    }


	public void TileClikcAction()
	{
		if (OnLevelTileSelected != null)
		{
			OnLevelTileSelected(this);
		}
	}

    #endregion


    #region Private

    private void SwitchView()
    {
        if (_currentLevelView == _frontView)
        {
            _currentLevelView = _backView;
        }
        else
        {
            _currentLevelView = _frontView;
        }
    }

    #endregion

}
