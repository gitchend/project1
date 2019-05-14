using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class remote_attack : spell_script
{
    void Start()
    {
        transform.position = direction2;
        gameObject.GetComponent<attack> ().set_attacker (speller);
    }
    void OnTriggerEnter2D (Collider2D collider)
    {
        bear bear = collider.gameObject.GetComponent<bear> ();
        if (bear != null)
        {
            charactor bearer = bear.get_parent ();
            if (speller == bearer)
            {
                return;
            }
        }
        Destroy (transform.gameObject);
    }
}