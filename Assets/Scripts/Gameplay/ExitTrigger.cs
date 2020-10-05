using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitTrigger : MonoBehaviour
{
    public GameObject WinPrefab;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Hamster"))
        {
            if (WinPrefab)
            {
                var obj = GameObject.Instantiate(WinPrefab, transform.position, transform.rotation);
                Destroy(obj, 2.0f);
            }

            GameManager.GM.TriggerLevelComplete();
        }
    }
}
