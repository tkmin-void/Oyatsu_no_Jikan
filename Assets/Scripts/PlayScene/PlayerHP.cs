using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using ClearSky;

public class PlayerHP : MonoBehaviour
{
    ///-------------------------------
    public GameObject[] HpObj_origin_inspector;
    private GameObject[] hpObjects;
    public GameObject menuCanvas;
    public GameObject resultCanvas;

    AudioSource audioSource;
    [SerializeField] AudioClip pleaseWaitClip, pleasePlayAgainClip;

    private int currentHP;
    [System.NonSerialized] public bool isDead;
    [System.NonSerialized] public bool isPaused = false;
    bool isInputEnabled;
    void Start()
    {
        isInputEnabled = true;
        audioSource = GetComponent<AudioSource>();

        // ����HP�������@�����l�R
        currentHP = HpObj_origin_inspector.Length;
        /*
         currentHP��0�ɂȂ����Ƃ��AHpObj_origin[-1]���Q�Ƃ��悤�Ƃ��ăG���[�ɂȂ�
         ���@�����������邽�߂�Instantiate�����I�u�W�F�N�g��ʂ̔z��ɕۑ�����
         */
        hpObjects = new GameObject[currentHP];

        // �N���A�������͎��񂾎��ɏo�郁�j���[�L�����o�X
        for (int i = 0; i <= currentHP - 1; ++i)
        {

            hpObjects[i] = Instantiate(HpObj_origin_inspector[i],
            new Vector3(transform.position.x + (i * 1),
            transform.position.y, transform.position.z),
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
        // �������Instantiate�����I�u�W�F�N�g�ł͂Ȃ����̃v���n�u�ɑ΂���SetActive���Ă��邽�ߖ���
        //HpObj_origin[currentHP].SetActive(false);�@
        //Instantiate�����I�u�W�F�N�g�𑀍삷��Ȃ�ϐ��ɑ�����Ȃ���΂Ȃ�Ȃ�

        currentHP--;        // �v���C���[HP -1

        hpObjects[currentHP].SetActive(false);
        // �C���f�b�N�X�̊֌W��0�ł͂Ȃ�1��ݒ�i�悭�Ȃ��j
        isDead = currentHP <= 0 ? true : false;  //  HP���P��菭�Ȃ��Ȃ����� true

        if (isDead)
        {
            PlayerController p = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            p.Die();

            EnemyController e = GameObject.FindGameObjectWithTag("Enemy").GetComponent<EnemyController>();
            e.Idle();

            // �Q�b���Menu��Result��\��
            Invoke("GameOver", 2f); Invoke("ShowResult", 2f);

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
        ScoreManager s = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();

        s.Update_ResultScore_HighScore();
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