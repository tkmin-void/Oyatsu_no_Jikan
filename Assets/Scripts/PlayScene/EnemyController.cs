using UnityEngine;

namespace ClearSky
{
    public class EnemyController : MonoBehaviour
    {
        private CountDownAtFirst countDown;
        private PlayerController playerController;

        private AudioSource audioSource;
        private Rigidbody2D rb;
        private Animator animator;
        private PlayerHP playerHp;
        public GameObject player;

        float movePower = 3f; 　　　 // 移動速度
        float jumpPower = 5f; 　　　// ジャンプ力
        float attackRange = 0.5f;    // 攻撃範囲
        float random_jump_possibility;

        private float localScale = 0.65f; //  エネミーサイズ

        bool isGrounded = false;
        bool isTouchingPlayer = false;

        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            playerHp = GameObject.Find("PlayerHP").GetComponent<PlayerHP>();
            countDown = GameObject.Find("CountDown").GetComponent<CountDownAtFirst>();
            playerController = player.GetComponent<PlayerController>();
            audioSource = GetComponent<AudioSource>();

            random_jump_possibility = UnityEngine.Random.value;
        }

        private void Update()
        {

            ConstraintMovableArea_X();

            if (!countDown.startGame) return;
            if (playerHp.isDead) return;


            if (isGrounded && // 地面に接触中
                !playerController.isGrounded && // プレイヤーが空中にいるとき 
                UnityEngine.Random.Range(0f, 1f) >= random_jump_possibility) // 50％以上の確率でジャンプ
            {
                Jump();
            }

            Run();
        }

        private void ConstraintMovableArea_X()
        {
            // 移動範囲制限、ステージ外に出ないように
            if (transform.localPosition.x < -9.0f)
            {
                transform.localPosition = new Vector3(-9.0f, -5.0f, 0);
            }
            else if (transform.localPosition.x > 8.8f)
            {
                transform.localPosition = new Vector3(8.8f, -5.0f, 0);
            }
        }


        void Run()
        {
            Vector3 moveVelocity = Vector3.zero;

            // プレイヤーとエネミーのX座標の距離
            float enemy_player_disX = player.transform.position.x - transform.position.x;

            if (enemy_player_disX > 1f)    // プレイヤーが右側にいる場合
            {
                // 右に反転
                transform.localScale = new Vector3(localScale, localScale, localScale);
                moveVelocity = Vector3.right;

                if (!animator.GetBool("isJump"))
                    animator.SetBool("isRun", true);
            }
            else if (enemy_player_disX < -1f)   // プレイヤーが左側にいる場合
            {
                // 左に反転
                transform.localScale = new Vector3(-localScale, localScale, localScale);
                moveVelocity = Vector3.left;

                if (!animator.GetBool("isJump"))
                    animator.SetBool("isRun", true);
            }

            RunAndAttackToPlayer(moveVelocity);
        }


        private void RunAndAttackToPlayer(Vector3 moveVelocity)
        {
            RaycastHit2D hitRight = Physics2D.Raycast(transform.position, Vector2.right, attackRange);
            RaycastHit2D hitLeft = Physics2D.Raycast(transform.position, Vector2.left, attackRange);

            // 左右のRayがプレイヤーに当たったら
            if (hitRight.collider != null && hitRight.collider.CompareTag("Player") ||
                 hitLeft.collider != null && hitLeft.collider.CompareTag("Player"))
            {

                Attack(); // 攻撃

                playerController.Hurt();  // ダメージアニメーション
                audioSource.clip = playerController.getDamageClip;
                audioSource.Play(); // ダメージ音
            }

            transform.position += moveVelocity * movePower * Time.deltaTime;
        }


        void Jump()
        {
            if (!animator.GetBool("isJump"))
                animator.SetBool("isJump", true);
            // 速度初期化
            rb.velocity = Vector2.zero;
            // Y軸方向の移動量を取得し、その方向へ力を加える
            Vector2 jumpVelocity = new Vector2(0, jumpPower);
            rb.AddForce(Vector2.up * jumpVelocity, ForceMode2D.Impulse);
        }


        void Attack()
        {
            if (!animator.GetBool("attack"))
                animator.SetTrigger("attack");
        }

        public void Idle()
        {
            if (!animator.GetBool("idle"))
                animator.SetBool("isRun", false);
        }

        public void CallLostHPFunc()
        {
            playerHp.DecreasePlayerHP();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            /* collision.gameObject.tagは 　      string　errorを返す
               collision.gameObject.CompareTagは  bool    falseを返す */

            animator.SetBool("isJump", false);

            // 地面に接触中なら
            if (collision.collider.CompareTag("Stage"))
            {
                // 3秒後にジャンプ入力可能
                Invoke("EnableJump", 3f);
            }
            // プレイヤーに接触中なら
            if (collision.collider.CompareTag("Player"))
            {
                // プレイヤーを攻撃
                isTouchingPlayer = true;
            }
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            // 地面から離れたら
            if (collision.collider.CompareTag("Stage"))
            {                
                isGrounded = false;
            }
            // プレイヤーから離れたら
            if (collision.collider.CompareTag("Player"))
            {
                isTouchingPlayer = false;
            }
        }

        private void EnableJump()
        {
            isGrounded = true;
        }
    }
}