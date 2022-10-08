using UnityEngine;

namespace Util 
{
  public class TetrisUtil {
    public static string GetProjectName()
    {
        string[] s = Application.dataPath.Split('/');
        string projectName = s[s.Length - 2];
        return projectName;
    }
  }
}