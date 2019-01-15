using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hp_recover : buff {
	private int timer = 0;
	public override void init2 () {
		dispersable = false;
	}
	public override void update () {
		if (timer++ % 3 == 0) {
			buff_to.set_hp(buff_to.hp_now+1);
		}
	}
	public override void start () { }
	public override void end () { }

}