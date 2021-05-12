using UnityEngine;
using System.Collections;

public class initPlayer : MonoBehaviour {

    public GameObject rider;
    public GameObject bike;
    public Material[] riderSkins;
    public Material[] bikeSkins;

    void Awake()
    {
        Time.timeScale = 1f;
        rider.renderer.material = riderSkins[PlayerPrefs.GetInt("riderSkin")];
        bike.renderer.material = bikeSkins[PlayerPrefs.GetInt("bikeSkin")]; 
    }
}
