using UnityEngine;
using System.Collections;

public class buttonExit : MonoBehaviour {

    public Texture2D btnNormal;
    public Texture2D btnPressed;
    public GameObject _audioManager;

    void OnMouseDown() {
        renderer.material.mainTexture = btnPressed;
        _audioManager.GetComponent<audioManager>().playAudio("buttonHit");
    }

    void OnMouseUp() {
        renderer.material.mainTexture = btnNormal;
        Application.Quit();
    }
}
