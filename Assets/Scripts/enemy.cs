using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy : charactor
{

    public int enemy_kind;

    private int about_to_do = 0;

    private bool move_lock = false;
    private bool attack_lock = false;

    private int envy_change_time = 2000;
    private int envy_time = 0;
    private GameObject img_obj_disappear;

    public override void Start2 ()
    {
        img_obj_disappear = Resources.Load("img_obj_disappear") as GameObject;
    }
    // Update is called once per frame
    public override void Update2 ()
    {
        switch (enemy_kind)
        {
        case 0:
            enemy_control_0 ();
            break;
        }
    }

    private void enemy_control_0 ()
    {
        envy_control ();
        floating_control (0.8f, 0.5f, 2.0f);

        if(die_control())
        {
            return;
        }
        if (is_stun)
        {
            if (is_anime_now_name ("hit1"))
            {
                set_anime_para (-1);
            }
            else if (is_anime_now_name ("hit2"))
            {
                if (is_hitted)
                {
                    set_anime_para (2);
                    is_hitted = false;
                }
            }
            else if (is_anime_now_name ("hit3"))
            {
                if (is_hitted)
                {
                    set_anime_para (2);
                    is_hitted = false;
                }
            }
            else
            {
                set_anime_para (2);
            }
            return;
        }
        else
        {
            if (is_anime_now_name ("hit1"))
            {
                set_anime_para (3);
            }
            else if (is_anime_now_name ("hit2"))
            {
                set_anime_para (3);
            }
        }

        move_lock = false;
        attack_lock = false;
        if (is_anime_now_name ("idle"))
        {
            move_lock = true;
            attack_lock = true;
            skill (-1);
        }
        else if (is_anime_now_name ("walk"))
        {
            move_lock = true;
            attack_lock = true;
            if (get_anime_normalized_time () % 1 > 0.4)
            {
                move (direction);
            }
            skill (-1);
        }
        else if (is_anime_now_name ("attack1"))
        {
            if (get_anime_normalized_time () > 0.7)
            {
                skill (-1);
            }
            else if (get_anime_normalized_time () > 0.4)
            {
                skill (0);
            }
        }
        else if (is_anime_now_name ("hit1"))
        {
            set_anime_para (-1);
        }

        think ();
        switch (about_to_do)
        {
        case 0:
            break;
        case 1:
            if (Mathf.Abs (target.get_position2 ().x - get_position2 ().x) > 0.5)
            {
                approach (1);
            }
            else
            {
                attack (4);
            }
            break;
        }
    }
    private void think ()
    {
        switch (about_to_do)
        {
        case 0:
            if (target != null)
            {
                about_to_do = 1;
            }
            else
            {
                about_to_do = 0;
            }
            break;
        }
    }
    private void approach (int walk_anime_id)
    {
        if (move_lock)
        {
            bool direction_miu = target.get_position2 ().x - get_position2 ().x > 0;
            if (direction_miu != direction)
            {
                turn ();
            }
            set_anime_para (walk_anime_id);
            if (against_wall)
            {
                set_speed (rb.velocity.x, jump_speed);
            }
        }
    }
    private void attack (int attack_anime_id)
    {
        if (attack_lock)
        {
            set_anime_para (attack_anime_id);
        }
    }
    private void envy_control ()
    {
        if (envy_time > 0)
        {
            envy_time--;
            return;
        }
        if (last_attacked != null)
        {
            target = last_attacked;
            envy_time = envy_change_time;
        }
    }
    private void die()
    {
        GameObject shadow = Instantiate (img_obj_disappear) as GameObject;
        shadow.GetComponent<alpha_reduce> ().die_in_frame = 300;
        shadow.GetComponent<alpha_reduce> ().color = 1;
        shadow.GetComponent<alpha_reduce> ().start_rate = 1;
        shadow.GetComponent<SpriteRenderer> ().sprite = sprite.GetComponent<SpriteRenderer> ().sprite;
        shadow.transform.position = new Vector3(transform.position.x, transform.position.y, 0.1f);
        shadow.transform.localScale = new Vector3((direction ? 1 : -1), 1, 1);

        Destroy(gameObject);
    }
    private bool die_control()
    {
        if (is_anime_now_name ("die1"))
        {
            if(once_in_animation())
            {
                bc.create_buff(100, this, this, 100, 0);
                for(int i = 0; i < 4; i++)
                {
                    ec.create_effect (10, direction, gameObject, new Vector2((direction ? -1 : 1) * (Random.value * 0.1f - 0.05f + 0.1f), Random.value * 0.1f - 0.2f), null, i * 2, new Vector2(-2 + i, 1));
                }
            }
            set_anime_para(-1);
        }
        else if (is_anime_now_name ("die2"))
        {
            if(once_in_animation())
            {
                for(int i = 0; i < 4; i++)
                {
                    ec.create_effect (10, direction, gameObject, new Vector2((direction ? -1 : 1) * (Random.value * 0.1f - 0.05f + 0.1f), Random.value * 0.1f - 0.2f), null, i * 2, new Vector2(-2 + i, 1));
                }
            }
            if(timer % 16 == 0)
            {
                ec.create_effect (11, direction, gameObject, new Vector2(0, 0), this, 0,  new Vector2(0, 0));
            }
            if(get_anime_normalized_time() > 5)
            {
                die();
            }
        }

        if(hp_now == 0 && !is_dead)
        {
            if(timer % 16 == 0)
            {
                ec.create_effect (11, direction, gameObject, new Vector2(0, 0), this, 0,  new Vector2(0, 0));
            }
        }
        if(!in_air && hp_now == 0 && !is_dead)
        {
            set_anime_para(5);
            gameObject.layer = 13;
            Destroy(transform.Find("bear").gameObject);
            is_dead = true;
            set_speed(speed * (direction ? -2 : 2), jump_speed / 8);
        }

        return is_dead;
    }

}