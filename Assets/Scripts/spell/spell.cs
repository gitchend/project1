using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spell
{
    public int spell_code;
    public charactor speller;
    public Vector2 direction2;
    public int wait_time = 0;
    public int move_mode = 0;
    public spell(int spell_code_set, charactor speller_set, Vector2 direction2_set, int wait_time_set, int move_mode_set)
    {
        spell_code = spell_code_set;
        speller = speller_set;
        direction2 = direction2_set;
        move_mode = move_mode_set;
        wait_time = wait_time_set;
    }

}