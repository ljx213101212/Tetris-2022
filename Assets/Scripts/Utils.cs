using UnityEngine;
using System.Collections.Generic;
using System.Linq;
namespace Util
{
  public class TetrisUtil
  {
    public static string GetProjectName()
    {
      string[] s = Application.dataPath.Split('/');
      string projectName = s[s.Length - 2];
      return projectName;
    }


    public static int GetWallKickIndex(int rotationIndex, int rotationDirection, int wallKickLength)
    {
      int wallKickIndex = rotationIndex * 2;

      if (rotationDirection > 0)
      {
        wallKickIndex--;
      }

      return Wrap(wallKickIndex, 0, wallKickLength);
    }

    public static int Wrap(int input, int min, int max)
    {
      if (input < min)
      {
        return max - (min - input) % (max - min);
      }
      else
      {
        return min + (input - min) % (max - min);
      }
    }

    public static bool IsOverlapping(List<Vector3Int> position1, List<Vector3Int> position2)
    {
      return position1.Any(p1 => position2.Any(p2 => p1.x == p2.x && p1.y == p2.y));
    }

    public static SUPER_ROTATION_POINT GetSuperRotationNumberByWallKickTest(int wallKickTest)
    {
      return (SUPER_ROTATION_POINT)(wallKickTest + 1);
    }


    public static TetrominoData GetRandomTetromino(TetrominoData[] tetrominoes)
    {
      int random = Random.Range(0, tetrominoes.Length);
      return tetrominoes[random];
    }

    public static float GetPlayerPrefVolume()
    {
      float volumeValue = PlayerPrefs.GetFloat("VolumeValue");
      if (!PlayerPrefs.HasKey("VolumeValue"))
      {
        volumeValue = Data.DefaultVolume;
      }
      return volumeValue;
    }
  }

}