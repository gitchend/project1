using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scroll_control : MonoBehaviour
{
    public Sprite[] runes;
    public Sprite[] rune_nums;
    private player target;

    private SpriteRenderer[] rune_renderers = new SpriteRenderer[4];
    private SpriteRenderer[] rune_num_renderers = new SpriteRenderer[3];

    private SpriteRenderer scroll1_renderer;
    private SpriteRenderer scroll2_renderer;
    private SpriteRenderer scroll3_renderer;

    private Animator scroll3_animator;
    private int animator_code;
    private List<int> anime_para_list = new List<int>();

    private SpriteMask scroll_mask_hole;
    private SpriteRenderer scroll_mask_hole_renderer;
    private SpriteRenderer scroll_mask_flame_renderer;
    private Animator scroll_mask_animator;

    private int rune_now;
    private int rune_cache = 0;

    private bool write_lock = false;
    private bool write_zero_lock = false;

    private bool is_scroll_pause = true;

    private int shine_rate;
    private int shine_rate_max;
    private int shine_rate_max2;

    private bool shine_direction;
    private bool shine_switch;
    private bool shine_switch_cache;


    void Start ()
    {
        target = GameObject.Find ("hero").GetComponent<player> ();
        rune_renderers[0] = transform.Find ("rune1").gameObject.GetComponent<SpriteRenderer>();
        rune_renderers[1] = transform.Find ("rune2").gameObject.GetComponent<SpriteRenderer>();
        rune_renderers[2] = transform.Find ("rune3").gameObject.GetComponent<SpriteRenderer>();
        rune_renderers[3] = transform.Find ("rune4").gameObject.GetComponent<SpriteRenderer>();

        rune_num_renderers[0] = transform.Find ("rune_num1").gameObject.GetComponent<SpriteRenderer>();
        rune_num_renderers[1] = transform.Find ("rune_num2").gameObject.GetComponent<SpriteRenderer>();
        rune_num_renderers[2] = transform.Find ("rune_num3").gameObject.GetComponent<SpriteRenderer>();

        scroll1_renderer = transform.Find ("scroll1").gameObject.GetComponent<SpriteRenderer>();
        scroll2_renderer = transform.Find ("scroll2").gameObject.GetComponent<SpriteRenderer>();
        scroll3_renderer = transform.Find ("scroll3").gameObject.GetComponent<SpriteRenderer>();
        scroll3_animator = transform.Find ("scroll3").gameObject.GetComponent<Animator>();

        //mask
        scroll_mask_animator = transform.Find ("scroll_mask_hole").gameObject.GetComponent<Animator>();
        scroll_mask_flame_renderer = transform.Find ("scroll_mask_flame").gameObject.GetComponent<SpriteRenderer>();

        scroll_mask_hole = transform.Find ("scroll_mask_hole").gameObject.GetComponent<SpriteMask>();
        scroll_mask_hole_renderer = transform.Find ("scroll_mask_hole").gameObject.GetComponent<SpriteRenderer>();

        foreach (AnimatorControllerParameter parameter in scroll3_animator.parameters)
        {
            anime_para_list.Add(parameter.nameHash);
        }

        shine_rate = 0;
        shine_rate_max = 200;
        shine_rate_max2 = 150;

        shine_direction = true;
        shine_switch = false;

    }

    void Update ()
    {
        rune_now = target.get_rune_now();
        deal_rune();
        deal_rune_num();
        deal_mask();
        deal_scroll();
        deal_scroll_animator();

        rune_cache = rune_now;
    }
    private void deal_mask()
    {
        scroll_mask_hole.sprite = scroll_mask_hole_renderer.sprite;
        scroll_mask_flame_renderer.sprite = get_larger_sprite(scroll_mask_hole_renderer.sprite);
        if(is_anime_now_name("scroll_mask1", scroll_mask_animator))
        {
            set_is_scroll_pause(true);
            target.set_scroll_write_lock(false);
        }
        else  if(is_anime_now_name("scroll_mask2", scroll_mask_animator))
        {
            set_is_scroll_pause(rune_now != 0);
            if(rune_now != 0)
            {
                scroll_mask_animator.SetBool("start", true );
            }
            target.set_scroll_write_lock(true);
        }
        else  if(is_anime_now_name("scroll_mask3", scroll_mask_animator))
        {

            set_is_scroll_pause(true);
            target.set_scroll_write_lock(true);
        }
        else  if(is_anime_now_name("scroll_mask4", scroll_mask_animator))
        {
            set_is_scroll_pause(rune_now == 0);
            if(rune_now == 0)
            {
                scroll_mask_animator.SetBool("start", false);
            }
            target.set_scroll_write_lock(true);
        }
    }
    private void deal_rune()
    {
        Dictionary<int, List<int>> dict = target.get_scroll_dict();
        for(int i = 0; i < 4; i++)
        {
            rune_renderers[i].sprite = runes[i + 4];
        }
        if(dict.ContainsKey(rune_now))
        {
            foreach(int i in dict[rune_now])
            {
                rune_renderers[i - 1].sprite = runes[i - 1];
            }
        }
    }
    private void deal_rune_num()
    {
        int rune_now_copy = rune_now;
        for(int i = 0 ; i < 2; i++)
        {
            if(rune_now_copy < 100)
            {
                rune_now_copy *= 10;
            }
        }
        int code1 = (int)(rune_now_copy / 100f);
        int code2 = (int)((rune_now_copy % 100) / 10f);
        int code3 = rune_now_copy % 10;

        rune_num_renderers[0].sprite = rune_nums[code1];
        rune_num_renderers[1].sprite = rune_nums[code2];
        rune_num_renderers[2].sprite = rune_nums[code3];
    }
    private void deal_scroll()
    {
        if(shine_switch)
        {
            if(shine_direction)
            {
                shine_rate += 1;
                if(shine_rate == shine_rate_max)
                {
                    shine_direction = !shine_direction;
                }
            }
            else
            {
                shine_rate -= 1;
                if(shine_rate == 0)
                {
                    shine_direction = !shine_direction;
                }
            }
        }
        else
        {
            shine_rate = 0;
            shine_direction = true;
        }
        float rate = shine_rate * 1.0f / shine_rate_max2;
        if(rate > 1)
        {
            rate = 1f;
        }
        scroll1_renderer.color = new Color(1 - 0.1f * rate, 1 - 0.1f * rate, 1 - 0.1f * rate);
        scroll2_renderer.color = new Color(1 - 0.1176f * (1 - rate), 1 - 0.3961f * (1 - rate), 1 - 0.7137f * (1 - rate));

    }
    private void deal_scroll_animator()
    {
        int rune_now_copy = rune_now;
        for(int i = 0 ; i < 2; i++)
        {
            if(rune_now_copy < 100)
            {
                rune_now_copy *= 10;
            }
        }
        int code1 = (int)(rune_now_copy / 100f);
        int code2 = (int)((rune_now_copy % 100) / 10f);
        int code3 = rune_now_copy % 10;

        if(!is_scroll_pause)
        {
            set_scroll3_code(0, code1);
            set_scroll3_code(1, code2);
            set_scroll3_code(2, code3);

        }


        if(get_anime_name_now().Length > 0 && get_anime_name_now().Split('_')[1].Split('-')[0].Length == 3)
        {
            target.set_scroll_ready(true);
            shine_switch = true;
        }
        else
        {
            target.set_scroll_ready(false);
            shine_switch = false;
        }
    }
    private void set_scroll3_code(int index, int code)
    {
        scroll3_animator.SetInteger(anime_para_list[index], code);
    }
    private string get_anime_name_now()
    {
        if(scroll3_renderer.sprite == null)
        {
            return "";
        }
        else
        {
            string sp = scroll3_renderer.sprite.name;
            string[] subs = sp.Split('_');
            return sp.Substring(0, sp.Length - subs[subs.Length - 1].Length - 1);
        }
    }
    public bool is_anime_now_name(string anime_name, Animator animator)
    {
        AnimatorStateInfo stateinfo = animator.GetCurrentAnimatorStateInfo(0);
        return stateinfo.IsName("Base Layer." + anime_name);
    }
    private Sprite get_larger_sprite(Sprite sprite)
    {
        if(sprite == null)
        {
            return null;
        }
        int sprite_x_min = (int)sprite.rect.xMin;
        int sprite_y_min = (int)sprite.rect.yMin;
        int sprite_width = (int)sprite.rect.width;
        int sprite_height = (int)sprite.rect.height;
        Texture2D texture2D = new Texture2D(sprite_width, sprite_height, sprite.texture.format, false);
        texture2D.SetPixels(sprite.texture.GetPixels(sprite_x_min, sprite_y_min, sprite_width, sprite_height));
        texture2D.filterMode = FilterMode.Point;

        int[,] location = new int[4, 2] {{0, -1}, {0, 1}, {-1, 0}, {1, 0}};
        for(int i = 0; i < sprite_width; i++)
        {
            for(int j = 0; j < sprite_height; j++)
            {
                Color color_set = sprite.texture.GetPixel(i + sprite_x_min, j + sprite_y_min);
                if(color_set.a == 0)
                {
                    continue;
                }
                for(int k = 0; k < 4; k++)
                {
                    int x = i + location[k, 0];
                    int y = j + location[k, 1];
                    if(x > 0 && x < sprite_width - 1 && y > 0 && y < sprite_height - 1)
                    {
                        texture2D.SetPixel(x, y, texture2D.GetPixel(i, j));
                    }
                }
            }
        }
        texture2D.Apply();
        Rect rect = new Rect (0, 0, sprite_width, sprite_height);
        return Sprite.Create(texture2D, rect, new Vector2(0.5f, 0.5f), 64.0f);
    }
    private void set_is_scroll_pause(bool is_scroll_pause_set)
    {
        is_scroll_pause = is_scroll_pause_set;
        scroll3_animator.speed = (is_scroll_pause ? 0 : 1);
    }

    private float get_anime_normalized_time(Animator animator)
    {
        AnimatorStateInfo stateinfo = animator.GetCurrentAnimatorStateInfo(0);
        return stateinfo.normalizedTime;
    }
}