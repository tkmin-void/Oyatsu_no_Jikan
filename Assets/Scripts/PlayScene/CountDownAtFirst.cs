using UnityEngine;

public class CountDownAtFirst : MonoBehaviour
{
    AudioSource audioSource;
    [SerializeField] private AudioClip readyClip, goClip;
    [SerializeField] private GameObject BGM_source;
    public bool startGame = false;
    float countDown = 2.5f;  //�@�Q�[���J�E���g�_�E��

    private void Awake()
    {
        // BGM�̓J�E���g�_�E�����I�����Ă���炷�悤�ɏ���

        audioSource = GetComponent<AudioSource>();
        audioSource.clip = readyClip;
        audioSource.Play();
    }


    private void Update()
    {
        if (!startGame)
        {
            countDown -= Time.deltaTime;

            // �J�E���g�_�E�����O�ɂȂ�����Q�[���J�n
            if (countDown <= 0)
            {
                startGame = true;
                //PlayGo();
                Invoke("PlayGo", 0.1f);

                // BGM�X�^�[�g
                BGM_source.SetActive(true);
            }
        }
        // �J�E���g�_�E�����܂��I����Ă��Ȃ���΁@2.5�b�Ƀ��Z�b�g
        else if (countDown <= 2.5f)
        {
            countDown = 3.5f;
        }
    }

    private void PlayGo()
    {
        // Go�N���b�v���Đ�����
        audioSource.clip = goClip;
        audioSource.Play();
    }
}
