    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{

    public float speed;

    private Transform player;
    private Vector2 target;

    public GameObject hitEffect;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        target = new Vector2(player.position.x, player.position.y);
    }

    private void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        if (transform.position.x == target.x && transform.position.y == target.y)
        {
            DestroyProjectile();
        }
        
           
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Wall"))
        {
            Destroy(this.gameObject);
        }

        if (collision.collider.CompareTag("Player"))
        {
            collision.collider.GetComponent<PlayerMovement>().playerHealth -= 10;
            Destroy(this.gameObject);
        }
        if (collision.collider.CompareTag("Bullet"))
        {
            Destroy(this.gameObject);
        }
    }

    void DestroyProjectile()
    {
        Destroy(this.gameObject);
    }
}

