using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

public class FlippitLevelEditor : EditorWindow {

	#region Private Properties

	[SerializeField]
	private List<LevelDefenition> _levels;

	private Vector2 _levelsScrollPosition = Vector2.zero;
	private Vector2 _movesScrollPosition = Vector2.zero;

	[SerializeField]
	private ReorderableList _levelsReordableList = null;

	[SerializeField]
	private ReorderableList _movesReordableList = null;

	[SerializeField]
	private LevelDefenition _selectedLevel = null;

	private Color _frontColor = Color.blue;
	private Color _backColor = Color.green;
	private int _tileDisplaySize = 40;

    private bool _markMoves = false;

	#endregion


	#region Menu Item

	[MenuItem("Flippit/Levels Editor")]
	public static void OpenEditorWindow()
	{
		EditorWindow.GetWindow<FlippitLevelEditor>();
	}

	#endregion


	#region GUI

	void OnGUI()
	{
		if (_levels == null)
		{
			_levels = LevelsSettigs.Instance.Levels;
		}

		EditorGUILayout.BeginHorizontal();
		{

			// levels list
			DrawLevelsListPanel();

			EditorGUILayout.BeginVertical("Box");
			{

				// menu
				DrawMenuPanel();


				if (_selectedLevel != null && _selectedLevel.BoardSize != null  && _selectedLevel.BoardSetup.Count == (_selectedLevel.BoardSize.x * _selectedLevel.BoardSize.y))
				{
					// level editor
					DrawLevelsEditorPanel();
				}
				else
				{
                    if (_selectedLevel != null)
                    {
                        if (_selectedLevel.BoardSetup == null)
                        {
                            _selectedLevel.BoardSetup = new List<TileDefenition>();
                        }

                        List<TileDefenition> tiles = new List<TileDefenition>();
                        for (int x = 0; x < _selectedLevel.BoardSize.x; x++)
                        {
                            for (int y = 0; y < _selectedLevel.BoardSize.y; y++)
                            {
                                TileDefenition tile = GetTileAtPosition(new Vector2(x, y));
                                if (tile == null)
                                {
                                    tile = new TileDefenition();
                                    tile.Position = new Vector2(x, y);
                                    tile.IsFlipped = false;
                                }
                                tiles.Add(tile);
                            }

                        }
                        _selectedLevel.BoardSetup = tiles;
                    }
					
				}
			}
			EditorGUILayout.EndVertical();




		}
		EditorGUILayout.EndHorizontal();

       
	}

	#endregion


	#region Private


	private void DrawLevelsListPanel()
	{
		EditorGUILayout.BeginVertical("Box", GUILayout.Width(position.width * 0.25f));
		{
			_levelsScrollPosition = EditorGUILayout.BeginScrollView(_levelsScrollPosition);
			{
				if (_levelsReordableList == null)
				{

					_levelsReordableList = new ReorderableList(_levels, typeof(LevelDefenition), true, false, true, true);

					_levelsReordableList.drawElementCallback = (rect, index, active, focused) => 
					{
						LevelDefenition level = _levels[index];
						EditorGUI.LabelField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), level.LevelID.ToString());

					};

					_levelsReordableList.onSelectCallback = (list)=>
					{
						_selectedLevel = _levels[list.index].Copy();
						_movesReordableList = null;
					};

					_levelsReordableList.onAddCallback = (list)=>
					{
						LevelDefenition newLevel = new LevelDefenition();
						newLevel.Steps = new List<Vector2>();
						newLevel.BoardSetup = new List<TileDefenition>();
						newLevel.LevelID = list.count + 1;
						_levels.Add(newLevel);
					};

					_levelsReordableList.onReorderCallback = (list)=>
					{
						for (int i =0; i< _levels.Count; i++)
						{
							LevelDefenition level = _levels[i];
							level.LevelID = i + 1;
						}
					};
				}

				if (_levelsReordableList != null)
				{
					_levelsReordableList.DoLayoutList();
				}
			}
			EditorGUILayout.EndScrollView();
		}
		EditorGUILayout.EndVertical();
	}


	private void DrawMenuPanel()
	{
		EditorGUILayout.BeginHorizontal("Box");
		{
			if (_selectedLevel != null)
			{
				EditorGUILayout.BeginVertical();
				{
					
					_selectedLevel.LevelID = EditorGUILayout.IntField("Level ID", _selectedLevel.LevelID);

					EditorGUILayout.BeginHorizontal("Box");
					{
						_selectedLevel.BoardSize = EditorGUILayout.Vector2Field("Board Size", _selectedLevel.BoardSize);

					}
					EditorGUILayout.EndHorizontal();


					EditorGUILayout.BeginHorizontal();
					{
						if (GUILayout.Button("Clear"))
						{
							foreach (TileDefenition tile in _selectedLevel.BoardSetup)
							{
								tile.IsFlipped = false;
								tile.TileType = eTileType.Normal;
							}
							_selectedLevel.Steps.Clear();
						}
						if (GUILayout.Button("Reset"))
						{
							if (_levelsReordableList != null && _levelsReordableList.index > 0)
							{
								_selectedLevel = _levels[_levelsReordableList.index].Copy();
								_movesReordableList = null;
							}
						}

						if (GUILayout.Button("Save"))
						{
							if (_selectedLevel != null && _levelsReordableList != null && _levelsReordableList.index > 0)
							{
								int saveIndex = _levelsReordableList.index;
								_levels[saveIndex] = _selectedLevel.Copy();

                                LevelsSettigs.Instance.Levels = _levels;

                                EditorUtility.SetDirty(LevelsSettigs.Instance);

                                Debug.Log("saved");
							}

						}
					}
					EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal("Box");
                    {
                        _selectedLevel.Stars3Steps = EditorGUILayout.IntField(_selectedLevel.Stars3Steps);
                        _selectedLevel.Stars2Steps = EditorGUILayout.IntField(_selectedLevel.Stars2Steps);
                        _selectedLevel.Stars1Steps = EditorGUILayout.IntField(_selectedLevel.Stars1Steps); 
                        if (GUILayout.Button("Auto Set"))
                        {
                            int movesCount = _selectedLevel.Steps.Count;
                            _selectedLevel.Stars3Steps = movesCount;
                            _selectedLevel.Stars2Steps = Mathf.RoundToInt(movesCount * 1.8f);
                            _selectedLevel.Stars1Steps = _selectedLevel.Stars2Steps + movesCount;
                        }
                    }
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    {
                        _markMoves = EditorGUILayout.Toggle("Show Moves", _markMoves);
                    }
                    EditorGUILayout.EndHorizontal();

				}
				EditorGUILayout.EndVertical();
			}
		}
		EditorGUILayout.EndHorizontal();
	}


	private void DrawLevelsEditorPanel()
	{
		EditorGUILayout.BeginHorizontal("Box");
		{

			if (_selectedLevel != null)
			{
				EditorGUILayout.BeginVertical("Box", GUILayout.Width(_tileDisplaySize * _selectedLevel.BoardSize.x));
				{
					if (_selectedLevel.BoardSetup != null && _selectedLevel.BoardSetup.Count > 0)
					{
						int tileCount = 0;

                        for (int y = (int)_selectedLevel.BoardSize.y -1 ; y >= 0 ; y-- )  
                        {
                            for (int x = 0; x < _selectedLevel.BoardSize.x; x++)
                            {
                                TileDefenition tileDefenition = GetTileAtPosition(new Vector2(x, y));

                                if (tileCount == 0)
                                {
                                    GUILayout.BeginHorizontal();
                                }

                                if (_markMoves && _selectedLevel.Steps.Contains(tileDefenition.Position))
                                {
                                    GUI.color = Color.magenta;
                                }

                                EditorGUILayout.BeginVertical("Box");
                                {


                                    if (tileDefenition.IsFlipped)
                                    {
                                        GUI.color = _backColor;
                                    }
                                    else if (tileDefenition.TileType == eTileType.Empty)
                                    {
                                        GUI.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
                                    }
                                    else
                                    {
                                        GUI.color = _frontColor;
                                    }

                                    string label = "";
                                    switch (tileDefenition.TileType)
                                    {
                                        case eTileType.Normal:
                                            {
                                                label = "";
                                                break;
                                            }
                                        case eTileType.Empty:
                                            {
                                                break;
                                            }
                                        case eTileType.LineHorizontal:
                                            {
                                                label = "⇔";
                                                break;
                                            }
                                        case eTileType.LineVertial:
                                            {
                                                label = "⇕";
                                                break;
                                            }
                                        case eTileType.Locked:
                                            {
                                                label = "L";
                                                break;
                                            }
                                        case eTileType.LinkA:
                                            {
                                                label = "A";
                                                break;
                                            }
                                        case eTileType.LinkB:
                                            {
                                                label = "B";
                                                break;
                                            }
                                        case eTileType.LinkC:
                                            {
                                                label = "C";
                                                break;
                                            }
                                        case eTileType.LinkD:
                                            {
                                                label = "D";
                                                break;
                                            }
                                    }

                                    if (GUILayout.Button(label, GUILayout.Width(_tileDisplaySize), GUILayout.Height(_tileDisplaySize)))
                                    {
                                        if (Event.current.button == 0)
                                        {
                                            Flip(tileDefenition);
                                        }
                                        else if (Event.current.button == 1)
                                        {

                                            GenericMenu menu = new GenericMenu();
                                            menu.AddItem(new GUIContent("Normal"), tileDefenition.TileType == eTileType.Normal, () =>
                                            {
                                                tileDefenition.TileType = eTileType.Normal;
                                            });
                                            menu.AddItem(new GUIContent("Empty"), tileDefenition.TileType == eTileType.Empty, () =>
                                            {
                                                tileDefenition.TileType = eTileType.Empty;
                                            });
                                            menu.AddSeparator("");
                                            menu.AddItem(new GUIContent("Special/Horizontal"), tileDefenition.TileType == eTileType.LineHorizontal, () =>
                                            {
                                                tileDefenition.TileType = eTileType.LineHorizontal;
                                            });
                                            menu.AddItem(new GUIContent("Special/Vertical"), tileDefenition.TileType == eTileType.LineVertial, () =>
                                            {
                                                tileDefenition.TileType = eTileType.LineVertial;
                                            });
                                            menu.AddItem(new GUIContent("Special/Locked"), tileDefenition.TileType == eTileType.Locked, () =>
                                            {
                                                tileDefenition.TileType = eTileType.Locked;
                                            });
                                            menu.AddItem(new GUIContent("Special/Linked/A"), tileDefenition.TileType == eTileType.LinkA, () =>
                                            {
                                                tileDefenition.TileType = eTileType.LinkA;
                                            });
                                            menu.AddItem(new GUIContent("Special/Linked/B"), tileDefenition.TileType == eTileType.LinkB, () =>
                                            {
                                                tileDefenition.TileType = eTileType.LinkB;
                                            });
                                            menu.AddItem(new GUIContent("Special/Linked/C"), tileDefenition.TileType == eTileType.LinkC, () =>
                                            {
                                                tileDefenition.TileType = eTileType.LinkC;
                                            });
                                            menu.AddItem(new GUIContent("Special/Linked/D"), tileDefenition.TileType == eTileType.LinkD, () =>
                                            {
                                                tileDefenition.TileType = eTileType.LinkD;
                                            });
                                            menu.AddSeparator("");
                                            menu.AddItem(new GUIContent("Clear"), false, () =>
                                            {
                                                tileDefenition.TileType = eTileType.Normal;
                                            });
                                            menu.ShowAsContext();
                                            Event.current.Use();
                                        }
                                    }



                                }
                                EditorGUILayout.EndVertical();

                                GUI.color = Color.white;

                                if (tileCount == _selectedLevel.BoardSize.x - 1)
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
					}


				}
				EditorGUILayout.EndVertical();


				EditorGUILayout.BeginVertical(GUILayout.Width(100));
				{
					// moves list
					if (_selectedLevel.Steps != null && _selectedLevel.Steps.Count > 0)
					{
						if (_movesReordableList == null)
						{
							_movesReordableList = new ReorderableList(_selectedLevel.Steps, typeof(Vector2), true, true, false, true);

							_movesReordableList.drawHeaderCallback = (rect) =>
							{
								EditorGUI.LabelField(rect, "Moves");
							};
						}

						if (_movesReordableList != null)
						{
							_movesReordableList.DoLayoutList();
						}
					}
				}
				EditorGUILayout.EndVertical();
			}
		}
		EditorGUILayout.EndHorizontal();
	}

	#endregion


	#region Private - Gameplay simultaion

	private void Flip(TileDefenition tile)
	{
		List <TileDefenition> tilesToFlip = new List<TileDefenition>();

        tilesToFlip.Add(tile);

        tilesToFlip.AddRange(GetAdjacentTiles(tile.Position));

        while (AddSpecialTiles(tilesToFlip));

        foreach (TileDefenition flippedTile in tilesToFlip)
		{
			if (flippedTile != null)
			{
				flippedTile.IsFlipped = !flippedTile.IsFlipped;
			}
		}

		if (!_selectedLevel.Steps.Contains(tile.Position))
		{
			_selectedLevel.Steps.Add(tile.Position);
		}
		else
		{
			_selectedLevel.Steps.Remove(tile.Position);
		}
	}

	private List<TileDefenition> GetAdjacentTiles(Vector2 position)
	{
		List<TileDefenition> adjacentTiles = new List<TileDefenition>();

		adjacentTiles.Add(GetTileAtPosition(position + new Vector2(0,1)));
		adjacentTiles.Add(GetTileAtPosition(position + new Vector2(0,-1)));
		adjacentTiles.Add(GetTileAtPosition(position + new Vector2(1,0)));
		adjacentTiles.Add(GetTileAtPosition(position + new Vector2(-1,0)));

		return adjacentTiles;
	}

    private bool AddSpecialTiles(List<TileDefenition> tilesToFlip)
    {
        List<TileDefenition> tilesToAdd = new List<TileDefenition>();

        foreach (TileDefenition flippingTile in tilesToFlip)
        {
            if (flippingTile != null)
            {
                switch (flippingTile.TileType)
                {
                    case eTileType.Empty:
                    case eTileType.Normal:
                        {
                            break;
                        }
                    case eTileType.LineVertial:
                        {
                            foreach (TileDefenition tile in _selectedLevel.BoardSetup)
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
                            foreach (TileDefenition tile in _selectedLevel.BoardSetup)
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
                    case eTileType.LinkA:
                    case eTileType.LinkB:
                    case eTileType.LinkC:
                    case eTileType.LinkD:
                        {
                            foreach (TileDefenition tile in _selectedLevel.BoardSetup)
                            {
                                if (tile.TileType == flippingTile.TileType && !tilesToFlip.Contains(tile))
                                {
                                    tilesToAdd.Add(tile);
                                }
                            }
                            break;
                        }
                }
            }
           
        }

        if (tilesToAdd.Count > 0)
        {
            tilesToFlip.AddRange(tilesToAdd);
        }

        return (tilesToAdd.Count > 0);
    }

	private TileDefenition GetTileAtPosition(Vector2 position)
	{
		foreach (TileDefenition tile in _selectedLevel.BoardSetup)
		{
			if (tile.Position == position)
			{
				return tile;
			}
		}
		return null;
	}

	#endregion

}
