using UnityEngine;
using System.Collections;

public class audioManager : MonoBehaviour {

    public AudioSource[] _audioSource;

    void Start()
    {
        _audioSource = this.GetComponents<AudioSource>();
    }

    public void playAudio(string audioState) { 
        switch(audioState){
            case "buttonHit":
                _audioSource[0].Play();
            break;
        }
    }
	
}
