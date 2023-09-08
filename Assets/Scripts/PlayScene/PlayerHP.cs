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

        // 現在HP初期化　初期値３
        currentHP = HpObj_origin_inspector.Length;
        /*
         currentHPが0になったとき、HpObj_origin[-1]を参照しようとしてエラーになる
         ↓　それを回避するためにInstantiateしたオブジェクトを別の配列に保存する
         */
        hpObjects = new GameObject[currentHP];

        // クリアもしくは死んだ時に出るメニューキャンバス
        for (int i = 0; i <= currentHP - 1; ++i)
        {

            hpObjects[i] = Instantiate(HpObj_origin_inspector[i],
            new Vector3(transform.position.x + (i * 1),
            transform.position.y, transform.position.z),
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
        // ↓これはInstantiateしたオブジェクトではなく元のプレハブに対してSetActiveしているため無効
        //HpObj_origin[currentHP].SetActive(false);　
        //Instantiateしたオブジェクトを操作するなら変数に代入しなければならない

        currentHP--;        // プレイヤーHP -1

        hpObjects[currentHP].SetActive(false);
        // インデックスの関係で0ではなく1を設定（よくない）
        isDead = currentHP <= 0 ? true : false;  //  HPが１より少なくなったら true

        if (isDead)
        {
            PlayerController p = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            p.Die();

            EnemyController e = GameObject.FindGameObjectWithTag("Enemy").GetComponent<EnemyController>();
            e.Idle();

            // ２秒後にMenuとResultを表示
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
        ScoreManager s = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();

        s.Update_ResultScore_HighScore();
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