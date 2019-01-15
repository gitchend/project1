using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class effect {
	public int effect_code;
	public bool direction;
	public int wait_time;
	public GameObject target;
	public effect(){}
	public effect (int effect_code_set, bool direction_set, int wait_time_set, GameObject target_set) {
		effect_code = effect_code_set;
		direction = direction_set;
		wait_time = wait_time_set;
		target = target_set;
	}
}