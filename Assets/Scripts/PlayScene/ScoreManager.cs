using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] Text scoreText;
    // --------------------------------------------------------------------
    /*  

     �C���X�y�N�^�[��Ŕ�A�N�e�B�u�ȃI�u�W�F�N�g���Q�Ƃ���ɂ́A���
        �E[SerializeField]�@  
�@�@�@�@�Epublic 
        �EFindWithTag
    �@�@�ETransform.Find�i�e�I�u�W�F�N�g����q�I�u�W�F�N�g���Q�Ƃł���j
    ������
     */

    [SerializeField] Text resultText;
    [SerializeField] Text highScoreText;
    // --------------------------------------------------------------------

    [SerializeField] int plusScorePoint = 100, minusScorePoint = 200;

    // �X�R�A------------------------------------------------------------
    [System.NonSerialized] public int currentScore = 0;
    int highScore = 0;
    string scoreKey = "Score"; // �X�R�A�̃L�[   
    private string highScoreKey = "HighScore"; // �n�C�X�R�A�̃L�[

    string scoreStr = "SCORE:";
    //�@-------------------------------------------------------------------
    bool updatedScore; // ���U���g�X�R�A�ƃn�C�X�R�A���X�V���ꂽ��

    private void Start()
    {
        scoreText = GameObject.Find("ScoreText").GetComponent<Text>();
        // �v���C���X�V����X�R�A�e�L�X�g
        scoreText.text = scoreStr + currentScore.ToString();

        // PlayerPrefs����n�C�X�R�A�̓ǂݍ���
        highScore = PlayerPrefs.GetInt(highScoreKey, 0);
        highScoreText.text = highScore.ToString();
    }

    // �_�����Z�A���Z�֐��APlayer��ScoreItem���ڐG�����Ƃ��Ă΂��
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
        // ����擾�X�R�A���n�C�X�R�A��荂�����
        if (currentScore > highScore)
        {
            PlayerPrefs.SetInt(highScoreKey, currentScore);   // �n�C�X�R�A��ۑ�����
            PlayerPrefs.Save();   // �ύX���m��
            //�n�C�X�R�A�e�L�X�g�X�V
            highScoreText.text = highScore.ToString();
            Debug.Log("New Record!!");
        }

        if (!updatedScore)
        {
            // ���U���g�X�R�A�X�V
            resultText.text = currentScore.ToString();
            updatedScore = true;
        }
    }
}
