using UnityEngine;
using System.Collections;

public class Motorcycle_Controller : MonoBehaviour
{
	//if this is activated the controlls will be got from touches else it'll be keyboard or joystick buttons
	public bool forMobile = false;
	
	//used for mobile to detect which button was touched
	public GUITexture throttleTexture;
	public GUITexture brakeTexture;
	public GUITexture leftTexture;
	public GUITexture rightTexture;
	
	//used to determine when player is crashed
	public static bool crash = false;
	public static bool crashed = false;	
	
	//used to enable/disable motorcycle controlling
	public static bool isControllable = true;
	
	//used to count scores
	public static int score = 0;
	
	public bool is2D = false;
	public bool usingAccelerometer = false;
	
	//used to change motorcycle characteristics
	public Rigidbody body;
	public Transform frontFork;
	public Transform frontWheel;
	public Transform rearFork;
	public Transform rearWheel;
	
	public float speed = 60.0f;
	public float groundedWeightFactor = 20.0f;
	public float inAirRotationSpeed = 10.0f;
	public float wheelieStrength = 15.0f;
	
	//used to attach biker's bones to motorcycle positions so the biker will follow the motorcycle
	public Transform hips;
	public Transform leftHand;
	public Transform rightHand;
	public Transform leftFoot;
	public Transform rightFoot;
	
	public Transform hipsPosition;	
	public Transform leftFootPosition;	
	public Transform leftHandPosition;	
	public Transform rightFootPosition;
	public Transform rightHandPosition;
	//-------------------------------------------
	
	//used to start/stop dirt particles
	public ParticleSystem dirt;
	
	//used for showing score particles when flips are done
	public ParticleSystem backflipParticle;
	public ParticleSystem frontflipParticle;	
	
	//used to show scores
	public GUIText scoreText;
	public Color scoreTextColor;
	
	//used to determine if motorcycle is grounded or in air
	private RaycastHit hit;	
	private bool onGround = false;		
	private bool inAir = false;		
	
	//used to manipulate engine sound pitch
	private AudioSource audioSource;
 	private float pitch;
	
	//used to allign biker to motorcycle
	private Vector3 tempPosition;
	private float smooth = 0.0f;
	
	//used to determine when flip is done
	private bool flip = false;			 	
	
	//used for knowing input	
	private bool accelerate = false;
	private bool brake = false;	
	private bool left = false;
	private bool right = false;
	private bool leftORright = false;			
	
	
	//start function is called once when game starts
	void Start ()
	{
		Input.multiTouchEnabled = true;
		//reset static variables
		crash = false;
		crashed = false;
		isControllable = true;
		score = 0;
		
		tempPosition = hipsPosition.position;
		
		//change score text color
		scoreText.material.color = scoreTextColor; 		
		
		//adding motorcycle body as a follow target for camera
		if(is2D)//if there is activated is2D checkbox on motorcycle, than you need to assign "CameraFollow2D.cs" script to camera
			Camera.main.GetComponent<CameraFollow2D>().target = body.transform;
		else
			Camera.main.GetComponent<SmoothFollow>().target = body.transform;		
		
		//ignoring collision between motorcycle wheels and body
		Physics.IgnoreCollision (frontWheel.collider, body.collider);
		Physics.IgnoreCollision (rearWheel.collider, body.collider);
		
		//ignoring collision between motorcycle and ragdoll
		Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Motorcycle"), LayerMask.NameToLayer ("Ragdoll"),true);
		
		//ignoring collision between motorcycle colliders
		Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Motorcycle"), LayerMask.NameToLayer ("Motorcycle"),true);
		
		//used to manipulate engine sound pitch
		audioSource = body.GetComponent<AudioSource>();		
		
		//setting rear wheel max angular rotation speed
		rearWheel.GetComponent<Rigidbody> ().maxAngularVelocity = speed;				
	}
	
		
	
//  Update is called once per frame
	void Update()
	{
		if(isControllable)
		{
			if(forMobile)
			{
				var touches = Input.touches;
				
				accelerate = false;
				brake = false;
				left = false;
				right = false;
				leftORright = false;							
				
				
				//use accelerometer for rotatin motorcycle left and right
				if(usingAccelerometer)
				{
					if(Screen.orientation == ScreenOrientation.LandscapeLeft || Screen.orientation == ScreenOrientation.LandscapeRight)
					{
						if(Input.acceleration.y > 0.07f)
							left = true;
						else if(Input.acceleration.y < -0.07f)
							right = true;
							
						if(left || right) //left or right button is touched
						leftORright = true;
					}
				}
				
				//detect which mobile buttons are pressed and make decisions accordingly
	    		foreach (var touch in touches)
				{						
					if(touch.phase != TouchPhase.Canceled && touch.phase != TouchPhase.Ended)
					{																							
						if(throttleTexture.HitTest (touch.position)) //if touch position is inside throttle texture
							accelerate = true;
						
						if(brakeTexture.HitTest (touch.position)) //if touch position is inside brake texture
							brake = true;
						
						if(leftTexture.HitTest (touch.position)) //left button is touched
							left = true;						
						
						if(rightTexture.HitTest (touch.position)) //right button is touched
							right = true;						
						
						if(left || right) //left or right button is touched
							leftORright = true;						
					}
					
				}
				
				if(Input.touchCount == 0)
				{
					
				}
			}
			else
			{
				//detect which keys are pressed. keys relevant to "Horizontal" and "Vertical" keywords are set in: Edit -> Project Settings -> Input
				if(Input.GetAxisRaw("Horizontal") != 0)
					leftORright = true;
				else
					leftORright = false;
				
				if(Input.GetAxisRaw("Horizontal") > 0)
					right = true;
				else
					right = false;
				
				if(Input.GetAxisRaw("Horizontal") < 0)
					left = true;
				else 
					left = false;
				
				if(Input.GetAxisRaw("Vertical") > 0 || Input.GetKey (KeyCode.Joystick1Button2))
					accelerate = true;
				else
					accelerate = false;				
				
				if(Input.GetAxisRaw("Vertical") < 0 || Input.GetKey (KeyCode.Joystick1Button1))
					brake = true;
				else
					brake = false;				
				//----------------------------------
			}
		
					
			if(body.rotation.eulerAngles.z > 210 && body.rotation.eulerAngles.z < 220)					
						flip = true; 																						
						
			if(body.rotation.eulerAngles.z > 320 && flip) //backflip is done
			{
				flip = false;				
				backflipParticle.Emit(1);
				score += 100;
				scoreText.text = "SCORE : " + score;
			}
			
			if(body.rotation.eulerAngles.z < 30 && flip) //frontflip is done
			{															
				flip = false;
				frontflipParticle.Emit(1);
				score += 150;	
				scoreText.text = "SCORE : " + score;					
			}
			
			//if any horizontal key (determined in edit -> project settings -> input)  is pressed or if "formobile" is activated, left or right buttons are touched or accelerometer is used
			if(leftORright)
			{
				if(right)//right horizontal key is pressed or if "formobile" is activated, right button is touched or using accelerometer
					tempPosition = Vector3.Lerp (tempPosition, hipsPosition.position + new Vector3(0.1f,0.1f,0f), smooth); //aligning bikers hips position to motorcycle to illustrate stand effect
				
				if(left)//left horizontal key is pressed or left button is touched on mobile or using accelerometer
					tempPosition = Vector3.Lerp (tempPosition, hipsPosition.position - new Vector3(0.1f,-0.05f,0f), smooth); //illustrating back lean
			}
			else tempPosition = Vector3.Lerp (tempPosition, hipsPosition.position, smooth); //used to smoothly follow biker's hips to motorcycle			
			
			//changing engine sound pitch depending rear wheel rotational speed
			if(accelerate) 
			{
				pitch = rearWheel.rigidbody.angularVelocity.sqrMagnitude / speed;
				pitch *= Time.deltaTime * 2;
				pitch = Mathf.Clamp (pitch + 1, 0.5f,1.8f);			
			}
			else
				pitch = Mathf.Clamp (pitch - Time.deltaTime * 2, 0.5f, 1.8f);																
		}		
		else if(!crashed) tempPosition = Vector3.Lerp (tempPosition, hipsPosition.position, smooth); //used to smoothly follow biker's hips to motorcycle
		
		if(crash && !crashed) //if player just crashed
			{											
				if(is2D)//if there is activated is2D checkbox on motorcycle, than you need to assign "CameraFollow2D.cs" script to camera
				Camera.main.GetComponent<CameraFollow2D>().target = hips; //make camera to follow biker's hips
				else
				Camera.main.GetComponent<SmoothFollow>().target = hips; //make camera to follow biker's hips
				
				//make bones not kinematic, so biker detaches from motorcycle
				hips.rigidbody.isKinematic = false;
				leftHand.rigidbody.isKinematic = false;
				rightHand.rigidbody.isKinematic = false;
				leftFoot.rigidbody.isKinematic = false;
				rightFoot.rigidbody.isKinematic = false;
				
				//add velocity to biker depending motorcycle speed
				hips.rigidbody.velocity  = body.rigidbody.velocity * 5;
				hips.rigidbody.angularVelocity  = body.rigidbody.angularVelocity * 5;
			
				//turn on collision between ragdoll and motorcycle
				Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Motorcycle"), LayerMask.NameToLayer ("Ragdoll"),false);
								
				if(!is2D) //disable all physics constraints if 2D isn't activated for motorcycle in inspector menu, so physics calculation will occur on all axis
				{
					hips.rigidbody.constraints = RigidbodyConstraints.None;
					body.constraints = RigidbodyConstraints.None;
					frontFork.rigidbody.constraints = RigidbodyConstraints.None;
					frontWheel.rigidbody.constraints = RigidbodyConstraints.None;
					rearFork.rigidbody.constraints = RigidbodyConstraints.None;
					rearWheel.rigidbody.constraints = RigidbodyConstraints.None;							
				}
				
				isControllable = false;
				crashed = true;
			}
			else if(!crashed) //if we aren't crashed, aligning biker's bones to motorcycle positions, so biker will follow to motorcycle
			{							
				hips.position = tempPosition;
				hips.rotation = hipsPosition.rotation;		
				leftHand.position = leftHandPosition.position;
				rightHand.position = rightHandPosition.position;
				leftFoot.position = leftFootPosition.position;
				rightFoot.position = rightFootPosition.position;			
			}
		
		//manipulating engine sound pitch
		pitch = Mathf.Clamp (pitch - Time.deltaTime * 2, 0.5f, 1.8f);
		audioSource.pitch = pitch;
		
		//if player is crashed and "R" is pressed or touch is detected on mobile, than restart current level
		if((Input.GetKeyDown (KeyCode.R) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)) && crashed)
		Application.LoadLevel (Application.loadedLevel);
	}
	
	
	
	//physics are calculated in FixedUpdate function
	void FixedUpdate ()
	{			
		if(isControllable){
			smooth = Mathf.Clamp (Time.deltaTime * body.velocity.sqrMagnitude * 2, 0.3f, 1);
			
			if (accelerate)
			{
				rearWheel.rigidbody.freezeRotation = false; //allow rotation to rear wheel
				rearWheel.rigidbody.AddTorque (new Vector3 (0, 0, -speed * Time.deltaTime),ForceMode.Impulse);	//add rotational speed to rear wheel
				
				if(onGround)//if motorcycle is standing on object tagged as "Ground"
				{			
					if(!dirt.isPlaying)
					dirt.Play (); //play dirt particle
					
					dirt.transform.position = rearWheel.position; //allign dirt to rear wheel
					
				} else dirt.Stop ();
				
			} else dirt.Stop ();
			
			if(brake)
				rearWheel.rigidbody.freezeRotation = true; //disable rotation for rear wheel if player is braking								
			else			
				rearWheel.rigidbody.freezeRotation = false; //enable rotation for rear wheel if player isn't braking
	        	
			if (left) { //left horizontal key (determined in edit -> project settings -> input) is pressed or left button is touched on mobile if "formobile" is activated
				if (!inAir) { //rotate left the motorcycle body
					body.AddTorque (new Vector3 (0, 0, (forMobile ? Mathf.Abs(Input.acceleration.y) : 1) * groundedWeightFactor * 100 * Time.deltaTime)); 
					body.AddForceAtPosition (body.transform.up * Mathf.Abs(Input.acceleration.y) * wheelieStrength * body.velocity.sqrMagnitude/100, 
						new Vector3 (frontWheel.position.x, frontWheel.position.y - 0.5f, body.transform.position.z));//add wheelie effect
				} else {
					body.AddTorque (new Vector3 (0, 0, (forMobile ? Mathf.Abs(Input.acceleration.y) : 1) * inAirRotationSpeed * 100 * Time.deltaTime));    
				}
	
			} else if (right) { //right horizontal key is pressed or right button is touched on mobile
				if (!inAir) { //rotate right the motorcycle body
					body.AddTorque (new Vector3 (0, 0, (forMobile ? Mathf.Abs(Input.acceleration.y) : 1) * -groundedWeightFactor * 100 * Time.deltaTime));				
				} else {
					body.AddTorque (new Vector3 (0, 0, (forMobile ? Mathf.Abs(Input.acceleration.y) : 1) * -inAirRotationSpeed * 100 * Time.deltaTime));   
				}
	
			}
					
			if(Physics.Raycast(rearWheel.position, -body.transform.up, out hit, rearWheel.collider.bounds.extents.y + 0.2f)) // cast ray to know if motorcycle is in air or grounded	
			{
				if(hit.collider.tag == "Ground") //if motorcycle is standig on object taged as "Ground"
					onGround = true;
				else
					onGround = false;
				
				inAir = false;									
			}
			else 
			{
				onGround = false;
				inAir = true;
			}
			
		}
		else dirt.Stop ();
	}
}
