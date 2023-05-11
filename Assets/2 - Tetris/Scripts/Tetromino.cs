using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace IAM.Tetris
{
	[CreateAssetMenu(fileName = "Tetronimo", menuName = "IAM/Tetris/Tetronimo")]
	public class Tetromino : ScriptableObject
	{
		[field: SerializeField] public bool DifferRotation { get; private set; }
		[field: SerializeField] public bool DifferWallKick { get; private set; }
		[field: SerializeField] public Tile Tile { get; private set; }
		[field: SerializeField] public Vector2Int[] Cells { get; private set; }
		
		private static readonly float Cos = -Mathf.Cos(Mathf.PI / 2f);
		private static readonly float Sin = -Mathf.Sin(Mathf.PI / 2f);
		private static readonly float[] RotationMatrix = new float[] { Cos, Sin, -Sin, Cos};
		private static readonly Vector2Int[,] WallKicksI = new Vector2Int[,] {
			{ new Vector2Int(0, 0), new Vector2Int(-2, 0), new Vector2Int( 1, 0), new Vector2Int(-2,-1), new Vector2Int( 1, 2) },
			{ new Vector2Int(0, 0), new Vector2Int( 2, 0), new Vector2Int(-1, 0), new Vector2Int( 2, 1), new Vector2Int(-1,-2) },
			{ new Vector2Int(0, 0), new Vector2Int(-1, 0), new Vector2Int( 2, 0), new Vector2Int(-1, 2), new Vector2Int( 2,-1) },
			{ new Vector2Int(0, 0), new Vector2Int( 1, 0), new Vector2Int(-2, 0), new Vector2Int( 1,-2), new Vector2Int(-2, 1) },
			{ new Vector2Int(0, 0), new Vector2Int( 2, 0), new Vector2Int(-1, 0), new Vector2Int( 2, 1), new Vector2Int(-1,-2) },
			{ new Vector2Int(0, 0), new Vector2Int(-2, 0), new Vector2Int( 1, 0), new Vector2Int(-2,-1), new Vector2Int( 1, 2) },
			{ new Vector2Int(0, 0), new Vector2Int( 1, 0), new Vector2Int(-2, 0), new Vector2Int( 1,-2), new Vector2Int(-2, 1) },
			{ new Vector2Int(0, 0), new Vector2Int(-1, 0), new Vector2Int( 2, 0), new Vector2Int(-1, 2), new Vector2Int( 2,-1) },
		};
		private static readonly Vector2Int[,] WallKicksJLOSTZ = new Vector2Int[,] {
			{ new Vector2Int(0, 0), new Vector2Int(-1, 0), new Vector2Int(-1, 1), new Vector2Int(0,-2), new Vector2Int(-1,-2) },
			{ new Vector2Int(0, 0), new Vector2Int( 1, 0), new Vector2Int( 1,-1), new Vector2Int(0, 2), new Vector2Int( 1, 2) },
			{ new Vector2Int(0, 0), new Vector2Int( 1, 0), new Vector2Int( 1,-1), new Vector2Int(0, 2), new Vector2Int( 1, 2) },
			{ new Vector2Int(0, 0), new Vector2Int(-1, 0), new Vector2Int(-1, 1), new Vector2Int(0,-2), new Vector2Int(-1,-2) },
			{ new Vector2Int(0, 0), new Vector2Int( 1, 0), new Vector2Int( 1, 1), new Vector2Int(0,-2), new Vector2Int( 1,-2) },
			{ new Vector2Int(0, 0), new Vector2Int(-1, 0), new Vector2Int(-1,-1), new Vector2Int(0, 2), new Vector2Int(-1, 2) },
			{ new Vector2Int(0, 0), new Vector2Int(-1, 0), new Vector2Int(-1,-1), new Vector2Int(0, 2), new Vector2Int(-1, 2) },
			{ new Vector2Int(0, 0), new Vector2Int( 1, 0), new Vector2Int( 1, 1), new Vector2Int(0,-2), new Vector2Int( 1,-2) },
		};

		public Vector2Int[,] WallKicks
		{
			get
			{
				if (DifferWallKick) { return WallKicksI; }
				else { return WallKicksJLOSTZ; }
			}
		}

		public Vector3Int Rotate(int rotationSense, Vector3 cell) 
		{
			int x, y;
			if (DifferRotation) 
			{
				cell.x -= 0.5f;
				cell.y -= 0.5f;
				x = Mathf.CeilToInt(((cell.x * RotationMatrix[0]) + (cell.y * RotationMatrix[1])) * rotationSense);
				y = Mathf.CeilToInt(((cell.x * RotationMatrix[2]) + (cell.y * RotationMatrix[3])) * rotationSense);
			}
			else 
			{
				x = Mathf.RoundToInt(((cell.x * RotationMatrix[0]) + (cell.y * RotationMatrix[1])) * rotationSense);
				y = Mathf.RoundToInt(((cell.x * RotationMatrix[2]) + (cell.y * RotationMatrix[3])) * rotationSense);
			}
			return new Vector3Int(x, y, 0);
		}
		
		public int GetWallKickIndex(int rotationIndex, int rotationDirection)
		{
			int wallKickIndex = rotationIndex * 2;
			if (rotationDirection < 0) { wallKickIndex--; }
			return wallKickIndex.Wrap(0, WallKicks.GetLength(0));
		}
	}
}
