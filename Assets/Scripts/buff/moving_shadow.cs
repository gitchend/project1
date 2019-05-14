using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moving_shadow : buff {
	private int die_in_frame=30;
	private int timer=0;
	private GameObject obj = null;
    private GameObject img_obj_disappear;

	public override void init2(){
		obj = buff_to.get_sprite_obj ();
        img_obj_disappear = Resources.Load("img_obj_disappear") as GameObject;
	}
	public override void update () {
		timer++;
		timer%=1000;
		if(timer%3==0){
			GameObject shadow = buff_controller.Instantiate (img_obj_disappear) as GameObject;
			shadow.GetComponent<alpha_reduce> ().die_in_frame=die_in_frame;
			shadow.GetComponent<SpriteRenderer> ().sprite=obj.GetComponent<SpriteRenderer> ().sprite;
			shadow.transform.position=new Vector3(obj.transform.position.x,obj.transform.position.y,0.1f);
			shadow.transform.localScale=new Vector3((buff_to.get_direction()?1:-1),1,1);
		}

	}
	public override void start () {
		
	}
	public override void end () {
		
	}

}