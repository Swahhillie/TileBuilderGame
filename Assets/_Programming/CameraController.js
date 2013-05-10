#pragma strict
@script RequireComponent(Camera)

class CameraController extends MonoBehaviour{
	var forwardSpeed:float = 1.0;
	var backwardSpeed:float = 0.5;
	var strafeSpeed:float = 1.0;
	var verticalSpeed:float = 1.0;
	var rotationSpeed:float = 1.0;
	var verticalSpeedSmooth:float = .03;
	
	var allowCameraRotation:boolean = false;
	var invertYControls:boolean = false;
	var cameraRotationLimit:Vector3 = Vector3(80,80,0);
	
	public var cameraPresetLocations:Transform[] ;
	private var cameraPresetIndex:int;
	private var moveVector:Vector3;
	
	function LateUpdate(){
		HandleKeyboardInput();
		//ScrollThroughPositions();
		if(allowCameraRotation)HandleMouseInput();
		if(Input.GetKeyDown("1"))camera.orthographic = !camera.orthographic;
	}
	private function HandleKeyboardInput(){
		
		moveVector.x = Input.GetAxis("Horizontal") * strafeSpeed;
		var dir:Vector3 = Vector3.Cross(transform.right, Vector3.up).normalized;
		Debug.DrawRay(transform.position, transform.position + dir, Color.cyan);
		//moveVector.z = (Input.GetAxis("Vertical") > 0)? Input.GetAxis("Vertical")* forwardSpeed : Input.GetAxis("Vertical") * backwardSpeed;
		//moveVector.y = (Input.GetButton("Jump"))? Mathf.Lerp(moveVector.y, verticalSpeed,Time.deltaTime * verticalSpeedSmooth): Mathf.Lerp(moveVector.y, 0,Time.deltaTime * verticalSpeedSmooth);
		
		transform.Translate(moveVector * Time.deltaTime);
		moveVector.y *= Time.deltaTime;
		
		
		
	}
	
	function ScrollThroughPositions(){
		var scrollDir:float = Input.GetAxisRaw("Mouse ScrollWheel");
		var changed:boolean = true;
		if(scrollDir < 0){
			//scrolling down;
			cameraPresetIndex = (cameraPresetIndex -1 < 0)? cameraPresetLocations.Length -1: cameraPresetIndex -1;
		}
		else if(scrollDir > 0){
			//scrolling up;
			cameraPresetIndex = (cameraPresetIndex +1) % cameraPresetLocations.Length;
		}
		else{
			//not scrolling
			changed = false;
		}
		if(changed){
			camera.transform.position = cameraPresetLocations[cameraPresetIndex].position;
			camera.transform.rotation = cameraPresetLocations[cameraPresetIndex].rotation;
		}
	
	}
	private function HandleMouseInput(){
		var rotation:Vector3;
		rotation.y = Input.GetAxis("Mouse X") * rotationSpeed;
		rotation.x = Input.GetAxis("Mouse Y") * (invertYControls? rotationSpeed:-rotationSpeed);
		
		transform.Rotate(rotation);
		
		//limit camera rotation
		var angles:Vector3 = transform.eulerAngles;
		angles.z = 0;
		transform.eulerAngles = angles;
	}
}