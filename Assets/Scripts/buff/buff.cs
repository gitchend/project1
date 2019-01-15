using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buff {
	public charactor buff_from;
	public charactor buff_to;
	public int buff_code;
	public int time_full;
	public int time_now;
	public int wait_time;
	public bool dispersable;

	public static buff new_buff (int buff_code) {
		switch (buff_code) {
			case 1:
				return new stun ();
			case 2:
				return new hp_recover();
			case 100:
			    return new shake();
			default:
				return new stun ();
		}
	}

	public void init (int buff_code_set, charactor buff_from_set, charactor buff_to_set, int time_full_set,int wait_time_set) {
		buff_code=buff_code_set;
		buff_from = buff_from_set;
		buff_to = buff_to_set;
		time_now = time_full = time_full_set;
		dispersable = false;
		wait_time=wait_time_set;
		init2 ();
	}
	public virtual void init2 () { }
	public virtual void update () { }
	public virtual void start () { }
	public virtual void end () { }

}