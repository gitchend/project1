using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class effect_script: MonoBehaviour
{
    protected bool direction;
    protected GameObject target;
    protected Vector2 position;
    protected charactor charactor;
    protected Vector2 direction2;
    protected bool is_active = false;

    public void set_direction (Vector2 direction_set)
    {
        direction2 = direction_set;
    }
    public void set_active()
    {
        is_active = true;
    }
}