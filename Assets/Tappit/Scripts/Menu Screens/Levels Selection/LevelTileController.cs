using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelTileController : MonoBehaviour {


    #region Public Properties

    public delegate void LevelTileSelectedDelegate(LevelTileController levelTile);

    public event LevelTileSelectedDelegate OnLevelTileSelected;

    #endregion


    #region Private Properties

    [SerializeField]
	private TextMeshProUGUI _levelLabel;

    [SerializeField]
    private LevelDefenition _levelDefenition;

    [SerializeField]
    private SpriteRenderer[] _stars;

    [SerializeField]
    private Sprite _fullStarSprite;

    [SerializeField]
    private Sprite _emptryStarSprite;

    #endregion


    #region Public

    public void SetLevel(LevelDefenition levelDefenition)
    {
        _levelDefenition = levelDefenition;
        _levelLabel.text = _levelDefenition.LevelID.ToString();

        int starsCount = AccountManager.Instance.StarsForLevel(_levelDefenition);

        for (int i=0; i < _stars.Length; i++)
        {
            if (i < starsCount)
            {
                _stars[i].sprite = _fullStarSprite;
            }
            else
            {
                _stars[i].sprite = _emptryStarSprite;
            }
        }
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

}
