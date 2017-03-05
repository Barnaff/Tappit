using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TappitLevelEditor : EditorWindow {

	#region Private Properties

	[SerializeField]
	private BoardController _boardController = null;

	[SerializeField]
	private LevelDefenition _currentLevel;

	private Vector2 _historyScrollPosition = Vector2.zero;

    private Vector2 _levelsScrollPosition = Vector2.zero;

    [SerializeField]
    private LevelsSettigs _levelsSettings;

	#endregion


	[MenuItem("Tappit/Levels Editor")]
	public static void OpenEditorWindow()
	{
		EditorWindow.GetWindow<TappitLevelEditor>();
	}


	void OnGUI()
	{
		if (_boardController == null)
		{
			_boardController = GameObject.FindObjectOfType<BoardController>();
            if (_boardController != null)
            {
                _boardController.OnTileClicked += OnTileClickedHandler;
            }
		}

        if (_levelsSettings == null)
        {
            _levelsSettings = LevelsSettigs.Instance;
        }


		if (_boardController != null)
		{
            GUILayout.BeginHorizontal();
            {
                GUILayout.BeginVertical("Box");
                {
                    if (GUILayout.Button("Add Level"))
                    {
                        _currentLevel = new LevelDefenition();
                        _levelsSettings.Levels.Add(_currentLevel);
                    }
                        

                    _levelsScrollPosition = GUILayout.BeginScrollView(_levelsScrollPosition);
                    {
                        GUILayout.BeginVertical();
                        {
                            foreach (LevelDefenition level in _levelsSettings.Levels)
                            {
                                GUILayout.BeginHorizontal("Box");
                                {
                                    if (GUILayout.Button(level.ChecpterID + "-" + level.LevelID))
                                    {
                                        _currentLevel = level;
                                        if (_boardController != null)
                                        {
                                            _boardController.InitWithLevel(_currentLevel);
                                        }
                                    }
                                    if (GUILayout.Button("X"))
                                    {
                                        _levelsSettings.Levels.Remove(level);
                                        GUILayout.EndHorizontal();
                                        break;
                                    }
                                }
                                GUILayout.EndHorizontal();
                            }
                        }
                        GUILayout.EndVertical();

                    }
                    GUILayout.EndScrollView();
                }
                GUILayout.EndVertical();


                GUILayout.BeginVertical("Box");
                {
                    if (_currentLevel != null)
                    {
                        if (GUILayout.Button("Reset"))
                        {
                            _boardController.Reset();
                        }

                        if (GUILayout.Button("Start Record"))
                        {

                        }

                        if (GUILayout.Button("Export"))
                        {
                            ExportLevelLayout();
                        }

                        GUILayout.BeginHorizontal();
                        {
                            _currentLevel.ChecpterID = EditorGUILayout.IntField( _currentLevel.ChecpterID);
                            GUILayout.Label("-");
                            _currentLevel.LevelID = EditorGUILayout.IntField(_currentLevel.LevelID);
                        }
                        GUILayout.EndHorizontal();
                       

                        _boardController.BoardSize = EditorGUILayout.Vector2Field("Board Size", _boardController.BoardSize);
                        _currentLevel.BoardSize = _boardController.BoardSize;


                        GUILayout.BeginVertical("Box");
                        {
                            GUILayout.BeginHorizontal();
                            {
                                GUILayout.Label("History");
                                if (GUILayout.Button("Clear"))
                                {
                                    _currentLevel.Steps.Clear();
                                }
                            }
                            GUILayout.EndHorizontal();

                            _historyScrollPosition = GUILayout.BeginScrollView(_historyScrollPosition);
                            {
                                GUILayout.BeginVertical();
                                {
                                    if (_currentLevel.Steps == null)
                                    {
                                        _currentLevel.Steps = new List<Vector2>();
                                    }
                                    foreach (Vector2 historyEntry in _currentLevel.Steps)
                                    {
                                        if (GUILayout.Button(historyEntry.x + "," + historyEntry.y))
                                        {

                                        }
                                    }
                                }
                                GUILayout.EndVertical();
                            }
                            GUILayout.EndScrollView();
                        }
                        GUILayout.EndVertical();

                    }
                    GUILayout.EndVertical();
                }


            }
            GUILayout.EndHorizontal();

            EditorUtility.SetDirty(_levelsSettings);
		}
	}

	private void ExportLevelLayout()
	{
        List<TileDefenition> tiles = new List<TileDefenition>();

        for (int y = 0; y < _currentLevel.BoardSize.y; y++)
        {
            for (int x = 0; x < _currentLevel.BoardSize.x; x++)
            {
                TileDefenition tileDefenition = new TileDefenition();
                tileDefenition.Position = new Vector2(x, y);
                tileDefenition.IsFlipped = _boardController.BoardTiles[y, x].IsFlipped;

                tiles.Add(tileDefenition);
            }
        }

        _currentLevel.BoardSetup = tiles;

        EditorUtility.SetDirty(_levelsSettings);
    }

	#region Events

	private void OnTileClickedHandler(TileController tileController)
	{
		if (_currentLevel != null)
		{
			_currentLevel.Steps.Add(tileController.Position);
            ExportLevelLayout();

            Repaint();
		}
	}

	#endregion

}
