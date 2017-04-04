using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelDefenition  
{
	public int LevelID;
    public int ChecpterID;
	public Vector2 BoardSize;
	public List<Vector2> Steps;
    public List<TileDefenition> BoardSetup;
    public int Stars3Steps;
	public int Stars2Steps;
	public int Stars1Steps;
    public string TopTutorialTitle;
    public string BottomTutorialTitle;


	public LevelDefenition Copy()
	{
		LevelDefenition copy = new LevelDefenition();

		copy.LevelID = LevelID;
		copy.ChecpterID = ChecpterID;
		copy.BoardSize = new Vector2(BoardSize.x, BoardSize.y);

		if (Steps != null)
		{
			copy.Steps = new List<Vector2>();
			foreach (Vector2 step in Steps)
			{
				copy.Steps.Add(new Vector2(step.x, step.y));
			}
		}

		if (BoardSetup != null)
		{
			copy.BoardSetup = new List<TileDefenition>();
			foreach (TileDefenition tile in BoardSetup)
			{
				copy.BoardSetup.Add(tile.Copy());
			}
		}

		copy.Stars1Steps = Stars1Steps;
		copy.Stars2Steps = Stars2Steps;
		copy.Stars3Steps = Stars3Steps;
		copy.TopTutorialTitle = TopTutorialTitle;
		copy.BottomTutorialTitle = BottomTutorialTitle;
		return copy;
	}
}


[System.Serializable]
public class TileDefenition
{
    public Vector2 Position;
    public eTileType TileType;
    public bool IsFlipped;

	public TileDefenition Copy()
	{
		TileDefenition copy = new TileDefenition();
		copy.Position = new Vector2(Position.x, Position.y);
		copy.TileType = TileType;
		copy.IsFlipped = IsFlipped;
		return copy;
	}
}





