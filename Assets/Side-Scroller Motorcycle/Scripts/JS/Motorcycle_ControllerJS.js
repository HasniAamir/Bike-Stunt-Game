
#pragma strict

	//if this is activated the controlls will be got from touches else it'll be keyboard or joystick buttons
	public var forMobile : boolean = false;
	
	//used for mobile to detect which button was touched
	public var throttleTexture : GUITexture;
	public var brakeTexture : GUITexture;
	public var leftTexture : GUITexture;
	public var rightTexture : GUITexture;
	
	//used to determine when player is crashed
	public static var crash : boolean = false;
	public static var crashed : boolean = false;	
	
	//used to enable/disable motorcycle controlling
	public static var isControllable : boolean = true;
	
	//used to count scores
	public static var score : int = 0;
	
	public var is2D : boolean = false;
	public var usingAccelerometer : boolean = false;
	
	//used to change motorcycle characteristics
	public var  body : Rigidbody;
	public var frontFork : Transform;
	public var frontWheel : Transform;
	public var rearFork : Transform;
	public var rearWheel : Transform;
	
	public var speed : float = 60.0f;
	public var groundedWeightFactor : float = 20.0f;
	public var inAirRotationSpeed : float = 10.0f;
	public var wheelieStrength : float = 15.0f;
	
	//used to attach biker's bones to motorcycle positions so the biker will follow the motorcycle
	public var hips : Transform;
	public var leftHand : Transform;
	public var rightHand : Transform;
	public var leftFoot : Transform;
	public var rightFoot : Transform;
	
	public var hipsPosition : Transform;	
	public var leftFootPosition : Transform;	
	public var leftHandPosition : Transform;	
	public var rightFootPosition : Transform;
	public var rightHandPosition : Transform;
	//-------------------------------------------
	
	//used to start/stop dirt particles
	public var dirt : ParticleSystem;
	
	//used for showing score particles when flips are done
	public var backflipParticle : ParticleSystem;
	public var frontflipParticle : ParticleSystem;	
	
	//used to show scores
	public var scoreText : GUIText;
	public var scoreTextColor : Color;
	
	//used to determine if motorcycle is grounded or in air
	private var hit : RaycastHit;
	private var onGround : boolean = false;		
	private var inAir : boolean = false;		
	
	//used to manipulate engine sound pitch
	private var audioSource : AudioSource;
 	private var pitch : float;
	
	//used to allign biker to motorcycle
	private var tempPosition : Vector3;
	private var smooth : float = 0.0f;
	
	//used to determine when flip is done
	private var flip : boolean = false;			 	
	
	//used for knowing input	
	private var accelerate : boolean = false;
	private var brake : boolean = false;
	private var left : boolean = false;
	private var right : boolean = false;
	private var leftORright : boolean = false;
	
	
	//start function is called once when game starts
	function Start ()
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
		if(is2D)//if there is activated is2D checkbox on motorcycle, than you need to assign "CameraFollow2DJS" script to camera
		Camera.main.GetComponent.<CameraFollow2DJS>().target = body.transform;
		else
		Camera.main.GetComponent.<SmoothFollow>().target = body.transform;		
		
		//ignoring collision between motorcycle wheels and body
		Physics.IgnoreCollision (frontWheel.collider, body.collider);
		Physics.IgnoreCollision (rearWheel.collider, body.collider);
		
		//ignoring collision between motorcycle and ragdoll
		Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Motorcycle"), LayerMask.NameToLayer ("Ragdoll"),true);
		
		//ignoring collision between motorcycle colliders
		Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Motorcycle"), LayerMask.NameToLayer ("Motorcycle"),true);
		
		//used to manipulate engine sound pitch
		audioSource = body.GetComponent.<AudioSource>();		
		
		//setting rear wheel max angular rotation speed
		rearWheel.GetComponent.<Rigidbody> ().maxAngularVelocity = speed;				
	}
	
		
	
//  Update is called once per frame
	function Update()
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
						if(Input.acceleration.y > 0.04f)
							left = true;
						else if(Input.acceleration.y < -0.04f)
							right = true;
							
						if(left || right) //left or right button is touched
						leftORright = true;
					}
				}
				
				//detect which mobile buttons are pressed and make decisions accordingly
	    		for (var touch in touches)
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
					Camera.main.GetComponent.<CameraFollow2DJS>().target = hips.transform; //make camera to follow biker's hips
				else
					Camera.main.GetComponent.<SmoothFollow>().target = hips.transform; //make camera to follow biker's hips
				
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
	function FixedUpdate ()
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
	        	
			if (left) { //left horizontal key (determined in edit -> project settings -> input) is pressed or left button is touched on mobile if "formobile" is activated or using accelerometer
				if (!inAir) { //rotate left the motorcycle body
					body.AddTorque (new Vector3 (0, 0, (forMobile ? Mathf.Abs(Input.acceleration.y) : 1) * groundedWeightFactor * 100 * Time.deltaTime)); 
					body.AddForceAtPosition (body.transform.up * (forMobile ? Mathf.Abs(Input.acceleration.y) : 1) * wheelieStrength * body.velocity.sqrMagnitude/100, 
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
					
			if(Physics.Raycast(rearWheel.position, -body.transform.up, hit, rearWheel.collider.bounds.extents.y + 0.2f)) // cast ray to know if motorcycle is in air or grounded	
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
