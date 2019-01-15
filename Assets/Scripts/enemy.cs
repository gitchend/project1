using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy : charactor {

	public int enemy_kind;

	private int about_to_do = 0;

	private bool move_lock = false;
	private bool attack_lock = false;

	private int envy_change_time = 2000;
	private int envy_time = 0;

	void Start () {
		init ();
	}
	// Update is called once per frame
	void Update () {
		switch (enemy_kind) {
			case 0:
				enemy_control_0 ();
				break;
		}
	}
	private void enemy_control_0 () {
		envy_control ();
		if (frame_extract_control ()) {
			return;
		}
		floating_control (0.8f, 0.5f, 2.0f);
		if (stun_control (2, 3)) {
			if (is_anime_now_name ("hit1")) {
				set_anime_para (-1);
			} else if (is_anime_now_name ("hit2")) {
				if (is_hitted) {
					set_anime_para (2);
					is_hitted=false;
				}
			} else if (is_anime_now_name ("hit3")) {
				if (is_hitted) {
					set_anime_para (2);
					is_hitted=false;
				}
			} else
				return;
		}

		move_lock = false;
		attack_lock = false;
		if (is_anime_now_name ("idle")) {
			move_lock = true;
			attack_lock = true;
			is_in_stun_circle = false;
			skill (-1);
		} else if (is_anime_now_name ("walk")) {
			move_lock = true;
			attack_lock = true;
			if (get_anime_normalized_time () % 1 > 0.4) {
				move (direction);
			}
			skill (-1);
		} else if (is_anime_now_name ("attack1")) {
			if (get_anime_normalized_time () > 0.7) {
				skill (-1);
			} else if (get_anime_normalized_time () > 0.4) {
				skill (0);
			}
		} else if (is_anime_now_name ("hit1")) {
			set_anime_para (-1);
		}

		think ();
		switch (about_to_do) {
			case 0:
				break;
			case 1:
				if (Mathf.Abs (target.get_position2 ().x - get_position2 ().x) > 0.5) {
					approach (1);
				} else {
					attack (4);
				}
				break;
		}
	}
	private void think () {
		switch (about_to_do) {
			case 0:
				if (target != null) {
					about_to_do = 1;
				}
				break;
		}
	}
	private void approach (int walk_anime_id) {
		if (move_lock) {
			bool direction_miu = target.get_position2 ().x - get_position2 ().x > 0;
			if (direction_miu != direction) {
				turn ();
			}
			set_anime_para (walk_anime_id);
			if (against_wall) {
				set_speed (rb.velocity.x, jump_speed);
			}
		}
	}
	private void attack (int attack_anime_id) {
		if (attack_lock) {
			set_anime_para (attack_anime_id);
		}
	}
	private void envy_control () {
		if (envy_time > 0) {
			envy_time--;
			return;
		}
		if (last_attacked != null) {
			target = last_attacked;
			envy_time = envy_change_time;
		}
	}

}