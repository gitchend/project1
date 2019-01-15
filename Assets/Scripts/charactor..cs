using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class charactor : MonoBehaviour {

	public float speed;
	public float jump_speed;

	public int hp_full;
	public int hp_now;

	protected bool is_dead = false;
	protected bool is_pause = false;
	protected bool in_air = false;
	protected bool against_wall = false;
	protected bool against_wall_2 = false;
	protected bool direction = true; //right
	protected bool is_stun = false;
	protected bool is_in_stun_circle = false;
	protected bool is_hitted = false;
	protected int stun_time = 0;
	protected int skill_spelling = -1;

	protected int frame_extract = 0;
	protected bool frame_extract_lock = false;
	protected Vector2 rb_velocity_cache;

	private List<GameObject> skills;
	protected Dictionary<int, buff> buff_map = new Dictionary<int, buff> ();

	protected effect_controller ec;
	protected audio_controller ac;
	protected spell_controller sc;
	protected buff_controller bc;

	protected Rigidbody2D rb;
	protected Animator animator;
	protected List<int> anime_para_list;
	protected int anime_para_now = -1;

	protected charactor target;
	protected charactor last_attacked;
	protected GameObject sprite;

	protected void init () {

		rb = GetComponent<Rigidbody2D> ();

		sprite = transform.Find ("sprite").gameObject;
		animator = sprite.GetComponent<Animator> ();
		anime_para_list = new List<int> ();
		foreach (AnimatorControllerParameter parameter in animator.parameters) {
			anime_para_list.Add (parameter.nameHash);
		}

		skills = new List<GameObject> ();
		foreach (Transform child in transform.Find ("skills")) {
			GameObject child_obj = child.gameObject;
			skills.Add (child_obj);
			child.gameObject.SetActive (false);
			attack attack = child_obj.GetComponent<attack> ();
			if (attack != null) {
				attack.set_attacker (this);
			}
		}
		ec = GameObject.Find ("effect_controller").GetComponent<effect_controller> ();
		ac = GameObject.Find ("audio_controller").GetComponent<audio_controller> ();
		sc = GameObject.Find ("spell_controller").GetComponent<spell_controller> ();
		bc = GameObject.Find ("buff_controller").GetComponent<buff_controller> ();

	}

	protected void move (bool direction_to) {
		if (direction_to != direction) {
			turn ();
		}
		if (against_wall) {
			set_speed (0, rb.velocity.y);
			if (!in_air) { }
		} else {
			set_speed (speed * (direction?1: -1), rb.velocity.y);
		}

	}
	protected void move (bool direction_to, float speed_to) {
		if (direction_to != direction) {
			turn ();
		}
		if (against_wall) {
			set_speed (0, rb.velocity.y);
			if (!in_air) { }
		} else {
			set_speed (speed_to * (direction?1: -1), rb.velocity.y);
		}

	}
	protected void turn () {
		direction = !direction;
		transform.localScale += new Vector3 (-2 * transform.localScale.x, 0, 0);
	}
	protected void add_speed (float speed_x, float speed_y) {
		rb.velocity += new Vector2 (speed_x, speed_y);
	}
	protected void set_speed (float speed_x, float speed_y) {
		rb.velocity = new Vector2 (speed_x, speed_y);
	}
	protected void set_anime_para (int index, bool is_any_state) {
		set_anime_para (index);
		set_anime_para (is_any_state);
	}
	protected void set_anime_para (bool is_any_state) {
		animator.SetBool (anime_para_list[0], is_any_state);
	}
	protected void set_anime_para (int index) {
		if (anime_para_now != index) {
			if (anime_para_now != -1) {
				animator.SetBool (anime_para_list[anime_para_now], false);
			}
			if (index != -1) {
				animator.SetBool (anime_para_list[index], true);
			}
			anime_para_now = index;
		}
	}
	protected int get_anime_para_now () {
		return anime_para_now;
	}
	protected void skill (int index) {
		if (skill_spelling != index) {
			if (skill_spelling != -1) {
				skills[skill_spelling].SetActive (false);
			}
			if (index != -1) {
				skills[index].SetActive (true);
				ac.create_audio (3, 0);
			}
			skill_spelling = index;
		}
	}
	protected float get_anime_normalized_time () {
		AnimatorStateInfo stateinfo = animator.GetCurrentAnimatorStateInfo (0);
		return stateinfo.normalizedTime;
	}
	protected void pause_control (bool isopen) {
		if (isopen) {
			rb.WakeUp ();
			rb.velocity = rb_velocity_cache;
			animator.speed = (1);
			frame_extract_lock = false;
			is_pause = false;
		} else {
			rb_velocity_cache = rb.velocity;
			rb.Sleep ();
			rb.gravityScale = 0;
			animator.speed = (0);
			frame_extract_lock = true;
			is_pause = true;
		}
	}

	protected bool frame_extract_control () {
		if (frame_extract > 0) {
			frame_extract--;
		} else if (frame_extract_lock) {
			pause_control (true);
		}
		return frame_extract > 0;
	}
	protected bool stun_control (int stun_anime_id, int stun_out_anime_id) {
		if (is_stun) {
			if (!is_in_stun_circle) {
				set_anime_para (stun_anime_id);
				is_in_stun_circle = true;
			}
			skill (-1);
		} else {
			if (is_in_stun_circle) {
				set_anime_para (stun_out_anime_id);
				is_in_stun_circle = false;
				return true;
			}
			return false;
		}
		return is_stun;
	}
	protected void floating_control (float limit, float scale1, float scale2) {
		if (Mathf.Abs (rb.velocity.y) < limit) {
			rb.gravityScale = scale1;
		} else {
			rb.gravityScale = scale2;
		}
	}
	protected void adjust_pixel(){
		sprite.transform.position=new Vector3(pixel_fix(sprite.transform.position.x),pixel_fix(sprite.transform.position.y),sprite.transform.position.z);
		sprite.transform.localPosition=new Vector3(sprite.transform.localPosition.x-(int)(sprite.transform.localPosition.x*64)/64.0f,sprite.transform.localPosition.y-(int)(sprite.transform.localPosition.y*64)/64.0f);
	}
	private float pixel_fix(float float_num){
		return (int)(float_num*64)/64.0f;
	}
	//getter & setter
	public bool is_anime_now_name (string anime_name) {
		AnimatorStateInfo stateinfo = animator.GetCurrentAnimatorStateInfo (0);
		return stateinfo.IsName ("Base Layer." + anime_name);
	}
	public void set_hp (int hp_set) {
		if (hp_set < 0) {
			hp_now = 0;
		} else if (hp_set > hp_full) {
			hp_now = hp_full;
		} else {
			hp_now = hp_set;
		}

	}
	public void set_in_air (bool in_air_now) {
		in_air = in_air_now;
	}
	public bool get_in_air () {
		return in_air;
	}
	public void set_against_wall (bool against_wall_now) {
		against_wall = against_wall_now;
	}
	public bool get_against_wall () {
		return against_wall;
	}
	public void set_against_wall_2 (bool against_wall_now) {
		against_wall_2 = against_wall_now;
	}
	public bool get_against_wall_2 () {
		return against_wall_2;
	}
	public void set_stun_time (int stun_time_now) {
		stun_time = stun_time_now;
	}
	public int get_stun_time () {
		return stun_time;
	}
	public void set_direction (bool direction_now) {
		if (direction != direction_now) {
			turn ();
		}
	}
	public bool get_direction () {
		return direction;
	}
	public Vector2 get_position2 () {
		return new Vector2 (transform.position.x, transform.position.y);
	}
	public Vector2 get_speed2 () {
		return new Vector2 (rb.velocity.x, rb.velocity.y);
	}
	public void set_frame_extract (int frame_extract_now) {
		if (frame_extract_now > 0) {
			pause_control (false);
		}
		frame_extract = frame_extract_now;
	}
	public int get_frame_extract () {
		return frame_extract;
	}
	public void set_last_attacked (charactor last_attacked_now) {
		last_attacked = last_attacked_now;
	}
	public charactor get_last_attacked () {
		return last_attacked;
	}
	public bool get_is_stun () {
		return is_stun;
	}
	public void set_is_stun (bool is_stun_set) {
		is_stun = is_stun_set;
	}
	public Dictionary<int, buff> get_buff_map () {
		return buff_map;
	}
	public buff_controller get_buff_controller () {
		return bc;
	}
	public GameObject get_sprite_obj () {
		return sprite;
	}
	public bool get_is_pause () {
		return is_pause;
	}
	public void hit_message (attack attack) { 
		hit_message2(attack);
	}
	public void hitted_message (attack attack) {
		hitted_message2(attack);
		is_hitted = true;
	}
	public virtual void hit_message2 (attack attack) {}
	public virtual void hitted_message2 (attack attack) {}

}