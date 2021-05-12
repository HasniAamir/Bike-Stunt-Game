using UnityEngine;
using System.Collections;

public class lobbyMenu : MonoBehaviour {

    public float btnX, btnY, btnWidth, btnHeight, btnOffsetX, btnOffsetY;
    public float angle, stopAngle;
    public Camera _mainCam;
    public GameObject _customizationMenu,_levelSelectionMenu,_mainMenu;
    public Light lightGarage,lightRider,lightBike;
    public bool enableLobbyGUI,animateCamRight;

    void Start()
    {
        animateCamRight = false;
        angle = 0;
    }
    void FixedUpdate()
    {
        //Animate the main camera to the right
        if (animateCamRight)
        {
            if (angle != stopAngle)
                _mainCam.transform.Rotate(Vector3.up, angle++);
        }
    }

    void OnGUI()
    {
        float screenX = Screen.width / 800f;
        float screenY = Screen.height / 400f;
        GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(screenX, screenY, 1f));

        if (enableLobbyGUI)
        {
            if (GUI.Button(new Rect(btnX, btnY, btnWidth, btnHeight), "Next"))
            {
                Time.timeScale = 0f;
                _mainCam.GetComponent<audioManager>().playAudio("buttonHit");
                lightGarage.gameObject.SetActive(false);
                this.gameObject.SetActive(false);
                _levelSelectionMenu.SetActive(true);
            }

            if (GUI.Button(new Rect(btnX + btnOffsetX, btnY, btnWidth, btnHeight), "Customize"))
            {
                lightRider.gameObject.SetActive(true);
                lightGarage.gameObject.SetActive(false);
                gameObject.SetActive(false);
                _customizationMenu.SetActive(true);
                _customizationMenu.GetComponent<customizationMenuV2>().changeRiderSkin = true;
            }

            if (GUI.Button(new Rect(btnX + (btnOffsetX * 2), btnY, btnWidth, btnHeight), "Back"))
            {
                animateCamRight = true;
                _mainCam.GetComponent<audioManager>().playAudio("buttonHit");
                _mainMenu.GetComponent<PlayButtonClick>().angle = 0;
                PlayButtonClick.animateCamLeft = false;
                enableLobbyGUI = false;
            }
        }
    }
}
