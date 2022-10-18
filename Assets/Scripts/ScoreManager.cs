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

  public void CalculatePoint(Data.ScoreType type, int level, int droppedLines)
  {
    //TODO 
    int points = 100;
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
