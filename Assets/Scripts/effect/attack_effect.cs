using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attack_effect : MonoBehaviour
{
    public bool effect_by_timescale = true;
    private Animator animator;
    void Start ()
    {
        animator = GetComponent<Animator> ();
        if(!effect_by_timescale)
        {
            animator.updateMode = AnimatorUpdateMode.UnscaledTime;
        }
    }
    void Update ()
    {
        if (is_anime_now_name ("blank"))
        {
            Destroy (transform.gameObject);
        }
    }
    private bool is_anime_now_name (string anime_name)
    {
        AnimatorStateInfo stateinfo = animator.GetCurrentAnimatorStateInfo (0);
        return stateinfo.IsName ("Base Layer." + anime_name);
    }
}