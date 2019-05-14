using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class against_wall : MonoBehaviour
{

    public int direction = 0;
    private charactor parent;
    // Use this for initialization
    void Start ()
    {
        parent = transform.parent.gameObject.GetComponent<player> ();
        if (parent == null)
        {
            parent = transform.parent.gameObject.GetComponent<enemy> ();
        }
    }
    // Update is called once per frame
    void Update () { }
    void OnTriggerExit2D (Collider2D collision)
    {
        switch(direction)
        {
        case 0:
            parent.set_in_air(true);
            break;
        case 1:
            parent.set_against_wall (false);
            break;
        case 2:
            parent.set_against_wall_2 (false);
            break;
        case 3:
            parent.set_against_ceiling(false);
            break;
        }
    }
    void OnTriggerStay2D (Collider2D collision)
    {
        switch(direction)
        {
        case 0:
            parent.set_in_air(false);
            break;
        case 1:
            parent.set_against_wall (true);
            break;
        case 2:
            parent.set_against_wall_2 (true);
            break;
        case 3:
            parent.set_against_ceiling(true);
            break;
        }
    }
}