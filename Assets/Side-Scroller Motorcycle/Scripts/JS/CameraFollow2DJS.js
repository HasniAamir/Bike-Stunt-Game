#pragma strict

	public var target : Transform;
	
	private var cam : Transform;
	
	function Start()
	{
		cam = transform;
	}
	
	// Update is called once per frame
	function Update () {
	 cam.position = new Vector3( target.position.x, target.position.y, cam.position.z);
	}
