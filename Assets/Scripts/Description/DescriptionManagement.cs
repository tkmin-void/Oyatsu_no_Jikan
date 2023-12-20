using UnityEngine;
using UnityEngine.SceneManagement;

public class DescriptionManagement : MonoBehaviour
{
    [SerializeField] private GameObject canvas_page1;
    [SerializeField] private GameObject canvas_page2;

    AudioSource audioSource;
    public AudioClip askReadyClip;
    bool isInputEnabled;

    private int state = 0;  // ��ԊǗ��t���O
    void Start()
    {
        canvas_page1.SetActive(true);
        canvas_page2.SetActive(false);

        audioSource = GetComponent<AudioSource>();
        isInputEnabled = true;
    }

    void Update()
    {

        if (isInputEnabled)
        {
            if (state == 0)
            {
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    if (canvas_page1.activeSelf)
                    {
                        // �P�y�[�W�ڂ��\���ɂ��ĂQ�y�[�W�ڂ�\��
                        canvas_page1.SetActive(false);
                        canvas_page2.SetActive(true);
                    }
                    state = 1;
                }
            }
            else if (state == 1)
            {
                if (Input.GetKeyDown(KeyCode.Return) && canvas_page2.activeSelf)
                {
                    audioSource.clip = askReadyClip;
                    audioSource.Play();

                    // �Q�D�T�b��ɃQ�[���J�n
                    isInputEnabled = false;
                    Invoke("MoveToPlayScene", 2.5f);
                }
            }
        }
    }

    void MoveToPlayScene()
    {
        SceneManager.LoadScene("Play");
    }
}