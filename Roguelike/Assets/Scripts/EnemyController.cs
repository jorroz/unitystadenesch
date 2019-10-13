using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{

    //public int startHealth = 100;
    public float health = 100f;

    [Header("Unity stuff")]
    public Image healthBar;

    // Start is called before the first frame update
    void Start()
    {
        //health = startHealth;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(health);

        healthBar.fillAmount = health / 100f;

        if(health <= 0f)
        {
            Destroy(this.gameObject);
        }
    }
}
