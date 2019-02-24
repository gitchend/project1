using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class camera_controller : MonoBehaviour
{
    private int reletive_rate;
    private GameObject target;

    private GameObject ui_group1;
    private GameObject ui_group2;
    private GameObject ui_group3;

    private List<background_layer> layers = new List<background_layer>();
    private GameObject layer1;
    private GameObject layer2;
    private GameObject layer3;
    private GameObject layer4;

    private GameObject background_mask;
    private int screen_width;
    private int screen_height;
    private float img_x;
    private float img_y;

    private GameObject image_obj;
    private GameObject image_obj_light;

    void Start()
    {
        target = GameObject.Find("hero").transform.Find("sprite").gameObject;
        ui_group1 = GameObject.Find("ui_group1");
        ui_group2 = GameObject.Find("ui_group2");
        ui_group3 = GameObject.Find("ui_group3");

        layer1 = GameObject.Find("layer1");
        layer2 = GameObject.Find("layer2");
        layer3 = GameObject.Find("layer3");
        layer4 = GameObject.Find("layer4");
        if (layer1 != null)
        {
            background_layer background_layer = new background_layer();
            background_layer.layer_obj = layer1;
            background_layer.set = layer1.GetComponent<background_set>();
            background_layer.speed_rate = 0.95f;
            background_layer.is_light = false;
            layers.Add(background_layer);
        }
        if (layer2 != null)
        {
            background_layer background_layer = new background_layer();
            background_layer.layer_obj = layer2;
            background_layer.set = layer2.GetComponent<background_set>();
            background_layer.speed_rate = 0.8f;
            background_layer.is_light = false;
            layers.Add(background_layer);
        }
        if (layer3 != null)
        {
            background_layer background_layer = new background_layer();
            background_layer.layer_obj = layer3;
            background_layer.set = layer3.GetComponent<background_set>();
            background_layer.speed_rate = 0.75f;
            background_layer.is_light = true;
            layers.Add(background_layer);
        }
        if (layer4 != null)
        {
            background_layer background_layer = new background_layer();
            background_layer.layer_obj = layer4;
            background_layer.set = layer4.GetComponent<background_set>();
            background_layer.speed_rate = 0.55f;
            background_layer.is_light = true;
            layers.Add(background_layer);
        }

        background_mask = GameObject.Find("background_mask");

        image_obj = Resources.Load("img_obj") as GameObject;
        image_obj_light = Resources.Load("img_obj_light") as GameObject;
        refresh_screen_size();
    }

    // Update is called once per frame
    void Update()
    {
        focous_target();
        adjust_ui();
        adjust_background();
        adjust_mask();
    }
    private void focous_target()
    {
        float target_x = target.transform.position.x;
        float target_y = target.transform.position.y+0.5f;

        transform.position = new Vector3(target_x, target_y, transform.position.z);
    }
    private void adjust_ui()
    {
        float ui_group1_x = transform.position.x - screen_width * 1.0f / reletive_rate / 16 * 7 / 64;
        float ui_group1_y = transform.position.y + screen_height * 1.0f / reletive_rate / 12 * 5 / 64;

        float ui_group2_x = transform.position.x - screen_width * 1.0f / reletive_rate / 16 * 7 / 64;
        float ui_group2_y = transform.position.y - screen_height * 1.0f / reletive_rate / 12 * 5 / 64;

        ui_group1.transform.position = new Vector3(ui_group1_x, ui_group1_y, ui_group1.transform.position.z);
        ui_group2.transform.position = new Vector3(ui_group2_x, ui_group2_y, ui_group2.transform.position.z);
        //ui_group3.transform.position = new Vector3 (transform.position.x, transform.position.y, ui_group3.transform.position.z);
    }
    private void adjust_background()
    {
        foreach (background_layer layer in layers)
        {
            move_img(layer);
            remove_invisible_img(layer);
            add_new_img(layer);
        }

        background_mask.transform.position = new Vector3(transform.position.x, transform.position.y, background_mask.transform.position.z);
    }
    private void move_img(background_layer layer)
    {
        Vector2 position_miu = get_position_miu(layer.speed_rate, layer.set);
        layer.layer_obj.transform.position=new Vector3(position_miu.x+layer.set.position_x,position_miu.y+layer.set.position_y,layer.layer_obj.transform.position.z);
        adjust_pixel(layer.layer_obj);
        // foreach (string key in layer.img_map.Keys)
        // {
        //     string[] index = key.Split('_');
        //     int index_x = int.Parse(index[0]);
        //     int index_y = int.Parse(index[1]);
        //     layer.img_map[key].transform.parent.position = new Vector3(index_x * img_x + position_miu.x, index_y * img_y + position_miu.y, layer.img_map[key].transform.parent.position.z);
        //     adjust_pixel(layer.img_map[key]);
        // }
    }
    private void remove_invisible_img(background_layer layer)
    {
        for (int i = layer.img_map.Count - 1; i >= 0; i--)
        {
            var element = layer.img_map.ElementAt(i);
            if (!is_img_visible(element.Value))
            {
                layer.img_map.Remove(element.Key);

                Destroy(element.Value);
            }
        }
    }
    private void add_new_img(background_layer layer)
    {
        Vector2 position_miu = get_position_miu(layer.speed_rate, layer.set);

        float visible_x_left = transform.position.x - (screen_width / 64.0f + img_x) / 2;
        float visible_x_right = visible_x_left + (screen_width / 64.0f + img_x);
        float visible_y_down = transform.position.y - (screen_height / 64.0f + img_y) / 2;
        float visible_y_up = visible_y_down + (screen_height / 64.0f + img_y);

        int x_low = (int)((visible_x_left - position_miu.x) / img_x);
        int x_high = (int)((visible_x_right - position_miu.x) / img_x);
        int y_low = (int)((visible_y_down - position_miu.y) / img_y);
        int y_high = (int)((visible_y_up - position_miu.y) / img_y);

        for (int i = x_low; i <= x_high; i++)
        {
            for (int j = y_low; j <= y_high; j++)
            {
                string key = i + "_" + j;
                if (!layer.img_map.ContainsKey(key))
                {
                    GameObject obj = Instantiate((layer.is_light ? image_obj_light : image_obj)) as GameObject;
                    obj.transform.parent = layer.layer_obj.transform;
                    obj.transform.localPosition = new Vector3(i * img_x, j * img_y, 0);
                    if (j == 0)
                    {
                        if (layer.set.imgs[0] != null)
                        {
                            obj.GetComponent<SpriteRenderer>().sprite = layer.set.imgs[0];
                        }
                    }
                    else if (j > 0)
                    {
                        if (layer.set.imgs[1] != null)
                        {
                            obj.GetComponent<SpriteRenderer>().sprite = layer.set.imgs[1];
                        }
                    }
                    else
                    {
                        if (layer.set.imgs[2] != null)
                        {
                            obj.GetComponent<SpriteRenderer>().sprite = layer.set.imgs[2];
                        }
                    }
                    layer.img_map[key] = obj;
                }
            }
        }
    }
    private void adjust_mask()
    {

    }
    private Vector2 get_position_miu(float speed_rate, background_set img_set)
    {
        Vector2 position_miu = transform.position * speed_rate;
        position_miu = new Vector2(position_miu.x + img_set.position_x, position_miu.y + img_set.position_y);
        return position_miu;
    }
    private bool is_img_visible(GameObject obj)
    {
        return obj.GetComponent<SpriteRenderer>().isVisible;
    }
    protected void adjust_pixel(GameObject obj)
    {
        obj.transform.position = new Vector3(pixel_fix(obj.transform.position.x), pixel_fix(obj.transform.position.y), obj.transform.position.z);
    }
    private float pixel_fix(float float_num)
    {
        return (int)(float_num * 64) / 64.0f;
    }
    public void refresh_screen_size()
    {
        screen_width = Screen.width;
        screen_height = Screen.height;
        int width_rate = (int)(screen_width / 512.0f);
        int height_rate = (int)(screen_height / 288.0f);
        reletive_rate = (width_rate < height_rate ? width_rate : height_rate);
        img_x = 1024 / 64.0f;
        img_y = 576 / 64.0f;
        background_mask.transform.localScale = new Vector2(screen_width * 1.0f / reletive_rate, screen_height * 1.0f / reletive_rate);
        background_mask.GetComponent<SpriteRenderer>().color = new Color(0, 1, 1, 0);
    }
}