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
    public effect() {}
    //attach to obj
    public effect (int effect_code_set, bool direction_set, int wait_time_set, GameObject target_set)
    {
        effect_code = effect_code_set;
        direction = direction_set;
        wait_time = wait_time_set;
        target = target_set;
        effect_mode = 0;
    }
    //create in world
    public effect (int effect_code_set, bool direction_set, int wait_time_set, Vector2 position_set)
    {
        effect_code = effect_code_set;
        direction = direction_set;
        wait_time = wait_time_set;
        position = position_set;
        effect_mode = 1;
    } public effect (int effect_code_set, bool direction_set, int wait_time_set, GameObject target_set, Vector2 position_set)
    {
        effect_code = effect_code_set;
        direction = direction_set;
        wait_time = wait_time_set;
        position = position_set;
        target = target_set;
        effect_mode = 2;
    }
}