using UnityEngine;
using System.Collections;

public class customizationMenu : MonoBehaviour
{

    public float buttonX, xOffset, yOffset, buttonY, buttonWidth, buttonHeight,commitButtonX,commitButtonY;
    public Material[] bikeSkins, riderSkins;
    public GameObject bike, rider,_selectionMenu;
    private int inc,incRider,incBike;
    public GUISkin uiSkin;
    public Texture2D leftTex, rightTex, commitTex, bikeCustomTex, riderCustomTex;
    public bool customize;
    public bool changeSkinBike, changeSkinRider,animateCamRight,animateCamLeft;
    public float _angle, _stopAtAngle;
    public Camera _mainCam;
    public Light lightGarage, lightRider, lightBike;
    private selectionMenu _selMenu;

    void Start()
    {
        _selMenu = _selectionMenu.GetComponent<selectionMenu>();
        animateCamRight = false;
        animateCamLeft = false;
        changeSkinBike = false;
        changeSkinRider = true;
        inc = 0;
        incRider = 0;
        incBike = 0;
        incRider = PlayerPrefs.GetInt("riderSkinIndex");
        incBike = PlayerPrefs.GetInt("bikeSkinIndex");
    }

    void FixedUpdate() {

        //Rotates the main camera to left
        if (animateCamRight)
        {
            if (_angle != _stopAtAngle)
                _mainCam.transform.Rotate(Vector3.up, _angle++);
        }

        //Rotates the main camera to left
        if (animateCamLeft)
        {
            //Debug.Log(angle+""+stopAtAngle);
            if (_angle != _stopAtAngle)
                _mainCam.transform.Rotate(Vector3.up, _angle--);
        }
    }

    void OnGUI()
    {
        float screenX = (float)Screen.width / 800;
        float screenY = (float)Screen.height / 400;
        GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(screenX, screenY, 1f));

        GUI.skin = uiSkin;

        if (customize)
        {
            GUI.backgroundColor = Color.clear;

                //Rider customization menu button on the left button
                if (changeSkinBike == true)
                {
                    GUI.DrawTexture(new Rect(buttonX, buttonY, buttonWidth, buttonHeight), riderCustomTex);
                    if (GUI.Button(new Rect(buttonX, buttonY, buttonWidth, buttonHeight), ""))
                    {
                        _mainCam.GetComponent<audioManager>().playAudio("buttonHit");
                        changeSkinRider=true;
                        //_mainCam.transform.Rotate(Vector3.up, -18);
                        animateCamRight= false;
                        _angle = 0;
                        _stopAtAngle = -7;
                        animateCamLeft = true;
                        lightRider.gameObject.SetActive(true);
                        lightBike.gameObject.SetActive(false);
                        changeSkinBike=false;
                    }
                }

                //Bike customization menu button on the right 
                if (changeSkinRider == true)
                {
                    GUI.DrawTexture(new Rect(buttonX + xOffset, buttonY, buttonWidth, buttonHeight), bikeCustomTex);
                    if (GUI.Button(new Rect(buttonX + xOffset, buttonY, buttonWidth, buttonHeight), ""))
                    {
                        _mainCam.GetComponent<audioManager>().playAudio("buttonHit");
                        changeSkinRider = false;
                        //_mainCam.transform.Rotate(Vector3.up, 18);
                        animateCamLeft = false;
                        _angle = 0;
                        _stopAtAngle = 7;
                        animateCamRight = true;
                        lightBike.gameObject.SetActive(true);
                        lightRider.gameObject.SetActive(false);
                        changeSkinBike = true;
                    }
                }

                //Cycle Left button
                GUI.DrawTexture(new Rect(buttonX, buttonY + yOffset, buttonWidth, buttonHeight), leftTex);
                if (GUI.Button(new Rect(buttonX, buttonY + yOffset, buttonWidth, buttonHeight), ""))
                {
                    _mainCam.GetComponent<audioManager>().playAudio("buttonHit");

                    if(changeSkinBike){
                        incBike--;
                        if (incBike < 0)
                        {
                            incBike = (int)bikeSkins.Length - 1;
                        }      
                    }

                    /*
                    inc--;
                    if (inc < 0)
                        inc = (int)bikeSkins.Length - 1;

                    if (changeSkinRider)
                    {
                        rider.renderer.material = riderSkins[inc];
                    }
                    else
                    {
                        bike.renderer.material = bikeSkins[inc];
                    }*/
                }


                //Back  button -Commits the customizations
                GUI.DrawTexture(new Rect(commitButtonX, commitButtonY, buttonWidth, buttonHeight), commitTex);
                if (GUI.Button(new Rect(commitButtonX, commitButtonY, buttonWidth, buttonHeight), "")) 
                {
                    //Animate main camera
                    _mainCam.GetComponent<audioManager>().playAudio("buttonHit");
                    if (changeSkinRider == true)
                    {
                        animateCamLeft = false;
                        _angle = 0;
                        _stopAtAngle = 6;
                        animateCamRight = true;
                        //_selMenu.customized = true;
                    }
                    if (changeSkinBike)
                    {
                        animateCamRight = false;
                        _angle = 0;
                        _stopAtAngle = -6;
                        animateCamLeft = true;
                        //_selMenu.customized = true;
                    }
                        
                    customize = false;
                   // _selMenu.customized = true;
                    _selMenu.activateButtons = true;
                    _selMenu.lightGarage.gameObject.SetActive(true);
                    _selMenu.lightBike.gameObject.SetActive(false);
                    _selMenu.lightRider.gameObject.SetActive(false);
                }
            

                //Cycle Right button
                GUI.DrawTexture(new Rect(buttonX + xOffset, buttonY + yOffset, buttonWidth, buttonHeight), rightTex);
                if (GUI.Button(new Rect(buttonX + xOffset, buttonY + yOffset, buttonWidth, buttonHeight), ""))
                {
                    _mainCam.GetComponent<audioManager>().playAudio("buttonHit");
                    inc++;
                    if (inc >= bikeSkins.Length)
                        inc = 0;
                    if (changeSkinRider)
                    {
                       rider.renderer.material = riderSkins[inc];
                    }
                    else
                    {
                        bike.renderer.material = bikeSkins[inc];
                    }
                }
                GUI.skin = null;    
        }
    }
}
