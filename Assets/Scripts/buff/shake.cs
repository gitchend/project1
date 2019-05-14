using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shake : buff {
	private GameObject obj = null;
	public override void init2 () {
		obj = buff_to.get_sprite_obj ();
	}
	public override void update () {
		obj.transform.localPosition = new Vector2 (((int) (Mathf.Sin (time_now*1.2f)*3) / 64.0f), ((int) (Mathf.Sin (time_now*1.2f)*1) / 64.0f));
	}
	public override void start () {
	}
	public override void end () {
		obj.transform.localPosition = new Vector2 (0, 0);
	}

}