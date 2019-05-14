using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class menu_control : MonoBehaviour
{
    private player target;
    private bool is_menu;

    private GameObject select;
    private GameObject press_effect;

    private int shine_rate;
    private int shine_rate_max;
    private int shine_rate_max2;
    private bool shine_direction;

    private int layer_now;
    private string[] layer_names = new string[4] {"暂停", "物品", "地图", "符字"};

    private List<Text> layer_names_text = new List<Text>();

    private int selection_now;
    private int control_lock;
    private bool select_pressed;

    private List<GameObject> layer0_selections = new List<GameObject>();

    void Start ()
    {
        target = GameObject.Find ("hero").GetComponent<player> ();
        select = transform.Find ("ui_group3_3/ui_group3_3_2/select").gameObject;
        press_effect = Resources.Load ("effect/press_effect") as GameObject;

        is_menu = target.get_is_menu();

        foreach(Transform trans in transform.Find ("ui_group3_3/ui_group3_3_2/pause"))
        {
            layer0_selections.Add(trans.gameObject);
        }
        foreach(Transform trans in transform.Find ("ui_group3_3/ui_group3_3_1/canvas"))
        {
            layer_names_text.Add(trans.gameObject.GetComponent<Text> ());
        }

        layer_now = 0;

        selection_now = 0;

        shine_rate = 0;
        shine_rate_max = 200;
        shine_rate_max2 = 150;
        shine_direction = true;
        is_menu = false;

        control_lock = 0;
        select_pressed = false;
        
        Application.targetFrameRate = 60;
    }

    void Update ()
    {
        menu_set();
        if(!is_menu)
        {
            return;
        }
        if(control_lock > 0)
        {
            control_lock--;
        }
        else
        {
            if(select_pressed)
            {
                auto_control();
                select_pressed = false;
            }
            else
            {
                if(!player_control())
                {
                    press_button_shine();
                }
            }
        }
    }
    private void auto_control()
    {
        int control_id = layer_now * 10 + selection_now;
        switch(control_id)
        {
        case 0:
            target.set_is_menu(false);
            break;
        case 1:
            break;
        case 4:
            target.set_is_menu(false);
            GameObject.Find("main_control").GetComponent<main_control>().change_level(0);
            break;

        }
    }
    private void menu_set()
    {
        bool is_menu_now = target.get_is_menu();
        if(!is_menu && is_menu_now)
        {
            change_layer(0);
        }

        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(is_menu_now);
        }
        is_menu = is_menu_now;

    }
    private bool player_control()
    {
        if(Input.GetKeyDown (KeyCode.Q))
        {
            change_layer(layer_now - 1);
        }
        else if(Input.GetKeyDown (KeyCode.E))
        {
            change_layer(layer_now + 1);
        }
        else if(Input.GetKeyDown (KeyCode.W))
        {
            change_selection(selection_now - 1);
        }
        else if(Input.GetKeyDown (KeyCode.S))
        {
            change_selection(selection_now + 1);
        }
        else if(Input.GetKeyDown (KeyCode.J ) || Input.GetKeyDown (KeyCode.Space))
        {
            select.GetComponent<SpriteRenderer> ().color = new Color(1, 1, 1, 0);
            GameObject obj = Instantiate (press_effect) as GameObject;
            obj.transform.parent = select.transform;
            obj.transform.localPosition = new Vector3(0, 0, 0);

            select_pressed = true;
            control_lock = 50;
        }
        else
        {
            return false;
        }
        return true;
    }

    private void change_layer(int layer_id)
    {
        int layer_max = layer_names.Length;

        layer_id = (layer_id + layer_max) % layer_max;
        int layer_left = (layer_id - 1 + layer_max) % layer_max;
        int layer_right = (layer_id + 1 + layer_max) % layer_max;

        layer_names_text[0].text = layer_names[layer_left];
        layer_names_text[1].text = layer_names[layer_id];
        layer_names_text[2].text = layer_names[layer_right];

        if(layer_now == 0)
        {
            change_selection(0);
        }
        layer_now = layer_id;
    }
    private void change_selection(int selection_id)
    {

        int selection_num = get_selection(layer_now).Count;
        selection_id = (selection_id + selection_num) % selection_num;

        shine_rate = shine_rate_max2;
        shine_direction = true;
        select.transform.position = get_selection(layer_now)[selection_id].transform.position;
        selection_now = selection_id;
    }
    private List<GameObject> get_selection(int id)
    {
        switch(id)
        {
        case 0:
            return layer0_selections;
        case 1:
            break;
        }
        return null;
    }
    private void press_button_shine()
    {
        if(shine_direction)
        {
            shine_rate += 4;
            if(shine_rate >= shine_rate_max)
            {
                shine_direction = !shine_direction;
            }
        }
        else
        {
            shine_rate -= 4;
            if(shine_rate <= 0)
            {
                shine_direction = !shine_direction;
            }
        }
        float rate = shine_rate * 1.0f / shine_rate_max2;
        if(rate > 1)
        {
            rate = 1f;
        }
        else if(rate < 0)
        {
            rate = 0;
        }
        select.GetComponent<SpriteRenderer> ().color = new Color(1, 1, 1, rate);
    }
}