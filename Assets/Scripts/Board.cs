using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.Events;
using System.Collections.Generic;

public class Board : MonoBehaviour
{
  public Tilemap tilemap { get; private set; }
  public Piece activePiece { get; private set; }
  public TetrominoData[] tetrominoes;

  public List<TetrominoData> tetrominoQueue = new List<TetrominoData>();

  public int tetrominoQueueLength = 4;

  public UnityEvent spawnPieceEvent;

  public SoundEffectsPlayer soundPlayer;

  public Vector2Int boardSize = new Vector2Int(10, 20);
  public Vector3Int spawnPosition = new Vector3Int(-1, 8, 0);

  bool m_Play;

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
    spawnPieceEvent = new UnityEvent();
    soundPlayer = GetComponentInChildren<SoundEffectsPlayer>();
    for (int i = 0; i < tetrominoes.Length; i++)
    {
      tetrominoes[i].Initialize();
    }
  }

  private void Start()
  {
    m_Play = false;
    InitializeTetrominoQueue();
    SpawnPiece();
  }

  void Update()
  {
    if (!PauseMenu.GameIsPaused && !m_Play)
    {
      soundPlayer.PlayBGM();
      m_Play = true;
    }

    if (PauseMenu.GameIsPaused && m_Play)
    {
      Debug.Log("shoud pause bgm");
      soundPlayer.PauseBGM();
      m_Play = false;
    }
  }


  private void InitializeTetrominoQueue()
  {

    for (int i = 0; i < tetrominoQueueLength; i++)
    {
      this.tetrominoQueue.Add(Util.TetrisUtil.GetRandomTetromino(tetrominoes));
    }
  }

  public void SpawnPiece()
  {

    TetrominoData data = this.tetrominoQueue[0];
    this.tetrominoQueue.RemoveAt(0);
    this.tetrominoQueue.Add(Util.TetrisUtil.GetRandomTetromino(tetrominoes));
    spawnPieceEvent.Invoke();

    activePiece.Initialize(this, spawnPosition, data);

    //TODO: Show Gameover Dialog
    if (IsGameOver())
    {
      this.soundPlayer.PlayGameOverAudio();
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

  public int LineClears()
  {
    int clearedLines = 0;
    int row = Bounds.yMin;
    while (row < Bounds.yMax)
    {
      if (IsLineFull(row))
      {
        ClearRow(row);
        clearedLines += 1;
      }
      else
      {
        row++;
      }
    }
    return clearedLines;
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
