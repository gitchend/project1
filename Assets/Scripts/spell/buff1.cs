using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buff1 : spell_script {
	void OnEnable () {
		if (speller != null) {
			speller.get_buff_controller().create_buff(2,speller,speller,100,0);
			Destroy (transform.gameObject);
		}
	}
}