using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectionController : MenuScreenBaseController {

    #region Private Properties

    [SerializeField]
    private LevelTileController _levelTilePrefab;

    [SerializeField]
    private Transform _levelsTilesContent;

    [SerializeField]
    private List<GameObject> _activeLevelsTiles;

    [SerializeField]
    private int _rowWidth;

    [SerializeField]
    private float _margins;

    [SerializeField]
    private Vector3 _screenSize;

    [SerializeField]
    private int _currentChepterIndex;

    #endregion


    #region Initialization

    void Start()
    {
        if (_levelTilePrefab != null)
        {
            _levelTilePrefab.gameObject.SetActive(false);
        }

        Vector3 corner = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 10));
        _screenSize = new Vector2(corner.x * 2, corner.y * 2);

        GenerateLevelTiles();
    }

    #endregion


    #region Public

    public void BackButtonAction()
    {
        
    }

   

    #endregion


    #region Private

    private void GenerateLevelTiles()
    {
        ChepterDefenition chepter = LevelsSettigs.Instance.Chepters[_currentChepterIndex];
        GameSetupManager.Instance.CurrentChepter = chepter;
        List<LevelDefenition> levels = chepter.Levels;
        int count = 0;
        for (int y = 0; y < levels.Count / _rowWidth; y++)
        {
            for (int x = 0; x < _rowWidth; x++)
            {
                //LevelTileController tile = CreateTile(x, y, levels[count]);
				LevelTileController tile = CreateTile(levels[count]);
                count++;
            }
        }
    }


	private LevelTileController CreateTile(LevelDefenition levelDefenition)
	{
		LevelTileController newTile = Instantiate(_levelTilePrefab);

		newTile.gameObject.SetActive(true);

		newTile.transform.SetParent(_levelsTilesContent);

		newTile.transform.localScale = Vector3.one;

		newTile.SetLevel(levelDefenition);

		newTile.OnLevelTileSelected += LevelTileSelectedHandler;

		return newTile;
	}

    private LevelTileController CreateTile(int x, int y, LevelDefenition levelDefenition)
    {
        LevelTileController newTile = Instantiate(_levelTilePrefab);
        newTile.gameObject.SetActive(true);

        newTile.transform.SetParent(_levelsTilesContent);

        float tileScale = (_screenSize.x * 0.8f - (_margins * (_rowWidth - 1))) / _rowWidth;

        newTile.transform.localScale = Vector3.one * tileScale;

        newTile.transform.localPosition = new Vector3(x * tileScale - (_rowWidth * tileScale / 2f) + (tileScale * 0.5f) + _margins * x - (_margins * (_rowWidth - 1) * 0.5f),
                                                      -y * tileScale + (_rowWidth * tileScale / 2f) - (tileScale * 0.5f) - _margins * y + (_margins * (_rowWidth - 1) * 0.5f), 0 );

        newTile.SetLevel(levelDefenition);

        newTile.OnLevelTileSelected += LevelTileSelectedHandler;

        return newTile;
    }

    private void LevelTileSelectedHandler(LevelTileController levelTileController)
    {
        FlowManager.Instance.StartLevel(levelTileController.LevelDefenition);
    }

    #endregion
}
