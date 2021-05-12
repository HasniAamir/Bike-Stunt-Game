using UnityEngine;
using System.Collections;

public class customizationMenuV2 : MonoBehaviour {

    public float buttonX, xOffset, yOffset, buttonY, buttonWidth, buttonHeight, commitButtonX, commitButtonY;
    public float bikeButtonX, bikeButtonY, bikeButtonWidth, bikeButtonHeight, riderButtonX, riderButtonY, riderButtonWidth, riderButtonHeight;
    public GUISkin uiSkin;
    public GameObject _lobby,lightRider,lightBike,lightGarage;
    public GameObject rider, bike;
    int bikeMatIndex=0, riderMatIndex=0;
    public Material[] riderMat, bikeMat;
    public bool changeBikeSkin, changeRiderSkin;

    void Awake() {
        bikeMatIndex = PlayerPrefs.GetInt("bikeSkin");
        riderMatIndex = PlayerPrefs.GetInt("riderSkin");
    }

    void Start() {
        changeRiderSkin = true;

        //rider.renderer.material = riderMat[];
    }

    void FixedUpdate()
    {
        if(changeBikeSkin)
            bike.renderer.material = bikeMat[bikeMatIndex];
        if(changeRiderSkin)
            rider.renderer.material = riderMat[riderMatIndex];
    }

    void OnGUI()
    {
        float scrX = Screen.width / 800f;
        float scrY = Screen.height / 400f;
        GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(scrX, scrY, 1f));
        GUI.skin = uiSkin;

        //GUI.backgroundColor = Color.clear;
        //Rider cutomization button
        if (!changeRiderSkin)
        {
            if (GUI.Button(new Rect(riderButtonX, riderButtonY, riderButtonWidth, riderButtonHeight), "Rider"))
            {
                changeRiderSkin = !changeRiderSkin;
                changeBikeSkin = false ;
                lightBike.SetActive(false);
                lightRider.SetActive(true);
            }
        }

        //Bike customization
        if (changeRiderSkin)
        {
                if (GUI.Button(new Rect(bikeButtonX, bikeButtonY, bikeButtonWidth, bikeButtonHeight), "Bike"))
                {
                    changeRiderSkin = !changeRiderSkin;
                    changeBikeSkin = true;
                    rider.renderer.material = riderMat[riderMatIndex];
                    lightBike.SetActive(true);
                    lightRider.SetActive(false);
                }
            }

            if (GUI.Button(new Rect(buttonX, buttonY, buttonWidth, buttonHeight), "Left"))
            {
                if (changeBikeSkin)
                {
                    bikeMatIndex--;
                    if (bikeMatIndex <= 0)
                        bikeMatIndex = 0;
                }
                    
                if (changeRiderSkin)
                {
                    riderMatIndex--;
                    if (riderMatIndex <= 0)
                        riderMatIndex = 0;
                }
            }

            if (GUI.Button(new Rect(buttonX + xOffset, buttonY, buttonWidth, buttonHeight), "Right"))
            {
                if (changeBikeSkin)
                {
                    bikeMatIndex++;
                    if (bikeMatIndex >= bikeMat.Length)
                        bikeMatIndex = bikeMat.Length-1;
                }

                    if (changeRiderSkin)
                    {
                        riderMatIndex++;
                        if (riderMatIndex >= riderMat.Length)
                            riderMatIndex = riderMat.Length-1;
                    }

                

            }

            if (GUI.Button(new Rect(buttonX + xOffset, buttonY + yOffset, buttonWidth, buttonHeight), "Back"))
            {
                lightGarage.SetActive(true);
                lightRider.SetActive(false);
                lightBike.SetActive(false);
                gameObject.SetActive(false);
                _lobby.SetActive(true);
                Debug.Log("Back btn pressed");
                PlayerPrefs.SetInt("bikeSkin", bikeMatIndex);
                PlayerPrefs.SetInt("riderSkin", riderMatIndex);
            }
        }


    int changeSkin(GameObject obj, Material[] matArray, int index)
    {
        index++;
        if (index < 0)
            index = 0;
        if (index >= matArray.Length)
            index = matArray.Length-1;
        obj.renderer.material = matArray[index];
        return index;
    }

}
