using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
  public static ScoreManager instance;
  public Text scoreText;
  //   public Text highscoreText;

  int score = 0;
  int highscore = 0;

  private void Awake()
  {
    instance = this;
  }
  // Start is called before the first frame update
  void Start()
  {
    scoreText.text = DisplayScoreText(score);
    //highscoreText.text = "HIGH SCORE: " + highscore.ToString();
  }

  public void CalculatePoint(ScoreType type, int level, int softDropLines, int hardDropLines, bool isBackToBackBonus)
  {

    Debug.Log($"CalculatePoint: type: {type} , level: {level} , softDropLines: {softDropLines}, hardDropLines: {hardDropLines}, isBackToBackBonus: {isBackToBackBonus}");

    int points = Data.SCORE_MAP[ScoreType.SOFT_DROP] * softDropLines + Data.SCORE_MAP[ScoreType.HARD_DROP] * hardDropLines;
    if (type != ScoreType.SOFT_DROP && type != ScoreType.HARD_DROP)
    {
      points = Data.SCORE_MAP[type] * level;
    }
    if (isBackToBackBonus)
    {
      points += Mathf.RoundToInt(Data.SCORE_MAP[type] / 2);
    }

    Debug.Log($"CalculatePoint: {points}");
    AddPoint(points);
  }

  public void AddPoint(int points)
  {
    score += points;
    scoreText.text = DisplayScoreText(score);
  }

  private string DisplayScoreText(int score)
  {
    return score.ToString() + " pts";
  }
}
