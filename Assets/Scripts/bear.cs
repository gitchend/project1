using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bear : MonoBehaviour
{
    private charactor bearer;
    private GameObject parent;
    private Rigidbody2D rb;
    private effect_controller ec;
    private audio_controller ac;
    private buff_controller bc;

    void Start ()
    {
        parent = transform.parent.gameObject;

        bearer = parent.GetComponent<enemy> ();
        if (bearer == null)
        {
            bearer = parent.GetComponent<player> ();
        }
        rb = parent.GetComponent<Rigidbody2D> ();
        ec = GameObject.Find ("effect_controller").GetComponent<effect_controller> ();
        ac = GameObject.Find ("audio_controller").GetComponent<audio_controller> ();
        bc = GameObject.Find ("buff_controller").GetComponent<buff_controller> ();
    }

    void Update ()
    {

    }
    void OnTriggerEnter2D (Collider2D collider)
    {
        attack attack = collider.gameObject.GetComponent<attack> ();
        charactor attacker = attack.get_attacker ();
        if (attacker == bearer)
        {
            return;
        }
        if (!attack.is_valid ())
        {
            return;
        }
        if(!bearer.get_is_attackable())
        {
            return;
        }
        //flag only one hit attack
        attack.attacked ();


        float position_x = bearer.transform.position.x - attacker.transform.position.x;
        bearer.set_direction (position_x < 0);

        Vector2 derection = transform.position - (attack.is_melee ? attacker.transform.position : attack.transform.position);
        rb.velocity = new Vector2 ((attack.beatback_damage * (derection.x > 0 ? 1 : -1)), rb.velocity.y);
        //floating
        if (attack.is_ground_floating || bearer.get_in_air ())
        {
            rb.velocity = new Vector2 (rb.velocity.x, attack.floating_damage);
        }

        attacker.add_speed(attack.self_beatback_damage* (position_x< 0 ? 1 : -1),attack.self_floating_damage);

        bearer.set_hp (bearer.hp_now - attack.damage);
        bc.create_buff (1, attacker, bearer, attack.stun, attack.frame_extract);


        //face to attacker

        bearer.set_last_attacked (attacker);

        attacker.hit_message (attack);
        bearer.hitted_message(attack);
        //frame_extract
        bearer.set_frame_extract (attack.frame_extract);
        if (attack.is_melee)
        {
            attacker.set_frame_extract (attack.frame_extract);
        }

        //effect
        ec.create_effect (1, false, parent, attack.frame_extract);
        ec.create_effect (2, (Random.value > 0.5f), parent, attack.frame_extract);

        ec.create_effect (5, bearer.get_direction(), parent, new Vector2((bearer.get_direction() ? 1 : -1) * (Random.value * 0.1f - 0.05f + 0.1f), Random.value * 0.1f - 0.12f), 0);

        bc.create_buff(100, bearer, bearer, 10, attack.frame_extract);

        //audio
        ac.create_audio ((Random.value > 0.5f ? 1 : 2), attack.frame_extract);
    }

    void OnTriggerExit2D (Collider2D collider) { }
    void OnTriggerStay2D (Collider2D collider) { }
    public charactor get_parent ()
    {
        return bearer;
    }
}