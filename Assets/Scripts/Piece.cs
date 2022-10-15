using UnityEngine;

public class Piece : MonoBehaviour
{
  public Board board { get; private set; }
  public TetrominoData data { get; private set; }
  public Vector3Int[] cells { get; private set; }
  public Vector3Int position { get; private set; }

  public float stepDelay = 1f;
  public float lockDelay = 0.05f;
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
    //one
    lockTime += Time.deltaTime;

    board.Clear(this);

    if (Input.GetKeyDown(KeyCode.LeftArrow))
    {
      Debug.Log("LeftArrow was pressed.");
      Move(Vector2Int.left);
    }
    if (Input.GetKeyDown(KeyCode.RightArrow))
    {
      Debug.Log("RightArrow was pressed.");
      Move(Vector2Int.right);
    }
    if (Input.GetKeyDown(KeyCode.DownArrow))
    {
      Move(Vector2Int.down);
    }
    if (Input.GetKeyDown(KeyCode.Space))
    {
      InstantDrop();
    }

    if (Time.time > this.stepTime)
    {
      this.Step();
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
      lockTime = 0f; // reset lock time
      return true;
    }
    return false;
  }

  private void Step()
  {

    this.stepTime = Time.time + this.stepDelay;

    if (!Move(Vector2Int.down))
    {
      Debug.Log("Touch bottom Time:" + Time.time);
      Lock();
    }
  }

  private void Lock()
  {
    //Render Tile Map
    board.Set(this);

    //Init the next piece by using current Piece object
    board.SpawnPiece();
    //TODO clear lines
  }

  private void InstantDrop()
  {
    while (Move(Vector2Int.down)) { }

    this.stepTime = 0f;
  }
}
