using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public GameObject hitEffect;


    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.CompareTag("Wall"))
        {
           // GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
            //Destroy(effect, 5f);
            Destroy(this.gameObject);
        }

        if(collision.collider.CompareTag("Enemy"))
        {
            collision.collider.GetComponent<EnemyController>().health -= 10;
            Destroy(this.gameObject);
        }
    }
}
