using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet_control : MonoBehaviour
{
    public Sprite[] img;
    private player target;
    private int bullet_max;
    private int bullet_now;
    private List<GameObject> bullet_list = new List<GameObject> ();
    private GameObject bullet_obj;

    void Start ()
    {
        target = GameObject.Find ("hero").GetComponent<player> ();
        bullet_max = target.get_bullet_max();
        bullet_now = target.get_bullet_now();

        bullet_obj = Resources.Load ("bullet_icon") as GameObject;

        init();
    }

    void Update ()
    {
        bullet_now = target.get_bullet_now();
        if(bullet_max != target.get_bullet_max())
        {
            init();
        }
        else
        {
            int num = (int)(bullet_max / 3);
            for(int i = 0; i < num; i++)
            {
                int num_this = bullet_now - 3 * i;
                if(num_this > 3)
                {
                    num_this = 3;
                }
                if(num_this < 0)
                {
                    num_this = 0;
                }
                GameObject obj = bullet_list[i];
                Sprite sprite_front=null;
                switch(num_this)
                {
                case 0:
                    sprite_front = null;
                    break;
                case 1:
                    sprite_front = (i % 2 == 0 ? img[5] : img[2]);
                    break;
                case 2:
                    sprite_front = (i % 2 == 0 ? img[4] : img[1]);
                    break;
                case 3:
                    sprite_front = (i % 2 == 0 ? img[3] : img[0]);
                    break;
                }
                obj.transform.Find("bullet_icon_front").gameObject.GetComponent<SpriteRenderer>().sprite = sprite_front;
            }
        }
    }
    private void init()
    {
        int num = (int)(bullet_max / 3);
        for(int i = 0; i < num; i++)
        {
            GameObject obj = Instantiate (bullet_obj) as GameObject;
            obj.transform.Find("bullet_icon_back").gameObject.GetComponent<SpriteRenderer>().sprite = (i % 2 == 0 ? img[6] : img[7]);
            obj.transform.parent = gameObject.transform;
            obj.transform.localPosition = new Vector2 (i * 0.234375f, 0);
            bullet_list.Add(obj);
        }
    }
}