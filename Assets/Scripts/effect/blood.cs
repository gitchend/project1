using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blood : MonoBehaviour {
    void Start () {
        GameObject pixel_res = Resources.Load ("pixel") as GameObject;

        for (int i = 0; i <= 20; i++) {
            GameObject pixel = Instantiate (pixel_res) as GameObject;
            particle particle = pixel.GetComponent<particle> ();

            int life_time = (int) (100 * Random.value + 100);
            int wait_time = (int) (5 * Random.value);
            particle.init (life_time, wait_time, new Color (0.7f, 0, 0));
            particle.set_speed (new Vector2 (Random.value*1*transform.localScale.x, Random.value * 2));
            particle.set_target (transform.parent.gameObject);
        }
        Destroy (transform.gameObject);
    }
}