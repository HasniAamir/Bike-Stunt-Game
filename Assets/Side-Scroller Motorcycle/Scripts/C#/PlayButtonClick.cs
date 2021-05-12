using UnityEngine;
using System.Collections;

public class PlayButtonClick : MonoBehaviour {

	public Texture2D NormalTexture;
	public Texture2D PressedTexture;
    public GameObject lobby;
    //public GameObject selectionScreen;
    public Camera _mainCamera;
    public float angle, stopAtAngle;
    public static bool animateCamLeft;
    //private selectionMenu _selectionScr;
    private lobbyMenu _lobbyMenu;

	// Use this for initialization
	void Start () {
		renderer.material.mainTexture = NormalTexture;
        //_selectionScr = selectionScreen.GetComponent<selectionMenu>();
        //selectionScreen.SetActive(false);
        _lobbyMenu = lobby.GetComponent<lobbyMenu>();
        animateCamLeft = false;
        angle = 0;
    }
	
	// Update is called once per frame
	void FixedUpdate() {
        if (animateCamLeft)
        {
            if (angle != stopAtAngle)
            {
                _mainCamera.transform.Rotate(Vector3.up, angle--);
                Debug.Log(angle);
            }
        }
    }

	void OnMouseDown(){
		renderer.material.mainTexture = PressedTexture;
        _mainCamera.GetComponent<audioManager>().playAudio("buttonHit");
    }

	void OnMouseUp(){

		renderer.material.mainTexture = NormalTexture;
		//GameObject.FindGameObjectWithTag ("bg").renderer.enabled = true;
		//GameObject.FindGameObjectWithTag ("loading").renderer.enabled = true;
        //Application.LoadLevel(1);
        animateCamLeft = true;
        //_selectionScr.customized = false;
        lobby.SetActive(true);
   
        Invoke("activateSelectionScreen", 0.3f);
	}

    void activateSelectionScreen() {
        //selectionScreen.SetActive(true);
        //_selectionScr.animateCam = false;
        //selectionMenu.animateCam = false;
        //_selectionScr.angle = 0;
        //_selectionScr.activateButtons = true;
        _lobbyMenu.enableLobbyGUI = true;
        _lobbyMenu.angle = 0;
        _lobbyMenu.animateCamRight = false;
    }
}
