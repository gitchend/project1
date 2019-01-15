using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audio_type1 : MonoBehaviour {
	private AudioSource audioSource;
	void Start () {
		audioSource = gameObject.GetComponent<AudioSource> ();
	}
	void Update () {
		if (!audioSource.isPlaying) {
			Destroy (transform.gameObject);
		}
	}
}