using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] Text scoreText;
    [SerializeField] Text resultText;
    [SerializeField] Text highScoreText;


    // ���_�X�R�A�A���_�X�R�A������
    [SerializeField] int plusScorePoint = 100, minusScorePoint = 200;
    // ���݂̃X�R�A
    [System.NonSerialized] public int currentScore = 0;
    // �n�C�X�R�A
    int highScore = 0;
    // �X�R�A�̃L�[
    string scoreKey = "Score";
    // �n�C�X�R�A�̃L�[
    const string highScoreKey = "HighScore";
    // �X�R�A
    const string scoreString = "SCORE:";
    // ���U���g�X�R�A�ƃn�C�X�R�A���X�V���ꂽ��
    bool hasUpdatedScore;


    private void Start()
    {
        scoreText = GameObject.Find("ScoreText").GetComponent<Text>();
        // �v���C���X�V����X�R�A�e�L�X�g
        scoreText.text = scoreString + currentScore.ToString();

        // PlayerPrefs����n�C�X�R�A�̓ǂݍ���
        highScore = PlayerPrefs.GetInt(highScoreKey, 0);
        highScoreText.text = highScore.ToString();
    }

    // �_�����Z�E���Z�֐������BPlayer��ScoreItem���ڐG�����Ƃ��Ă΂��
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
        // ����擾�X�R�A���n�C�X�R�A��荂�����
        if (currentScore > highScore)
        {
            // �n�C�X�R�A��ۑ�����
            PlayerPrefs.SetInt(highScoreKey, currentScore);
            // �ύX���m��
            PlayerPrefs.Save();
            //�n�C�X�R�A�e�L�X�g�X�V
            highScoreText.text = highScore.ToString();
            Debug.Log("New Record!!");
        }

        if (!hasUpdatedScore)
        {
            // ���U���g�X�R�A�X�V
            resultText.text = currentScore.ToString();
            hasUpdatedScore = true;
        }
    }
}
