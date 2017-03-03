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
			_boardController.OnTileClicked += OnTileClickedHandler;
		}

		if (_currentLevel == null)
		{
			_currentLevel = new LevelDefenition();
		}

		if (_boardController != null)
		{
			GUILayout.BeginVertical("Box");
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

				_boardController.BoardSize = EditorGUILayout.Vector2Field("Board Size", _boardController.BoardSize);
				_currentLevel.BoardSize = _boardController.BoardSize;
			}
			GUILayout.EndVertical();


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
	}

	private void ExportLevelLayout()
	{
		if (_currentLevel != null)
		{
			_currentLevel.StartingPosition = new int[(int)_currentLevel.BoardSize.x,(int)_currentLevel.BoardSize.y];
			for (int y = 0; y < _currentLevel.BoardSize.y; y++)
			{
				for (int x = 0; x < _currentLevel.BoardSize.x; x++)
				{
					_currentLevel.StartingPosition[y,x] = _boardController.BoardTiles[y,x].IsFlipped ? 1 : 0;
				}
			}
		}

		string output = "";
		for (int y = 0; y < _currentLevel.BoardSize.y; y++)
		{
			for (int x = 0; x < _currentLevel.BoardSize.x; x++)
			{
				output += _currentLevel.StartingPosition[y,x].ToString();
			}
			output += "\n";
		}

		Debug.Log(output);
	}

	#region Events

	private void OnTileClickedHandler(TileController tileController)
	{
		if (_currentLevel != null)
		{
			_currentLevel.Steps.Add(tileController.Position);
			Repaint();
		}
	}

	#endregion

}
