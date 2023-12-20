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

    // 現在のHP
    private int currentHP;
    // 死亡状態フラグ
    [System.NonSerialized] public bool isDead;
    // 死亡後のゲーム一時停止状態
    [System.NonSerialized] public bool isPaused = false;
    // 二重入力防止用
    bool isInputEnabled;

    void Start()
    {
        isInputEnabled = true;
        audioSource = GetComponent<AudioSource>();

        // 現在HP初期化、インスペクターから設定
        currentHP = hpObject_origin.Length;

        /* currentHPが0になったとき、hpObject_origin[-1]を参照しようとしてエラーになる
         ↓　それを回避するためにInstantiateしたオブジェクトを別の配列に保存する */

        hpObjects = new GameObject[currentHP];

        // クリアもしくは死んだ時に出るメニューキャンバス
        for (int i = 0; i <= currentHP - 1; ++i)
        {
            hpObjects[i] =

            Instantiate(hpObject_origin[i], // 生成
            new Vector3(
            transform.position.x + (i * 1),
            transform.position.y,
            transform.position.z),
            Quaternion.identity);

            // HPオブジェクト表示
            hpObjects[i].SetActive(true);
        }

        // メニューキャンバス非表示
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
        /* ↓これはInstantiateしたオブジェクトではなく元のプレハブに対してSetActiveしているため無効
           hpObject_origin[currentHP].SetActive(false);　
         Instantiateしたオブジェクトを操作するなら変数に代入しなければならない 
             ※ Start関数内の　 hpObjects = new GameObject[currentHP];   */

        currentHP--;  // HP -1

        hpObjects[currentHP].SetActive(false);
        isDead = currentHP < 1 ? true : false;  //  HPが１より少なくなったら true

        if (isDead)
        {
            var player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            player.Die();

            var enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<EnemyController>();
            enemy.Idle();

            // ２秒後にMenuとResultを表示
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
        // メニューキャンバス表示（リトライ、タイトルに戻る）
        menuCanvas.SetActive(true);
        //  リトライフラグON
        isPaused = true;
    }

    private void ShowResult()
    {
        // リザルトキャンバス表示（スコア、ハイスコア）
        resultCanvas.SetActive(true);

        // 死亡したタイミングでリザルトスコアを更新
        var score = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();

        score.OverwriteHighScore();
        //  リトライフラグON
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