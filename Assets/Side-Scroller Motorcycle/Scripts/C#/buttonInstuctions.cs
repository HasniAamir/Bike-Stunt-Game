using UnityEngine;
using System.Collections;

public class buttonInstuctions : MonoBehaviour {
    public Texture2D texButtonPressed,texButtonUp;
    public GameObject _audioManager;

    void OnMouseDown() {
        renderer.material.mainTexture = texButtonPressed;
        _audioManager.GetComponent<audioManager>().playAudio("buttonHit");
    }

    void OnMouseUp() {
        renderer.material.mainTexture = texButtonUp;
    }
}
