using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class item_control : MonoBehaviour {
	public Sprite[] item_icon_large;
	public Sprite[] item_icon_small;
	private player target;
	private Dictionary<int, int> item_map = new Dictionary<int, int> ();
	private List<int> item_code_list = new List<int> ();
	private int item_index;
	SpriteRenderer icon_select;
	SpriteRenderer icon_last;
	SpriteRenderer icon_next1;
	SpriteRenderer icon_next2;
	void Start () {
		target = GameObject.Find ("hero").GetComponent<player> ();
		item_map = target.get_item_map ();
		item_code_list = target.get_item_code_list ();
		item_index = -1;

		icon_select = transform.Find ("icon_select").gameObject.GetComponent<SpriteRenderer> ();
		icon_last = transform.Find ("icon_last").gameObject.GetComponent<SpriteRenderer> ();
		icon_next1 = transform.Find ("icon_next1").gameObject.GetComponent<SpriteRenderer> ();
		icon_next2 = transform.Find ("icon_next2").gameObject.GetComponent<SpriteRenderer> ();
	}
	void Update () {
		if (item_index != target.get_item_index_now ()) {
			item_index = target.get_item_index_now ();
			draw_icon ();
		}
	}
	private void draw_icon () {
		if (item_code_list.Count > 0) {
			int icon_large=item_code_list[item_index];
			int icon_small1 = item_code_list[(item_index + item_code_list.Count - 1) % item_code_list.Count];
			int icon_small2 = item_code_list[(item_index + 1) % item_code_list.Count];
			int icon_small3 = item_code_list[(item_index + 2) % item_code_list.Count];
			icon_select.sprite = item_icon_large[icon_large];
			icon_last.sprite = item_icon_small[icon_small1];
			icon_next1.sprite = item_icon_small[icon_small2];
			icon_next2.sprite = item_icon_small[icon_small3];

		}
	}

}