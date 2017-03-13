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

	#endregion


	#region Public

	public void Flip(bool animated, System.Action completionAction)
	{
        StartCoroutine(FlipTileCorutine(animated, completionAction)); 
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
		if (OnTileClicked != null)
		{
			OnTileClicked(this);
		}
	}

    #endregion


    #region Private

    private IEnumerator FlipTileCorutine(bool animated, System.Action completionAction)
    {
        _isFlipped = !_isFlipped;

        DOTween.Complete(this.transform);

        if (animated)
        {
            this.transform.DOMoveZ(Random.Range(-1.5f, -3f), 0.3f).SetLoops(2, LoopType.Yoyo).SetRelative();
            this.transform.DOLocalRotate(new Vector3(0, 180f, 0), 0.5f).SetRelative().SetDelay(Random.Range(0.2f, 0.3f));

            yield return new WaitForSeconds(0.7f);
        }
        else
        {
            Vector3 rotation = this.transform.rotation.eulerAngles;
            rotation.y += 180f;
            this.transform.rotation = Quaternion.Euler(rotation);
        }

        if (completionAction != null)
        {
            completionAction();
        }

    }

    #endregion
}
