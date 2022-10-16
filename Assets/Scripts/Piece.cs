using UnityEngine;

public class Piece : MonoBehaviour
{
  public Board board { get; private set; }
  public TetrominoData data { get; private set; }
  public Vector3Int[] cells { get; private set; }
  public Vector3Int position { get; private set; }

  private int rotationStatus = (int)ROTATION_STATUS.ORIGIN;
  public float stepDelay = 1f;
  public float lockDelay = 0.5f; // Lock Down Timer is 0.5 seconds
  private float stepTime;
  private float lockTime;
  public void Initialize(Board board, Vector3Int position, TetrominoData data)
  {
    this.board = board;
    this.data = data;
    this.position = position;

    this.stepTime = Time.time + stepDelay;
    this.lockTime = 0f;

    if (cells == null)
    {
      cells = new Vector3Int[data.cells.Length];
    }

    for (int i = 0; i < cells.Length; i++)
    {
      cells[i] = (Vector3Int)data.cells[i];
    }
  }

  //https://docs.unity3d.com/2019.3/Documentation/Manual/TimeFrameManagement.html
  void Update()
  {

    board.Clear(this);

    if (Input.GetKeyDown(KeyCode.LeftArrow))
    {
      Move(Vector2Int.left);
    }
    if (Input.GetKeyDown(KeyCode.RightArrow))
    {
      Move(Vector2Int.right);
    }
    if (Input.GetKeyDown(KeyCode.DownArrow))
    {
      Move(Vector2Int.down);
    }
    if (Input.GetKeyDown(KeyCode.Space))
    {
      HardDrop();
    }
    if (Input.GetKeyDown(KeyCode.A))
    {
      TryToRotate(Data.ROTATE_DIRECTION_VALUE[ROTATE_DIRECTION.LEFT]);
    }
    if (Input.GetKeyDown(KeyCode.D))
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

    Debug.Log("Moving: " + isNextMoveValid);
    if (isNextMoveValid)
    {
      position = newPosition;
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
      Debug.Log("Touch bottom Time:" + Time.time);
      this.lockTime = Time.time;
    }
  }

  private void Lock()
  {
    //Render Tile Map
    board.Set(this);

    //Init the next piece by using current Piece object
    board.SpawnPiece();

    this.lockTime = 0f; // reset lock time
    //TODO clear lines
  }

  private void HardDrop()
  {
    while (Move(Vector2Int.down)) { }

    this.stepTime = 0f;
  }


  private void TryToRotate(int direction)
  {
    int originalRotatioStatus = rotationStatus;

    rotationStatus = Util.TetrisUtil.Wrap(rotationStatus - direction, 0, data.cells.GetLength(0));
    Rotate(direction);

    if (!TestWallKick(direction))
    {
      rotationStatus = originalRotatioStatus;
      Rotate(-direction);
    }
  }

  //https://en.wikipedia.org/wiki/Rotation_matrix
  private void Rotate(int direction)
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

      Debug.Log("Before Rotation:" + current);
      Debug.Log("After Rotation:" + cells[i]);
    }
    //Render Tile Map
    //board.Set(this);
  }

  //https://tetris.fandom.com/wiki/SRS
  private bool TestWallKick(int direction)
  {

    Debug.Log("TestWallKick Index param: " + rotationStatus + " " + (int)direction + " " + data.wallKicks.GetLength(0));
    int wallKickIndex = Util.TetrisUtil.GetWallKickIndex(rotationStatus, (int)direction, data.wallKicks.GetLength(0));


    Debug.Log("TestWallKick Index: " + wallKickIndex);
    for (int i = 0; i < data.wallKicks.GetLength(1); i++)
    {
      Vector2Int translation = data.wallKicks[wallKickIndex, i];

      if (Move(translation))
      {
        return true;
      }
    }

    return false;
  }
}
