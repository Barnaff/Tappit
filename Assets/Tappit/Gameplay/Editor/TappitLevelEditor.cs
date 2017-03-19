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

    [SerializeField]
    private bool _isRecording = false;

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


		if (_boardController != null && Application.isPlaying)
		{
            GUILayout.BeginHorizontal();
            {
                GUILayout.BeginVertical("Box", GUILayout.Width(100));
                {
                    _levelsScrollPosition = GUILayout.BeginScrollView(_levelsScrollPosition);
                    {
                        GUILayout.BeginVertical();
                        {
                            foreach (LevelDefenition level in _levelsSettings.Levels)
                            {
                                if (level == _currentLevel)
                                {
                                    GUI.color = Color.green;
                                }

                                GUILayout.BeginHorizontal("Box");
                                {
                                    if (GUILayout.Button(level.ChecpterID + "-" + level.LevelID))
                                    {
                                        _currentLevel = level;
                                        if (_boardController != null)
                                        {
                                            _boardController.BuildLevel(_currentLevel);
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

                                GUI.color = Color.white;
                            }

                            if (GUILayout.Button("Add Level"))
                            {
                                LevelDefenition newLevel = new LevelDefenition();
                                newLevel.BoardSetup = new List<TileDefenition>();

                                _levelsSettings.Levels.Add(newLevel);
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
                            _boardController.BuildLevel(_currentLevel);
                        }

                        if (GUILayout.Button("Clear Board"))
                        {
                            foreach (TileDefenition tiledefenition in _currentLevel.BoardSetup)
                            {
                                tiledefenition.IsFlipped = false;
                            }
                            _currentLevel.Steps.Clear();
                            _boardController.BuildLevel(_currentLevel);
                        }

                        _isRecording = EditorGUILayout.Toggle("Record", _isRecording, "Button");

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

                        GUILayout.BeginVertical("Box");
                        {
                            GUILayout.Label("Stars");

                            GUILayout.BeginHorizontal();
                            {
                                _currentLevel.Stars1Steps = EditorGUILayout.IntField(_currentLevel.Stars1Steps);
                                _currentLevel.Stars2Steps = EditorGUILayout.IntField(_currentLevel.Stars2Steps);
                                _currentLevel.Stars3Steps = EditorGUILayout.IntField(_currentLevel.Stars3Steps);

                            }
                            GUILayout.EndHorizontal();
                        }
                        GUILayout.EndVertical();

                        _boardController.BoardSize = EditorGUILayout.Vector2Field("Board Size", _boardController.BoardSize);
                        _currentLevel.BoardSize = _boardController.BoardSize;


                        GUILayout.BeginVertical("Box");
                        {
                            GUILayout.BeginVertical("Box");
                            {
                                int tileCount = 0;
                                for (int i = _currentLevel.BoardSetup.Count - 1; i >= 0 ; i--)
                                {
                                    if (tileCount == 0)
                                    {
                                        GUILayout.BeginHorizontal("Box");
                                    }


                                    TileDefenition tileDefenition = _currentLevel.BoardSetup[i];
                                    tileDefenition.TileType = (eTileType)EditorGUILayout.EnumPopup(tileDefenition.TileType);

                                    if (tileCount == _currentLevel.BoardSize.x - 1)
                                    {
                                        GUILayout.EndHorizontal();
                                        tileCount = 0;
                                    }
                                    else
                                    {
                                        tileCount++;
                                    }
                                }
                            }
                            GUILayout.EndVertical();


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

    private void UpdateStarsCount()
    {
        int movesCount = _currentLevel.Steps.Count;

        _currentLevel.Stars3Steps = movesCount;
        _currentLevel.Stars2Steps = Mathf.RoundToInt(movesCount * 1.8f);
        _currentLevel.Stars1Steps = _currentLevel.Stars2Steps + movesCount;
    }

	#region Events

	private void OnTileClickedHandler(TileController tileController)
	{
		if (_currentLevel != null  && _isRecording)
		{
			_currentLevel.Steps.Add(tileController.Position);
            ExportLevelLayout();
            UpdateStarsCount();
            Repaint();
		}
	}

	#endregion

}
