using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scroll1 : spell_script
{

    private Animator animator;
    private bool attacked = false;
    private bool inited = false;
    private bool direction;
    private charactor bearer;
    private int time_full;
    void OnEnable ()
    {
        if (speller != null)
        {
            Vector2 parent_position = speller.get_position2 ();
            bool speller_direction = speller.get_direction ();
            transform.position = new Vector3 (parent_position.x + (speller_direction ? 1 : -1) * 0.35f, parent_position.y + (-0.1f - 0.05f * Random.value), -5);
            transform.localScale = new Vector2 ((speller_direction ? 1 : -1), 1);
            transform.parent=speller.transform;
            attack attack = gameObject.GetComponent<attack> ();
            attack.set_attacker (speller);
            animator = GetComponent<Animator> ();
            inited = true;
            time_full = 300;
        }
    }
    void Update ()
    {
        if (!inited)
        {
            return;
        }
        if (is_anime_now_name ("scroll_bullet1_1"))
        {
            animator.SetBool ("floating", false);
        }
        else if (is_anime_now_name ("scroll_bullet1_2"))
        {
            if (!attacked)
            {
                Destroy (transform.gameObject);
            }
            else
            {
                if(direction != bearer.get_direction())
                {
                    turn ();
                }
                if (bearer.get_speed2 ().y < -0.2f || Mathf.Abs (bearer.get_speed2 ().x) > 0.2f)
                {
                    animator.SetBool ("floating", true);
                }
            }
        }
        if(attacked)
        {
            time_full--;
        }
        if(time_full < 0)
        {
            Destroy (transform.gameObject);
        }
    }
    void OnTriggerEnter2D (Collider2D collider)
    {
        if (!inited)
        {
            return;
        }
        bear bear = collider.gameObject.GetComponent<bear> ();
        if (bear != null && speller != bear.get_parent ())
        {
            attacked = true;
            bearer = bear.get_parent ();
            direction = bearer.get_direction();
            transform.parent = bearer.transform;
            bc.create_buff(1, speller, bearer, time_full, 0);
            speller.set_scroll_hit();
        }
    }
    private bool is_anime_now_name (string anime_name)
    {
        AnimatorStateInfo stateinfo = animator.GetCurrentAnimatorStateInfo (0);
        return stateinfo.IsName ("Base Layer." + anime_name);
    }
    private void turn ()
    {
        direction = !direction;
        transform.localScale += new Vector3 (-2 * transform.localScale.x, 0, 0);
    }
}