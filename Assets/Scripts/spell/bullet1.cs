using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet1 : spell_script {
	public Sprite[] bullet_sprite;
	private SpriteRenderer sr;
	private float speed;
	private Rigidbody2D rb;
	private bool is_dead = false;
	private int dead_time;
	
	void OnEnable () {
		if (speller != null) {
			direction2 = direction2.normalized;
			rb = GetComponent<Rigidbody2D> ();
			sr = GetComponent<SpriteRenderer> ();
			speed = 15f;
			Vector2 parent_position = speller.get_position2 ();
			transform.position = new Vector3 (parent_position.x + (direction2.x > 0 ? 1 : -1) * 0.2f, parent_position.y -0.1f+ (-0.1f * Random.value),transform.position.z);
			attack attack = gameObject.GetComponent<attack> ();
			attack.set_attacker (speller);
			dead_time = 200;
		}
	}
	void Update () {
		if (is_dead) {
			if (dead_time-- < 0) {
				Destroy (transform.gameObject);
			}
		} else {
			if (move_mode == 0) {
				rb.velocity = direction2 * speed;
			}

			float x = direction2.x;
			float y = direction2.y;
			if (x == 0) {
				sr.sprite = bullet_sprite[0];
			} else {
				float rate = Mathf.Abs (y / x);
				if (rate < 0.19f) {
					sr.sprite = bullet_sprite[0];
				} else if (rate > 5.02f) {
					sr.sprite = bullet_sprite[4];
				} else if (rate < 1.49f && rate > 0.66f) {
					sr.sprite = bullet_sprite[2];
				} else {
					if (rate < 1) {
						sr.sprite = bullet_sprite[1];
					} else {
						sr.sprite = bullet_sprite[3];
					}
				}
			}
			transform.localScale = new Vector2 ((x > 0 ? 1 : -1), (y < 0 ? 1 : -1));
		}

	}

	void OnTriggerEnter2D (Collider2D collider) {
		if (!is_dead) {
			bear bear = collider.gameObject.GetComponent<bear> ();
			if (bear != null) {
				charactor bearer = bear.get_parent ();
				if (speller == bearer) {
					return;
				}else{
					dead_time=0;
				}
			}
			Destroy (GetComponent<Collider2D> ());
			Destroy (GetComponent<Rigidbody2D> ());
			is_dead = true;
			transform.parent = collider.gameObject.transform;
		}

	}
}