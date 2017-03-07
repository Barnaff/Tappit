using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarsPanelController : MonoBehaviour {

    #region Private properties

    [SerializeField]
    private Image[] _starsImages;

    [SerializeField]
    private Sprite _emptyStarSprite;

    [SerializeField]
    private Sprite _fullStarSprite;

    [SerializeField]
    private Sprite _flashingStarSprite;

    [SerializeField]
    private int _currentStarsCount = 0;

    #endregion


    #region Public

    public void SetStars(int starsCount)
    {
        if (_currentStarsCount != starsCount)
        {
            _currentStarsCount = starsCount;

            for (int i=0; i< _starsImages.Length ; i++)
            {
                if (i < _currentStarsCount)
                {
                    _starsImages[i].sprite = _fullStarSprite;
                }
                else
                {
                    _starsImages[i].sprite = _emptyStarSprite;
                }
            }
        }
    }

    #endregion
}
