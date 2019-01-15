using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spell_script: MonoBehaviour {
	protected charactor speller;
	protected buff_controller bc;
	protected Vector2 direction2;
	protected int move_mode;
	void Start () {
		bc = GameObject.Find ("buff_controller").GetComponent<buff_controller> ();
	}
	public charactor get_speller () {
		return speller;
	}
	public void set_speller (charactor speller_set) {
		speller = speller_set;
	}
	public void set_move_mode (int move_mode_set) {
		move_mode = move_mode_set;
	}	public void set_direction (Vector2 direction_set) {
		direction2 = direction_set;

	}
}