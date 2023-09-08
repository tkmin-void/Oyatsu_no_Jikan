using ClearSky;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CountDownAtFirst countDown;
    private AudioSource audioSource;
    private Animator anim;
    private Rigidbody2D rb2d;
    private ScoreManager scoreManager;
    private PlayerController player;
    // プレイヤーHPを取得
    private PlayerHP p;

    public AudioClip getItemClip, getDamageClip, getHealClip, jumpSEClip;

    // Member--------------------------
    float playerLocalScale = 0.6f;
    [System.NonSerialized] public float runSpeed;
    int jumpPower = 8;
    string highScoreHash = "highScore";
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
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        anim = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
        rb2d = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        scoreManager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
        p = GameObject.Find("PlayerHP").GetComponent<PlayerHP>();
    }


    void Update()
    {
        // 移動範囲制御＿X軸（ステージから落ちないように）
        if (player.transform.localPosition.x < -9.0f)
        {
            player.transform.localPosition = new Vector3(-9.0f, -5.0f, 0);
        }
        else if (player.transform.localPosition.x > 8.8f)
        {
            player.transform.localPosition = new Vector3(8.8f, -5.0f, 0);
        }
        //-------------------------------------------------------------------


        if (!countDown.startGame) return;  // カウントダウン
        if (p.isDead) return;      // GameOverの時
        if (p.isPaused) return;



        float x = Input.GetAxis("Horizontal");
        if (x == 0)
        {
            direction = DIRECTION.STOP;
        }
        if (x > 0)
        {
            direction = DIRECTION.RIGHT;
        }
        if (x < 0)
        {
            direction = DIRECTION.LEFT;
        }


        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {

            Jump();
            audioSource.clip = jumpSEClip;
            audioSource.Play();
        }
    }

    private void FixedUpdate()
    {
        if (!countDown.startGame) return;  // カウントダウン
        if (p.isDead) return;　// GameOverの時        
        if (p.isPaused) return;


        switch (direction)
        {
            case DIRECTION.STOP:
                runSpeed = 0;
                anim.SetBool("isRun", false);
                break;
            case DIRECTION.RIGHT:
                anim.SetBool("isRun", true);
                transform.localScale = new Vector3(playerLocalScale, playerLocalScale, playerLocalScale);
                runSpeed = 5;
                break;
            case DIRECTION.LEFT:
                anim.SetBool("isRun", true);
                transform.localScale = new Vector3(-playerLocalScale, playerLocalScale, playerLocalScale);
                runSpeed = -5;
                break;
        }
        rb2d.velocity = new Vector2(runSpeed, rb2d.velocity.y);
    }

    public void Idle()
    {
        anim.SetTrigger("idle");
    }
  

    public void Hurt()
    {
        anim.SetTrigger("hurt");

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
        anim.SetTrigger("die");
    }

    public void Run()
    {
        if (!anim.GetBool("isRun"))
        {
            anim.SetBool("isRun", true);
        }
        else
        {
            anim.SetBool("isRun", false);
        }
    }
    public void Jump()
    {
        if (!anim.GetBool("isJump"))
        {
            anim.SetBool("isJump", true);
            rb2d.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
        }
        else
        {
            anim.SetBool("isJump", false);
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
            p.DecreasePlayerHP();

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

            p.PlusHP();
            audioSource.clip = getHealClip;
            audioSource.Play();
        }


        anim.SetBool("isJump", false);

        // 地面に接触中なら
        if (collision.gameObject.CompareTag("Stage"))
        {
            // 1秒後にジャンプ入力可能
            Invoke("EnableJump", 1f);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // 地面から離れたら
        if (collision.gameObject.CompareTag("Stage"))
        {
            // 空中にいるとする
            isGrounded = false;
        }
    }

    // ジャンプ入力を有効化するメソッド
    private void EnableJump()
    {
        // 地面に着地中とする
        isGrounded = true;
    }
}