using UnityEngine;
using UnityEngine.SceneManagement;


public class TitleToDescription : MonoBehaviour
{
    AudioSource audioSource;
    [SerializeField] AudioClip startClip, endClip;

    bool isInputEnabled;  // 二重入力防止用

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        isInputEnabled = true;
    }

    void Update()
    {
        if (isInputEnabled)
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                audioSource.clip = startClip;
                audioSource.Play();

                isInputEnabled = false;
                Invoke("MoveToDescriptionScene", 2f);
            }


            if (Input.GetKeyDown(KeyCode.E))
            {
                audioSource.clip = endClip;
                audioSource.Play();

                isInputEnabled = false;
                Invoke("EndGame", 2f);
            }
        }
    }

    public void EndGame()
    {
        // ゲーム終了
        UnityEditor.EditorApplication.isPlaying = false;
    }

    public void MoveToDescriptionScene()
    {   
        // チュートリアルへ飛ぶ
        SceneManager.LoadScene("Description");
    }
}