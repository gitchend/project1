using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class against_wal : MonoBehaviour {

	public bool is_second;
	private charactor parent;
	// Use this for initialization
	void Start () {
		parent = transform.parent.gameObject.GetComponent<player> ();
		if (parent == null) {
			parent = transform.parent.gameObject.GetComponent<enemy> ();
		}
	}
	// Update is called once per frame
	void Update () { }
	void OnTriggerExit2D (Collider2D collision) {
		if (is_second) {
			parent.set_against_wall_2 (false);
		} else {
			parent.set_against_wall (false);
		}
	}
	void OnTriggerStay2D (Collider2D collision) {
		if (is_second) {
			parent.set_against_wall_2 (true);
		} else {
			parent.set_against_wall (true);
		}
	}
}