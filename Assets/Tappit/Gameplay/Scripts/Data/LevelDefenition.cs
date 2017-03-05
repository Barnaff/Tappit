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
}

[System.Serializable]
public class TileDefenition
{
    public Vector2 Position;

    public eTileType TileType;

    public bool IsFlipped;
}





