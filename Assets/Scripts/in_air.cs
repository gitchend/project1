using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class in_air : MonoBehaviour {
	
	private charactor parent;
	// Use this for initialization
	void Start () {
		parent=transform.parent.gameObject.GetComponent<player>();
		if(parent==null){
			parent=transform.parent.gameObject.GetComponent<enemy>();
		}
	}
	// Update is called once per frame
	void Update () {
		
	}
	void OnTriggerExit2D(Collider2D collision){
        parent.set_in_air(true);
    }
	void OnTriggerStay2D(Collider2D collision){
        parent.set_in_air(false);
    }
}
