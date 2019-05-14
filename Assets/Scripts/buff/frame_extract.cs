using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class frame_extract : buff {
	public override void init2(){
		dispersable=true;
	}
	public override void update () { }
	public override void start () {
		buff_to.set_is_pause (true);
	}
	public override void end () {
		buff_to.set_is_pause (false);
	}
}