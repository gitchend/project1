using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class particle : MonoBehaviour {

	protected Rigidbody2D rb;
	protected SpriteRenderer renderer;
	private int life_time = 0;
	private int wait_time = 0;
	private Color color_now;
	private Vector2 velocity;
	private GameObject target;
	private void init () {
		rb = GetComponent<Rigidbody2D> ();
		renderer = GetComponent<SpriteRenderer> ();
		renderer.enabled = false;
	}
	public void init (int life_time_set) {
		init ();
		life_time = life_time_set;
	}
	public void init (int life_time_set, int wait_time_set, Color color) {
		init ();
		renderer.color = color;
		life_time = life_time_set;
		wait_time = wait_time_set;
	}
	void Update () {
		if (wait_time > 0) {
			wait_time--;
		} else {
			if (wait_time == 0) {
				renderer.enabled = true;
				transform.position = target.transform.position;
				transform.position +=new Vector3(0,0,-5f);
				rb.velocity = velocity;
				wait_time--;
			}
			if (life_time-- <= 0) {
				Destroy (transform.gameObject);
			}
		}
	}
	public void set_speed (Vector2 velocity_set) {
		velocity = velocity_set;
	}
	public void set_target (GameObject target_set) {
		target = target_set;
	}
}