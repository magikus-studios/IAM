using UnityEngine;
using UnityEngine.Tilemaps;

namespace IAM.Tetris
{
    public class Piece
    {
        public Vector3Int[] Cells;
        public Vector3Int Position;
        public Tetromino Tetromino;
        public Vector2Int[,] WallKicks;
        public int RotationIndex;

        public int GetWallKickIndex(int rotationSense) { return Tetromino.GetWallKickIndex(RotationIndex, rotationSense); }

        public Piece(Vector3Int position, int rotationIndex, Tetromino tetromino)
        {
            Position = position;
            RotationIndex = rotationIndex;
            Tetromino = tetromino;

            WallKicks = tetromino.WallKicks;
            Cells = new Vector3Int[tetromino.Cells.Length];
            for (int i = 0; i < tetromino.Cells.Length; i++) { Cells[i] = (Vector3Int)tetromino.Cells[i]; }
        }
        private Piece(Piece piece)
        {
            Position = piece.Position;
            RotationIndex = piece.RotationIndex;
            Tetromino = piece.Tetromino;

            WallKicks = piece.WallKicks;
            Cells = new Vector3Int[piece.Cells.Length];
            for (int i = 0; i < piece.Cells.Length; i++) { Cells[i] = piece.Cells[i]; }
        }

        public void Rotate(int rotationSense) 
        {
            RotationIndex += rotationSense;
            RotationIndex = RotationIndex.Wrap(0, 4);
            for (int i = 0; i < Cells.Length; i++) { Cells[i] = Tetromino.Rotate(rotationSense, Cells[i]); }
        }
        public void ClearFromTilemap(Tilemap tilemap) { for (int i = 0; i < Cells.Length; i++) { tilemap.SetTile(Cells[i] + Position, null); } }
        public void SetToTilemap(Tilemap tilemap, Tile tile) { for (int i = 0; i < Cells.Length; i++) { tilemap.SetTile(Cells[i] + Position, tile); } }
        public void SetToTilemap(Tilemap tilemap) { for (int i = 0; i < Cells.Length; i++) { tilemap.SetTile(Cells[i] + Position, Tetromino.Tile); } }
        public Piece Copy() { return new Piece(this); }
    }
}