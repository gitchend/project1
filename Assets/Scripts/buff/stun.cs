using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stun : buff {
	public override void init2(){
		dispersable=true;
	}
	public override void update () { }
	public override void start () {
		buff_to.set_is_stun (true);
	}
	public override void end () {
		if (buff_to.get_in_air ()) {
			time_now+=5;
		} else {
			buff_to.set_is_stun (false);
		}
	}
}