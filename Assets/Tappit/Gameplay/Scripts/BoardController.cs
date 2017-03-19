using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BoardController : MonoBehaviour {


    #region Public

	public event TileCLickedDelegate OnTileClicked;

    #endregion


    #region Private properties

	[SerializeField]
	private Vector2 _boardSize;

	[SerializeField]
	private TileController _tilePrefab;

	[SerializeField]
	private TileController[,] _boardTiles;

    [SerializeField]
    private LevelDefenition _currentLevel;

    [SerializeField]
    private Vector2 _screenSize;

    #endregion

	

	#region Public

    public void BuildLevel(LevelDefenition level)
    {
        if (level == null)
        {
            Debug.LogError("ERROR - Level is null");
        }
        _currentLevel = level;
        if (_currentLevel != null)
        {
            _boardSize = _currentLevel.BoardSize;
        }
       

        Vector3 corner = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 10));
        _screenSize = new Vector2(corner.x * 2, corner.y * 2);

        Reset();

    }

	public void Reset()
	{
		if (_boardTiles != null)
		{
			foreach (TileController tileController in _boardTiles)
			{
                if (tileController != null && tileController.gameObject != null)
                {
                    tileController.OnTileClicked -= OnTileCLickedHandler;
                    Lean.LeanPool.Despawn(tileController.gameObject);
                }
			}
		}

		BuildBoard();
	}


	public Vector2 BoardSize
	{
		get
		{
			return _boardSize;
		}
		set
		{
			_boardSize = value;
		}
	}

	public TileController[,] BoardTiles
	{
		get
		{
			return _boardTiles;
		}
	}

	public bool IsLevelComplete
	{
		get
		{
			bool sucsess = true;
			bool? lastTile = null;
			foreach (TileController tileController in _boardTiles)
			{
				if (lastTile == null)
				{
					lastTile = tileController.IsFlipped;
				}
				if (tileController.IsFlipped != lastTile)
				{
					return false;
				}
			}

			return sucsess;
		}
	}

    public void FlipTile(TileController tileController)
    {
        List<TileController> tilesToFlip = GetAdjacentTIles(tileController.Position);
        tilesToFlip.Add(tileController);

        while (AddFlipsForSpecialTiles(tilesToFlip)) ;
        

        foreach (TileController tile in tilesToFlip)
        {
            if (tile == tileController)
            {
                tileController.Flip(true, 0, () =>
                {

                });
            }
            else
            {
                float flipDelay = Vector2.Distance(tileController.Position, tile.Position);
                tile.Flip(true, flipDelay, null);
            }
        }
    }

    private bool AddFlipsForSpecialTiles(List<TileController> tilesToFlip)
    {
        List<TileController> tilesToAdd = new List<TileController>();

        foreach (TileController flippingTile in tilesToFlip)
        {
            switch (flippingTile.TileDefenition.TileType)
            {
                case eTileType.Empty:
                case eTileType.Normal:
                    {
                        break;
                    }
                case eTileType.LineVertial:
                    {
                        foreach (TileController tile in _boardTiles)
                        {
                            if (tile.Position.x == flippingTile.Position.x)
                            {
                                if (!tilesToFlip.Contains(tile))
                                {
                                    tilesToAdd.Add(tile);
                                }
                            }
                        }
                        break;
                    }
                case eTileType.LineHorizontal:
                    {
                        foreach (TileController tile in _boardTiles)
                        {
                            if (tile.Position.y == flippingTile.Position.y)
                            {
                                if (!tilesToFlip.Contains(tile))
                                {
                                    tilesToAdd.Add(tile);
                                }
                            }
                        }
                        break;
                    }
            }
        }

        if (tilesToAdd.Count > 0)
        {
            tilesToFlip.AddRange(tilesToAdd);
        }

        return (tilesToAdd.Count > 0);
    }

    public Vector2 GetPositionForTileAtIndex(Vector2 tileIndex)
    {
        return _boardTiles[(int)tileIndex.y, (int)tileIndex.x].transform.position;
    }

    public void DisplayIntro()
    {
        foreach (TileController tileController in _boardTiles)
        {
            Vector3 endPosition = tileController.transform.position;
            Vector3 endRotation = tileController.transform.rotation.eulerAngles;
            tileController.transform.position = new Vector3(endPosition.x, -_screenSize.y, endPosition.z);
            Vector3 midPosition = endPosition;
            midPosition.z -= Random.Range(2.0f, 4.0f);
            tileController.transform.DORotate((Random.insideUnitSphere * 5.0f) + endRotation, 0.5f).OnComplete(() =>
            {
                tileController.transform.DORotate(endRotation, 0.5f);

            });
            tileController.transform.DOMove(midPosition, 0.5f).SetEase(Ease.OutCubic).OnComplete(() =>
            {
                tileController.transform.DOMove(endPosition, 0.3f).SetEase(Ease.InCubic);

            });
        }
    }


    public void DisplayOutro()
    {
        foreach (TileController tileController in _boardTiles)
        {
            Vector3 newPosition = tileController.transform.position;
            Vector3 originalRotation = tileController.transform.rotation.eulerAngles;
            newPosition.z -= Random.Range(3.0f, 6.0f);

            tileController.transform.DORotate((Random.insideUnitSphere * 30.0f) + tileController.transform.rotation.eulerAngles, 0.5f);
            tileController.transform.DOMove(newPosition, 0.4f).OnComplete(()=>
            {
                float delay = Random.Range(0.1f, 0.2f);
                tileController.transform.DORotate(originalRotation, 0.4f);
                tileController.transform.DOScale(Vector3.zero, 0.4f).SetDelay(delay);
                tileController.transform.DOMove(Vector3.zero, 0.4f).SetDelay(delay);
            });
        }
    }

    #endregion



    #region Private

    private void BuildBoard()
	{
		_boardTiles = new TileController[(int)_boardSize.y , (int)_boardSize.x];

		for (int y=0; y< _boardSize.y; y++)
		{
			for (int x=0; x< _boardSize.x; x++)
			{
                TileDefenition tileDefenition = null;
                if (_currentLevel != null && _currentLevel.BoardSetup != null)
                {
                    tileDefenition = GetTileDefenition(new Vector2(x, y));
                }

                TileController tileController = CreateTile(x,y, tileDefenition);
				_boardTiles[y,x] = tileController;
			}
		}
    }

	private TileController CreateTile(int x, int y, TileDefenition tileDefenition)
	{
        TileController newTileController = Lean.LeanPool.Spawn(_tilePrefab);

		newTileController.transform.SetParent(this.transform);

		float tileScale = _screenSize.x * 0.8f / _boardSize.x;

		newTileController.transform.localScale = Vector3.one * tileScale;

		newTileController.transform.localPosition = new Vector3(x * tileScale - (_boardSize.x * tileScale / 2f) + (tileScale * 0.5f) , 
																y * tileScale - (_boardSize.y * tileScale / 2f) + (tileScale * 0.5f),0);

		newTileController.OnTileClicked += OnTileCLickedHandler;

		newTileController.Position = new Vector2(x ,y);

        newTileController.SetFace(tileDefenition.IsFlipped);

        newTileController.TileDefenition = tileDefenition;

        return newTileController;
	}


	private List<TileController> GetAdjacentTIles(Vector2 position)
	{
		List<TileController> adjacentTiles = new List<TileController>();

		int x = (int)position.x;
		int y = (int)position.y;

		// left
		if (x > 0)
		{
			adjacentTiles.Add(_boardTiles[y , x -1]);
		}

		// right
		if (x < _boardSize.x - 1)
		{
			adjacentTiles.Add(_boardTiles[y , x + 1]);
		}

		// up
		if (y > 0)
		{
			adjacentTiles.Add(_boardTiles[y - 1 , x]);
		}

		// down
		if (y < _boardSize.y - 1)
		{
			adjacentTiles.Add(_boardTiles[y + 1 , x]);
		}

		return adjacentTiles;
	}

    private TileDefenition GetTileDefenition(Vector2 tilePosition)
    {
        foreach (TileDefenition tileDefenition in _currentLevel.BoardSetup)
        {
            if (tileDefenition.Position == tilePosition)
            {
                return tileDefenition;
            }
        }

        return null;
    }

	#endregion


	#region Events

	private void OnTileCLickedHandler(TileController tileController)
	{
		//FlipTile(tileController);

		if (OnTileClicked != null)
		{
			OnTileClicked(tileController);
		}
	}

	#endregion


}
