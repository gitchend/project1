using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attack : MonoBehaviour
{
    public int damage = 0;
    public int floating_damage = 0;
    public int beatback_damage = 0;
    public int self_floating_damage = 0;
    public int self_beatback_damage = 0;
    public int stun = 0;
    public int frame_extract = 0;
    public bool is_single_dmg = true;
    public bool is_melee = true;
    public bool is_ground_floating = false;
    public int attack_kind = 0;
    public Vector2 attack_direction = new Vector2(1, 0);

    private bool is_attacked = false;

    private charactor attacker;
    void OnEnable()
    {
        is_attacked = false;
    }
    public charactor get_attacker ()
    {
        return attacker;
    }
    public void set_attacker (charactor attacker_set)
    {
        attacker = attacker_set;
    }
    public bool is_valid()
    {
        return !(is_single_dmg && is_attacked);
    }
    public void attacked()
    {
        is_attacked = true;
    }

}