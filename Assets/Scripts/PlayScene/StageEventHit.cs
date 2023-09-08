using UnityEngine;

public class StageEventHit: MonoBehaviour
{

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("ScoreItem") ||
           collision.gameObject.CompareTag("DamageItem") ||
           collision.gameObject.CompareTag("PlayerLIFE") ||
           collision.gameObject.CompareTag("HealItem"))
        {
            Destroy(collision.gameObject);
        }
    }
}
