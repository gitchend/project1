using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attack_effect : MonoBehaviour {
    private Animator animator;
    void Start () {
        animator = GetComponent<Animator> ();
        transform.localPosition=new Vector3(0,0,-5f);
    }
    void Update () {
        if (is_anime_now_name ("blank")) {
            Destroy (transform.gameObject);
        }
    }
    private bool is_anime_now_name (string anime_name) {
        AnimatorStateInfo stateinfo = animator.GetCurrentAnimatorStateInfo (0);
        return stateinfo.IsName ("Base Layer." + anime_name);
    }
}