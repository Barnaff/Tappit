using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarsPanelController : MonoBehaviour {

    #region Private properties

    [SerializeField]
    private Image _starsImages;

    [SerializeField]
    private Sprite _emptyStarSprite;

    [SerializeField]
    private Sprite _fullStarSprite;

    [SerializeField]
    private Sprite _flashingStarSprite;

    #endregion


    #region Public

    public void SetStars(int starsCount)
    {

    }

    #endregion
}
