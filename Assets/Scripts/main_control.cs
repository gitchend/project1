using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class main_control : MonoBehaviour
{

    private int level_now;
    private Dictionary<int, string> level_names = new Dictionary<int, string>();
    private SpriteRenderer loading_back;
    private SpriteRenderer loading_logo;

    private int screen_width;
    private int screen_height;
    private int reletive_rate;

    private player player;
    private GameObject loading_obj;

    public static main_control only_one;

    private bool is_loading=false;

    void Start ()
    {
        if(only_one == null)
        {
            only_one = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if(only_one != this)
            {
                Destroy(gameObject);
                return;
            }
        }
        refresh_screen_size();
        level_now = 0;

        player = GameObject.Find("hero").GetComponent<player>();
        loading_obj = GameObject.Find("loading");

        transform.Find("loading/back").localScale = new Vector2(screen_width * 1.0f / reletive_rate, screen_height * 1.0f / reletive_rate);

        loading_back = transform.Find("loading/back").gameObject.GetComponent<SpriteRenderer>();
        loading_logo = transform.Find("loading/logo").gameObject.GetComponent<SpriteRenderer>();

        loading_back.color = new Color(0, 0, 0, 0);
        loading_logo.color = new Color(1, 1, 1, 0);

        level_names[0] = "main_menu";
        level_names[1] = "level_1";
        level_names[2] = "level_2";
        level_names[3] = "level_3";
        level_names[4] = "level_4";

        level_names[100] = "test";

        Application.targetFrameRate = 60;

        set_controller_active(false);

    }
    void Update ()
    {
    	if(is_loading){
    		player.catch_up_keyboard();
    	}

    }
    private void refresh_screen_size()
    {
        screen_width = Screen.width;
        screen_height = Screen.height;
        int width_rate = (int)(screen_width / 512.0f);
        int height_rate = (int)(screen_height / 288.0f);
        reletive_rate = (width_rate < height_rate ? width_rate : height_rate);

    }
    public void change_level(int level_to)
    {
        change_level(level_to, 0);
    }
    public void change_level(int level_to, int gate_num)
    {

        if(level_names.ContainsKey(level_to))
        {
    		is_loading=true;
            StartCoroutine (LoadScene (level_to, gate_num));
        }
    }

    IEnumerator LoadScene(int level_to, int gate_num)
    {
        yield return StartCoroutine(loading_anime(true));

        string scene_name = level_names[level_to];
        if(level_to == 0)
        {
            set_controller_active(false);
        }

        Resources.UnloadUnusedAssets();
        System.GC.Collect();

        AsyncOperation async2 = Application.LoadLevelAsync(scene_name);
        yield return async2;

        Debug.Log(scene_name + " loaded.");
        if(level_to == 0)
        {

            loading_obj.transform.position = new Vector3(0, 0, loading_obj.transform.position.z);
        }

        if(level_to != 0 && level_now == 0)
        {
            set_controller_active(true);
        }
        if(level_to != 0)
        {
            Transform gate_trans = GameObject.Find("map_gate").transform;
            player.transform.position = gate_trans.GetChild(gate_num < gate_trans.childCount ? gate_num : 0).position;
        }

        level_now = level_to;

        player.set_against_wall(false);
        player.set_against_wall_2(false);

        is_loading=false;

        yield return StartCoroutine(loading_anime(false));
    }
    IEnumerator loading_anime(bool statu)
    {
        int rate_max = 20; //finish in x frames

        for(int i = 0; i <= rate_max; i++)
        {
            loading_back.color = new Color(0, 0, 0, (statu ? i : rate_max - i) * 1.0f / rate_max);
            loading_logo.color = new Color(1, 1, 1, (statu ? i : rate_max - i) * 1.0f / rate_max);
            yield return null;
        }
    }
    private void set_controller_active(bool statu)
    {
        foreach(Transform trans in transform)
        {
            trans.gameObject.SetActive(statu);
        }
        loading_obj.SetActive(true);
    }
}
