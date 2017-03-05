using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BoardController : MonoBehaviour {


    #region Public

    public delegate void LevelCompleteDelegate();

	public event TileCLickedDelegate OnTileClicked;

    public event LevelCompleteDelegate OnLevelComplete;

    #endregion


    #region Private properties

    [SerializeField]
	private int _tilesOnScreen;

	[SerializeField]
	private Vector2 _boardSize;

	[SerializeField]
	private TileController _tilePrefab;

	[SerializeField]
	private TileController[,] _boardTiles;

    [SerializeField]
    private LevelDefenition _currentLevel;

    [SerializeField]
    private bool _levelCompleted = false;


	#endregion


	// Use this for initialization
	void Start () {

		//BuildBoard();
	}
	

	#region Public

    public void InitWithLevel(LevelDefenition level)
    {
        if (level == null)
        {
            Debug.LogError("ERROR - Level is null");
        }
        _currentLevel = level;
        _boardSize = _currentLevel.BoardSize;

        Reset();

    }

	public void Reset()
	{
		if (_boardTiles != null)
		{
			foreach (TileController tileController in _boardTiles)
			{
				Destroy(tileController.gameObject);
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
		TileController newTileController = Instantiate(_tilePrefab);

		newTileController.transform.SetParent(this.transform);

		float tileScale = _tilesOnScreen / _boardSize.x;

		newTileController.transform.localScale = Vector3.one * tileScale;

		newTileController.transform.localPosition = new Vector3(x * tileScale - (_boardSize.x * tileScale / 2f) + (tileScale * 0.5f) , 
																y * tileScale - (_boardSize.y * tileScale / 2f) + (tileScale * 0.5f),0);

		newTileController.OnTileClicked += OnTileCLickedHandler;

		newTileController.Position = new Vector2(x ,y);

        if (tileDefenition != null && tileDefenition.IsFlipped)
        {
            newTileController.Flip(false);
        }

		return newTileController;
	}


	private void FlipTile(TileController tileController)
	{
		List<TileController> adjacentTiles = GetAdjacentTIles(tileController.Position);

		tileController.Flip(true);

		foreach (TileController adjacentTile in adjacentTiles)
		{
			adjacentTile.Flip(true);
		}
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

    private bool IsLevelComplete
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

	#endregion


	#region Events

	private void OnTileCLickedHandler(TileController tileController)
	{
        if (!_levelCompleted)
        {
            FlipTile(tileController);

            if (OnTileClicked != null)
            {
                OnTileClicked(tileController);
            }

            if (IsLevelComplete && OnLevelComplete != null)
            {
                _levelCompleted = true;
                OnLevelComplete();
            }
        }
	}

	#endregion


}
