using UnityEngine;
using UnityEngine.Tilemaps;
using Util;

public class Board : MonoBehaviour
{
  public Tilemap tilemap { get; private set; }
  public Piece activePiece { get; private set; }
  public TetrominoData[] tetrominoes;
  public Vector2Int boardSize = new Vector2Int(10, 20);
  public Vector3Int spawnPosition = new Vector3Int(-1, 8, 0);

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
    activePiece = GetComponentInChildren<Piece>();

    for (int i = 0; i < tetrominoes.Length; i++)
    {
      tetrominoes[i].Initialize();
    }


    Debug.Log(TetrisUtil.GetProjectName() + "Awake!");
    Debug.Log(tetrominoes);
  }

  private void Start()
  {
    //DebugSpawnPiece();
    SpawnPiece();
  }


  public void DebugSpawnPiece()
  {
    int random = 0;

    TetrominoData data = tetrominoes[random];

    activePiece.Initialize(this, spawnPosition, data);
    Debug.Log("SpawnPiece:" + Time.time);

    //TODO: Show Gameover Dialog
    if (IsGameOver())
    {
      tilemap.ClearAllTiles();
    }
    else
    {
      Set(activePiece);
    }
  }

  public void SpawnPiece()
  {
    int random = Random.Range(0, tetrominoes.Length);

    TetrominoData data = tetrominoes[random];

    activePiece.Initialize(this, spawnPosition, data);
    Debug.Log("SpawnPiece:" + Time.time);

    //TODO: Show Gameover Dialog
    if (IsGameOver())
    {
      tilemap.ClearAllTiles();
    }
    else
    {
      Set(activePiece);
    }
  }

  public void Set(Piece piece)
  {
    for (int i = 0; i < piece.cells.Length; i++)
    {
      Vector3Int tilePosition = piece.cells[i] + piece.position;
      //For debugging rotation
      // piece.data.tile.color = new Color(0.5f + i / 10f, 0.5f + i / 10f, 0.5f + i / 10f);
      tilemap.SetTile(tilePosition, piece.data.tile);
    }
  }

  public void Clear(Piece piece)
  {
    for (int i = 0; i < piece.cells.Length; i++)
    {
      Vector3Int tilePosition = piece.cells[i] + piece.position;
      tilemap.SetTile(tilePosition, null);
    }
  }

  public bool IsOutOfBound(Piece piece, Vector3Int newPosition)
  {
    for (int i = 0; i < piece.cells.Length; i++)
    {
      Vector3Int newTilePosition = piece.cells[i] + newPosition;
      //1. An out of bounds tile is invalid
      if (!Bounds.Contains((Vector2Int)newTilePosition))
      {
        return true;
      }
    }
    return false;
  }

  //The position is only valid if every cell is valid
  public bool IsPositionValid(Piece piece, Vector3Int newPosition)
  {
    for (int i = 0; i < piece.cells.Length; i++)
    {
      Vector3Int newTilePosition = piece.cells[i] + newPosition;
      //1. An out of bounds tile is invalid
      if (!Bounds.Contains((Vector2Int)newTilePosition))
      {
        return false;
      }

      //2. A tile already occupies the position, thus invalid
      if (tilemap.GetTile(newTilePosition))
      {
        return false;
      }
    }

    return true;
  }

  public bool IsGameOver()
  {
    return !IsPositionValid(activePiece, spawnPosition);
  }

  public bool IsLineFull(int row)
  {
    for (int x = Bounds.xMin; x < Bounds.xMax; x++)
    {
      Vector3Int currentCell = new Vector3Int(x, row, 0);
      if (!tilemap.HasTile(currentCell))
      {
        return false;
      }
    }
    return true;
  }

  public void LineClears()
  {
    Debug.Log("LineClears : " + tilemap.cellBounds.yMin + " " + tilemap.cellBounds.yMax);
    Debug.Log("Bounds : " + Bounds.yMin + " " + Bounds.yMax);
    int row = Bounds.yMin;
    while (row < Bounds.yMax)
    {
      if (IsLineFull(row))
      {
        ClearRow(row);
      }
      else
      {
        row++;
      }
    }
  }

  public void ClearRow(int row)
  {
    for (int x = Bounds.xMin; x < Bounds.xMax; x++)
    {
      Vector3Int currentCell = new Vector3Int(x, row, 0);
      tilemap.SetTile(currentCell, null);
    }

    for (int y = row; y < Bounds.yMax - 1; y++)
    {
      for (int x = Bounds.xMin; x < Bounds.xMax; x++)
      {
        Vector3Int sourcePosition = new Vector3Int(x, y + 1, 0);
        Vector3Int targetPosition = new Vector3Int(x, y, 0);

        TileBase sourceTile = tilemap.GetTile(sourcePosition);
        tilemap.SetTile(targetPosition, sourceTile);
      }
    }
  }
}
