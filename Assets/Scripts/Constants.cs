using System.Collections.Generic;
using UnityEngine;

public static class Data
{

  //counterclockwise rotation pi/2 (90 degree)
  public static readonly float cos = Mathf.Cos(Mathf.PI / 2f);
  public static readonly float sin = Mathf.Sin(Mathf.PI / 2f);

  //https://en.wikipedia.org/wiki/Rotation_matrix
  public static readonly float[] RotationMatrix = new float[] { cos, sin, -sin, cos };

  public static readonly Dictionary<Tetromino, Vector2Int[]> Cells = new Dictionary<Tetromino, Vector2Int[]>()
    {
        { Tetromino.I, new Vector2Int[] { new Vector2Int(-1, 1), new Vector2Int( 0, 1), new Vector2Int( 1, 1), new Vector2Int( 2, 1) } },
        { Tetromino.J, new Vector2Int[] { new Vector2Int(-1, 1), new Vector2Int(-1, 0), new Vector2Int( 0, 0), new Vector2Int( 1, 0) } },
        { Tetromino.L, new Vector2Int[] { new Vector2Int( 1, 1), new Vector2Int(-1, 0), new Vector2Int( 0, 0), new Vector2Int( 1, 0) } },
        { Tetromino.O, new Vector2Int[] { new Vector2Int( 0, 1), new Vector2Int( 1, 1), new Vector2Int( 0, 0), new Vector2Int( 1, 0) } },
        { Tetromino.S, new Vector2Int[] { new Vector2Int( 0, 1), new Vector2Int( 1, 1), new Vector2Int(-1, 0), new Vector2Int( 0, 0) } },
        { Tetromino.T, new Vector2Int[] { new Vector2Int( 0, 1), new Vector2Int(-1, 0), new Vector2Int( 0, 0), new Vector2Int( 1, 0) } },
        { Tetromino.Z, new Vector2Int[] { new Vector2Int(-1, 1), new Vector2Int( 0, 1), new Vector2Int( 0, 0), new Vector2Int( 1, 0) } },
    };

  //Related to Tetromino.T North
  public static readonly Vector2Int[] TSpinCells = new Vector2Int[4]
  {
    new Vector2Int(Cells[Tetromino.T][1].x, Cells[Tetromino.T][1].y + 1),
    new Vector2Int(Cells[Tetromino.T][3].x, Cells[Tetromino.T][3].y - 1),
    new Vector2Int(Cells[Tetromino.T][1].x, Cells[Tetromino.T][1].y + 1),
    new Vector2Int(Cells[Tetromino.T][3].x, Cells[Tetromino.T][3].y - 1)
  };

  //https://tetris.fandom.com/wiki/SRS
  private static readonly Vector2Int[,] WallKicksI = new Vector2Int[,] {
        { new Vector2Int(0, 0), new Vector2Int(-2, 0), new Vector2Int( 1, 0), new Vector2Int(-2,-1), new Vector2Int( 1, 2) },
        { new Vector2Int(0, 0), new Vector2Int( 2, 0), new Vector2Int(-1, 0), new Vector2Int( 2, 1), new Vector2Int(-1,-2) },
        { new Vector2Int(0, 0), new Vector2Int(-1, 0), new Vector2Int( 2, 0), new Vector2Int(-1, 2), new Vector2Int( 2,-1) },
        { new Vector2Int(0, 0), new Vector2Int( 1, 0), new Vector2Int(-2, 0), new Vector2Int( 1,-2), new Vector2Int(-2, 1) },
        { new Vector2Int(0, 0), new Vector2Int( 2, 0), new Vector2Int(-1, 0), new Vector2Int( 2, 1), new Vector2Int(-1,-2) },
        { new Vector2Int(0, 0), new Vector2Int(-2, 0), new Vector2Int( 1, 0), new Vector2Int(-2,-1), new Vector2Int( 1, 2) },
        { new Vector2Int(0, 0), new Vector2Int( 1, 0), new Vector2Int(-2, 0), new Vector2Int( 1,-2), new Vector2Int(-2, 1) },
        { new Vector2Int(0, 0), new Vector2Int(-1, 0), new Vector2Int( 2, 0), new Vector2Int(-1, 2), new Vector2Int( 2,-1) },
    };

  private static readonly Vector2Int[,] WallKicksJLOSTZ = new Vector2Int[,] {
        { new Vector2Int(0, 0), new Vector2Int(-1, 0), new Vector2Int(-1, 1), new Vector2Int(0,-2), new Vector2Int(-1,-2) },
        { new Vector2Int(0, 0), new Vector2Int( 1, 0), new Vector2Int( 1,-1), new Vector2Int(0, 2), new Vector2Int( 1, 2) },
        { new Vector2Int(0, 0), new Vector2Int( 1, 0), new Vector2Int( 1,-1), new Vector2Int(0, 2), new Vector2Int( 1, 2) },
        { new Vector2Int(0, 0), new Vector2Int(-1, 0), new Vector2Int(-1, 1), new Vector2Int(0,-2), new Vector2Int(-1,-2) },
        { new Vector2Int(0, 0), new Vector2Int( 1, 0), new Vector2Int( 1, 1), new Vector2Int(0,-2), new Vector2Int( 1,-2) },
        { new Vector2Int(0, 0), new Vector2Int(-1, 0), new Vector2Int(-1,-1), new Vector2Int(0, 2), new Vector2Int(-1, 2) },
        { new Vector2Int(0, 0), new Vector2Int(-1, 0), new Vector2Int(-1,-1), new Vector2Int(0, 2), new Vector2Int(-1, 2) },
        { new Vector2Int(0, 0), new Vector2Int( 1, 0), new Vector2Int( 1, 1), new Vector2Int(0,-2), new Vector2Int( 1,-2) },
    };

  public static readonly Dictionary<Tetromino, Vector2Int[,]> WallKicks = new Dictionary<Tetromino, Vector2Int[,]>()
    {
        { Tetromino.I, WallKicksI },
        { Tetromino.J, WallKicksJLOSTZ },
        { Tetromino.L, WallKicksJLOSTZ },
        { Tetromino.O, WallKicksJLOSTZ },
        { Tetromino.S, WallKicksJLOSTZ },
        { Tetromino.T, WallKicksJLOSTZ },
        { Tetromino.Z, WallKicksJLOSTZ },
    };

  public static readonly Dictionary<ROTATE_DIRECTION, int> ROTATE_DIRECTION_VALUE = new Dictionary<ROTATE_DIRECTION, int>()
{
  {ROTATE_DIRECTION.LEFT,  1},
  {ROTATE_DIRECTION.RIGHT,  -1}
};




  public static readonly Dictionary<ScoreType, int> SCORE_MAP = new Dictionary<ScoreType, int>()
  {
    {ScoreType.SINGLE, 100},
    {ScoreType.DOUBLE, 300},
    {ScoreType.TRIPLE, 500},
    {ScoreType.TETRIS, 800},
    {ScoreType.MINI_TSPIN, 100},
    {ScoreType.MINI_TSPIN_SINGLE, 200},
    {ScoreType.TSPIN, 400},
    {ScoreType.TSPIN_SINGLE, 800},
    {ScoreType.TSPIN_DOUBLE, 1200},
    {ScoreType.TSPIN_TRIPLE, 1600},
    {ScoreType.SOFT_DROP, 1},
    {ScoreType.HARD_DROP, 2},
  };

}

public enum ScoreType
{
  SINGLE,
  DOUBLE,
  TRIPLE,
  TETRIS,
  MINI_TSPIN,
  MINI_TSPIN_SINGLE,
  TSPIN,
  TSPIN_SINGLE,
  TSPIN_DOUBLE,
  TSPIN_TRIPLE,
  SOFT_DROP,
  HARD_DROP
}

public enum TSPinCell
{
  A, B, C, D
}

public enum ROTATION_STATUS
{
  ORIGIN = 0,
  CLOCKWISE_90 = 1,
  CLOCKWISE_180 = 2,
  CLOSEWISE_270 = 3
}

public enum ROTATE_DIRECTION
{
  LEFT, RIGHT
}

public enum SUPER_ROTATION_POINT
{
  POINT_0 = 0, //Invalid Point
  POINT_1 = 1,
  POINT_2 = 2,
  POINT_3 = 3,
  POINT_4 = 4,
  POINT_5 = 5
}