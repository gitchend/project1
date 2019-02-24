using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class effect_controller : MonoBehaviour
{

    private Dictionary<int, GameObject> effect_map = new Dictionary<int, GameObject> ();
    private List<effect> effect_list = new List<effect> ();
    void Start ()
    {
        effect_map[1] = Resources.Load ("effect/blood") as GameObject;
        effect_map[2] = Resources.Load ("effect/attack_effect1") as GameObject;
        effect_map[3] = Resources.Load ("effect/dust1") as GameObject;
        effect_map[4] = Resources.Load ("effect/dust2") as GameObject;
        effect_map[5] = Resources.Load ("effect/attack_shock1") as GameObject;
    }
    void Update ()
    {
        if (effect_list.Count == 0)
        {
            return;
        }
        for (int i = effect_list.Count - 1; i >= 0; i--)
        {
            effect effect = effect_list[i];
            if (effect.wait_time > 0)
            {
                effect.wait_time--;
            }
            else
            {
                GameObject obj = Instantiate (effect_map[effect.effect_code]) as GameObject;

                switch(effect.effect_mode)
                {
                case 0:
                    obj.transform.parent = effect.target.transform;
                    obj.transform.localPosition = new Vector3 (0, 0, obj.transform.position.z);
                    break;
                case 1:
                    obj.transform.position = new Vector3 (effect.position.x, effect.position.y, obj.transform.position.z);
                    break;
                case 2:
                    obj.transform.parent = effect.target.transform;
                    obj.transform.localPosition = new Vector3 (effect.position.x, effect.position.y, obj.transform.position.z);
                    break;
                }
                if (effect.direction)
                {
                    obj.transform.localScale += new Vector3 (-2 * obj.transform.localScale.x, 0, 0);
                    obj.transform.localPosition = new Vector3 (obj.transform.localPosition.x * -1.0f, obj.transform.localPosition.y, obj.transform.localPosition.z);
                }
                effect_list.Remove (effect);
            }
        }
    }
    //add to obj
    public void create_effect (int effect_code, bool direction, GameObject target_set, int wait_time)
    {
        effect_list.Add (new effect (effect_code, direction, wait_time, target_set));
    }
    //world position
    public void create_effect (int effect_code, bool direction, Vector2 position, int wait_time)
    {
        effect_list.Add (new effect (effect_code, direction, wait_time, position));
    }
    //add to obj with reletive position
    public void create_effect (int effect_code, bool direction, GameObject target_set, Vector2 position, int wait_time)
    {
        effect_list.Add (new effect (effect_code, direction, wait_time, target_set, position));
    }
}