using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hp_control : MonoBehaviour {
	private player target;
	private GameObject mask_1;
	private GameObject mask_2;
	private int half_bar_length;
	private int bar_length_max;
	void Start () {
		target = GameObject.Find ("hero").GetComponent<player> ();
		mask_1 = transform.Find ("hp_mask_1").gameObject;
		mask_2 = transform.Find ("hp_mask_2").gameObject;

		bar_length_max = half_bar_length = 98;
	}

	void Update () {
		int full_bar_length = (int) (target.hp_now * bar_length_max / target.hp_full);
		if (half_bar_length > full_bar_length) {
			half_bar_length--;
		} else {
			half_bar_length = full_bar_length;
		}
		set_mask_length (1, full_bar_length);
		set_mask_length (2, half_bar_length);

	}
	private void set_mask_length (int mask_id, int length) {
		int mid_length = (length - bar_length_max) / 2;
		if (mask_id == 1) {
			mask_1.transform.localScale = new Vector2 (length, 16);
			mask_1.transform.localPosition = new Vector2 (mid_length / 64.0f, 0);
		} else {
			mask_2.transform.localScale = new Vector2 (length, 16);
			mask_2.transform.localPosition = new Vector2 (mid_length / 64.0f, 0);
		}
	}
}