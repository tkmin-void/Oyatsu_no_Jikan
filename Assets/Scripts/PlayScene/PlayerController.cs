using UnityEngine;



public class PlayerController : MonoBehaviour
{
    private CountDownAtFirst countDown;
    private AudioSource audioSource;
    private Animator animator;
    private Rigidbody2D rb2d;
    private ScoreManager scoreManager;
    private PlayerController controller;
    private PlayerHP hp;

    public AudioClip getItemClip;
    public AudioClip getDamageClip;
    public AudioClip getHealClip;
    public AudioClip jumpSEClip;

    // Member--------------------------

    // プレイヤーのサイズ
    float playerLocalScale = 0.6f;
    // プレイヤーの走る速度
    [System.NonSerialized] public float runSpeed;
    // プレイヤーのジャンプ力
    int jumpPower = 8;
    // ハイスコアのハッシュ
    string highScoreHash = "highScore";
    // 着地判定
    [System.NonSerialized] public bool isGrounded;


    public enum DIRECTION
    {
        STOP,
        RIGHT,
        LEFT,
    }

    DIRECTION direction = DIRECTION.STOP;


    void Awake()
    {
        isGrounded = true;
        audioSource = GetComponent<AudioSource>();

        countDown = GameObject.Find("CountDown").GetComponent<CountDownAtFirst>();
        controller = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        animator = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
        rb2d = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        scoreManager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
        hp = GameObject.Find("PlayerHP").GetComponent<PlayerHP>();
    }


    private void ConstraintMovableArea_X()
    {
        // 移動範囲制御＿X軸（ステージから落ちないように）
        if (controller.transform.localPosition.x < -9.0f)
        {
            controller.transform.localPosition = new Vector3(-9.0f, -5.0f, 0);
        }
        else if (controller.transform.localPosition.x > 8.8f)
        {
            controller.transform.localPosition = new Vector3(8.8f, -5.0f, 0);
        }
    }

    private void ApplyForce_Player_X()
    {
        float x = Input.GetAxis("Horizontal");

        if (x == 0) direction = DIRECTION.STOP;
        if (x > 0) direction = DIRECTION.RIGHT;
        if (x < 0) direction = DIRECTION.LEFT;
    }


    private void Update()
    {
        if (!countDown.startGame) return;  // カウントダウン
        if (hp.isDead) return; // GameOverの時        

        // プレイヤーの移動範囲制御
        ConstraintMovableArea_X();

        // スペースキーでジャンプ
        if (Input.GetKeyDown(KeyCode.Space)) Jump();
    }


    private void FixedUpdate()
    {
        if (!countDown.startGame) return;  // カウントダウン
        if (hp.isDead) return; // GameOverの時

        // プレイヤーのX軸方向の移動
        ApplyForce_Player_X();

        switch (direction)
        {
            case DIRECTION.STOP:
                runSpeed = 0;
                animator.SetBool("isRun", false);
                break;
            case DIRECTION.RIGHT:
                animator.SetBool("isRun", true);
                transform.localScale = new Vector3(playerLocalScale, playerLocalScale, playerLocalScale);
                runSpeed = 5;
                break;
            case DIRECTION.LEFT:
                animator.SetBool("isRun", true);
                transform.localScale = new Vector3(-playerLocalScale, playerLocalScale, playerLocalScale);
                runSpeed = -5;
                break;
        }
        rb2d.velocity = new Vector2(runSpeed, rb2d.velocity.y);
    }

    public void Idle()
    {
        animator.SetTrigger("idle");
    }


    public void Hurt()
    {
        animator.SetTrigger("hurt");

        if (playerLocalScale == 0.6f)
            rb2d.AddForce(new Vector2(-5f, 1f), ForceMode2D.Impulse);
        else
            rb2d.AddForce(new Vector2(5f, 1f), ForceMode2D.Impulse);
    }

    public void Die()
    {
        runSpeed = 0;
        rb2d.velocity = Vector2.zero;
        rb2d.angularVelocity = 0f;
        animator.SetTrigger("die");
    }

    public void Run()
    {
        if (!animator.GetBool("isRun"))  
            animator.SetBool("isRun", true);
        else                             
            animator.SetBool("isRun", false);
    }

    public void Jump()
    {
        if (!animator.GetBool("isJump"))
        {
            animator.SetBool("isJump", true);
            rb2d.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            audioSource.clip = jumpSEClip;
            audioSource.Play();
        }
        else
        {
            animator.SetBool("isJump", false);
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // プレイヤーがスコアアイテムに衝突
        if (collision.gameObject.tag == "ScoreItem")
        {
            Destroy(collision.gameObject);

            scoreManager.AddScore();
            audioSource.clip = getItemClip;
            audioSource.Play();
        }


        // プレイヤーがダメージアイテムに衝突
        if (collision.gameObject.tag == "DamageItem")
        {
            hp.DecreasePlayerHP();

            Destroy(collision.gameObject);
            // スコア減点
            scoreManager.MinusScore();

            Hurt(); // ダメージアニメーション
            audioSource.clip = getDamageClip;
            audioSource.Play();
        }


        // プレイヤーが回復アイテムに衝突
        if (collision.gameObject.tag == "HealItem")
        {
            Destroy(collision.gameObject);

            hp.PlusHP();
            audioSource.clip = getHealClip;
            audioSource.Play();
        }


        // 地面に接触後
        if (collision.gameObject.CompareTag("Stage"))
        {
            // 1秒後にジャンプ入力可能
            Invoke("EnableJump", 1.5f);
        }

        // ジャンプアニメーションはすぐ戻す
        animator.SetBool("isJump", false);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // 地面から離れたら
        if (collision.gameObject.CompareTag("Stage"))
        {
            isGrounded = false;
        }
    }

    private void EnableJump()
    {
        // 地面に着地中とする
        isGrounded = true;
    }
}