using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class effect_controller : MonoBehaviour {

	private Dictionary<int, GameObject> effect_map = new Dictionary<int, GameObject> ();
	private List<effect> effect_list = new List<effect> ();
	void Start () {
		effect_map[1] = Resources.Load ("blood") as GameObject;
		effect_map[2] = Resources.Load ("attack_effect1") as GameObject;
	}
	void Update () {
		if (effect_list.Count == 0) {
			return;
		}
		for (int i = effect_list.Count - 1; i >= 0; i--) {
			effect effect = effect_list[i];
			if (effect.wait_time > 0) {
				effect.wait_time--;
			} else {
				GameObject obj = Instantiate (effect_map[effect.effect_code]) as GameObject;
				if (effect.direction) {
					obj.transform.localScale += new Vector3 (-2 * obj.transform.localScale.x, 0, 0);
				}
				obj.transform.parent = effect.target.transform;
				effect_list.Remove (effect);
			}
		}
	}
	public void create_effect (int effect_code, bool direction, GameObject target_set, int wait_time) {
		effect_list.Add (new effect (effect_code, direction, wait_time, target_set));
	}

}