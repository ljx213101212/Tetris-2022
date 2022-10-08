using UnityEngine;

public class Piece : MonoBehaviour
{
  public TetrominoData data { get; private set; }
  public Vector3Int[] cells { get; private set; }
  public Vector3Int position { get; private set; }
  
  public void Initialize(Vector3Int position, TetrominoData data)
  {
      this.data = data;
      this.position = position;

      if (cells == null) {
          cells = new Vector3Int[data.cells.Length];
      }

      for (int i = 0; i < cells.Length; i++) {
          cells[i] = (Vector3Int)data.cells[i];
      }
  }

}