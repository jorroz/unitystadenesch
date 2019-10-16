using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    public float speed;
    public float inertia = 4;
    public float damping = 0.3F;
    public float stoppingDistance;
    public float retreatDistance;

    public float timeBtwShots;
    public float startTimeBtwShots;


    public GameObject projectile;
    public Transform player;

    public Rigidbody2D body;



    //public int startHealth = 100;
    public float health = 100f;

    [Header("Unity stuff")]
    public Image healthBar;

    // Start is called before the first frame update
    void Start()
    {

        body = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        timeBtwShots = startTimeBtwShots;

        //health = startHealth;

    }

    // Update is called once per frame
    void Update()
    {
        healthBar.fillAmount = health / 100f;

        Vector2 diff = player.position - transform.position;
        float expectedAngle = Mathf.Atan2(diff.y, diff.x) / Mathf.PI * 180;
        transform.rotation = Quaternion.Euler(0, 0, expectedAngle);

        if (timeBtwShots <= 0)
        {
            Vector3 p = transform.TransformPoint(1, 0, 0);
            GameObject bullet = Instantiate(projectile, p, transform.rotation);
            Rigidbody2D body = bullet.GetComponent<Rigidbody2D>();
            body.velocity = (p - transform.position) * 15;
            timeBtwShots = startTimeBtwShots;

        }
        else
        {
            timeBtwShots -= Time.deltaTime;
        }





        Debug.Log(health);



        if (health <= 0f)
        {
            Destroy(this.gameObject);
        }

    }


    void FixedUpdate()
    {

        // Movement
        if (Vector2.Distance(transform.position, player.position) > stoppingDistance)
        {
            // body.velocity = body.velocity;
            // Vector2 rel = player.position - transform.position;
            // float mul = 1 / inertia;
            // rel *= mul * Time.fixedDeltaTime;


            Vector2 dir = player.position - transform.position;
            dir.Normalize();
            // dir -= body.velocity;
            dir *= speed / inertia;
            body.velocity += dir * speed;
        }
        else if (Vector2.Distance(transform.position, player.position) < stoppingDistance && Vector2.Distance(transform.position, player.position) > retreatDistance)
        {

            // transform.position = this.transform.position;
        }
        else if (Vector2.Distance(transform.position, player.position) < retreatDistance)
        {

            // body.velocity = body.velocity;
            // Vector2 rel = player.position - transform.position;
            // float mul = 1 / inertia;
            // rel *= mul * Time.fixedDeltaTime;

            Vector2 dir = player.position - transform.position;
            dir.Normalize();
            // dir += body.velocity;
            dir *= speed / inertia;
            body.velocity -= dir * speed;
        }

        body.velocity *= 1 - damping;
    }
}
