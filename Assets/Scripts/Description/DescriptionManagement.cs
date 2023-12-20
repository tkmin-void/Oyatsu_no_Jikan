using UnityEngine;
using UnityEngine.SceneManagement;

public class DescriptionManagement : MonoBehaviour
{
    [SerializeField] private GameObject canvas_page1;
    [SerializeField] private GameObject canvas_page2;

    AudioSource audioSource;
    public AudioClip askReadyClip;
    bool isInputEnabled;

    private int state = 0;  // 状態管理フラグ
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
                        // １ページ目を非表示にして２ページ目を表示
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

                    // ２．５秒後にゲーム開始
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