using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sword_light : spell_script
{
    private float speed;
    private Rigidbody2D rb;
    private bool is_dead = false;
    private bool direction;
    private int life_time;
    private GameObject explode_attack;

    void OnEnable ()
    {
        if (speller != null)
        {
            rb = GetComponent<Rigidbody2D> ();
            explode_attack = transform.Find("attack").gameObject;
            explode_attack.SetActive(false);

            direction = speller.get_direction();
            speed = 3f;

            Vector2 parent_position = speller.get_position2 ();

            transform.position = new Vector3 (parent_position.x + (direction ? 1 : -1) * 0.4f, parent_position.y-0.05f, transform.position.z);
            transform.localScale = new Vector2 ((direction ? 1 : -1), 1);

            attack attack = gameObject.GetComponent<attack> ();
            attack.set_attacker (speller);
            explode_attack.GetComponent<attack> ().set_attacker (speller);

            life_time = 200;
        }
    }
    void Update ()
    {
        if (!is_dead)
        {
            rb.velocity = new Vector2((direction ? 1 : - 1)*speed, 0);
            if(life_time-- < 0)
            {
                is_dead = true;
                rb.velocity = new Vector2(0, 0);
                Destroy (GetComponent<Collider2D> ());
                explode_attack.SetActive(true);
                life_time = 100;
            }
        }
        else if(life_time-- > 0)
        {
            rb.velocity = new Vector2((direction ? 0.2f : - 0.2f)*speed, 0);
            if(life_time % 20 == 0)
            {

                explode_attack.SetActive(true);
            }
            if(life_time % 20 == 10)
            {
                explode_attack.SetActive(false);
            }
        }
        else
        {
            Destroy (transform.gameObject);
        }

    }
    void OnTriggerEnter2D (Collider2D collider)
    {
        if (!is_dead)
        {
            bear bear = collider.gameObject.GetComponent<bear> ();
            if (bear != null)
            {
                charactor bearer = bear.get_parent ();
                if (speller == bearer)
                {
                    return;
                }
            }
            life_time = 0;
        }
    }
}