using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] Text scoreText;
    // --------------------------------------------------------------------
    /*  

     インスペクター上で非アクティブなオブジェクトを参照するには、主に
        ・[SerializeField]　  
　　　　・public 
        ・FindWithTag
    　　・Transform.Find（親オブジェクトから子オブジェクトを参照できる）
    がある
     */

    [SerializeField] Text resultText;
    [SerializeField] Text highScoreText;
    // --------------------------------------------------------------------

    [SerializeField] int plusScorePoint = 100, minusScorePoint = 200;

    // スコア------------------------------------------------------------
    [System.NonSerialized] public int currentScore = 0;
    int highScore = 0;
    string scoreKey = "Score"; // スコアのキー   
    private string highScoreKey = "HighScore"; // ハイスコアのキー

    string scoreStr = "SCORE:";
    //　-------------------------------------------------------------------
    bool updatedScore; // リザルトスコアとハイスコアが更新されたか

    private void Start()
    {
        scoreText = GameObject.Find("ScoreText").GetComponent<Text>();
        // プレイ中更新するスコアテキスト
        scoreText.text = scoreStr + currentScore.ToString();

        // PlayerPrefsからハイスコアの読み込み
        highScore = PlayerPrefs.GetInt(highScoreKey, 0);
        highScoreText.text = highScore.ToString();
    }

    // 点数加算、減算関数、PlayerとScoreItemが接触したとき呼ばれる
    public void AddScore()
    {
        currentScore += this.plusScorePoint;
        scoreText.text = scoreStr + currentScore.ToString();
    }
    public void MinusScore()
    {
        currentScore -= this.minusScorePoint;
        scoreText.text = scoreStr + currentScore.ToString();
    }

    public void Update_ResultScore_HighScore()
    {
        // 今回取得スコアがハイスコアより高ければ
        if (currentScore > highScore)
        {
            PlayerPrefs.SetInt(highScoreKey, currentScore);   // ハイスコアを保存する
            PlayerPrefs.Save();   // 変更を確定
            //ハイスコアテキスト更新
            highScoreText.text = highScore.ToString();
            Debug.Log("New Record!!");
        }

        if (!updatedScore)
        {
            // リザルトスコア更新
            resultText.text = currentScore.ToString();
            updatedScore = true;
        }
    }
}
