using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class background_layer{

    public GameObject layer_obj;
	public background_set set;
	public Dictionary<string, GameObject> img_map = new Dictionary<string, GameObject> ();
	public float speed_rate;
    public bool is_light;
    public GameObject pixel_basic;
    public Vector2 layer_position_unpixeled;

}
