using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class main_menu : MonoBehaviour
{
    private int screen_width;
    private int screen_height;
    private int reletive_rate;

    private GameObject title;
    private GameObject select;
    private Text press;

    private int shine_rate;
    private int shine_rate_max;
    private int shine_rate_max2;
    private bool shine_direction;

    private GameObject press_effect;

    private int anime_id;
    private int anime_rate;
    private int anime_rate_max;
    private float  anime_rate_f;

    private int selection_now = 0;
    private List<GameObject> selections = new List<GameObject>();

    private int test_level=2;

    void Start ()
    {
        refresh_screen_size();

        transform.Find("main/background_dark").localScale = new Vector2(screen_width * 1.0f / reletive_rate, screen_height * 1.0f / reletive_rate);

        title = transform.Find("main/title").gameObject;
        select = transform.Find("main/select").gameObject;
        press = transform.Find("main/press/canvas/text").GetComponent<Text> ();
        press_effect = Resources.Load ("effect/press_effect") as GameObject;

        selections.Add(transform.Find("main/continue").gameObject);
        selections.Add(transform.Find("main/new_game").gameObject);
        selections.Add(transform.Find("main/option").gameObject);
        selections.Add(transform.Find("main/quit").gameObject);

        foreach(GameObject selection_obj in selections)
        {
            selection_obj.transform.Find("canvas/text").GetComponent<Text> ().color = new Color(1, 1, 1, 0);
        }

        shine_rate = 0;
        shine_rate_max = 200;
        shine_rate_max2 = 150;
        shine_direction = true;

        anime_id = 0;
        anime_rate = 0;
        //anime_rate_max = 50;
        anime_rate_max = 5;

        anime_rate_f = 0;
    }

    void Update ()
    {
        switch(anime_id)
        {
        case-1:
            break;
        case 0:
            press_button_shine();
            if(Input.anyKeyDown)
            {
                select.GetComponent<SpriteRenderer> ().color = new Color(1, 1, 1, 0);
                GameObject obj = Instantiate (press_effect) as GameObject;
                obj.transform.position = transform.Find("main/press").position;
                anime_id = 1;
            }
            break;
        case 1:
            if(anime_frame(1))
            {
                press.color = new Color(1, 1, 1, 1 - anime_rate_f);
            }
            else
            {
                anime_rate = 0;
                anime_id = 2;
                change_selection(0);
            }

            break;
        case 2:
            if(anime_frame(2))
            {
                title.transform.position = new Vector3(title.transform.position.x, title.transform.position.y + (1 / 64.0f) * (int)(anime_rate_f * 3.0f), title.transform.position.z);
                for(int i = selections.Count - 1; i >= 0; i--)
                {
                    GameObject selection_obj = selections[i];
                    int anime_start = (selections.Count - 1 - i) * anime_rate_max / 5;
                    int anime_still = anime_rate_max / 5 * 2;
                    if(anime_rate >= anime_start && anime_rate <= anime_start + anime_still)
                    {
                        float anime_rate_s = (anime_rate - anime_start) * 1.0f / anime_still;
                        selection_obj.transform.Find("canvas/text").GetComponent<Text> ().color = new Color(1, 1, 1, anime_rate_s);
                    }
                }
            }
            else
            {
                anime_rate = 0;
                anime_id = 3;
                select.GetComponent<SpriteRenderer> ().color = new Color(1, 1, 1, 0);
            }
            break;
        case 3:
            if(Input.GetKeyDown (KeyCode.W))
            {
                change_selection(selection_now - 1);
            }
            else if(Input.GetKeyDown (KeyCode.S))
            {
                change_selection(selection_now + 1);
            }
            press_button_shine();

            if(Input.GetKeyDown (KeyCode.J ) || Input.GetKeyDown (KeyCode.Space))
            {
                select.GetComponent<SpriteRenderer> ().color = new Color(1, 1, 1, 0);
                GameObject obj = Instantiate (press_effect) as GameObject;
                obj.transform.position = select.transform.position;

                anime_rate = 0;
                anime_id = 4;
            }
            break;
        case 4:
            if(anime_frame(4))
            {
                //selections[selection_now].transform.Find("canvas/text").GetComponent<Text> ().color = new Color(1, 1, 1, 1 - anime_rate_f);
            }
            else
            {
                anime_rate = 0;
                anime_id = 5;
            }
            break;
        case 5:

            select_press();
            anime_rate = 0;
            anime_id = -1;
            break;
        }

    }
    private void change_selection(int selection_id)
    {
        selection_id = (selection_id + 4) % 4;
        shine_rate = shine_rate_max2;
        shine_direction = true;
        select.transform.position = selections[selection_id].transform.position;
        selection_now = selection_id;

    }
    private bool anime_frame(int anime_id_now)
    {
        anime_rate++;

        anime_rate_f = anime_rate * 1.0f / anime_rate_max;
        if(anime_rate_f < 0)
        {
            anime_rate_f = 0;
        }
        if(anime_rate_f > 1)
        {
            anime_rate_f = 1;
        }

        return anime_rate <= anime_rate_max;
    }
    private void refresh_screen_size()
    {
        
        screen_width = Screen.width;
        screen_height = Screen.height;
        int width_rate = (int)(screen_width / 256.0f);
        int height_rate = (int)(screen_height / 144.0f);
        reletive_rate = (width_rate < height_rate ? width_rate : height_rate);

        float camera_size = screen_height * 1.0f / reletive_rate / 2 / 64;
        GameObject.Find("camera").GetComponent<Camera>().orthographicSize = camera_size ;

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
    private void select_press()
    {
        switch(selection_now)
        {
        case 0:
            GameObject.Find("main_control").GetComponent<main_control>().change_level(test_level);
            break;
        case 3:
            Application.Quit();
            break;
        }
    }
}