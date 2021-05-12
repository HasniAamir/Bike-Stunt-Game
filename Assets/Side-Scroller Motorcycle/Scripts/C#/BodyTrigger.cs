using UnityEngine;
using System.Collections;

public class BodyTrigger : MonoBehaviour {
	public static bool finish = false;
	
	//used to play sounds
	public AudioClip bonesCrackSound;
	public AudioClip hitSound;
	public AudioClip oohCrowdSound;
	
	private AudioSource bonesCrackSC;
	private	AudioSource hitSC;
	private AudioSource oohCrowdSC;	
	
	//used to show text when entered in finish
	public GUIText winText;
	public GUIText crashText;		
	public Color winTextColor;
	public Color crashTextColor;
	
	//used to know if next level exists.
	private bool nextLevel = false;		
		
	
	void Start()
	{			
		finish = false;
		
		//change text colors
		winText.material.color = winTextColor;
		crashText.material.color = crashTextColor;
		
		//ignoring collision between biker's bodytrigger and motorcycle body
		Physics.IgnoreCollision (this.collider, transform.parent.collider);
		
		//add new audio sources and add audio clips to them, used to play sounds
		bonesCrackSC = gameObject.AddComponent<AudioSource>();
		hitSC = gameObject.AddComponent<AudioSource>();
		oohCrowdSC = gameObject.AddComponent<AudioSource>();
		
		bonesCrackSC.playOnAwake = false;
		hitSC.playOnAwake = false;
		oohCrowdSC.playOnAwake = false;
		
		bonesCrackSC.rolloffMode = AudioRolloffMode.Linear;
		hitSC.rolloffMode = AudioRolloffMode.Linear;
		oohCrowdSC.rolloffMode = AudioRolloffMode.Linear;
		
		bonesCrackSC.clip = bonesCrackSound;
		hitSC.clip = hitSound;
		oohCrowdSC.clip = oohCrowdSound;
		//--------------------------------------------------
	}
	
	void OnTriggerEnter(Collider obj)
	{
		if(obj.gameObject.tag == "Finish" && !Motorcycle_Controller.crash)//if entered in finish trigger
		{
			finish = true;
			
			Motorcycle_Controller.isControllable = false; //disable motorcycle controlling
			
			var m = transform.root.GetComponent<Motorcycle_Controller>();
			m.rearWheel.rigidbody.freezeRotation = true; //disable rear wheel rotation
			
			winText.enabled = true; //show win text				
			
			if(Application.loadedLevel < Application.levelCount - 1) //if won level isn't last level (levels are set in File -> Build Settings)
			{
				nextLevel = true;
				
				if(m.forMobile)
					winText.text = "CONGRATULATIONS, YOU WON! \n YOUR SCORE IS: " + Motorcycle_Controller.score + "\n\n TAP ON SCREEN FOR NEXT LEVEL";
				else
					winText.text = "CONGRATULATIONS, YOU WON! \n YOUR SCORE IS: " + Motorcycle_Controller.score + "\n\n PRESS SPACE FOR NEXT LEVEL";				
			}
			else //won level is last one
			{
				if(m.forMobile)
					winText.text = "CONGRATULATIONS, YOU WON! \n YOUR SCORE IS: " + Motorcycle_Controller.score + "\n\n TAP ON SCREEN TO PLAY FIRST LEVEL";				
				else
					winText.text = "CONGRATULATIONS, YOU WON! \n YOUR SCORE IS: " + Motorcycle_Controller.score + "\n\n PRESS SPACE TO PLAY FIRST LEVEL";				
				
				nextLevel = false;
			}
		}
		else //if entered in any other trigger than "Finish", that means player crashed
		{
			if(!Motorcycle_Controller.crash)
			{
				Motorcycle_Controller.crash = true;
				
				//play sounds
				bonesCrackSC.Play ();
				hitSC.Play ();
				oohCrowdSC.Play ();
				
				if(!finish) //if we haven't entered in finish make crash text visible
				{
					crashText.enabled = true;
					
					var m = transform.root.GetComponent<Motorcycle_Controller>();
					if(m.forMobile)
						crashText.text = "TAP ON SCREEN TO RESTART";
					else
						crashText.text = "PRESS 'R' TO RESTART";
					
				}
			} 
		}
	}
	
	void Update()
	{		
		if((Input.GetKeyDown(KeyCode.Space) || (Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Began)) && finish) //if motorcycle entered in finish and space is pressed
		{
			if(nextLevel)
				Application.LoadLevel(Application.loadedLevel + 1);	//load next level
			else			
				Application.LoadLevel(0); //load first level
		}				
	}		
}