using UnityEngine;


public class ItemSpawnerMonster : MonoBehaviour
{
    private PlayerHP playerHp;
    private CountDownAtFirst countDown;

    Animator animator;
    public GameObject[] scoreItems;
    public GameObject bombTrap;
    public GameObject healItem;

    int hashRandomIdle = Animator.StringToHash("randomIdle");
    int hashRandomSpawn = Animator.StringToHash("randomSpawn");

    float bombSpawnLimiter = 0.5f;

    float timer = 0;
    float spawnTime = 4.0f; // アイテム生成の間隔調整

    private void Start()
    {
        countDown = GameObject.Find("CountDown").GetComponent<CountDownAtFirst>();

        animator = GetComponent<Animator>();

        playerHp = GameObject.Find("PlayerHP").GetComponent<PlayerHP>();

        bombSpawnLimiter = UnityEngine.Random.value;
    }


    private void Update()
    {
        if (!countDown.startGame) return;  // カウントダウン
        if (playerHp.isDead) return;

        // スポナーのアニメーションをランダム再生
        PlayRandomIdleAnim();
        PlayRandomAttackAnim();
    }

    private void FixedUpdate()
    {
        if (playerHp.isPaused) return;
        ItemSpawn_RndPos();
    }


    void ItemSpawn_RndPos()
    {

        timer += Time.deltaTime;
        // 一定時間経過後、かつモンスターが正面を向いているとき
        if (timer > spawnTime && animator.GetCurrentAnimatorStateInfo(0).IsName("orb_front"))
        {
            timer = 0;

            GameObject[] receiveIndex = new GameObject[3];

            // 8個のアイテムの中からランダムで３つを配列に格納
            receiveIndex[0] = Instantiate(scoreItems[UnityEngine.Random.Range(0, scoreItems.Length)]);
            receiveIndex[1] = Instantiate(scoreItems[UnityEngine.Random.Range(0, scoreItems.Length)]);
            receiveIndex[2] = Instantiate(scoreItems[UnityEngine.Random.Range(0, scoreItems.Length)]);

            for (int i = 0; i < receiveIndex.Length; i++)
            {
                //　アイテム生成範囲　X　Y
                float x = UnityEngine.Random.Range(-7.0f, 7.0f);
                float y = UnityEngine.Random.Range(5, 10);

                receiveIndex[i].transform.position = new Vector3(x + transform.position.x, y + transform.position.y, 0);
            }

            // ボム生成(初期50％の確率)
            if (UnityEngine.Random.value > bombSpawnLimiter)
            {
                GameObject trap = Instantiate(bombTrap);
                trap.transform.position = new Vector3(UnityEngine.Random.Range(-7.0f, 7.0f), UnityEngine.Random.Range(5, 10), 0);

                // 1000スコアごとに確率が10％増加
                ScoreManager s = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
                if (s.currentScore >= 1000) bombSpawnLimiter -= 0.1f;
            }

            // 回復アイテムを20％の確率で生成
            if (UnityEngine.Random.value > 0.8f)
            {
                GameObject heal = Instantiate(healItem);
                heal.transform.position = new Vector3(UnityEngine.Random.Range(-7.0f, 7.0f), UnityEngine.Random.Range(5, 10), 0);
            }
        }
    }


    void PlayRandomIdleAnim()
    {
        int randomIndex = UnityEngine.Random.Range(0, 5);
        animator.SetInteger(hashRandomIdle, randomIndex);
    }

    void PlayRandomAttackAnim()
    {
        int randomIndex = UnityEngine.Random.Range(0, 5);
        animator.SetInteger(hashRandomSpawn, randomIndex);
    }
}