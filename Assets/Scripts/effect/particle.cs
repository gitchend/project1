using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class particle : MonoBehaviour
{
    public float position_z=0.1f;

    protected Rigidbody2D rb;
    protected SpriteRenderer renderer;
    private int life_time = 0;
    private int life_time_max = 0;
    private int wait_time = 0;
    private Color color_from;
    private Color color_to;
    private Vector2 velocity;
    private Vector2 position_miu;
    private GameObject target;
    private GameObject sprite;

    private void init ()
    {
        rb = GetComponent<Rigidbody2D> ();
        sprite = transform.Find("sprite").gameObject;
        renderer = sprite.GetComponent<SpriteRenderer> ();
        renderer.enabled = false;

    }
    public void init (int life_time_set)
    {
        init ();
        life_time = life_time_set;
        color_from = renderer.color;
        color_to = color_from;
        life_time_max = life_time;
    }
    public void init (int life_time_set, int wait_time_set, Color color)
    {
        init ();
        renderer.color = color;
        life_time = life_time_set;
        wait_time = wait_time_set;
        color_from = renderer.color;
        color_to = color_from;
        life_time_max = life_time;
    }
    public void init (int life_time_set, int wait_time_set, Color color_now_set, Color color_to_set)
    {
        init ();
        renderer.color = color_now_set;
        color_to = color_to_set;
        life_time = life_time_set;
        wait_time = wait_time_set;
        color_from = renderer.color;
        life_time_max = life_time;

    }
    void Update ()
    {
        adjust_pixel();
        if (wait_time > 0)
        {
            wait_time--;
        }
        else
        {
            if (wait_time == 0)
            {
                if(target==null){
                    Destroy (transform.gameObject);
                    return;
                }
                if(position_miu == null)
                {
                    position_miu = new Vector2(0, 0);
                }
                renderer.enabled = true;
                transform.position = target.transform.position;
                transform.position += new Vector3(position_miu.x, position_miu.y, position_z);
                rb.velocity = velocity;
                wait_time--;
            }
            if (life_time-- <= 0)
            {
                Destroy (transform.gameObject);
                return;
            }
            else
            {
                if(color_to != null)
                {
                    float rate = 1 - (life_time * 1.0f / life_time_max );
                    rate *= 2;
                    if(rate > 1)
                    {
                        rate = 1;
                    }
                    Color color_now = new Color(color_from.r + (color_to.r - color_from.r)*rate,
                                                color_from.g + (color_to.g - color_from.g)*rate,
                                                color_from.b + (color_to.b - color_from.b)*rate,
                                                color_from.a + (color_to.a - color_from.a)*rate);
                    renderer.color = color_now;

                }
            }
        }
    }
    public void set_gravity(float gravity_set)
    {
        rb.gravityScale = gravity_set;
    }
    public void set_speed (Vector2 velocity_set)
    {
        velocity = velocity_set;
    }
    public void set_target (GameObject target_set)
    {
        target = target_set;
    }
    public void set_position_miu(Vector2 position_miu_set)
    {
        position_miu = position_miu_set;
    }
    void OnTriggerEnter2D (Collider2D collider)
    {
        Destroy (GetComponent<Collider2D> ());
        Destroy (GetComponent<Rigidbody2D> ());

    }
    private void adjust_pixel()
    {
        if(((int)transform.localScale.x) % 2 == 0)
        {
            sprite.transform.position = new Vector3(pixel_fix(transform.position.x), pixel_fix(transform.position.y), transform.position.z);
        }
        else
        {
            sprite.transform.position = new Vector3(pixel_fix2(transform.position.x), pixel_fix2(transform.position.y), transform.position.z);
        }

    }
    private float pixel_fix(float float_num)
    {
        return (int)(float_num * 64 - 1) / 64.0f;
    }
    private float pixel_fix2(float float_num)
    {
        return (int)(float_num * 64 - 1) / 64.0f + 1 / 128.0f;
    }

}