using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public delegate void TileCLickedDelegate(TileController tileController);

public class TileController : MonoBehaviour {

	#region Public Properties

	public event TileCLickedDelegate OnTileClicked;

	[SerializeField]
	public Vector2 Position;

	#endregion


	#region Private Properties

	[SerializeField]
	private bool _isFlipped;

    [SerializeField]
    private GameObject[] _tileIndicatorContainers;


    [SerializeField]
    private TileDefenition _tileDefenition;

    private bool _isFlipping = false;


    [Header("Sounds")]
    [SerializeField]
    private SoundResource _beginFLipSoundEffect;

    [SerializeField]
    private SoundResource _flipSoundEffect;

    #endregion


    #region Public

    public void SetFace(bool isFlipped)
    {
        _isFlipped = isFlipped;
        if (_isFlipped)
        {
            Vector3 rotation = this.transform.rotation.eulerAngles;
            rotation.y += 180f;
            this.transform.rotation = Quaternion.Euler(rotation);
        }
        else
        {
            Vector3 rotation = this.transform.rotation.eulerAngles;
            rotation.y = 0f;
            this.transform.rotation = Quaternion.Euler(rotation);
        }

        this.transform.rotation = Quaternion.Euler((Random.insideUnitSphere * 2.0f) + this.transform.rotation.eulerAngles);

        _isFlipping = false;

        this.gameObject.GetComponent<Collider>().enabled = true;
    }

	public void Flip(bool animated, float delay, System.Action completionAction)
	{
        StartCoroutine(FlipTileCorutine(animated, delay, completionAction)); 
	}

    public TileDefenition TileDefenition
    {
        set
        {
            _tileDefenition = value;

            switch (_tileDefenition.TileType)
            {
                case eTileType.LineHorizontal:
                case eTileType.LineVertial:
                    {
                        SetTileIndicator(_tileDefenition.TileType);
                        break;
                    }
                default:
                    {
                        foreach (GameObject tileIndicatorContainer in _tileIndicatorContainers)
                        {
                            tileIndicatorContainer.SetActive(false);
                        }
                        break;
                    }
            }
        }
        get
        {
            return _tileDefenition;
        }
    }

    private void SetTileIndicator(eTileType tileType)
    {
        foreach (GameObject tileIndicatorContainer in _tileIndicatorContainers)
        {
            tileIndicatorContainer.SetActive(true);

            for (int i = 0; i < tileIndicatorContainer.transform.childCount; i++)
            {
                if (tileIndicatorContainer.transform.GetChild(i) != null)
                {
                    Lean.LeanPool.Despawn(tileIndicatorContainer.transform.GetChild(i).gameObject);
                }
            }

            GameObject indicaotrPrefab = GameplayAssets.Instance.IndicatorForTile(tileType);
            if (indicaotrPrefab != null)
            {
                Lean.LeanPool.Spawn(indicaotrPrefab, Vector3.zero, Quaternion.identity, tileIndicatorContainer.transform);
            }
        }
    }

    public bool IsFlipped
	{
		get
		{
			return _isFlipped;
		}
	}

	#endregion


	#region User Interactions

	void OnMouseDown()
	{
        if (!_isFlipping)
        {
            if (OnTileClicked != null)
            {
                OnTileClicked(this);
            }
        }
	}

    #endregion


    #region Private

    private IEnumerator FlipTileCorutine(bool animated, float delay, System.Action completionAction)
    {
        _isFlipping = true;
        _isFlipped = !_isFlipped;

       DOTween.Complete(this.transform);

        delay *= 0.1f;

        if (animated)
        {
           

            this.gameObject.GetComponent<Collider>().enabled = false;
            this.transform.DOMoveZ(-3f, 0.3f).SetLoops(2, LoopType.Yoyo).SetRelative().SetDelay(delay).OnPlay(()=>
            {
                if (_beginFLipSoundEffect != null)
                {
                    _beginFLipSoundEffect.Play();
                }
            });
            this.transform.DOLocalRotate(new Vector3(0, 180f, 0), 0.5f).SetRelative().SetDelay(delay + 0.2f).OnStart(()=>
            {
                if (_flipSoundEffect != null)
                {
                    _flipSoundEffect.Play();
                }
            });


            yield return new WaitForSeconds(delay + 0.7f);
        }
        else
        {
            Vector3 rotation = this.transform.rotation.eulerAngles;
            rotation.y += 180f;
            this.transform.rotation = Quaternion.Euler(rotation);
        }

        if (animated)
        {
            this.gameObject.GetComponent<Collider>().enabled = true;
        }
        

        if (completionAction != null)
        {
            completionAction();
        }
        _isFlipping = false;
    }

    #endregion
}
