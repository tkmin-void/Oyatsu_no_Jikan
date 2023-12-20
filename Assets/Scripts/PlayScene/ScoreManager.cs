using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] Text scoreText;
    [SerializeField] Text resultText;
    [SerializeField] Text highScoreText;


    // 加点スコア、減点スコア初期化
    [SerializeField] int plusScorePoint = 100, minusScorePoint = 200;
    // 現在のスコア
    [System.NonSerialized] public int currentScore = 0;
    // ハイスコア
    int highScore = 0;
    // スコアのキー
    string scoreKey = "Score";
    // ハイスコアのキー
    const string highScoreKey = "HighScore";
    // スコア
    const string scoreString = "SCORE:";
    // リザルトスコアとハイスコアが更新されたか
    bool hasUpdatedScore;


    private void Start()
    {
        scoreText = GameObject.Find("ScoreText").GetComponent<Text>();
        // プレイ中更新するスコアテキスト
        scoreText.text = scoreString + currentScore.ToString();

        // PlayerPrefsからハイスコアの読み込み
        highScore = PlayerPrefs.GetInt(highScoreKey, 0);
        highScoreText.text = highScore.ToString();
    }

    // 点数加算・減算関数処理。PlayerとScoreItemが接触したとき呼ばれる
    public void AddScore()
    {
        currentScore += this.plusScorePoint;
        scoreText.text = scoreString + currentScore.ToString();
    }
    public void MinusScore()
    {
        currentScore -= this.minusScorePoint;
        scoreText.text = scoreString + currentScore.ToString();
    }


    public void OverwriteHighScore()
    {
        // 今回取得スコアがハイスコアより高ければ
        if (currentScore > highScore)
        {
            // ハイスコアを保存する
            PlayerPrefs.SetInt(highScoreKey, currentScore);
            // 変更を確定
            PlayerPrefs.Save();
            //ハイスコアテキスト更新
            highScoreText.text = highScore.ToString();
            Debug.Log("New Record!!");
        }

        if (!hasUpdatedScore)
        {
            // リザルトスコア更新
            resultText.text = currentScore.ToString();
            hasUpdatedScore = true;
        }
    }
}
