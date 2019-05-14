using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buff_controller : MonoBehaviour
{

    private List<charactor> charactors = new List<charactor> ();
    void Start () { }
    void Update ()
    {
        foreach (charactor charactor in charactors)
        {
            if (charactor == null)
            {
                continue;
            }
            foreach (buff buff in charactor.get_buff_map ().Values)
            {
                if (!buff.buff_to.get_is_pause () || buff.buff_code == 1 || buff.buff_code == 3)
                {
                    if (buff.wait_time > 0)
                    {
                        buff.wait_time--;
                    }
                    else
                    {
                        if (buff.time_now > 0)
                        {
                            if (buff.time_full == buff.time_now)
                            {
                                buff.start ();
                            }
                            if (buff.time_now > 1)
                            {
                                buff.update ();
                            }
                            if (buff.time_now == 1)
                            {
                                buff.end ();
                            }
                            buff.time_now--;
                        }
                        else if (buff.time_now < 0)
                        {
                            buff.update ();
                        }
                    }
                }
            }
        }
    }
    public void create_buff (int buff_code, charactor buff_from, charactor buff_to, int time_full, int wait_time)
    {
        if (time_full > 0)
        {
            if (!charactor_exist (buff_to))
            {
                charactors.Add (buff_to);
            }
            buff old_buff = null;
            if (buff_to.get_buff_map ().ContainsKey (buff_code))
            {
                old_buff = buff_to.get_buff_map () [buff_code];
            }
            if (old_buff == null)
            {
                buff new_buff = buff.new_buff (buff_code);
                new_buff.init (buff_code, buff_from, buff_to, time_full, wait_time, this);
                buff_to.get_buff_map () [buff_code] = new_buff;
            }
            else
            {
                if (time_full > old_buff.time_now)
                {
                    old_buff.time_full = time_full;
                    if (old_buff.time_now == 0)
                    {
                        old_buff.time_now = old_buff.time_full + 1;
                    }
                    else if (old_buff.time_now > 0)
                    {
                        old_buff.time_now = old_buff.time_full - 1;
                    }
                }
            }
        }
    }
    private bool charactor_exist (charactor charactor_find)
    {
        foreach (charactor charactor in charactors)
        {
            if (charactor_find == charactor)
                return true;
        }
        return false;
    }
}