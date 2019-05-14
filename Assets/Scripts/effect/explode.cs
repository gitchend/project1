using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explode : MonoBehaviour
{
    void Start ()
    {
        GameObject pixel_res = Resources.Load ("pixel") as GameObject;

        for (int i = 0; i <= 50; i++)
        {
            GameObject pixel = Instantiate (pixel_res) as GameObject;
            particle particle = pixel.GetComponent<particle> ();

            int size = (int)(Random.value * 3) + 1;
            pixel.transform.localScale = new Vector2(size, size);

            int life_time = (int) (60 * Random.value + 60);
            int wait_time = (int) (15 * Random.value);

            float speed = 3.5f;
            Vector2 speed2 = new Vector2 (Random.value * 2 - 1, Random.value * 2 - 1 ).normalized *speed;

            particle.init (life_time, wait_time, new Color(1, 0.8978f, 0, 1), new Color (1f, 0.259f, 0, 1));
            particle.set_gravity(0.05f * (Random.value * 2 - 1)+0.8f);
            particle.set_speed (speed2);
            particle.set_target (transform.parent.gameObject);
            particle.set_position_miu(transform.localPosition);
        }
        Destroy (transform.gameObject);
    }
}