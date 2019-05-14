using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spell_controller : MonoBehaviour
{

    private Dictionary<int, GameObject> spell_map = new Dictionary<int, GameObject> ();
    private Dictionary<int, int> code_dict = new Dictionary<int, int>();
    private List<spell> spell_list = new List<spell> ();
    void Start ()
    {
        //item
        spell_map[1] = Resources.Load ("spells/knife1") as GameObject;
        spell_map[3] = Resources.Load ("spells/hp_recover1") as GameObject;
        spell_map[4] = Resources.Load ("spells/sword_light") as GameObject;
        spell_map[5] = Resources.Load ("spells/remote_attack") as GameObject;

        //scroll
        spell_map[100] = Resources.Load ("spells/scroll1") as GameObject;
        spell_map[332] = Resources.Load ("spells/fireball1") as GameObject;

        //spell

        //spell_code
        code_dict[224] = 100;
    }
    void Update ()
    {
        if (spell_list.Count == 0)
        {
            return;
        }
        for (int i = spell_list.Count - 1; i >= 0; i--)
        {
            spell spell = spell_list[i];
            if (spell.wait_time > 0)
            {
                spell.wait_time--;
            }
            else
            {
                GameObject obj = Instantiate (spell_map[get_code(spell.spell_code)]) as GameObject;
                obj.SetActive (false);
                spell_script spell_script = obj.GetComponent<spell_script> ();
                spell_script.set_speller (spell.speller);
                spell_script.set_direction (spell.direction2);
                spell_script.set_move_mode (spell.move_mode);
                obj.SetActive (true);
                spell_list.Remove (spell);
            }
        }
    }
    private int get_code(int code)
    {
        if(code_dict.ContainsKey(code))
        {
            return code_dict[code];
        }
        else
        {
            return code;
        }
    }
    public void create_spell (int spell_code, charactor speller, Vector2 direction, int wait_time, int move_mode)
    {
        if (spell_map.ContainsKey (get_code(spell_code)))
        {
            spell_list.Add (new spell (spell_code, speller, direction, wait_time, move_mode));
        }
    }
    public void create_spell (int spell_code, charactor speller)
    {
        if (spell_map.ContainsKey (get_code(spell_code)))
        {
            spell_list.Add (new spell (spell_code, speller, new Vector2 (0, 0), 0, 0));
        }
    }
}