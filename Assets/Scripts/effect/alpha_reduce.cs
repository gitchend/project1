using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class alpha_reduce : MonoBehaviour
{
    public int die_in_frame;
    public float color=0;
    public float start_rate=0.8f;
    private SpriteRenderer renderer=null;
    private int frame_now;
    void Start ()
    {
        renderer = GetComponent<SpriteRenderer> ();
        frame_now = (int)(die_in_frame*start_rate);
    }
    void Update ()
    {
        if(die_in_frame > 0 && frame_now >= 0)
        {
            float die_rate=(int)((frame_now * 1.0f / die_in_frame)/0.2f)*0.2f;
            renderer.color = new Color(color, color, color,die_rate );
            frame_now--;
        }
        else
        {
            Destroy (transform.gameObject);
        }
    }
}