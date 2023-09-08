using UnityEngine;

// �t�@�C���ۑ��@File.WriteAllText("test.txt","Hello Unity");

public class ItemSpawnerMonster : MonoBehaviour
{
    [SerializeField] GameObject stage;
    private PlayerHP p;
    private CountDownAtFirst countDown;

    ScoreManager scoreMgr;
    Animator monmusuAnim;
    Rigidbody2D rb2d;
    public GameObject[] scoreItems;
    public GameObject bombTrap;
    public GameObject healItem;

    int hashRandomIdle = Animator.StringToHash("randomIdle");
    int hashRandomSpawn = Animator.StringToHash("randomSpawn");

    float bombSpawnLimiter = 0.5f;
    private void Start()
    {
        countDown = GameObject.Find("CountDown").GetComponent<CountDownAtFirst>();

        monmusuAnim = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        stage = GameObject.FindGameObjectWithTag("Stage").GetComponent<GameObject>();

        scoreMgr = GetComponent<ScoreManager>();
        p = GameObject.Find("PlayerHP").GetComponent<PlayerHP>();

        bombSpawnLimiter = UnityEngine.Random.value;
    }


    private void Update()
    {
        if (!countDown.startGame) return;  // �J�E���g�_�E��

        if (p.isDead) return;
        if (p.isPaused) return;
        PlayRandomIdleAnim();// �X�|�i�[�̃A�j���[�V�����������_���Đ�
        PlayRandomAttackAnim();
    }

    private void FixedUpdate()
    {

        if (p.isPaused) return;
        ItemSpawn_RndPos();
    }


    float timer = 0;
    float spawnTime = 4.0f;
    void ItemSpawn_RndPos()
    {

        this.timer += Time.deltaTime;
        // ��莞�Ԍo�ߌ�A�������X�^�[�����ʂ������Ă���Ƃ�
        if (this.timer > spawnTime && monmusuAnim.GetCurrentAnimatorStateInfo(0).IsName("orb_front"))
        {
            timer = 0;

            GameObject[] receiveIndex = new GameObject[3];

            // 8�̃A�C�e���̒����烉���_���łR��z��Ɋi�[
            receiveIndex[0] = Instantiate(scoreItems[UnityEngine.Random.Range(0, scoreItems.Length)]);
            receiveIndex[1] = Instantiate(scoreItems[UnityEngine.Random.Range(0, scoreItems.Length)]);
            receiveIndex[2] = Instantiate(scoreItems[UnityEngine.Random.Range(0, scoreItems.Length)]);

            // ��----��ok


            for (int i = 0; i < receiveIndex.Length; i++)
            {
                float x = UnityEngine.Random.Range(-7.0f, 7.0f);
                float y = UnityEngine.Random.Range(5, 10);

                receiveIndex[i].transform.position = new Vector3(x + transform.position.x, y + transform.position.y, 0);
            }

            // �g���b�v����(����50���̊m��)
            if (UnityEngine.Random.value > bombSpawnLimiter)
            {
                GameObject trap = Instantiate(bombTrap);
                trap.transform.position = new Vector3(UnityEngine.Random.Range(-7.0f, 7.0f), UnityEngine.Random.Range(5, 10), 0);

                ScoreManager s = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
                if (s.currentScore >= 1000) bombSpawnLimiter -= 0.1f; // 1000�X�R�A���ƂɊm����10������
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
        monmusuAnim.SetInteger(hashRandomIdle, randomIndex);
    }

    void PlayRandomAttackAnim()
    {
        int randomIndex = UnityEngine.Random.Range(0, 5);
        monmusuAnim.SetInteger(hashRandomSpawn, randomIndex);
    }
}