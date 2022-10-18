using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using System.Linq;
public class Ghost : MonoBehaviour
{
  public Tile tile;
  public Board mainBoard;
  public Piece trackingPiece;

  public Tilemap tilemap { get; private set; }
  public Vector3Int[] cells { get; private set; }
  public Vector3Int position { get; private set; }


  private void Awake()
  {
    tilemap = GetComponentInChildren<Tilemap>();
    cells = new Vector3Int[4];
  }

  private void LateUpdate()
  {
    GhostClear();
    GhostGenerate();
    GhostHardDrop();
    GhostSet();

  }

  public void GhostGenerate()
  {
    for (int i = 0; i < trackingPiece.cells.Length; i++)
    {
      cells[i] = trackingPiece.cells[i];
    }
  }

  public bool GhostMove(Vector2Int translation)
  {
    Vector3Int newPosition = trackingPiece.position;
    newPosition.x += translation.x;
    newPosition.y += translation.y;

    bool isNextMoveValid = mainBoard.IsPositionValid(trackingPiece, newPosition);

    if (isNextMoveValid)
    {
      Debug.Log("GhostMove: " + isNextMoveValid);
      this.position = newPosition;
      return true;
    }
    return false;
  }

  private void GhostHardDrop()
  {

    //Remove tracking piece temporarily to avoid current cell makes the next positon invalid.
    mainBoard.Clear(trackingPiece);
    for (int y = trackingPiece.position.y; y >= mainBoard.Bounds.yMin; y--)
    {
      Vector3Int newPosition = new Vector3Int(trackingPiece.position.x, y - 1);
      bool isNextMoveValid = mainBoard.IsPositionValid(trackingPiece, newPosition);

      if (!isNextMoveValid)
      {
        break;
      }
      this.position = newPosition;
    }
    mainBoard.Set(trackingPiece);
  }

  public void GhostSet()
  {
    for (int i = 0; i < this.cells.Length; i++)
    {
      Vector3Int tilePosition = this.cells[i] + this.position;
      tilemap.SetTile(tilePosition, tile);
    }
  }

  public void GhostClear()
  {
    for (int i = 0; i < this.cells.Length; i++)
    {
      Vector3Int tilePosition = this.cells[i] + this.position;
      tilemap.SetTile(tilePosition, null);
    }
  }

}
