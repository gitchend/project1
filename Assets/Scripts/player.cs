using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : charactor {

	private bool keydown_w = false;
	private bool keydown_a = false;
	private bool keydown_s = false;
	private bool keydown_d = false;
	private bool keydown_j = false;
	private bool keydown_k = false;
	private bool keydown_u = false;
	private bool keydown_i = false;
	private bool keydown_q = false;
	private bool keydown_e = false;
	private bool keydown_shift = false;
	private bool keydown_space = false;

	private bool moving_control_valid = false;
	private bool item_change_control_valid = false;

	private bool move_lock = false;
	private bool walk_lock = false;
	private bool turn_lock = false;
	private bool jump_lock = false;
	private bool hit_floor_lock = false;
	private bool roll_lock = false;
	private bool attack_light_lock = false;
	private bool attack_heavy_lock = false;
	private bool item_lock = false;
	private bool scroll_lock = false;
	private bool is_air_jump = false;
	private bool is_climb_jump = false;
	private bool is_air_jumped = false;
	private bool is_climbed = false;
	private bool is_air_attacked = false;
	private bool is_spell_used = false;

	private int jump_charging = 0;
	private int jump_charging_max = 5;

	private int mp_cost_now = 0;

	private int collider_index_now = -1;
	private List<CapsuleCollider2D> collider_list = new List<CapsuleCollider2D> ();
	private GameObject against_wall_obj;

	private Dictionary<int, int> item_map = new Dictionary<int, int> ();
	private List<int> item_code_list = new List<int> ();
	private int item_index_now = 0;
	private int item_change_lock = 0;
	private int mp_max = 5;
	private int mp_now = 2;
	private int mp_charge_max = 100;
	private int mp_charge_now = 0;

	void Start () {
		init ();
		foreach (CapsuleCollider2D collider in gameObject.GetComponents<CapsuleCollider2D> ()) {
			collider.enabled = false;
			collider_list.Add (collider);
		}
		against_wall_obj = transform.Find ("aganst_wall").gameObject;
		set_collider (0);

		item_map[1] = 5;
		item_map[2] = 5;
		item_map[3] = 5;
		item_code_list.Add (1);
		item_code_list.Add (2);
		item_code_list.Add (3);
	}
	private bool movement_control () {
		if (is_anime_now_name ("idle")) {
			move_lock = true;
			walk_lock = true;
			jump_lock = true;
			roll_lock = true;
			attack_light_lock = true;
			item_lock = true;
			scroll_lock = true;
			if (in_air) {
				set_anime_para (5);
				return true;
			}
		} else if (is_anime_now_name ("walk")) {
			move_lock = true;
			walk_lock = true;
			jump_lock = true;
			roll_lock = true;
			attack_light_lock = true;
			item_lock = true;
			scroll_lock = true;
			if (in_air) {
				set_anime_para (5);
				return true;
			}
		} else if (is_anime_now_name ("jump1")) {
			set_anime_para (-1);
			set_speed (0, 0);
			is_air_attacked = false;
		} else if (is_anime_now_name ("jump2")) {
			move_lock = !is_climb_jump;
			if (jump_charging == 0 || (keydown_space && jump_charging < jump_charging_max)) {
				if (is_climb_jump) {
					move (direction);
				}
				set_speed (rb.velocity.x, jump_speed);
				jump_charging++;
			}
			if (rb.velocity.y < 4) {
				if (!is_air_jump || is_climb_jump) {
					set_anime_para (5);
				} else {
					set_anime_para (6);
				}
				return true;
			}
		} else if (is_anime_now_name ("jump3")) {
			move_lock = true;
			jump_lock = !is_air_jumped;
			attack_light_lock = !is_air_attacked;
			hit_floor_lock = true;
			item_lock = true;
			jump_charging = 0;
			is_climb_jump = false;
			is_climbed = false;

			if (in_air && !against_wall && against_wall_2) {
				set_speed (0, -jump_speed / 4);
				return true;
			}
			if (against_wall && against_wall_2 && !is_climbed && (moving_control_valid && keydown_d == direction)) {
				set_anime_para (10);
				turn ();
				return true;
			}

		} else if (is_anime_now_name ("jump4")) {
			jump_lock = true;
			is_air_jumped = false;
			if (get_anime_para_now () == 1) {
				return true;
			}
			set_anime_para (1);
		} else if (is_anime_now_name ("jump5")) {
			move_lock = true;
			hit_floor_lock = true;
			attack_light_lock = !is_air_attacked;
			item_lock = true;
			jump_charging = 0;
			is_climb_jump = false;
			is_climbed = false;
			if (in_air && !against_wall && against_wall_2) {
				set_speed (0, -jump_speed / 4);
				return true;
			}
			if (against_wall && against_wall_2 && !is_climbed && (moving_control_valid && keydown_d == direction)) {
				set_anime_para (10);
				turn ();
				return true;
			}
		} else if (is_anime_now_name ("jump6")) {
			rb.gravityScale = 0;
			set_speed (0, 0);
		} else if (is_anime_now_name ("climb")) {
			jump_lock = true;
			is_climbed = true;
			is_climb_jump = true;
			item_lock = true;
			rb.gravityScale = 0;
			set_speed (0, 0);
			if (!(moving_control_valid && keydown_d != direction)) {
				set_anime_para (5);
				return true;
			}
		} else if (is_anime_now_name ("roll_1")) {
			move (direction, speed * 1.5f);
			set_collider (1);
			gameObject.layer = 13;

		} else if (is_anime_now_name ("roll_2")) {
			attack_light_lock = true;
			jump_lock = true;
			roll_lock = true;
			item_lock = true;
			scroll_lock = true;
			turn_lock=true;
			set_anime_para (-1);
			set_collider (0);
			gameObject.layer = 0;
			if (in_air) {
				set_anime_para (5);
				return true;
			}
		} else if (is_anime_now_name ("roll_3")) {
			
			set_anime_para (1);
		}
		return false;
	}
	private bool ground_attack_control () {
		if (is_anime_now_name ("attack1")) {
			turn_lock=true;
			attack_light_lock = true;
			attack_heavy_lock = true;
			jump_lock = true;
			roll_lock = true;
			item_lock = true;
			scroll_lock = true;
			set_anime_para (-1);
			skill (-1);
		} else if (is_anime_now_name ("attack2")) {
			attack_light_lock = true;
			attack_heavy_lock = true;
			jump_lock = true;
			mp_cost_now = 1;
			roll_lock = true;
			item_lock = true;
			scroll_lock = true;
			set_anime_para (-1);
			skill (-1);
		} else if (is_anime_now_name ("attack3")) {
			attack_light_lock = true;
			jump_lock = true;
			roll_lock = true;
			item_lock = true;
			scroll_lock = true;
			set_anime_para (-1);
			skill (-1);
		} else if (is_anime_now_name ("attack4")) {
			attack_light_lock = true;
			jump_lock = true;
			roll_lock = true;
			item_lock = true;
			scroll_lock = true;
			set_anime_para (-1);
			skill (-1);
		} else if (is_anime_now_name ("attack0-1")) {
			if (get_anime_normalized_time () < 0.4) {
				if (moving_control_valid && direction == keydown_d) {
					move (direction, speed * 1.2f);
				} else {
					move (direction, speed * 0.8f);
				}
			} else {
				skill (0);
			}
		} else if (is_anime_now_name ("attack1-2")) {
			if (get_anime_normalized_time () < 0.4) {
				if (moving_control_valid && direction == keydown_d) {
					move (direction, speed * 1.2f);
				} else {
					move (direction, speed * 0.8f);
				}
			} else {
				skill (1);
			}
		} else if (is_anime_now_name ("attack2-3")) {
			if (get_anime_normalized_time () < 0.5) {
				if (moving_control_valid && direction == keydown_d) {
					move (direction, speed * 1.2f);
				} else {
					move (direction, speed * 0.8f);
				}
			} else {
				skill (2);
			}
		} else if (is_anime_now_name ("attack3-4")) {
			if (get_anime_normalized_time () < 0.625) {
				if (moving_control_valid && direction == keydown_d) {
					move (direction, speed * 1.2f);
				} else {
					move (direction, speed * 0.8f);
				}
			} else {
				skill (3);
			}
		} else if (is_anime_now_name ("attack4-1")) {
			if (get_anime_normalized_time () < 0.5) {
				if (moving_control_valid && direction == keydown_d) {
					move (direction, speed * 1.2f);
				} else {
					move (direction, speed * 0.8f);
				}
			} else {
				skill (4);
			}
		} else if (is_anime_now_name ("replace")) {
			roll_lock = true;
			if (get_anime_normalized_time () > 0.6) {
				add_mp_charge_to_mp ();
				set_anime_para (1);
			}
		}
		return false;
	}
	private bool air_attack_control () {
		if (is_anime_now_name ("air_attack1")) {
			jump_lock = !is_air_jumped;
			attack_light_lock = true;
			hit_floor_lock = true;
			item_lock = true;
			set_anime_para (-1);
			skill (-1);
		} else if (is_anime_now_name ("air_attack2")) {
			jump_lock = !is_air_jumped;
			attack_light_lock = true;
			hit_floor_lock = true;
			item_lock = true;
			set_anime_para (-1);
			skill (-1);
		} else if (is_anime_now_name ("air_attack3")) {
			jump_lock = !is_air_jumped;
			hit_floor_lock = true;
			item_lock = true;
			set_anime_para (-1);
			skill (-1);
		} else if (is_anime_now_name ("air_attack0-1")) {
			if (get_anime_normalized_time () < 0.4) {
				move (direction, speed * 0.2f);
			} else {
				move (direction, speed * 0.2f);
				set_speed (rb.velocity.x, jump_speed / 4);
				skill (5);
			}
		} else if (is_anime_now_name ("air_attack1-2")) {
			if (get_anime_normalized_time () < 0.34) {
				move (direction, speed * 0.2f);
			} else {
				set_speed (rb.velocity.x, jump_speed * 0.3f);
				skill (6);
			}
		} else if (is_anime_now_name ("air_attack2-3")) {
			is_air_attacked = true;
			if (get_anime_normalized_time () > 0.5) {
				skill (7);
				move (direction, speed * 1.2f);
				set_speed (rb.velocity.x, 0);
			}
		} else if (is_anime_now_name ("air_replace")) {
			hit_floor_lock = true;
			add_mp_charge_to_mp ();

		}
		return false;
	}
	private bool item_control () {
		if (is_anime_now_name ("item1_1")) {
			if (get_anime_normalized_time () > 0.5 && !is_spell_used) {
				sc.create_spell (item_code_list[item_index_now], this, new Vector2 ((direction?1: -1), 0), 0, 0);
				is_spell_used = true;
			}

		} else if (is_anime_now_name ("item1_2")) {
			is_spell_used = false;
			set_anime_para (1);

		} else if (is_anime_now_name ("item2_1")) {
			if (get_anime_normalized_time () > 0.5 && !is_spell_used) {
				sc.create_spell (item_code_list[item_index_now], this, new Vector2 ((direction?1: -1), 0), 0, 0);
				is_spell_used = true;
			}
		} else if (is_anime_now_name ("item2_2")) {
			is_spell_used = false;
			set_anime_para (1);

		} else if (is_anime_now_name ("air_item1_1")) {
			if (get_anime_normalized_time () > 0.5) {
				if (!is_spell_used) {
					sc.create_spell (item_code_list[item_index_now], this, new Vector2 ((direction?1: -1), 0), 0, 0);
					is_spell_used = true;
				}
				if (get_anime_normalized_time () < 2.0) {
					set_speed (rb.velocity.x, jump_speed / 8);
				}
			}
		} else if (is_anime_now_name ("air_item1_2")) {
			is_spell_used = false;
			hit_floor_lock = true;
			set_anime_para (-1);

		} else if (is_anime_now_name ("air_item2_1")) {
			if (get_anime_normalized_time () < 0.2) {
				set_speed (-speed * (direction?1: -1), jump_speed / 2);
			}
			if (get_anime_normalized_time () > 0.7) {
				if (!is_spell_used) {
					sc.create_spell (item_code_list[item_index_now], this, new Vector2 ((direction?1: -1), -1), 0, 0);
					sc.create_spell (item_code_list[item_index_now], this, new Vector2 ((direction?1: -1), -0.67f), 0, 0);
					sc.create_spell (item_code_list[item_index_now], this, new Vector2 ((direction?1: -1), -0.41f), 0, 0);
					is_spell_used = true;
				}
				if (get_anime_normalized_time () < 0.8) {
					set_speed (rb.velocity.x, jump_speed / 4);
				}
			}
		} else if (is_anime_now_name ("air_item2_2")) {
			is_spell_used = false;
			hit_floor_lock = true;
			set_anime_para (-1);

		} else if (is_anime_now_name ("climb_item1_1")) {
			rb.gravityScale = 0;
			set_speed (0, 0);
			if (get_anime_normalized_time () > 0.5) {
				if (!is_spell_used) {
					if (keydown_w) {
						sc.create_spell (item_code_list[item_index_now], this, new Vector2 ((direction?1: -1), 0), 0, 0);
					} else {
						sc.create_spell (item_code_list[item_index_now], this, new Vector2 ((direction?1: -1), -0.4f), 0, 0);
					}
					is_spell_used = true;
				}
			}
		} else if (is_anime_now_name ("climb_item1_2")) {
			rb.gravityScale = 0;
			set_speed (0, 0);
			is_spell_used = false;
			hit_floor_lock = true;
			set_anime_para (-1);
			if (!(moving_control_valid && keydown_d != direction)) {
				set_anime_para (5);
				return true;
			}
		} else if (is_anime_now_name ("climb_item2_1")) {
			rb.gravityScale = 0;
			set_speed (0, 0);
			if (get_anime_normalized_time () > 0.5) {
				if (!is_spell_used) {
					sc.create_spell (item_code_list[item_index_now], this, new Vector2 ((direction?1: -1), -1), 0, 0);
					is_spell_used = true;
				}
			}
		} else if (is_anime_now_name ("climb_item2_2")) {
			rb.gravityScale = 0;
			set_speed (0, 0);
			is_spell_used = false;
			hit_floor_lock = true;
			set_anime_para (-1);
			if (!(moving_control_valid && keydown_d != direction)) {
				set_anime_para (5);
				return true;
			}

		} else if (is_anime_now_name ("scroll1_1")) {
			if (get_anime_normalized_time () > 0.5 && !is_spell_used) {
				sc.create_spell (101, this);
				is_spell_used = true;
			}

		} else if (is_anime_now_name ("scroll1_2")) {
			set_anime_para (1);
			is_spell_used = false;
		}
		return false;
	}
	private bool player_control () {
		//item_change
		item_change_control ();
		//move
		if (move_lock && moving_control_valid) {
			move (keydown_d);
		}
		//hit_floor
		if (hit_floor_lock && rb.velocity.y <= 0) {
			if (!in_air) {
				set_anime_para (7);
				return true;
			}
		}
		//roll
		if (roll_lock && keydown_shift) {
			set_anime_para (3);
			if (moving_control_valid && direction != keydown_d) {
				turn ();
			}
			return true;
		}
		//attack
		if (attack_light_lock && keydown_j) {
			set_anime_para (8);
			if (turn_lock && direction != keydown_d) {
				turn ();
			}
			return true;
		}
		if (attack_heavy_lock && keydown_k && mp_cost ()) {
			set_anime_para (9);
			if (turn_lock && direction != keydown_d) {
				turn ();
			}
			return true;
		}
		//use_item
		if (item_lock && keydown_i && item_map[item_code_list[item_index_now]] > 0) {
			if (is_climb_jump) {
				switch (item_code_list[item_index_now]) {
					case 1:
						if (keydown_s) {
							set_anime_para (16);
						} else {
							set_anime_para (15);
						}
						break;
					default:
						break;
				}
			} else if (in_air) {
				switch (item_code_list[item_index_now]) {
					case 1:
						if (keydown_s) {
							set_anime_para (17);
						} else {
							set_anime_para (14);
						}
						break;
					default:
						break;
				}
			} else {
				switch (item_code_list[item_index_now]) {
					case 1:
						if (keydown_s) {
							set_anime_para (17);
						} else {
							set_anime_para (11);
						}
						break;
					case 3:
						set_anime_para (13);
						break;
					default:
						set_anime_para (11);
						break;
				}
			}
			return true;
		}
		//use_scroll
		if (scroll_lock && keydown_u) {
			set_anime_para (12);
			return true;
		}
		//jump
		if (jump_lock && Input.GetKeyDown (KeyCode.Space)) {
			set_anime_para (4);
			if (in_air) {
				is_air_jump = true;
				if (!is_climb_jump) {
					is_air_jumped = true;
				}
			} else {
				is_air_jump = false;
			}
			return true;
		}
		//walk
		if (walk_lock) {
			if (moving_control_valid) {
				set_anime_para (2);
			} else {
				set_anime_para (1);
			}
			return true;
		}

		return false;
	}
	void Update () {
		adjust_pixel ();
		keyboard_listen ();
		if (frame_extract_control ()) return;
		floating_control (0.4f, 2.0f, 3.5f);

		move_lock = false;
		walk_lock = false;
		turn_lock = false;
		jump_lock = false;
		roll_lock = false;
		attack_light_lock = false;
		attack_heavy_lock = false;
		item_lock = false;
		scroll_lock = false;
		hit_floor_lock = false;

		//auto_control

		if (movement_control ()) return;
		if (ground_attack_control ()) return;
		if (air_attack_control ()) return;
		if (item_control ()) return;

		if (is_anime_now_name ("spell1_1")) {
			if (get_anime_normalized_time () < 0.5) {
				if (moving_control_valid && direction == keydown_d) {
					move (direction, speed * 1.2f);
				} else {
					move (direction, speed * 0.8f);
				}
			} else {
				skill (8);
			}
		} else if (is_anime_now_name ("spell1_2")) {
			jump_lock = true;
			attack_light_lock = true;
			roll_lock = true;
			skill (-1);
			set_anime_para (-1);

		} else if (is_anime_now_name ("spell2_1")) {

		} else if (is_anime_now_name ("spell2_2")) {
			if (((int) (get_anime_normalized_time () / 0.2)) % 3 == 0) {
				skill (9);
			} else {
				skill (-1);
			}
			move (direction, speed * 4);
			set_anime_para (-1);

		} else if (is_anime_now_name ("spell2_3")) {
			skill (10);
			set_anime_para (-1);
		}

		//control
		if (player_control ()) return;
	}

	private void keyboard_listen () {
		if (Input.GetKeyDown (KeyCode.W)) { keydown_w = true; }
		if (Input.GetKeyUp (KeyCode.W)) { keydown_w = false; }
		if (Input.GetKeyDown (KeyCode.A)) { keydown_a = true; }
		if (Input.GetKeyUp (KeyCode.A)) { keydown_a = false; }
		if (Input.GetKeyDown (KeyCode.S)) { keydown_s = true; }
		if (Input.GetKeyUp (KeyCode.S)) { keydown_s = false; }
		if (Input.GetKeyDown (KeyCode.D)) { keydown_d = true; }
		if (Input.GetKeyUp (KeyCode.D)) { keydown_d = false; }
		if (Input.GetKeyDown (KeyCode.J)) { keydown_j = true; }
		if (Input.GetKeyUp (KeyCode.J)) { keydown_j = false; }
		if (Input.GetKeyDown (KeyCode.K)) { keydown_k = true; }
		if (Input.GetKeyUp (KeyCode.K)) { keydown_k = false; }
		if (Input.GetKeyDown (KeyCode.I)) { keydown_i = true; }
		if (Input.GetKeyUp (KeyCode.I)) { keydown_i = false; }
		if (Input.GetKeyDown (KeyCode.U)) { keydown_u = true; }
		if (Input.GetKeyUp (KeyCode.U)) { keydown_u = false; }
		if (Input.GetKeyDown (KeyCode.Q)) { keydown_q = true; }
		if (Input.GetKeyUp (KeyCode.Q)) { keydown_q = false; }
		if (Input.GetKeyDown (KeyCode.E)) { keydown_e = true; }
		if (Input.GetKeyUp (KeyCode.E)) { keydown_e = false; }
		if (Input.GetKeyDown (KeyCode.LeftShift)) { keydown_shift = true; }
		if (Input.GetKeyUp (KeyCode.LeftShift)) { keydown_shift = false; }
		if (Input.GetKeyDown (KeyCode.Space)) { keydown_space = true; }
		if (Input.GetKeyUp (KeyCode.Space)) { keydown_space = false; }

		moving_control_valid = !((keydown_d && keydown_a) || !(keydown_d || keydown_a));
		item_change_control_valid = !((keydown_q && keydown_e) || !(keydown_q || keydown_e));
	}
	private void set_collider (int index_set) {
		if (index_set != collider_index_now) {
			if (collider_index_now != -1) {
				collider_list[collider_index_now].enabled = false;
			}
			collider_list[index_set].enabled = true;
			if (index_set == 0) {
				against_wall_obj.SetActive (true);
			} else {
				against_wall_obj.SetActive (false);
				set_against_wall (false);
			}
			collider_index_now = index_set;
		}
	}
	public Dictionary<int, int> get_item_map () {
		return item_map;
	}
	public List<int> get_item_code_list () {
		return item_code_list;
	}
	public int get_item_index_now () {
		return item_index_now;
	}
	public int get_mp_charge_max () {
		return mp_charge_max;
	}
	public int get_mp_charge_now () {
		return mp_charge_now;
	}
	public int get_mp_max () {
		return mp_max;
	}
	public int get_mp_now () {
		return mp_now;
	}
	public override void hit_message2 (attack attack) {
		add_mp_charge_now (20);
	}
	private void add_mp_charge_now (int mp_charge_now_set) {
		mp_charge_now += mp_charge_now_set;
		if (mp_charge_now > mp_charge_max) {
			mp_charge_now = mp_charge_max;
		}
	}
	private bool mp_cost () {
		if (mp_cost_now <= mp_now) {
			mp_now -= mp_cost_now;
			return true;
		} else if (mp_charge_now == mp_charge_max) {
			mp_charge_now = 0;
			return true;
		}
		return false;
	}
	private void add_mp_charge_to_mp () {
		if (mp_charge_now == mp_charge_max && mp_now < mp_max) {
			mp_charge_now = 0;
			mp_now++;
		}
	}
	private void item_change (bool direction) {
		item_index_now = (item_index_now + (direction?1: -1) + item_code_list.Count) % item_code_list.Count;
	}
	private void item_change_control () {
		if (item_change_lock > 0) {
			item_change_lock--;
			return;
		} else {
			if (item_change_control_valid) {
				item_change (keydown_e);
				item_change_lock = 10;
			}
		}
	}
}