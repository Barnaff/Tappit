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

    [SerializeField]
    private GameObject _lockIcon;

    [SerializeField]
    private GameObject _tileBox;

    [SerializeField]
    private Texture2D _tileTexture;

    [SerializeField]
    private Material _tileMaterial;

    [SerializeField]
    private Material _selectedTileMaterial;

    #endregion


    #region Public

    public void SetLevel(int levelID, int starsCount, bool isSelected, bool isLocked)
    {
        _levelLabel.text = levelID.ToString();

        if (isSelected)
        {
            Debug.Log("selected level " + levelID);
            _tileBox.GetComponent<Renderer>().material = _selectedTileMaterial;
        }
        else
        {
            _tileBox.GetComponent<Renderer>().material = _tileMaterial;
        }

        _tileBox.GetComponent<Renderer>().material.mainTexture = _tileTexture;

        if (isLocked)
        {
            _lockIcon.SetActive(true);

            for (int i = 0; i < _stars.Length; i++)
            {
                _stars[i].gameObject.SetActive(false);
            }
            _tileBox.GetComponent<Renderer>().material.color = new Color(0.2f, 0.2f, 0.2f);
        }
        else
        {
            _lockIcon.SetActive(false);
            _tileBox.GetComponent<Renderer>().material.color = Color.white;
            for (int i = 0; i < _stars.Length; i++)
            {
                _stars[i].gameObject.SetActive(true);
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
    }

    public void Disable()
    {
        _tileBox.GetComponent<Renderer>().material.color = new Color(0.2f, 0.2f, 0.2f);
    }

    #endregion
}
