using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attack_scrap : effect_script
{
    private int wait = 1;
    void Start ()
    {

    }
    void Update()
    {
        if(is_active)
        {
            wait--;
            if(wait < 0)
            {
                GameObject pixel_res = Resources.Load ("pixel") as GameObject;
                for (int i = 0; i <= 5; i++)
                {
                    GameObject pixel = Instantiate (pixel_res) as GameObject;
                    particle particle = pixel.GetComponent<particle> ();

                    int size = (int)(Random.value * 3) + 1;
                    pixel.transform.localScale = new Vector2(size, size);

                    int life_time = (int) (20 * Random.value + 20);
                    int wait_time = (int)(2 * Random.value);


                    particle.init (life_time, wait_time, new Color(0, 0, 0, 1), new Color(0, 0, 0, 0));
                    particle.set_speed (random_vector(direction2) * (Random.value * 8 + 8));
                    particle.set_gravity(0.05f * (Random.value * 2 - 1) + 0.1f);
                    particle.set_target (transform.parent.gameObject);
                }
                for (int i = 0; i <= 20; i++)
                {
                    GameObject pixel = Instantiate (pixel_res) as GameObject;
                    particle particle = pixel.GetComponent<particle> ();

                    int size = (int)(Random.value * 3) + 1;
                    pixel.transform.localScale = new Vector2(size, size);

                    int life_time = (int) (35 * Random.value + 35);
                    int wait_time = (int)(2 * Random.value);


                    particle.init (life_time, wait_time, new Color(0, 0, 0, 1), new Color(0, 0, 0, 0));
                    particle.set_speed (random_vector(direction2) * (Random.value * 4 + 4));
                    particle.set_gravity(0.05f * (Random.value * 2 - 1) + 1.5f);
                    particle.set_target (transform.parent.gameObject);
                }
                Destroy (transform.gameObject);
            }

        }


    }
    private Vector2 random_vector(Vector2 vector2)
    {
        Vector2 offset = (vector2 + new Vector2(1, 1)) * 0.9f;
        return new Vector2((vector2.x + Random.value * offset.x), vector2.y + Random.value * offset.y).normalized;
    }
}