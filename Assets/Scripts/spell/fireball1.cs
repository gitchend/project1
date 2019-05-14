using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fireball1 : spell_script
{
    private float speed;
    private Rigidbody2D rb;
    private bool is_dead = false;
    private Animator animator;
    private bool direction;
    private int life_time;
    private Light light;
    private float intensity;
    private GameObject explode_attack;

    void OnEnable ()
    {
        if (speller != null)
        {
            rb = GetComponent<Rigidbody2D> ();
            animator = GetComponent<Animator> ();
            light = transform.Find("light").gameObject.GetComponent<Light>();
            explode_attack = transform.Find("attack").gameObject;
            explode_attack.SetActive(false);
            intensity = 5;

            direction = speller.get_direction();
            speed = 5f;

            Vector2 parent_position = speller.get_position2 ();

            transform.position = new Vector3 (parent_position.x + (direction ? 1 : -1) * 0.4f, parent_position.y - 0.1f, transform.position.z);
            transform.localScale = new Vector2 ((direction ? -1 : 1), 1);

            attack attack = gameObject.GetComponent<attack> ();
            attack.set_attacker (speller);
            explode_attack.GetComponent<attack> ().set_attacker (speller);

            life_time = 300;
            speller.set_scroll_hit();
        }
    }
    void Update ()
    {
        if (!is_dead)
        {
            if(is_anime_now_name("fireball1_1"))
            {
                rb.velocity = new Vector2((direction ? 0.5f : - 0.5f )*speed, 0);
                if( life_time % 8 == 0)
                {
                    ec.create_effect (8, false, transform.gameObject, new Vector2(0, 0), 0);
                }
                light.intensity = get_anime_normalized_time() * intensity;
            }
            else if(is_anime_now_name("fireball1_2"))
            {
                light.intensity = intensity;
                rb.velocity = new Vector2((direction ? 1 : -1)*speed, 0);
                if( life_time % 8 == 0)
                {
                    ec.create_effect (8, false, transform.gameObject, new Vector2(0, 0), 0);
                }
            }
            if(life_time-- < 0)
            {
                is_dead = true;
                rb.velocity = new Vector2(0, 0);
                Destroy (GetComponent<Collider2D> ());
                Destroy (GetComponent<Rigidbody2D> ());
                animator.SetBool(animator.parameters[0].nameHash, true);
                explode_attack.SetActive(true);
                ec.create_effect (9, false, transform.gameObject, new Vector2((direction ? 0.5f : -0.5f), 0), 0);

            }

        }

        if(is_anime_now_name("fireball1_2"))
        {
            light.intensity = intensity + 2;
        }
        if(is_anime_now_name("blank"))
        {
            Destroy (transform.gameObject);
        }

    }
    private bool is_anime_now_name(string anime_name)
    {
        AnimatorStateInfo stateinfo = animator.GetCurrentAnimatorStateInfo(0);
        return stateinfo.IsName("Base Layer." + anime_name);
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
    protected float get_anime_normalized_time()
    {
        AnimatorStateInfo stateinfo = animator.GetCurrentAnimatorStateInfo(0);
        return stateinfo.normalizedTime;
    }
}