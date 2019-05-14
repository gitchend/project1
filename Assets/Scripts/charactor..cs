using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class charactor : MonoBehaviour
{

    public float speed;
    public float jump_speed;

    public int hp_full;
    public int hp_now;

    protected bool is_dead = false;
    protected bool is_pause = false;
    protected bool is_pause_cache = false;

    protected bool in_air = false;
    protected bool against_wall = false;
    protected bool against_wall_2 = false;
    protected bool against_ceiling = false;

    protected bool direction = true; //right
    protected bool is_stun = false;
    protected bool is_hitted = false;
    protected bool is_attackable = true;
    protected int skill_spelling = -1;

    protected Vector2 rb_velocity_cache;

    private List<GameObject> skills;
    private List<Animator> effect_animators = new List<Animator>();
    protected Dictionary<int, buff> buff_map = new Dictionary<int, buff>();

    protected effect_controller ec;
    protected audio_controller ac;
    protected spell_controller sc;
    protected buff_controller bc;

    protected Rigidbody2D rb;
    protected Animator animator;
    protected List<int> anime_para_list;
    protected int anime_para_now = -1;
    protected string anime_name_cache = "";
    protected bool effect_create_lock = true;

    protected charactor target;
    protected charactor last_attack;
    protected charactor last_attacked;
    protected GameObject sprite;

    protected int timer = 0;


    void Start()
    {

        rb = GetComponent<Rigidbody2D>();

        sprite = transform.Find("sprite").gameObject;
        animator = sprite.GetComponent<Animator>();
        anime_para_list = new List<int>();
        foreach (AnimatorControllerParameter parameter in animator.parameters)
        {
            anime_para_list.Add(parameter.nameHash);
        }

        skills = new List<GameObject>();
        foreach (Transform child in transform.Find("skills"))
        {
            GameObject child_obj = child.gameObject;
            skills.Add(child_obj);
            child.gameObject.SetActive(false);
            attack attack = child_obj.GetComponent<attack>();
            if (attack != null)
            {
                attack.set_attacker(this);
            }
        }
        ec = GameObject.Find("effect_controller").GetComponent<effect_controller>();
        ac = GameObject.Find("audio_controller").GetComponent<audio_controller>();
        sc = GameObject.Find("spell_controller").GetComponent<spell_controller>();
        bc = GameObject.Find("buff_controller").GetComponent<buff_controller>();

        Start2();

    }
    void Update ()
    {
        if(EarlyUpdate())
        {
            return;
        }

        timer++;
        timer %= 1000;
        if(!is_anime_now_name(anime_name_cache))
        {
            effect_create_lock = true;
        }
        anime_name_cache = get_anime_name_now();
        //adjust_pixel ();
        if (is_pause) return;

        Update2();
    }
    void LateUpdate()
    {
        frame_extract_control ();
    }

    public virtual void Start2() { }
    public virtual bool EarlyUpdate()
    {
        return false;
    }
    public virtual void Update2() { }

    protected void move(bool direction_to)
    {
        if (against_wall)
        {
            set_speed(0, rb.velocity.y);
            if (!in_air) { }
        }
        else
        {
            set_speed(speed * (direction_to ? 1 : -1), rb.velocity.y);
        }

    }
    protected void move(bool direction_to, float speed_to)
    {
        if (against_wall)
        {
            set_speed(0, rb.velocity.y);
            if (!in_air) { }
        }
        else
        {
            set_speed(speed_to * (direction_to ? 1 : -1), rb.velocity.y);
        }

    }
    protected void turn()
    {
        direction = !direction;
        transform.localScale += new Vector3(-2 * transform.localScale.x, 0, 0);
    }
    public void add_speed(float speed_x, float speed_y)
    {
        rb.velocity += new Vector2(speed_x, speed_y);
    }
    protected void set_speed(float speed_x, float speed_y)
    {
        rb.velocity = new Vector2(speed_x, speed_y);
    }
    protected void set_anime_para(int index, bool is_any_state)
    {
        set_anime_para(index);
        set_anime_para(is_any_state);
    }
    protected void set_anime_para(bool is_any_state)
    {
        animator.SetBool(anime_para_list[0], is_any_state);
    }
    protected void set_anime_para(int index)
    {
        if (anime_para_now != index)
        {
            if (anime_para_now != -1)
            {
                animator.SetBool(anime_para_list[anime_para_now], false);
            }
            if (index != -1)
            {
                animator.SetBool(anime_para_list[index], true);
            }
            anime_para_now = index;
        }
    }
    protected int get_anime_para_now()
    {
        return anime_para_now;
    }
    protected string get_anime_name_now()
    {
        string sp = sprite.GetComponent<SpriteRenderer>().sprite.name;
        string[] subs = sp.Split('_');
        return sp.Substring(0, sp.Length - subs[subs.Length - 1].Length - 1);
    }
    protected void skill(int index)
    {
        add_speed(Random.value * 0.001f - 0.0005f, 0);
        if (skill_spelling != index)
        {
            if (skill_spelling != -1)
            {
                skills[skill_spelling].SetActive(false);
            }
            if (index != -1)
            {
                skills[index].SetActive(true);
                ac.create_audio(3, 0);
            }
            skill_spelling = index;
        }
    }
    protected float get_anime_normalized_time()
    {
        AnimatorStateInfo stateinfo = animator.GetCurrentAnimatorStateInfo(0);
        return stateinfo.normalizedTime;
    }
    public void add_effect_animator(Animator effect_animator)
    {
        effect_animators.Add(effect_animator);
    }

    private void frame_extract_control()
    {
        bool is_statu_change = false;
        if (is_pause && !is_pause_cache)
        {
            rb_velocity_cache = rb.velocity;
            rb.Sleep();
            rb.gravityScale = 0;
            animator.speed = 0;
            is_statu_change = true;
        }
        else if (!is_pause && is_pause_cache)
        {
            rb.WakeUp();
            rb.velocity = rb_velocity_cache;
            animator.speed = 1;
            is_statu_change = true;
        }
        // effect pause
        if(!is_statu_change)
        {
            for (int i = effect_animators.Count - 1; i >= 0; i--)
            {
                Animator effect_animator = effect_animators[i];
                if(effect_animator == null)
                {
                    effect_animators.Remove (effect_animator);
                }
                else
                {
                    effect_animator.speed = ((is_pause ? 0 : 1));
                }
            }
        }
        is_pause_cache = is_pause;
    }

    protected void floating_control(float limit, float scale1, float scale2)
    {
        if (Mathf.Abs(rb.velocity.y) < limit)
        {
            rb.gravityScale = scale1;
        }
        else
        {
            rb.gravityScale = scale2;
        }
    }
    protected void adjust_pixel()
    {
        sprite.transform.position = new Vector3(pixel_fix(transform.position.x), pixel_fix(transform.position.y), transform.position.z);
    }
    protected bool once_in_animation()
    {
        timer = 0;
        if(effect_create_lock)
        {
            effect_create_lock = false;
            return true;
        }
        else
        {
            return false;
        }

    }
    private float pixel_fix(float float_num)
    {
        float ret_num = 0;
        if(float_num >= 0)
        {
            ret_num = (int)(float_num * 64) / 64.0f;
        }
        else
        {
            ret_num = ((int)(float_num * 64) - 1 ) / 64.0f;
        }
        return ret_num;
    }
    //getter & setter
    public bool is_anime_now_name(string anime_name)
    {
        AnimatorStateInfo stateinfo = animator.GetCurrentAnimatorStateInfo(0);
        return stateinfo.IsName("Base Layer." + anime_name);
    }
    public void set_is_pause(bool is_pause_set)
    {
        is_pause = is_pause_set;
    }
    public void set_hp(int hp_set)
    {
        if (hp_set < 0)
        {
            hp_now = 0;
        }
        else if (hp_set > hp_full)
        {
            hp_now = hp_full;
        }
        else
        {
            hp_now = hp_set;
        }

    }
    public void set_in_air(bool in_air_now)
    {
        in_air = in_air_now;
    }
    public bool get_in_air()
    {
        return in_air;
    }
    public void set_against_wall(bool against_wall_now)
    {
        against_wall = against_wall_now;
    }
    public bool get_against_wall()
    {
        return against_wall;
    }
    public void set_against_wall_2(bool against_wall_now)
    {
        against_wall_2 = against_wall_now;
    }
    public bool get_against_wall_2()
    {
        return against_wall_2;
    }
    public void set_against_ceiling(bool against_ceiling_now)
    {
        against_ceiling = against_ceiling_now;
    }
    public bool get_against_ceiling()
    {
        return against_ceiling;
    }
    public void set_direction(bool direction_now)
    {
        if (direction != direction_now)
        {
            turn();
        }
    }
    public bool get_direction()
    {
        return direction;
    }
    public Vector2 get_position2()
    {
        return new Vector2(transform.position.x, transform.position.y);
    }
    public Vector2 get_speed2()
    {
        return new Vector2(rb.velocity.x, rb.velocity.y);
    }
    public void set_last_attacked(charactor last_attacked_now)
    {
        last_attacked = last_attacked_now;
    }
    public void set_last_attack(charactor last_attack_now)
    {
        last_attack = last_attack_now;
    }
    public charactor get_last_attacked()
    {
        return last_attacked;
    }
    public charactor get_last_attack()
    {
        return last_attack;
    }
    public bool get_is_stun()
    {
        return is_stun;
    }
    public bool get_is_attackable()
    {
        return is_attackable;
    }
    public void set_is_stun(bool is_stun_set)
    {
        is_stun = is_stun_set;
    }
    public Dictionary<int, buff> get_buff_map()
    {
        return buff_map;
    }
    public buff_controller get_buff_controller()
    {
        return bc;
    }
    public GameObject get_sprite_obj()
    {
        return sprite;
    }
    public bool get_is_pause()
    {
        return is_pause;
    }
    public void hit_message(attack attack)
    {
        hit_message2(attack);
    }
    public void hitted_message(attack attack)
    {
        hitted_message2(attack);
        is_hitted = true;
    }
    public virtual void hit_message2(attack attack) { }
    public virtual void hitted_message2(attack attack) { }
    public virtual void set_scroll_hit() { }

}