using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class gate : MonoBehaviour
{
    public int level = 0;
    public int gate_num = 0;
    public bool direction = false ;
    public bool direction_y = false ; //0 left ; 1 right ; 2 up ; 3 down

    void Start()
    {

    }
    void OnTriggerStay2D (Collider2D collider)
    {
        player player = collider.gameObject.transform.parent.GetComponent<player>();
        bool is_gate_open = false;
        if(direction_y)
        {
            if(direction)
            {
                if(player.is_anime_now_name("jump2"))
                {
                    is_gate_open = true;
                }
            }
            else
            {
                if(player.is_anime_now_name("jump3") || player.is_anime_now_name("jump5"))
                {
                    is_gate_open = true;
                }
            }
        }
        else
        {
            if(player.is_anime_now_name("run2") || player.is_anime_now_name("run1"))
            {
                if(player.get_direction() == direction)
                {
                    is_gate_open = true;
                }
            }
        }
        if(is_gate_open)
        {
            player.set_cross_gate(direction, direction_y);
            GameObject.Find("main_control").GetComponent<main_control>().change_level(level, gate_num);
        }

    }
}