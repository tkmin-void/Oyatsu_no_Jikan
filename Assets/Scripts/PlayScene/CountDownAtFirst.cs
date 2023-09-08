using UnityEngine;

public class CountDownAtFirst : MonoBehaviour
{
    AudioSource audioSource;
    [SerializeField] private AudioClip readyClip, goClip;
    [SerializeField] private GameObject BGM_source;
    public bool startGame = false;
    float countDown = 2.5f;  //　ゲームカウントダウン

    private void Awake()
    {
        // BGMはカウントダウンが終了してから鳴らすように処理

        audioSource = GetComponent<AudioSource>();
        audioSource.clip = readyClip;
        audioSource.Play();
    }


    private void Update()
    {
        if (!startGame)
        {
            countDown -= Time.deltaTime;

            // カウントダウンが０になったらゲーム開始
            if (countDown <= 0)
            {
                startGame = true;
                //PlayGo();
                Invoke("PlayGo", 0.1f);

                // BGMスタート
                BGM_source.SetActive(true);
            }
        }
        // カウントダウンがまだ終わっていなければ　2.5秒にリセット
        else if (countDown <= 2.5f)
        {
            countDown = 3.5f;
        }
    }

    private void PlayGo()
    {
        // Goクリップを再生する
        audioSource.clip = goClip;
        audioSource.Play();
    }
}
