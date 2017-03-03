using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelDefenition  
{
	public int LevelID;

	public Vector2 BoardSize;

	public List<Vector2> Steps;

	public int[,] StartingPosition;

	public int Stars3Steps;

	public int Stars2Steps;

	public int Stars1Steps;
}



