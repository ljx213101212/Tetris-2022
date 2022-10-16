using UnityEngine;

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
  }
}