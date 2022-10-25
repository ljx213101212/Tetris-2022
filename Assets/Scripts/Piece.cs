using UnityEngine;
using System.Collections.Generic;

public class Piece : MonoBehaviour
{
  public Board board { get; private set; }
  public TetrominoData data { get; private set; }
  public Vector3Int[] cells { get; private set; }

  public Vector3Int[] tSpinCells { get; private set; }//could be null
  public Vector3Int position { get; private set; }

  private int rotationStatus = (int)ROTATION_STATUS.ORIGIN;
  public float stepDelay = 1f;
  public float lockDelay = 0.5f; // Lock Down Timer is 0.5 seconds
  private float stepTime;
  private float lockTime;
  private int dropLines;
  private int hardDropLines;
  private bool hardDroping = false;//Update every lock
  private SUPER_ROTATION_POINT latestSRSPoint = SUPER_ROTATION_POINT.POINT_0; //Update every lock

  private int backToBack = 0; // 0: default, 1: hold, 2: bonus

  private bool hasRotation5 = false; //Update every lock

  public void Initialize(Board board, Vector3Int position, TetrominoData data)
  {
    this.board = board;
    this.data = data;
    this.position = position;
    this.tSpinCells = null; // reset tSpin by default
    this.hasRotation5 = false;// reset has Rotation5
    this.backToBack = 0;

    this.stepTime = Time.time + stepDelay;
    this.lockTime = 0f;
    this.dropLines = 0;
    this.hardDropLines = 0;

    if (cells == null)
    {
      cells = new Vector3Int[data.cells.Length];
    }

    for (int i = 0; i < cells.Length; i++)
    {
      cells[i] = (Vector3Int)data.cells[i];
    }

    //check if its a T
    if (data.tetromino == Tetromino.T)
    {
      int tSpinCellCount = Data.TSpinCells.Length;
      tSpinCells = new Vector3Int[tSpinCellCount];
      for (int i = 0; i < tSpinCellCount; i++)
      {
        tSpinCells[i] = (Vector3Int)Data.TSpinCells[i];
      }
    }
  }

  //https://docs.unity3d.com/2019.3/Documentation/Manual/TimeFrameManagement.html
  void Update()
  {

    board.Clear(this);

    if (Input.GetKeyDown(KeyCode.LeftArrow) && !hardDroping)
    {
      if (Move(Vector2Int.left))
      {
        this.board.soundPlayer.PlayMoveAudio();
      }
    }
    if (Input.GetKeyDown(KeyCode.RightArrow) && !hardDroping)
    {
      if (Move(Vector2Int.right))
      {
        {
          this.board.soundPlayer.PlayMoveAudio();
        }
      }
    }
    if (Input.GetKeyDown(KeyCode.DownArrow) && !hardDroping)
    {
      Move(Vector2Int.down);
    }
    if (Input.GetKeyDown(KeyCode.Space))
    {
      HardDrop();
    }
    if (Input.GetKeyDown(KeyCode.A) && !hardDroping)
    {
      TryToRotate(Data.ROTATE_DIRECTION_VALUE[ROTATE_DIRECTION.LEFT]);
    }
    if (Input.GetKeyDown(KeyCode.D) && !hardDroping)
    {
      TryToRotate(Data.ROTATE_DIRECTION_VALUE[ROTATE_DIRECTION.RIGHT]);
    }

    //feature: step
    if (Time.time > this.stepTime)
    {
      this.Step();
    }

    if (this.lockTime > 0 && Time.time > this.lockTime + this.lockDelay)
    {
      this.Lock();
    }
    board.Set(this);
  }

  public bool Move(Vector2Int translation)
  {
    Vector3Int newPosition = position;
    newPosition.x += translation.x;
    newPosition.y += translation.y;

    bool isNextMoveValid = board.IsPositionValid(this, newPosition);

    if (isNextMoveValid)
    {
      int ydiff = (int)Mathf.Abs(newPosition.y - position.y);
      position = newPosition;
      dropLines += ydiff;
      return true;
    }
    return false;
  }

  private void Step()
  {

    this.stepTime = Time.time + this.stepDelay;

    //0.5 seconds
    if (!Move(Vector2Int.down))
    {
      this.lockTime = Time.time;
    }
  }

  private void Lock()
  {
    //Render Tile Map
    board.Set(this);

    //Feature: clear lines
    int clearedLines = board.LineClears();
    if (clearedLines > 0)
    {
      this.board.soundPlayer.PlayLineClearAudio();
    }

    //Feature: calculate points (before spawn piece)
    ScoreType type = GetScoreType(clearedLines, this.tSpinCells);
    bool isBackToBackBonus = IsBackToBackBonus(type);
    bool isBackToBackBreak = IsBackToBackBreak(type);
    updateBackToBack(isBackToBackBonus, isBackToBackBreak);
    ScoreManager.instance.CalculatePoint(type, 1, this.dropLines - this.hardDropLines, this.hardDropLines, isBackToBackBonus && this.backToBack == 2);

    //Init the next piece by using current Piece object
    board.SpawnPiece();

    this.lockTime = 0f; // reset lock time
    this.hardDroping = false; //reset hardDroping status
    this.latestSRSPoint = SUPER_ROTATION_POINT.POINT_0; // reset latest Super Rotation Point
    this.hasRotation5 = false;// reset has Rotation5
  }

  public void HardDrop()
  {
    hardDroping = true;
    while (Move(Vector2Int.down))
    {
      hardDropLines++;
    }

    this.stepTime = 0f;

    Debug.Log("this.board.soundPlayer", this.board.soundPlayer);
    this.board.soundPlayer.PlayHardDropAudio();
  }


  private void TryToRotate(int direction)
  {
    int originalRotatioStatus = rotationStatus;

    rotationStatus = Util.TetrisUtil.Wrap(rotationStatus - direction, 0, data.cells.GetLength(0));
    Rotate(direction, this.cells);

    int wallKickTestResult = TestWallKick(direction);
    //if all failed
    if (wallKickTestResult < 0)
    {
      rotationStatus = originalRotatioStatus;
      Rotate(-direction, this.cells);
    }
    //Rotate succesfully
    else
    {
      if (this.tSpinCells != null)
      {
        Rotate(direction, this.tSpinCells);
      }
      this.latestSRSPoint = Util.TetrisUtil.GetSuperRotationNumberByWallKickTest(wallKickTestResult);
      if (this.latestSRSPoint == SUPER_ROTATION_POINT.POINT_5)
      {
        this.hasRotation5 = true;
      }
      this.board.soundPlayer.PlayRotationAudio();
    }
  }

  //https://en.wikipedia.org/wiki/Rotation_matrix
  private void Rotate(int direction, Vector3Int[] cells)
  {
    for (int i = 0; i < cells.Length; i++)
    {
      Vector3 current = cells[i];

      int x1, y1;
      if (data.tetromino == Tetromino.I || data.tetromino == Tetromino.O)
      {
        //delta +0.5
        current.x -= 0.5f;
        current.y -= 0.5f;
        x1 = Mathf.CeilToInt(current.x * Data.cos * direction - current.y * Data.sin * direction);
        y1 = Mathf.CeilToInt(current.x * Data.sin * direction + current.y * Data.cos * direction);
      }
      else
      {
        x1 = Mathf.RoundToInt(current.x * Data.cos * direction - current.y * Data.sin * direction);
        y1 = Mathf.RoundToInt(current.x * Data.sin * direction + current.y * Data.cos * direction);
      }
      cells[i] = new Vector3Int(x1, y1, 0);
    }
  }

  //https://tetris.fandom.com/wiki/SRS
  /*
    returns:
    -1: All Super Rotation failed
    0: Rotation 1 
    1: Rotation 2 
    2: Rotation 3 
    3: Rotation 4 
    4: Rotation 5 
  */
  private int TestWallKick(int direction)
  {
    int wallKickIndex = Util.TetrisUtil.GetWallKickIndex(rotationStatus, (int)direction, data.wallKicks.GetLength(0));

    for (int i = 0; i < data.wallKicks.GetLength(1); i++)
    {
      Vector2Int translation = data.wallKicks[wallKickIndex, i];

      if (Move(translation))
      {
        return i;
      }
    }

    return -1;
  }

  public List<Vector3Int> GetCurrentCellPositions(Vector3Int position)
  {
    List<Vector3Int> positions = new List<Vector3Int>();
    for (int i = 0; i < this.cells.Length; i++)
    {
      Vector3Int currentCellPosition = this.cells[i] + position;
      positions.Add(currentCellPosition);
    }
    return positions;
  }

  public ScoreType GetScoreType(int clearedLevels, Vector3Int[] tSpinCells)
  {
    if (tSpinCells != null)
    {
      return GetTSpinType(clearedLevels, tSpinCells);
    }

    return GetNormalLineClearType(clearedLevels);
  }

  public ScoreType GetTSpinType(int clearedLevels, Vector3Int[] tSpinCells)
  {
    bool TouchA = this.board.tilemap.HasTile(tSpinCells[0]);
    bool TouchB = this.board.tilemap.HasTile(tSpinCells[1]);
    bool TouchC = this.board.tilemap.HasTile(tSpinCells[2]);
    bool TouchD = this.board.tilemap.HasTile(tSpinCells[3]);

    bool isTSpin = IsTSpin(TouchA, TouchB, TouchC, TouchD);
    bool isMiniTSpin = IsMiniTSpin(TouchA, TouchB, TouchC, TouchD);

    if (hasRotation5 && isMiniTSpin)
    {
      isTSpin = true;
    }

    if (isTSpin)
    {
      if (clearedLevels == 0)
      {
        return ScoreType.TSPIN;
      }
      if (clearedLevels == 1)
      {
        return ScoreType.TSPIN_SINGLE;
      }
      if (clearedLevels == 2)
      {
        return ScoreType.TSPIN_DOUBLE;
      }
      if (clearedLevels == 3)
      {
        return ScoreType.TSPIN_TRIPLE;
      }
      if (clearedLevels > 3)
      {
        Debug.Log("Bug check: TSpin with cleared Levels 3+");
        return ScoreType.TSPIN_TRIPLE;
      }
    }
    else if (isMiniTSpin)
    {
      if (clearedLevels == 0)
      {
        return ScoreType.MINI_TSPIN;
      }
      if (clearedLevels == 1)
      {
        return ScoreType.MINI_TSPIN_SINGLE;
      }
      if (clearedLevels > 1)
      {
        Debug.Log("Bug check: Mini TSpin with cleared Levels 2+");
        return ScoreType.TSPIN;
      }
    }

    return GetNormalLineClearType(clearedLevels);
  }

  public ScoreType GetNormalLineClearType(int clearedLevels)
  {
    if (clearedLevels == 1)
    {
      return ScoreType.SINGLE;
    }

    if (clearedLevels == 2)
    {
      return ScoreType.DOUBLE;
    }

    if (clearedLevels == 3)
    {
      return ScoreType.TRIPLE;
    }

    if (clearedLevels == 4)
    {
      return ScoreType.TETRIS;
    }

    return this.hardDroping ? ScoreType.HARD_DROP : ScoreType.SOFT_DROP;
  }

  public bool IsTSpin(bool TouchA, bool TouchB, bool TouchC, bool TouchD)
  {
    return TouchA && TouchB && (TouchC || TouchD);
  }

  public bool IsMiniTSpin(bool TouchA, bool TouchB, bool TouchC, bool TouchD)
  {
    return ((TouchA || TouchB) && (TouchC && TouchD));
  }

  public bool IsBackToBackBreak(ScoreType type)
  {
    return type == ScoreType.SOFT_DROP ||
           type == ScoreType.HARD_DROP ||
           type == ScoreType.SINGLE ||
           type == ScoreType.DOUBLE ||
          type == ScoreType.TRIPLE;
  }

  public bool IsBackToBackBonus(ScoreType type)
  {
    return type == ScoreType.MINI_TSPIN_SINGLE ||
      type == ScoreType.TSPIN_SINGLE ||
        type == ScoreType.TSPIN_DOUBLE ||
        type == ScoreType.TSPIN_TRIPLE;
  }

  public void updateBackToBack(bool isBackToBackBonus, bool isBackToBackBreak)
  {
    if (isBackToBackBonus)
    {
      this.backToBack = 2;
    }
    else if (!isBackToBackBonus && !isBackToBackBreak)
    {
      this.backToBack = 1;
    }
    else
    {
      this.backToBack = 0;
    }
  }

}
