using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class player : charactor
{

    //keybord
    private bool keydown_w = false;
    private bool keydown_a = false;
    private bool keydown_s = false;
    private bool keydown_d = false;
    private bool keydown_j = false;
    private bool keydown_k = false;
    private bool keydown_u = false;
    private bool keydown_i = false;
    private bool keydown_shift = false;
    private bool keydown_space = false;

    private KeyCode keyboard_cache = KeyCode.None;
    private int keyboard_cache_timer = 0;

    private bool moving_control_valid = false;

    //action lock
    private bool move_lock = false;
    private bool walk_lock = false;
    private bool turn_walk_lock = false; //cancel action & turn & walk
    private bool turn_lock = false;//as soon as action lauching
    private bool jump_lock = false;
    private bool hit_floor_lock = false;
    private bool roll_lock = false;
    private bool attack_light_lock = false;
    private bool attack_heavy_lock = false;
    private bool item_lock = false;
    private bool scroll_lock = false;
    private bool is_air_jump = false;
    private bool is_climb_jump = false;
    private bool is_air_jumped = false;
    private bool is_climbed = false;
    private bool is_air_attacked = false;

    private int cross_gate_lock = 0;

    private float max_height = 0;
    private bool gate_direction = false ;
    private bool gate_direction_y = false ;

    private bool is_menu = false;

    private bool scroll_write_lock = true;

    private int jump_charging = 0;
    private int jump_charging_max = 10;

    private bool is_scroll_ready = false;

    private GameObject against_wall_obj;
    private GameObject interaction_obj;

    private Dictionary<int, List<int>> scroll_dict = new Dictionary<int, List<int>>();
    //gameplay data
    private Dictionary<int, int> item_map = new Dictionary<int, int> ();
    private List<int> scrolls = new List<int>();
    private int rune_now = 0;
    private List<int> item_code_list = new List<int> ();
    private int item_index_now = 0;
    private int mp_max = 5;
    private int mp_now = 2;
    private int mp_charge_max = 100;
    private int mp_charge_now = 0;
    private int bullet_max = 6;
    private int bullet_now = 5;

    public override void Start2 ()
    {
        against_wall_obj = transform.Find ("against_wall").gameObject;
        interaction_obj = transform.Find ("interaction").gameObject;


        scrolls.Add(224);
        scrolls.Add(332);
        refresh_scroll();

        //speed=10;
        //jump_speed=20;

    }
    public override bool EarlyUpdate()
    {
        if(cross_gate_lock <= 0)
        {
            keyboard_listen ();
        }
        if(is_menu)
        {
            Time.timeScale = 0;
            return true;
        }
        else
        {
            Time.timeScale = 1;
        }
        scroll_control();

        return false;
    }
    public override void Update2 ()
    {

        floating_control (0.5f, 1.0f, 3.8f);
        set_collider (0);

        move_lock = false;
        walk_lock = false;
        turn_walk_lock = false;
        turn_lock = false;
        jump_lock = false;
        roll_lock = false;
        attack_light_lock = false;
        attack_heavy_lock = false;
        item_lock = false;
        scroll_lock = false;
        hit_floor_lock = false;

        //auto_control
        cross_gate_control();

        if (movement_control ()) return;
        if (ground_attack_control ()) return;
        if (air_attack_control ()) return;
        if (item_control ()) return;

        if (is_anime_now_name ("spell1_1"))
        {
            if (get_anime_normalized_time () < 0.43)
            {
                if (moving_control_valid && direction == keydown_d)
                {
                    move (direction, speed * 0.8f);
                }
                else
                {
                    move (direction, speed * 0.6f);
                }
            }
            else
            {
                skill (8);
            }
        }
        else if (is_anime_now_name ("spell1_2"))
        {
            jump_lock = true;
            attack_light_lock = true;
            roll_lock = true;
            skill (-1);
            set_anime_para (-1);

        }
        else if (is_anime_now_name ("spell2_1"))
        {
            set_collider (1);
        }
        else if (is_anime_now_name ("spell2_2"))
        {
            bc.create_buff(101, this, this, 5, 0);
            if (timer % 4 == 0)
            {
                skill (9);
            }
            else
            {
                skill (-1);
            }
            move (direction, speed * 2);
            set_anime_para (-1);

        }
        else if (is_anime_now_name ("spell2_3"))
        {
            skill (10);
            set_anime_para (-1);
        }
        else if (is_anime_now_name ("spell3_1"))
        {
            if (get_anime_normalized_time () < 0.572)
            {
                if (moving_control_valid && direction == keydown_d)
                {
                    move (direction, speed * 1.2f);
                }
                else
                {
                    move (direction, speed * 0.8f);
                }
            }
            else
            {
                if(get_anime_normalized_time () < 0.75)
                {
                    skill (11);
                }
                else
                {
                    skill (12);
                }
            }
        }
        else if (is_anime_now_name ("spell3_2"))
        {
            jump_lock = true;
            attack_light_lock = true;
            roll_lock = true;
            skill (-1);
            set_anime_para (-1);
        }
        else if (is_anime_now_name ("spell4_1"))
        {
            set_anime_para (-1);
        }
        else if (is_anime_now_name ("spell4_2"))
        {
            attack_light_lock = true;
            attack_heavy_lock = true;
        }
        else if (is_anime_now_name ("spell4_3"))
        {
            if (get_anime_normalized_time () < 0.2)
            {
                if (moving_control_valid && direction == keydown_d)
                {
                    move (direction, speed * 0.4f);
                }
            }
            if (timer % 3 == 0)
            {
                create_smoke(0);
                skill (13);
            }
            else
            {
                skill (-1);
            }
        }
        else if (is_anime_now_name ("spell4_4"))
        {
            if( get_anime_normalized_time () < 0.1 && once_in_animation() )
            {
                create_smoke(1);
            }
            bc.create_buff(101, this, this, 5, 0);
            if(timer % 8 == 0)
            {
                create_smoke(0);
            }
            if (get_anime_normalized_time () < 0.4)
            {
                move (!direction, speed * 0.8f);
            }
            else if (get_anime_normalized_time () < 0.63)
            {
                if (moving_control_valid && direction == keydown_d)
                {
                    move (direction, speed * 1.2f);
                }
                else
                {
                    move (direction, speed * 0.8f);
                }
                if(get_anime_normalized_time () >= 0.5 )
                {
                    effect_create_lock = true;
                }
            }
            else
            {
                if(once_in_animation())
                {
                    ec. create_effect (7, !direction, gameObject, new Vector2((direction ? 0.6f : -0.6f), -0.14f), this, 0);
                }
                if (moving_control_valid && direction == keydown_d)
                {
                    move (direction, speed * 1.2f);
                }
                else
                {
                    move (direction, speed * 0.8f);
                }
                if (get_anime_normalized_time () < 0.75)
                {
                    skill (15);
                }
                else
                {
                    skill (14);

                }
            }
        }
        else if (is_anime_now_name ("spell4_5"))
        {
            jump_lock = true;
            attack_light_lock = true;
            roll_lock = true;

            if(once_in_animation())
            {
                create_smoke(2);
            }
            if (get_anime_normalized_time () < 0.5)
            {
                if(timer % 8 == 0)
                {
                    create_smoke(0);
                }
            }

            skill (-1);
            set_anime_para (-1);

        }
        else if (is_anime_now_name ("spell4_6"))
        {
            if (moving_control_valid && direction == keydown_d)
            {
                move (direction, speed * 0.8f);
            }
            else
            {
                move (direction, speed * 0.4f);
            }
        }
        else if (is_anime_now_name ("spell5"))
        {
            if(get_anime_normalized_time () > 0.34)
            {
                if (moving_control_valid && direction == keydown_d)
                {
                    move (direction, speed * 0.8f);
                }
                else
                {
                    move (direction, speed * 0.4f);
                }
            }
            if(get_anime_normalized_time () > 0.67 && once_in_animation())
            {
                sc.create_spell(4, this);
            }

        }

        //control
        if (player_control ())
        {
            keyboard_cache = KeyCode.None;
            return;
        }


    }
    private void create_smoke(int kind)
    {
        switch(kind)
        {
        case 0://dust small
            ec.create_effect (4, false, new Vector2(transform.position.x, transform.position.y - 0.35f), 0);
            break;
        case 1://dust large
            ec.create_effect (3, false, new Vector2(transform.position.x, transform.position.y - 0.4f), 0);
            break;
        case 2://hit floor
            ec.create_effect (3, false, new Vector2(transform.position.x + (direction ? 0.4f : -0.4f), transform.position.y - 0.4f), 0);
            for(int i = 0; i <= 4; i++)
            {
                ec.create_effect (4, false, new Vector2(transform.position.x + (direction ? 0.4f : -0.4f) + Random.value * 0.6f - 0.3f,  transform.position.y + Random.value * 0.2f - 0.4f), 0);
            }
            break;
        case 3://run_dust
            ec.create_effect (4, false, new Vector2(transform.position.x + Random.value * 0.1f - 0.05f - (direction ? 0.2f : -0.2f),   Random.value * 0.1f - 0.05f + transform.position.y - 0.35f), 0);
            break;
        }
    }
    public void catch_up_keyboard()
    {
        keydown_w = Input.GetKey(KeyCode.W);
        keydown_a = Input.GetKey(KeyCode.A);
        keydown_s = Input.GetKey(KeyCode.S);
        keydown_d = Input.GetKey(KeyCode.D);
        keydown_j = Input.GetKey(KeyCode.J);
        keydown_k = Input.GetKey(KeyCode.K);
        keydown_u = Input.GetKey(KeyCode.U);
        keydown_i = Input.GetKey(KeyCode.I);
        keydown_shift = Input.GetKey(KeyCode.LeftShift);
        keydown_space = Input.GetKey(KeyCode.Space);

        moving_control_valid = !((keydown_d && keydown_a) || !(keydown_d || keydown_a));

        keyboard_cache = KeyCode.None;
        keyboard_cache_timer = 0;
    }
    private void cross_gate_control()
    {
        if(cross_gate_lock > 0)
        {
            if(gate_direction_y)
            {
                if(gate_direction)
                {
                    if(direction)
                    {
                        keydown_d = true;
                        keydown_a = false;
                        moving_control_valid = true;
                    }
                    else
                    {
                        keydown_a = true;
                        keydown_d = false;
                        moving_control_valid = true;
                    }
                    if(cross_gate_lock > 15 )
                    {
                        keydown_space = true;
                        jump_charging = 0;
                    }
                }
            }
            else
            {
                if(gate_direction)
                {
                    keydown_d = true;
                    keydown_a = false;
                    moving_control_valid = true;
                }
                else
                {
                    keydown_a = true;
                    keydown_d = false;
                    moving_control_valid = true;
                }
            }
            cross_gate_lock--;
            if(cross_gate_lock == 0)
            {
                catch_up_keyboard();
                interaction_obj.SetActive(true);
            }
        }
    }
    public void set_cross_gate(bool gate_direction_set, bool gate_direction_y_set)
    {
        gate_direction = gate_direction_set;
        gate_direction_y = gate_direction_y_set;
        cross_gate_lock = 40;
        interaction_obj.SetActive(false);
    }

    private bool movement_control ()
    {
        if (is_anime_now_name ("idle"))
        {
            move_lock = true;
            walk_lock = true;
            turn_lock = true;
            jump_lock = true;
            roll_lock = true;
            attack_light_lock = true;
            item_lock = true;
            scroll_lock = true;
            if (in_air)
            {
                set_anime_para (5);
                return true;
            }
        }
        else if (is_anime_now_name ("run1"))
        {
            walk_lock = true;
            jump_lock = true;
            roll_lock = true;
            attack_light_lock = true;
            item_lock = true;
            scroll_lock = true;
            move(direction, speed);

            if (in_air)
            {
                set_anime_para (5);
                return true;
            }
            if(timer % 4 == 0)
            {
                create_smoke(3);
            }

        }
        else if (is_anime_now_name ("run2"))
        {
            move_lock = true;
            walk_lock = true;
            jump_lock = true;
            roll_lock = true;
            attack_light_lock = true;
            item_lock = true;
            scroll_lock = true;
            if (in_air)
            {
                set_anime_para (5);
                return true;
            }
            if(timer % 4 == 0)
            {
                create_smoke(3);
            }
        }
        else if (is_anime_now_name ("run3"))
        {
            turn_walk_lock = true;
            jump_lock = true;
            attack_light_lock = true;
            scroll_lock = true;

            move (direction, speed / 4);

            if (in_air)
            {
                set_anime_para (5);
                return true;
            }
            if(timer % 8 == 0)
            {
                create_smoke(3);
            }
        }
        else if (is_anime_now_name ("run4"))
        {
            jump_lock = true;
            roll_lock = true;
            attack_light_lock = true;
            item_lock = true;
            scroll_lock = true;

            move (!direction, speed / 4);

            if (in_air)
            {
                set_anime_para (5);
                return true;
            }
            if(once_in_animation())
            {
                create_smoke(2);
            }
            if(timer % 8 == 0)
            {
                create_smoke(3);
            }
        }
        else if (is_anime_now_name ("jump1"))
        {
            move_lock = true;
            set_anime_para (-1);
            is_air_attacked = false;

        }
        else if (is_anime_now_name ("jump2"))
        {
            move_lock = !is_climb_jump;
            turn_lock = !is_climb_jump;
            jump_lock = !is_air_jumped;
            if(once_in_animation())
            {
                create_smoke(2);
            }
            if (get_anime_normalized_time () < 0.5)
            {
                create_smoke(0);
            }

            if (jump_charging == 0 || (keydown_space && jump_charging < jump_charging_max))
            {
                if(against_ceiling)
                {
                    set_speed (0, 0);
                }
                else
                {
                    if (is_climb_jump)
                    {
                        move (direction);
                    }
                    set_speed (rb.velocity.x, jump_speed);
                    jump_charging++;
                }

            }
            if (rb.velocity.y < 4)
            {
                if (!is_air_jump || is_climb_jump)
                {
                    set_anime_para (5);
                }
                else
                {
                    set_anime_para (6);
                }
                return true;
            }
        }
        else if (is_anime_now_name ("jump3"))
        {
            move_lock = true;
            jump_lock = !is_air_jumped;
            turn_lock = true;
            attack_light_lock = !is_air_attacked;
            hit_floor_lock = true;
            item_lock = true;
            jump_charging = 0;
            is_climb_jump = false;
            is_climbed = false;

            if((moving_control_valid && keydown_d == direction))
            {
                if (in_air && !against_wall && against_wall_2)
                {
                    if(rb.velocity.y <= 0)
                    {
                        set_speed (0, -jump_speed / 4);
                    }
                    else
                    {
                        set_speed (rb.velocity.x, jump_speed / 2 );
                    }
                    return true;
                }
                if (against_wall && against_wall_2 && !is_climbed )
                {
                    set_anime_para (10);
                    return true;
                }
            }


        }
        else if (is_anime_now_name ("jump4"))
        {
            if(once_in_animation())
            {
                create_smoke(2);
            }
            turn_lock = true;
            jump_lock = true;
            roll_lock = true;
            is_air_jumped = false;
            if((moving_control_valid && keydown_d == direction))
            {
                move(direction, speed / 2);
            }

            set_anime_para (1);
        }
        else if (is_anime_now_name ("jump5"))
        {
            jump_lock = true;

            move_lock = true;
            turn_lock = true;
            hit_floor_lock = true;
            attack_light_lock = !is_air_attacked;
            item_lock = true;
            jump_charging = 0;
            is_climb_jump = false;
            is_climbed = false;


            if((moving_control_valid && keydown_d == direction))
            {
                if (in_air && !against_wall && against_wall_2)
                {
                    if(rb.velocity.y <= 0)
                    {
                        set_speed (0, -jump_speed / 4);
                    }
                    else
                    {
                        set_speed (rb.velocity.x, jump_speed / 2 );
                    }
                    return true;
                }
                if (against_wall && against_wall_2 && !is_climbed )
                {
                    set_anime_para (10);
                    return true;
                }
            }

        }
        else if (is_anime_now_name ("jump6"))
        {
            rb.gravityScale = 0;
            set_speed (0, 0);
        }
        else if (is_anime_now_name ("climb"))
        {
            jump_lock = true;
            is_climbed = true;
            is_climb_jump = true;
            item_lock = true;
            hit_floor_lock = true;

            rb.gravityScale = 0;
            set_speed (0, -0.8f);
            if(timer % 8 == 0)
            {
                ec.create_effect (4, false, new Vector2(transform.position.x + (direction ? -1 : 1) * (Random.value * 0.1f - 0.05f - 0.2f), transform.position.y + 0.1f), 0);
                ec.create_effect (4, false, new Vector2(transform.position.x + (direction ? -1 : 1) * (Random.value * 0.1f - 0.05f - 0.2f),  transform.position.y - 0.3f), 0);
            }
            if (!against_wall_2 || !(moving_control_valid && keydown_d == direction))
            {
                set_anime_para (5);
                return true;
            }
        }
        else if (is_anime_now_name ("flash1"))
        {
            if(once_in_animation())
            {
                create_smoke(2);
            }

            bc.create_buff(101, this, this, 5, 0);
            if(timer % 8 == 0)
            {
                create_smoke(0);
            }
            move (direction, speed * 2f);
            set_collider (1);
            set_anime_para (-1);
        }
        return false;
    }
    private bool ground_attack_control ()
    {
        if (is_anime_now_name ("attack1"))
        {
            attack_light_lock = true;
            attack_heavy_lock = true;
            jump_lock = true;
            roll_lock = true;
            item_lock = true;
            scroll_lock = true;
            set_anime_para (-1);
            skill (-1);
        }
        else if (is_anime_now_name ("attack2"))
        {
            attack_light_lock = true;
            attack_heavy_lock = true;
            jump_lock = true;
            roll_lock = true;
            item_lock = true;
            scroll_lock = true;
            set_anime_para (-1);
            skill (-1);
        }
        else if (is_anime_now_name ("attack3"))
        {
            attack_light_lock = true;
            attack_heavy_lock = true;
            jump_lock = true;
            roll_lock = true;
            item_lock = true;
            scroll_lock = true;
            set_anime_para (-1);
            skill (-1);
        }
        else if (is_anime_now_name ("attack4"))
        {
            if(once_in_animation())
            {
                create_smoke(2);
            }
            attack_light_lock = true;
            jump_lock = true;
            roll_lock = true;
            item_lock = true;
            scroll_lock = true;
            set_anime_para (-1);
            skill (-1);
        }
        else if (is_anime_now_name ("attack0-1"))
        {
            if (get_anime_normalized_time () < 0.4)
            {
                if (moving_control_valid && direction == keydown_d)
                {
                    move (direction, speed * 0.6f);
                }
                else
                {
                    move (direction, speed * 0.3f);
                }
            }
            else
            {
                skill (0);
            }
            if(timer % 8 == 0)
            {
                create_smoke(0);
            }
        }
        else if (is_anime_now_name ("attack1-2"))
        {
            if (get_anime_normalized_time () < 0.4)
            {
                if (moving_control_valid && direction == keydown_d)
                {
                    move (direction, speed * 0.6f);
                }
                else
                {
                    move (direction, speed * 0.3f);
                }
            }
            else
            {
                skill (1);
            }
            if(timer % 8 == 0)
            {
                create_smoke(0);
            }
        }
        else if (is_anime_now_name ("attack2-3"))
        {
            if (get_anime_normalized_time () < 0.5)
            {
                if (moving_control_valid && direction == keydown_d)
                {
                    move (direction, speed * 0.6f);
                }
                else
                {
                    move (direction, speed * 0.3f);
                }
            }
            else
            {
                skill (2);
            }
            if(timer % 8 == 0)
            {
                create_smoke(0);
            }
        }
        else if (is_anime_now_name ("attack3-4"))
        {
            if (get_anime_normalized_time () < 0.625)
            {
                if (moving_control_valid && direction == keydown_d)
                {
                    move (direction, speed * 0.6f);
                }
                else
                {
                    move (direction, speed * 0.3f);
                }
            }
            else
            {
                skill (3);
            }
            if(once_in_animation())
            {
                create_smoke(2);
            }
            if(timer % 8 == 0)
            {
                create_smoke(0);
            }
        }
        else if (is_anime_now_name ("attack4-1"))
        {
            if (get_anime_normalized_time () < 0.5)
            {
                if (moving_control_valid && direction == keydown_d)
                {
                    move (direction, speed * 0.6f);
                }
                else
                {
                    move (direction, speed * 0.3f);
                }
            }
            else
            {
                skill (4);
            }
            if(timer % 8 == 0)
            {
                create_smoke(0);
            }
        }
        else if (is_anime_now_name ("replace"))
        {
            attack_light_lock = true;
            jump_lock = true;
            roll_lock = true;
            item_lock = true;
            scroll_lock = true;
            if (get_anime_normalized_time () > 0.4 && once_in_animation())
            {
                if(mp_now < mp_max)
                {
                    mp_now += 1;
                }
                add_mp_charge_to_mp ();
                set_anime_para (1);
                reset_bullet();
                ec. create_effect (13, !direction, gameObject, new Vector2((direction ? -0.1f : 0.1f), -0.15f), this, 0);
            }
        }
        else if (is_anime_now_name ("replace2"))
        {
            attack_light_lock = true;
            jump_lock = true;
            roll_lock = true;
            item_lock = true;
            scroll_lock = true;
            if (get_anime_normalized_time () > 0.25 && once_in_animation())
            {
                if(mp_now < mp_max)
                {
                    mp_now += 1;
                }
                add_mp_charge_to_mp ();
                set_anime_para (1);
                reset_bullet();
                ec. create_effect (12, !direction, gameObject, new Vector2(0, 0), this, 0);
                if(last_attack!=null){
                    sc.create_spell (5, this, new Vector2 (last_attack.transform.position.x,last_attack.transform.position.y), 0, 0);
                }
            }
        }
        return false;
    }
    private bool air_attack_control ()
    {
        if (is_anime_now_name ("air_attack1"))
        {
            jump_lock = !is_air_jumped;
            attack_light_lock = true;
            hit_floor_lock = true;
            item_lock = true;
            set_anime_para (-1);
            skill (-1);
        }
        else if (is_anime_now_name ("air_attack2"))
        {
            jump_lock = !is_air_jumped;
            attack_light_lock = true;
            attack_heavy_lock = true;
            hit_floor_lock = true;
            item_lock = true;
            set_anime_para (-1);
            skill (-1);
        }
        else if (is_anime_now_name ("air_attack3"))
        {
            jump_lock = !is_air_jumped;
            hit_floor_lock = true;
            item_lock = true;
            set_anime_para (-1);
            skill (-1);
        }
        else if (is_anime_now_name ("air_attack0-1"))
        {
            if (get_anime_normalized_time () < 0.4)
            {
            }
            else
            {
                set_speed (rb.velocity.x, jump_speed * 0.3f);
                skill (5);
            }
        }
        else if (is_anime_now_name ("air_attack1-2"))
        {
            if (get_anime_normalized_time () < 0.34)
            {

                move(direction, speed * 0.1f);
            }
            else
            {
                set_speed (rb.velocity.x, jump_speed * 0.3f);
                skill (6);
            }
        }
        else if (is_anime_now_name ("air_attack2-3"))
        {
            is_air_attacked = true;
            if (get_anime_normalized_time () < 0.4)
            {
                move(direction, speed * 0.1f);
                set_speed (rb.velocity.x, 0);
            }
            else
            {
                move(direction, speed);
                set_speed (rb.velocity.x, 0);
                skill (7);
            }
        }
        else if (is_anime_now_name ("air_spell1_1"))
        {
            move(direction, speed * 0.2f);
            set_speed (rb.velocity.x,  jump_speed * 0.3f);
            if (get_anime_normalized_time () < 0.6)
            {
                skill (16);
            }
            set_anime_para (-1);
        }
        else if (is_anime_now_name ("air_spell1_2"))
        {
            bc.create_buff(101, this, this, 5, 0);
            set_speed (rb.velocity.x, -jump_speed);
            if(!in_air)
            {
                set_anime_para (9);
            }
            if (get_anime_normalized_time () < 0.6 && timer % 8 == 0)
            {

                skill (16);
            }
            else
            {
                skill (-1);
            }

        }
        else if (is_anime_now_name ("air_spell1_3"))
        {
            if(once_in_animation())
            {
                skill (17);
                create_smoke(2);
            }
            else
            {
                skill(-1);
            }
        }
        else if (is_anime_now_name ("air_replace"))
        {
            hit_floor_lock = true;
            add_mp_charge_to_mp ();
        }
        return false;
    }
    private bool item_control ()
    {
        if (is_anime_now_name ("item1_1"))
        {
            if (get_anime_normalized_time () > 0.5 && once_in_animation())
            {
                sc.create_spell (1, this, new Vector2 ((direction ? 1 : -1), 0), 0, 0);
            }

        }
        else if (is_anime_now_name ("item1_2"))
        {
            set_anime_para (1);

        }
        else if (is_anime_now_name ("item2_1"))
        {
            if (get_anime_normalized_time () > 0.5 && once_in_animation())
            {
                sc.create_spell (1, this, new Vector2 ((direction ? 1 : -1), 0.5f), 0, 0);
            }
        }
        else if (is_anime_now_name ("item2_2"))
        {
            set_anime_para (1);

        }
        else if (is_anime_now_name ("air_item1_1"))
        {
            if (get_anime_normalized_time () > 0.5)
            {
                if (once_in_animation())
                {
                    sc.create_spell (1, this, new Vector2 ((direction ? 1 : -1), 0), 0, 0);
                }
                if (get_anime_normalized_time () < 2.0)
                {
                    set_speed (rb.velocity.x, jump_speed / 8);
                }
            }
        }
        else if (is_anime_now_name ("air_item1_2"))
        {
            hit_floor_lock = true;
            set_anime_para (-1);

        }
        else if (is_anime_now_name ("air_item2_1"))
        {
            if (get_anime_normalized_time () < 0.2)
            {
                set_speed (-speed * (direction ? 1 : -1) * 0.5f, jump_speed * 0.8f);
            }
            if (get_anime_normalized_time () > 0.7)
            {
                if (once_in_animation())
                {
                    sc.create_spell (1, this, new Vector2 ((direction ? 1 : -1), -1), 0, 0);
                    sc.create_spell (1, this, new Vector2 ((direction ? 1 : -1), -0.67f), 0, 0);
                    sc.create_spell (1, this, new Vector2 ((direction ? 1 : -1), -0.41f), 0, 0);
                }
                if (get_anime_normalized_time () < 0.8)
                {
                    set_speed (rb.velocity.x, 0);
                }
            }
            set_collider (1);
        }
        else if (is_anime_now_name ("air_item2_2"))
        {
            hit_floor_lock = true;
            set_anime_para (-1);

        }
        else if (is_anime_now_name ("climb_item1_1"))
        {
            rb.gravityScale = 0;
            set_speed (0, 0);
            if (get_anime_normalized_time () > 0.5)
            {
                if (once_in_animation())
                {
                    if (keydown_w)
                    {
                        sc.create_spell (1, this, new Vector2 ((direction ? -1 : 1), 0), 0, 0);
                    }
                    else
                    {
                        sc.create_spell (1, this, new Vector2 ((direction ? -1 : 1), -0.4f), 0, 0);
                    }
                }
            }
        }
        else if (is_anime_now_name ("climb_item1_2"))
        {
            rb.gravityScale = 0;
            set_speed (0, 0);
            hit_floor_lock = true;
            set_anime_para (-1);
            if (!(moving_control_valid &&  keydown_d == direction))
            {
                set_anime_para (5);
                return true;
            }
        }
        else if (is_anime_now_name ("climb_item2_1"))
        {
            rb.gravityScale = 0;
            set_speed (0, 0);
            if (get_anime_normalized_time () > 0.5)
            {
                if (once_in_animation())
                {
                    sc.create_spell (1, this, new Vector2 ((direction ? -1 : 1), -1), 0, 0);
                }
            }
        }
        else if (is_anime_now_name ("climb_item2_2"))
        {
            rb.gravityScale = 0;
            set_speed (0, 0);
            hit_floor_lock = true;
            set_anime_para (-1);
            if (!(moving_control_valid && keydown_d == direction))
            {
                set_anime_para (5);
                return true;
            }

        }
        else if (is_anime_now_name ("scroll1_1"))
        {
            if(get_anime_normalized_time () < 0.5)
            {
                move (direction, speed * 1.2f);
            }
            if (get_anime_normalized_time () > 0.5 && once_in_animation())
            {
                if(rune_now > 100 && is_scroll_ready)
                {
                    sc.create_spell (rune_now, this);
                }
                else
                {
                    sc.create_spell (100, this);
                }
                set_scroll_write_lock(false);
            }
            set_anime_para (-1);
        }
        else if (is_anime_now_name ("scroll1_2"))
        {
            set_anime_para (1);
        }
        return false;
    }
    private bool player_control ()
    {
        //move
        if (move_lock && moving_control_valid)
        {
            move (keydown_d);
        }
        //turn
        if (turn_lock && moving_control_valid && direction != keydown_d)
        {
            turn ();
        }
        //hit_floor
        if(in_air)
        {
            if(max_height < transform.position.y)
            {
                max_height = transform.position.y;
            }
        }
        if (hit_floor_lock && !in_air)
        {
            float fall_height = max_height - transform.position.y;
            max_height = transform.position.y;
            if (!in_air)
            {
                set_anime_para (7);
                return true;
            }
        }
        //roll
        if (roll_lock && (keydown_shift || keyboard_cache == KeyCode.LeftShift) && mp_cost_flash())
        {
            if(moving_control_valid && direction != keydown_d)
            {
                turn ();
            }
            set_anime_para (3);
            return true;
        }
        //attack
        if (attack_light_lock && (keydown_j || keyboard_cache == KeyCode.J))
        {
            if(moving_control_valid && direction != keydown_d)
            {
                turn ();
            }
            set_anime_para (8);
            return true;
        }
        if (attack_heavy_lock && (keydown_k || keyboard_cache == KeyCode.K))
        {
            if(moving_control_valid && direction != keydown_d)
            {
                turn ();
            }
            if(keydown_s)
            {
                set_anime_para (18);

            }
            else
            {
                set_anime_para (9);

            }
            return true;
        }
        //use_item
        if (item_lock && (keydown_i || keyboard_cache == KeyCode.I))
        {
            if(keyboard_cache == KeyCode.I)
            {
                keyboard_cache = KeyCode.None;
            }
            bool use_item_sec = false;
            if (keydown_s)
            {
                if(bullet_cost(3))
                {
                    use_item_sec = true;
                    if(is_climb_jump)
                    {
                        set_anime_para (16);
                    }
                    else
                    {
                        set_anime_para (17);
                    }
                }
            }
            else if(keydown_w)
            {
                if(bullet_cost(1))
                {
                    use_item_sec = true;
                    if(is_climb_jump)
                    {
                        set_anime_para (15);
                    }
                    else if(in_air)
                    {
                        set_anime_para (14);
                    }
                    else
                    {
                        set_anime_para (13);
                    }
                }
            }
            else
            {
                if(bullet_cost(1))
                {
                    use_item_sec = true;
                    if(is_climb_jump)
                    {
                        set_anime_para (15);
                    }
                    else if(in_air)
                    {
                        set_anime_para (14);
                    }
                    else
                    {
                        set_anime_para (11);
                    }
                }
            }
            if(use_item_sec)
            {
                return true;
            }
        }
        //use_scroll
        if (scroll_lock && (keydown_u || keyboard_cache == KeyCode.U) && mp_cost_test())
        {
            if(keyboard_cache == KeyCode.U)
            {
                keyboard_cache = KeyCode.None;
            }
            if(moving_control_valid && direction != keydown_d)
            {
                turn ();
            }
            set_anime_para (12);
            return true;
        }
        //jump
        if (jump_lock && (Input.GetKeyDown (KeyCode.Space) || keyboard_cache == KeyCode.Space))
        {
            set_anime_para (4);
            if (in_air)
            {
                is_air_jump = true;
                if (!is_climb_jump)
                {
                    is_air_jumped = true;
                }
                else
                {
                    turn();
                }
            }
            else
            {
                is_air_jump = false;
            }
            return true;
        }
        //walk
        if (walk_lock)
        {
            if (moving_control_valid && direction == keydown_d)
            {
                set_anime_para (2);
            }
            else
            {
                set_anime_para (1);
            }
            return true;
        }
        if(turn_walk_lock && moving_control_valid && direction != keydown_d)
        {
            set_anime_para (2);
            turn();
            return true;
        }

        return false;
    }
    private void scroll_control()
    {
        if(rune_now < 100 && scroll_write_lock)
        {
            int rune_next = 0;
            foreach(int i in scroll_dict[rune_now])
            {
                switch(i)
                {
                case 1:
                    if (Input.GetKeyDown (KeyCode.Alpha1))
                    {
                        rune_next = 1;
                    }
                    break;
                case 2:
                    if (Input.GetKeyDown (KeyCode.Alpha2))
                    {
                        rune_next = 2;
                    }
                    break;
                case 3:
                    if (Input.GetKeyDown (KeyCode.Alpha3))
                    {
                        rune_next = 3;
                    }
                    break;
                case 4:
                    if (Input.GetKeyDown (KeyCode.Alpha4))
                    {
                        rune_next = 4;
                    }
                    break;
                }
            }
            if(rune_next != 0)
            {
                rune_now = rune_now * 10 + rune_next;
            }
        }

    }
    private void keyboard_listen ()
    {
        if (Input.GetKeyDown (KeyCode.W))
        {
            keydown_w = true;
            keyboard_cache = KeyCode.W;
            keyboard_cache_timer = 0;
        }
        if (Input.GetKeyUp (KeyCode.W))
        {
            keydown_w = false;
        }
        if (Input.GetKeyDown (KeyCode.A))
        {
            keydown_a = true;
            keyboard_cache = KeyCode.A;
            keyboard_cache_timer = 0;
        }
        if (Input.GetKeyUp (KeyCode.A))
        {
            keydown_a = false;
        }
        if (Input.GetKeyDown (KeyCode.S))
        {
            keydown_s = true;
            keyboard_cache = KeyCode.S;
            keyboard_cache_timer = 0;
        }
        if (Input.GetKeyUp (KeyCode.S))
        {
            keydown_s = false;
        }
        if (Input.GetKeyDown (KeyCode.D))
        {
            keydown_d = true;
            keyboard_cache = KeyCode.D;
            keyboard_cache_timer = 0;
        }
        if (Input.GetKeyUp (KeyCode.D))
        {
            keydown_d = false;
        }
        if (Input.GetKeyDown (KeyCode.J))
        {
            keydown_j = true;
            keyboard_cache = KeyCode.J;
            keyboard_cache_timer = 0;
        }
        if (Input.GetKeyUp (KeyCode.J))
        {
            keydown_j = false;
        }
        if (Input.GetKeyDown (KeyCode.K))
        {
            keydown_k = true;
            keyboard_cache = KeyCode.K;
            keyboard_cache_timer = 0;
        }
        if (Input.GetKeyUp (KeyCode.K))
        {
            keydown_k = false;
        }
        if (Input.GetKeyDown (KeyCode.I))
        {
            keydown_i = true;
            keyboard_cache = KeyCode.I;
            keyboard_cache_timer = 0;
        }
        if (Input.GetKeyUp (KeyCode.I))
        {
            keydown_i = false;
        }
        if (Input.GetKeyDown (KeyCode.U))
        {
            keydown_u = true;
            keyboard_cache = KeyCode.U;
            keyboard_cache_timer = 0;
        }
        if (Input.GetKeyUp (KeyCode.U))
        {
            keydown_u = false;
        }
        if (Input.GetKeyDown (KeyCode.LeftShift))
        {
            keydown_shift = true;
            keyboard_cache = KeyCode.LeftShift;
            keyboard_cache_timer = 0;
        }
        if (Input.GetKeyUp (KeyCode.LeftShift))
        {
            keydown_shift = false;
        }
        if (Input.GetKeyDown (KeyCode.Space))
        {
            keydown_space = true;
            keyboard_cache = KeyCode.Space;
            keyboard_cache_timer = 0;
        }
        if (Input.GetKeyUp (KeyCode.Space))
        {
            keydown_space = false;
        }

        moving_control_valid = !((keydown_d && keydown_a) || !(keydown_d || keydown_a));

        if(keyboard_cache != KeyCode.None)
        {
            keyboard_cache_timer++;
            if(keyboard_cache_timer > 50)
            {
                keyboard_cache = KeyCode.None;
            }
        }
        if (Input.GetKeyUp (KeyCode.Escape))
        {
            is_menu = !is_menu;
        }
        if(Input.GetKeyUp (KeyCode.P))
        {
            SceneManager.LoadScene("test", LoadSceneMode.Single);
        }

    }
    private void set_collider (int index_set)
    {
        if (index_set == 0)
        {
            against_wall_obj.SetActive (true);
            gameObject.layer = 0;
            is_attackable = true;
        }
        else
        {
            against_wall_obj.SetActive (false);
            gameObject.layer = 13;
            set_against_wall (false);
            is_attackable = false;
        }
    }
    private void refresh_scroll()
    {
        scroll_dict = new Dictionary<int, List<int>>();
        scroll_dict[0] = new List<int>();
        foreach(int code in scrolls)
        {
            int code1 = (int)(code / 100f);
            int code2 = (int)((code % 100) / 10f);
            int code3 = code % 10;

            int code4 = (int)(code / 10f);

            if(!scroll_dict.ContainsKey(code1))
            {
                scroll_dict[code1] = new List<int>();
            }
            if(!scroll_dict.ContainsKey(code4))
            {
                scroll_dict[code4] = new List<int>();
            }
            scroll_dict[0].Add(code1);
            scroll_dict[code1].Add(code2);
            scroll_dict[code4].Add(code3);
        }
    }
    public Dictionary<int, int> get_item_map ()
    {
        return item_map;
    }
    public void learn_scroll(int code)
    {
        scrolls.Add(code);
        refresh_scroll();
    }
    public List<int> get_item_code_list ()
    {
        return item_code_list;
    }
    public Dictionary<int, List<int>> get_scroll_dict()
    {
        return scroll_dict;
    }
    public int get_rune_now()
    {
        return rune_now;
    }
    public int get_item_index_now ()
    {
        return item_index_now;
    }
    public List<int> get_scrolls()
    {
        return scrolls;
    }
    public int get_mp_charge_max ()
    {
        return mp_charge_max;
    }
    public int get_mp_charge_now ()
    {
        return mp_charge_now;
    }
    public int get_mp_max ()
    {
        return mp_max;
    }
    public int get_mp_now ()
    {
        return mp_now;
    }
    public int get_bullet_max()
    {
        return bullet_max;
    }
    public int get_bullet_now()
    {
        return bullet_now;
    }
    private void reset_bullet()
    {
        bullet_now = bullet_max;
    }
    public bool get_is_menu()
    {
        return is_menu;
    }
    public void set_is_menu(bool is_menu_set)
    {
        is_menu = is_menu_set;
    }
    public void set_scroll_ready(bool is_scroll_ready_set)
    {
        is_scroll_ready = is_scroll_ready_set;
    }
    public override void set_scroll_hit()
    {
        rune_now = 0;
        is_scroll_ready = false;
        mp_cost();
    }
    public void set_scroll_write_lock(bool scroll_write_lock_set)
    {
        scroll_write_lock = scroll_write_lock_set;
    }
    public override void hit_message2 (attack attack)
    {
        add_mp_charge_now (attack.damage);
    }
    private void add_mp_charge_now (int mp_charge_now_set)
    {
        mp_charge_now += mp_charge_now_set;
        if (mp_charge_now > mp_charge_max)
        {
            mp_charge_now = mp_charge_max;
        }
    }
    private bool mp_cost ()
    {
        if (mp_charge_now == mp_charge_max)
        {
            mp_charge_now = 0;
            return true;
        }
        else if(mp_now > 0)
        {
            mp_now -= 1;
            return true;
        }
        return false;
    }
    private bool bullet_cost(int num)
    {
        if(bullet_now >= num)
        {
            bullet_now -= num;
            return true;
        }
        return false;
    }
    private bool mp_cost_test ()
    {
        if (mp_charge_now == mp_charge_max)
        {
            return true;
        }
        else if(mp_now > 0)
        {
            return true;
        }

        return false;
    }
    private bool mp_cost_flash()
    {
        if (mp_charge_now > 0)
        {
            mp_charge_now = 0;
            return true;
        }
        return false;

    }
    private void add_mp_charge_to_mp ()
    {
        if (mp_charge_now == mp_charge_max && mp_now < mp_max)
        {
            mp_charge_now = 0;
            mp_now++;
        }
    }
}