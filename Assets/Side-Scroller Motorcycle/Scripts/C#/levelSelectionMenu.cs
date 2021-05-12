using UnityEngine;
using System.Collections;

public class levelSelectionMenu : MonoBehaviour {

    public float buttonX, buttonY, buttonWidth, buttonHeight, buttonOffsetX, buttonArrowX, buttonArrowY;
    public Camera _mainCam;
    public GameObject _selectionMenu;
    public Light lightGarage;
    public Texture2D bgTex;
    public Texture2D[] levelThumbnails;
    public float texX, texY, texWidth, texHeight;
    public float texBgX, texBgY, texBgW, texBgH;
    private int currentLevelIndex=0;

 /*   void Update()
    {
        Debug.Log("" + currentLevelIndex);
    }*/

    void OnGUI()
    {
        float screenX = Screen.width / 800f;
        float screenY = Screen.height / 400f;
        GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(screenX, screenY, 1f));

        GUI.DrawTexture(new Rect(texBgX, texBgY, texBgW, texBgH), bgTex);

        GUI.DrawTexture(new Rect(texX, texY, texWidth, texHeight), levelThumbnails[currentLevelIndex]);
        //<<<Level list navigation buttons>>>
        //Left -Navigate left
        if(GUI.Button(new Rect(buttonArrowX,buttonArrowY,buttonWidth,buttonHeight),"Left"))
        {
            currentLevelIndex--;
            if (currentLevelIndex <= 0)
            {
                currentLevelIndex = 0;
                Debug.Log(""+currentLevelIndex);
            }
        }

        //Right -Navigate right
        if(GUI.Button(new Rect(buttonArrowX+buttonOffsetX,buttonArrowY,buttonWidth,buttonHeight),"Right"))
        {
            int length=(int)levelThumbnails.Length;
            currentLevelIndex++;
            if(currentLevelIndex >= length)
            {
                currentLevelIndex=length-1;
                Debug.Log("" + currentLevelIndex);
            }

        }

            //Start button -Starts the game
            if (GUI.Button(new Rect(buttonX, buttonY, buttonWidth, buttonHeight), "Start"))
            {
                _mainCam.GetComponent<audioManager>().playAudio("buttonHit");
                Application.LoadLevel(currentLevelIndex+1);
            }

            //Customize button -Takes to the player and vehicel customization menu
            if (GUI.Button(new Rect(buttonX + buttonOffsetX, buttonY, buttonWidth, buttonHeight), "Back"))
            {
                _mainCam.GetComponent<audioManager>().playAudio("buttonHit");
                lightGarage.gameObject.SetActive(true);
                gameObject.SetActive(false);
                _selectionMenu.SetActive(true);
                Time.timeScale = 1f;
            }
    }

}
