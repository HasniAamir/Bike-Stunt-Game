using UnityEngine;
using System.Collections;

public class selectionMenu: MonoBehaviour {

    public float buttonX, buttonY, buttonWidth, buttonHeight, buttonOffsetX, buttonOffsetY, stopAtAngle, angle, customAngle;
    public bool animateCam,activateButtons = true,animateRiderCam=false, customized;
    public Camera _mainCam;
    public GameObject mainMenuRef,_customizationMenu,_levelSelecitonMenu;
    public Light lightGarage, lightRider, lightBike;
    private PlayButtonClick _mainMenu;

    void Start() {
        customized = false;
        angle = 0;
        _mainMenu = mainMenuRef.GetComponent<PlayButtonClick>();
    }

    void FixedUpdate() {
        //Animates cam to left
        if (animateCam) {
            if(angle != stopAtAngle)
            _mainCam.transform.Rotate(Vector3.up, angle++);
        }

        //Animates cam to right
        if(animateRiderCam)
            {
                if (angle != stopAtAngle)
                    _mainCam.transform.Rotate(Vector3.up, angle--);   
            }

       
    }

/*    void Update()
    {
        if (customized)
        {
            _mainCam.transform.Rotate(0f, customAngle, 0f);
            customized = false;
        }
    }*/

    void OnGUI() {
        float screenX = Screen.width / 800f;
        float screenY = Screen.height / 400f;
        GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(screenX,screenY, 1f));
        
        
        if (activateButtons)
        {
            //Play button -Takes to the level selection screen
            if (GUI.Button(new Rect(buttonX,buttonY, buttonWidth, buttonHeight), "Next"))
            {
                _mainCam.GetComponent<audioManager>().playAudio("buttonHit");
                lightGarage.gameObject.SetActive(false);
                this.gameObject.SetActive(false);
                _levelSelecitonMenu.SetActive(true);
            }

            //Customize button -Takes to the player and vehicel customization menu
            if(GUI.Button(new Rect(buttonX+buttonOffsetX,buttonY, buttonWidth,buttonHeight),"Customize"))
            {
                _mainCam.GetComponent<audioManager>().playAudio("buttonHit");
                lightBike.gameObject.SetActive(true);
                angle = 0;
                stopAtAngle = -6;
                animateRiderCam = true;
                lightRider.gameObject.SetActive(true);
                lightBike.gameObject.SetActive(false);
                lightGarage.gameObject.SetActive(false);
                _customizationMenu.SetActive(true);
                /*
                _customizationMenu.GetComponent<customizationMenu>()._angle = 0;
                _customizationMenu.GetComponent<customizationMenu>()._stopAtAngle = 0;*/
                _customizationMenu.GetComponent<customizationMenu>().animateCamLeft = false;
                //_customizationMenu.GetComponent<customizationMenu>().animateCamRight = false;
                _customizationMenu.GetComponent<customizationMenu>().customize = true;
                activateButtons = false;
                //animateCam = false;   
            }

            //Back button -Takes back to main menu
            if (GUI.Button(new Rect(buttonX + (buttonOffsetX * 2), buttonY, buttonWidth, buttonHeight), "Back")) 
            {
                _mainCam.GetComponent<audioManager>().playAudio("buttonHit");
                animateCam = true;
                mainMenuRef.GetComponent<PlayButtonClick>().angle = 0;
                PlayButtonClick.animateCamLeft = false;
                activateButtons = false;
                //customized = true;
            }
        }
    }
}
