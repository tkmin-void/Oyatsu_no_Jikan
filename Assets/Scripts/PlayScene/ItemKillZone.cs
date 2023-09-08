using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemKillZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject != null) Destroy(collision.gameObject);
    }
}
