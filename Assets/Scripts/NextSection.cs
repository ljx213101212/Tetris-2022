using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class NextSection : MonoBehaviour
{
  public Tilemap tilemap { get; private set; }
  public Board mainBoard;
  //   public TetrominoData[] tetrominoes;

  public Vector2Int boardSize = new Vector2Int(6, 12);

  public static Vector3Int nextPosition1 = new Vector3Int(-1, 5, 0);
  public static Vector3Int nextPosition2 = new Vector3Int(-1, 1, 0);
  public static Vector3Int nextPosition3 = new Vector3Int(-1, -3, 0);
  public static Vector3Int nextPosition4 = new Vector3Int(-1, -6, 0);

  public Vector3Int[] nextPositions = new Vector3Int[] { nextPosition1, nextPosition2, nextPosition3, nextPosition4 };
  public RectInt Bounds
  {
    get
    {
      Vector2Int position = new Vector2Int(-boardSize.x / 2, -boardSize.y / 2);
      return new RectInt(position, boardSize);
    }
  }

  private void Awake()
  {
    tilemap = GetComponentInChildren<Tilemap>();

  }

  void Start()
  {
    mainBoard.spawnPieceEvent.AddListener(UpdateNextSection);
  }

  void UpdateNextSection()
  {
    tilemap.ClearAllTiles();
    for (int i = 0; i < mainBoard.tetrominoQueue.Count; i++)
    {
      Set(mainBoard.tetrominoQueue[i], nextPositions[i]);
    }
  }
  public void Set(TetrominoData data, Vector3Int nextPosition)
  {
    for (int i = 0; i < data.cells.Length; i++)
    {
      Vector3Int next1Position = (Vector3Int)data.cells[i] + nextPosition;
      tilemap.SetTile(next1Position, data.tile);
    }
  }

}
