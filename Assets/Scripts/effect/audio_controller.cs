using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audio_controller : MonoBehaviour {

	private Dictionary<int, GameObject> audio_map = new Dictionary<int, GameObject> ();
	private List<audio> audio_list = new List<audio> ();
	void Start () {
		audio_map[1] = Resources.Load ("audios/attack1") as GameObject;
		audio_map[2] = Resources.Load ("audios/attack2") as GameObject;
		audio_map[3] = Resources.Load ("audios/attack_miss") as GameObject;
	}
	void Update () {
		if(audio_list.Count==0){
			return;
		}
		for (int i = audio_list.Count - 1; i >= 0; i--) {
			audio audio = audio_list[i];
			if (audio.wait_time > 0) {
				audio.wait_time--;
			} else {
				GameObject obj = Instantiate (audio_map[audio.audio_code]) as GameObject;
				audio_list.Remove (audio);
			}
		}
	}
	public void create_audio (int audio_code, int wait_time) {
		audio_list.Add (new audio (audio_code, wait_time));
	}
}