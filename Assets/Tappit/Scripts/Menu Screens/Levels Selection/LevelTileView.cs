using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelTileView : MonoBehaviour {

    #region Private Properties

    [SerializeField]
    private TextMeshProUGUI _levelLabel;

    [SerializeField]
    private SpriteRenderer[] _stars;

    [SerializeField]
    private Sprite _fullStarSprite;

    [SerializeField]
    private Sprite _emptryStarSprite;

    #endregion


    #region Public

    public void SetLevel(int levelID, int starsCount)
    {
        _levelLabel.text = levelID.ToString();

        for (int i = 0; i < _stars.Length; i++)
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

    #endregion
}
