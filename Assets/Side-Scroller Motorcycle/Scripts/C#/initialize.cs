using UnityEngine;
using System.Collections;

public class initialize : MonoBehaviour {
    
    public GameObject rider, bike;
    public Material[] riderMat, bikeMat;

    void Awake()
    {
        foreach (Transform child in gameObject.transform)
        {
            if (child.tag == "menuGUI")
                child.gameObject.SetActive(false);

            rider.renderer.material = riderMat[PlayerPrefs.GetInt("riderSkin")];
            bike.renderer.material = bikeMat[PlayerPrefs.GetInt("bikeSkin")];
        }
    }	
}
