using UnityEngine;

public class Piece : MonoBehaviour
{
    public Board board { get; private set; }
    public TetrominoData data { get; private set; }
    public Vector3Int[] cells { get; private set; }
    public Vector3Int position { get; private set; }

    public void Initialize(Board board, Vector3Int position, TetrominoData data)
    {
        this.board = board;
        this.data = data;
        this.position = position;

        if (cells == null)
        {
            cells = new Vector3Int[data.cells.Length];
        }

        for (int i = 0; i < cells.Length; i++)
        {
            cells[i] = (Vector3Int)data.cells[i];
        }
    }

    void Update()
    {
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

        if (Input.GetKeyDown(KeyCode.Space)) { }

        board.Set(this);
    }

    public void Move(Vector2Int translation)
    {
        Vector3Int newPosition = position;
        newPosition.x += translation.x;
        newPosition.y += translation.y;

        bool isNextMoveValid = board.isPositionValid(this, newPosition);

        Debug.Log("Moving: " + isNextMoveValid);
        if (isNextMoveValid)
        {
            position = newPosition;
        }
    }
}
