using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    public float speed;
    public float stoppingDistance;
    public float retreatDistance;

    public float timeBtwShots;
    public float startTimeBtwShots;


    public GameObject projectile;
    public Transform Player;
   


    //public int startHealth = 100;
    public float health = 100f;

    [Header("Unity stuff")]
    public Image healthBar;

    // Start is called before the first frame update
    void Start()
    {

        Player = GameObject.FindGameObjectWithTag("Player").GetComponent <Transform>();
        timeBtwShots = startTimeBtwShots;

        //health = startHealth;

    }

        // Update is called once per frame
    void Update()
    {
        healthBar.fillAmount = health / 100f;

        if (Vector2.Distance(transform.position, Player.position) > stoppingDistance)
        {

            transform.position = Vector2.MoveTowards(transform.position, Player.position, speed * Time.deltaTime);
        }
        else if (Vector2.Distance(transform.position, Player.position) < stoppingDistance && Vector2.Distance(transform.position, Player.position) > retreatDistance)
        {

            transform.position = this.transform.position;
        }
        else if (Vector2.Distance(transform.position, Player.position) < retreatDistance)
        {

            transform.position = Vector2.MoveTowards(transform.position, Player.position, -speed * Time.deltaTime);
        }

        if (timeBtwShots <=0){
            Instantiate(projectile, transform.position, Quaternion.identity);
            timeBtwShots = startTimeBtwShots;


        }else {
            timeBtwShots -= Time.deltaTime;
        }
        




        Debug.Log(health);

        

        if(health <= 0f)
        {
            Destroy(this.gameObject);
        }
    }
}
