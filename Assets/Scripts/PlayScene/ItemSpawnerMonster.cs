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
    float spawnTime = 4.0f; // �A�C�e�������̊Ԋu����

    private void Start()
    {
        countDown = GameObject.Find("CountDown").GetComponent<CountDownAtFirst>();

        animator = GetComponent<Animator>();

        playerHp = GameObject.Find("PlayerHP").GetComponent<PlayerHP>();

        bombSpawnLimiter = UnityEngine.Random.value;
    }


    private void Update()
    {
        if (!countDown.startGame) return;  // �J�E���g�_�E��
        if (playerHp.isDead) return;

        // �X�|�i�[�̃A�j���[�V�����������_���Đ�
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
        // ��莞�Ԍo�ߌ�A�������X�^�[�����ʂ������Ă���Ƃ�
        if (timer > spawnTime && animator.GetCurrentAnimatorStateInfo(0).IsName("orb_front"))
        {
            timer = 0;

            GameObject[] receiveIndex = new GameObject[3];

            // 8�̃A�C�e���̒����烉���_���łR��z��Ɋi�[
            receiveIndex[0] = Instantiate(scoreItems[UnityEngine.Random.Range(0, scoreItems.Length)]);
            receiveIndex[1] = Instantiate(scoreItems[UnityEngine.Random.Range(0, scoreItems.Length)]);
            receiveIndex[2] = Instantiate(scoreItems[UnityEngine.Random.Range(0, scoreItems.Length)]);

            for (int i = 0; i < receiveIndex.Length; i++)
            {
                //�@�A�C�e�������͈́@X�@Y
                float x = UnityEngine.Random.Range(-7.0f, 7.0f);
                float y = UnityEngine.Random.Range(5, 10);

                receiveIndex[i].transform.position = new Vector3(x + transform.position.x, y + transform.position.y, 0);
            }

            // �{������(����50���̊m��)
            if (UnityEngine.Random.value > bombSpawnLimiter)
            {
                GameObject trap = Instantiate(bombTrap);
                trap.transform.position = new Vector3(UnityEngine.Random.Range(-7.0f, 7.0f), UnityEngine.Random.Range(5, 10), 0);

                // 1000�X�R�A���ƂɊm����10������
                ScoreManager s = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
                if (s.currentScore >= 1000) bombSpawnLimiter -= 0.1f;
            }

            // �񕜃A�C�e����20���̊m���Ő���
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