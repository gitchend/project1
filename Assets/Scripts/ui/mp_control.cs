using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mp_control : MonoBehaviour {
	public Sprite mp_full;
	public Sprite mp_empty;
	private player target;
	private GameObject mask;
	private GameObject mp_full_obj;
	private List<GameObject> mp_list = new List<GameObject> ();
	private int mp_max;
	private int mp_now;

	private int bar_length_max;
	void Start () {
		target = GameObject.Find ("hero").GetComponent<player> ();
		mask = transform.Find ("mp_mask").gameObject;
		mp_full_obj = transform.Find ("mp_full").gameObject;
		bar_length_max = 20;
		mp_max = target.get_mp_max ();
		mp_now = -1;
		init ();
	}
	void Update () {
		if (mp_max != target.get_mp_max ()) {
			init ();
		} else {
			if (mp_now != target.get_mp_now ()) {
				mp_now = target.get_mp_now ();
				for (int i = 0; i < mp_list.Count; i++) {
					if (i < mp_now) {
						mp_list[i].GetComponent<SpriteRenderer> ().sprite = mp_full;
					} else {
						mp_list[i].GetComponent<SpriteRenderer> ().sprite = mp_empty;
					}
				}
			}
			int bar_length_now = (int) (target.get_mp_charge_now () * bar_length_max * 1.0f / target.get_mp_charge_max ());
			set_mask_length (bar_length_now);
			mp_full_obj.SetActive (bar_length_now == bar_length_max);
		}
	}
	private void init () {
		foreach (GameObject mp_obj in mp_list) {
			Destroy (transform.gameObject);
		}
		mp_list = new List<GameObject> ();
		for (int i = 0; i < mp_max; i++) {
			GameObject mp_obj = new GameObject ();
			mp_obj.transform.parent = gameObject.transform;
			mp_obj.transform.localPosition = new Vector2 (i * 0.15625f + 0.28125f, -0.109375f);
			SpriteRenderer sr = mp_obj.AddComponent<SpriteRenderer> () as SpriteRenderer;
			sr.sprite = mp_empty;
			mp_list.Add (mp_obj);
		}
	}
	private void set_mask_length (int length) {
		mask.transform.localScale = new Vector2 (32, length);
		mask.transform.localPosition = new Vector2 (0, (length - bar_length_max) / 128.0f);

	}
}