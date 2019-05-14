using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class effect
{
    public int effect_mode;
    public int effect_code;
    public bool direction;
    public int wait_time;
    public GameObject target;
    public Vector2 position;
    public charactor charactor;
    public Vector2 direction2;

    public effect() {}

    public effect (int effect_code_set, bool direction_set, int wait_time_set, GameObject target_set)
    {
        effect_code = effect_code_set;
        direction = direction_set;
        wait_time = wait_time_set;
        target = target_set;
        effect_mode = 0;
    }
    public effect (int effect_code_set, bool direction_set, int wait_time_set, Vector2 position_set)
    {
        effect_code = effect_code_set;
        direction = direction_set;
        wait_time = wait_time_set;
        position = position_set;
        effect_mode = 1;
    } 
    public effect (int effect_code_set, bool direction_set, int wait_time_set, GameObject target_set, Vector2 position_set)
    {
        effect_code = effect_code_set;
        direction = direction_set;
        wait_time = wait_time_set;
        position = position_set;
        target = target_set;
        effect_mode = 2;
    } 
    public effect (int effect_code_set, bool direction_set, int wait_time_set, GameObject target_set, Vector2 position_set, charactor charactor_set)
    {
        effect_code = effect_code_set;
        direction = direction_set;
        wait_time = wait_time_set;
        position = position_set;
        target = target_set;
        charactor = charactor_set;
        effect_mode = 3;
    }
    public effect (int effect_code_set, bool direction_set, int wait_time_set, GameObject target_set, Vector2 position_set, charactor charactor_set,Vector2 direction2_set)
    {
        effect_code = effect_code_set;
        direction = direction_set;
        wait_time = wait_time_set;
        position = position_set;
        target = target_set;
        charactor = charactor_set;
        direction2=direction2_set;
        effect_mode = 4;
    }
}