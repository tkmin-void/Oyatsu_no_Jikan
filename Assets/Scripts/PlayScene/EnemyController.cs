using UnityEngine;

namespace ClearSky
{
    public class EnemyController : MonoBehaviour
    {
        private CountDownAtFirst countDown;
        private PlayerController playerController;

        private AudioSource audioSource;
        private Rigidbody2D rb;
        private Animator anim;
        private PlayerHP p;
        public GameObject player;

        float movePower = 3f; 　　　 // 移動速度
        float jumpPower = 5f; 　　　// ジャンプ力
        float attackRange = 30f;    // 攻撃範囲
        float randomValue;

        private float direction = 0.65f; 　　// ローカルスケール用

        bool isGrounded = false;
        bool isTouchingPlayer = false;

        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            anim = GetComponent<Animator>();
            p = GameObject.Find("PlayerHP").GetComponent<PlayerHP>();
            countDown = GameObject.Find("CountDown").GetComponent<CountDownAtFirst>();
            playerController = player.GetComponent<PlayerController>();
            audioSource = GetComponent<AudioSource>();

            randomValue = UnityEngine.Random.value;
        }

        private void Update()
        {
            //固定------------------------------------------------------------------------

            // 移動範囲制限、ステージ外に出ないように
            if (transform.localPosition.x < -9.0f)
            {
                transform.localPosition = new Vector3(-9.0f, -5.0f, 0);
            }
            else if (transform.localPosition.x > 8.8f)
            {
                transform.localPosition = new Vector3(8.8f, -5.0f, 0);
            }
            //----------------------------------------------------------------------------

            if (!countDown.startGame) return;  // カウントダウン

            if (p.isDead) return;
            if (p.isPaused) return;


            // 地面に接触中＆ プレイヤーが空中にいるとき & 50％異常の確率でジャンプ
            if (isGrounded && !playerController.isGrounded && UnityEngine.Random.Range(0f, 1f) >= randomValue)
            {
                Jump();
            }

            /*
              Random.Range(0, 1)　>= 0.5f だと 
            int型とみなされるため結果は常に falseになってしまう。つまり
            　Random.Range(0f, 1f)　>= 0.5f とするか
            　Random.Range(0, 2)　>= 1 とする必要がある
             */
            Run();
        }

        void Run()
        {
            Vector3 moveVelocity = Vector3.zero;

            // プレイヤーとエネミーのX座標の距離
            float difX = player.transform.position.x - transform.position.x;

            if (difX > 1f)    // プレイヤーが右側にいる場合
            {
                // 右に反転
                transform.localScale = new Vector3(direction, direction, direction);

                //float moveAmount = direction * movePower * Time.deltaTime;   // 移動量            
                //transform.Translate(moveAmount, 0, 0);   // 移動

                moveVelocity = Vector3.right;

                if (!anim.GetBool("isJump"))
                    anim.SetBool("isRun", true);

            }
            else if (difX < -1f)   // プレイヤーが左側にいる場合
            {
                // 左に反転
                transform.localScale = new Vector3(-direction, direction, direction);

                //float moveAmount = -direction * movePower * Time.deltaTime;   // 移動量            
                //transform.Translate(moveAmount, 0, 0);   // 移動

                moveVelocity = Vector3.left;

                if (!anim.GetBool("isJump"))
                    anim.SetBool("isRun", true);
            }

            RaycastHit2D hitRight = Physics2D.Raycast(transform.position, Vector2.right, attackRange);
            RaycastHit2D hitLeft = Physics2D.Raycast(transform.position, Vector2.left, attackRange);

            // Rayがプレイヤーに当たったら
            if (hitRight.collider != null && hitRight.collider.CompareTag("Player")
                || hitLeft.collider != null && hitLeft.collider.CompareTag("Player"))
            {

                if (anim.GetBool("isRun"))
                    anim.SetBool("isRun", false);
                Attack(); // 攻撃

                playerController.Hurt();  // ダメージアニメーション
                audioSource.clip = playerController.getDamageClip;
                audioSource.Play(); // ダメージ音
            }

            transform.position += moveVelocity * movePower * Time.deltaTime;
        }

        void Jump()
        {
            if (!anim.GetBool("isJump"))
                anim.SetBool("isJump", true);
            // 速度初期化
            rb.velocity = Vector2.zero;
            // Y軸方向の移動量を取得し、その方向へ力を加える
            Vector2 jumpVelocity = new Vector2(0, jumpPower);
            rb.AddForce(Vector2.up * jumpVelocity, ForceMode2D.Impulse);
        }

        void Attack()
        {

            anim.SetTrigger("attack");
        }

        public void Idle()
        {
            if (!anim.GetBool("idle"))
                anim.SetBool("isRun", false);
        }

        public void CallLostHPFunc()
        {
            p.DecreasePlayerHP();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            anim.SetBool("isJump", false);

            // 地面に接触中なら
            if (collision.collider.CompareTag("Stage"))
            {
                // 3秒後にジャンプ入力可能
                Invoke("EnableJump", 3f);

            }
            // プレイヤーに接触中なら
            if (collision.collider.CompareTag("Player"))
            {
                // プレイヤーと接触中とする
                isTouchingPlayer = true;
            }
            /*
             collision.gameObject.tagは 　      string　　error  を返す
             collision.gameObject.CompareTagは  bool      false　を返す
             */
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            // 地面から離れたら
            if (collision.collider.CompareTag("Stage"))  // collision.gameObject は不適切
            {
                // 空中にいるとする
                isGrounded = false;
            }
            // プレイヤーから離れたら
            if (collision.collider.CompareTag("Player"))
            {
                // プレイヤーと接触中でないとする
                isTouchingPlayer = false;
            }
        }

        // ジャンプ入力を有効化するメソッド
        private void EnableJump()
        {
            // 地面に着地中とする
            isGrounded = true;
        }
    }
}