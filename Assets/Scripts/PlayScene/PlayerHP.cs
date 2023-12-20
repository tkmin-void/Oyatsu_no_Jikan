using ClearSky;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHP : MonoBehaviour
{
    private GameObject[] hpObjects;
    public GameObject[] hpObject_origin;
    public GameObject menuCanvas;
    public GameObject resultCanvas;

    AudioSource audioSource;
    [SerializeField] AudioClip pleaseWaitClip;
    [SerializeField] AudioClip pleasePlayAgainClip;

    // ���݂�HP
    private int currentHP;
    // ���S��ԃt���O
    [System.NonSerialized] public bool isDead;
    // ���S��̃Q�[���ꎞ��~���
    [System.NonSerialized] public bool isPaused = false;
    // ��d���͖h�~�p
    bool isInputEnabled;

    void Start()
    {
        isInputEnabled = true;
        audioSource = GetComponent<AudioSource>();

        // ����HP�������A�C���X�y�N�^�[����ݒ�
        currentHP = hpObject_origin.Length;

        /* currentHP��0�ɂȂ����Ƃ��AhpObject_origin[-1]���Q�Ƃ��悤�Ƃ��ăG���[�ɂȂ�
         ���@�����������邽�߂�Instantiate�����I�u�W�F�N�g��ʂ̔z��ɕۑ����� */

        hpObjects = new GameObject[currentHP];

        // �N���A�������͎��񂾎��ɏo�郁�j���[�L�����o�X
        for (int i = 0; i <= currentHP - 1; ++i)
        {
            hpObjects[i] =

            Instantiate(hpObject_origin[i], // ����
            new Vector3(
            transform.position.x + (i * 1),
            transform.position.y,
            transform.position.z),
            Quaternion.identity);

            // HP�I�u�W�F�N�g�\��
            hpObjects[i].SetActive(true);
        }

        // ���j���[�L�����o�X��\��
        menuCanvas.SetActive(false);
    }

    private void Update()
    {
        if (isPaused)
        {
            if (isInputEnabled)
            {
                if (Input.GetKeyDown(KeyCode.R))
                {
                    audioSource.clip = pleaseWaitClip;
                    audioSource.Play();
                    isInputEnabled = false;
                    Invoke("ReloadPlayScene", 2f);
                }

                if (Input.GetKeyDown(KeyCode.T))
                {
                    audioSource.clip = pleasePlayAgainClip;
                    audioSource.Play();
                    isInputEnabled = false;
                    Invoke("BackToTitleScene", 2f);
                }
            }
        }
    }

    public void DecreasePlayerHP()
    {
        /* �������Instantiate�����I�u�W�F�N�g�ł͂Ȃ����̃v���n�u�ɑ΂���SetActive���Ă��邽�ߖ���
           hpObject_origin[currentHP].SetActive(false);�@
         Instantiate�����I�u�W�F�N�g�𑀍삷��Ȃ�ϐ��ɑ�����Ȃ���΂Ȃ�Ȃ� 
             �� Start�֐����́@ hpObjects = new GameObject[currentHP];   */

        currentHP--;  // HP -1

        hpObjects[currentHP].SetActive(false);
        isDead = currentHP < 1 ? true : false;  //  HP���P��菭�Ȃ��Ȃ����� true

        if (isDead)
        {
            var player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            player.Die();

            var enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<EnemyController>();
            enemy.Idle();

            // �Q�b���Menu��Result��\��
            Invoke("GameOver", 2f); 
            Invoke("ShowResult", 2f);

            isPaused = true;
        }
    }

    public void PlusHP()
    {
        currentHP++;
    }

    private void GameOver()
    {
        // ���j���[�L�����o�X�\���i���g���C�A�^�C�g���ɖ߂�j
        menuCanvas.SetActive(true);
        //  ���g���C�t���OON
        isPaused = true;
    }

    private void ShowResult()
    {
        // ���U���g�L�����o�X�\���i�X�R�A�A�n�C�X�R�A�j
        resultCanvas.SetActive(true);

        // ���S�����^�C�~���O�Ń��U���g�X�R�A���X�V
        var score = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();

        score.OverwriteHighScore();
        //  ���g���C�t���OON
        isPaused = true;
    }

    void ReloadPlayScene()
    {
        SceneManager.LoadScene("Play");
    }

    void BackToTitleScene()
    {
        SceneManager.LoadScene("Title");

    }
}