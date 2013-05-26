using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]

public class RTSCamera : MonoBehaviour {

	// Use this for initialization
	public float camRotateSpeed = 5.0f;
	public float camMoveSpeed  = 5.0f;
	public bool allowRotate = false;
	private Transform guide;
	private bool lerping;
	
	void Start () {
		guide = new GameObject("CameraGuide").transform;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void LerpFromToTransform(Transform subject, Transform start, Transform end, float overTime){
		if(!lerping){
			StartCoroutine(RotateSmoothed(subject, start.rotation, end.rotation, overTime));
			StartCoroutine(LerpFromToPosition(subject, start.position, end.position, overTime));	
			StartCoroutine(ResetLerping(overTime));
		}
		else{
			Debug.Log("Already lerping");
		}
		
	}
	IEnumerator ResetLerping(float seconds){
		yield return new WaitForSeconds(seconds);
		lerping = false;
	}
	public IEnumerator RotateSmoothed(Transform subject, Quaternion startR, Quaternion endR, float overTime){
			
		float startTime = Time.time;
		float duration = overTime;
		float percent = (Time.time - startTime) / duration;
		
		while(percent < 1.0f){
			percent = (Time.time - startTime) / duration;
			subject.rotation = Quaternion.Slerp(startR, endR, percent);
			yield return null;
		}
	}
	
	
	public IEnumerator LerpFromToPosition(Transform subject, Vector3 start, Vector3 end, float overTime){
		float startTime = Time.time;
		float duration = overTime;
		float percent = (Time.time - startTime) / duration;
		while(percent < 1.0f){
			percent = (Time.time - startTime) / duration;
			subject.position = Vector3.Lerp(start, end, percent);
			yield return null;
		}
	}
	void LateUpdate(){
		//keyboard
		Vector3 move = Vector3.zero;
		move.x = Input.GetAxis("Horizontal"); // a and d
		move.y = Input.GetAxis("CameraVertical"); // shift and control
		move.z = Input.GetAxis("Vertical"); // w and s
		if(camera.orthographic){
			camera.orthographicSize = Mathf.Clamp(camera.orthographicSize + move.y * Time.deltaTime * 100, 5.0f, 100.0f);
			move.y = 0;
		}
		move = Vector3.Normalize(move);
		
		move *= camMoveSpeed * Time.deltaTime;
		//make relative to camera holder rotation
		
		
		guide.position = transform.position;
		
		guide.right = transform.right;
		guide.forward = Vector3.Cross(guide.right, Vector3.up);
		
		move = guide.TransformDirection(move);
		
		transform.position += move;
		
		//mouse
		if(Input.GetMouseButton(2) && allowRotate){
			float angle = Input.GetAxis("Mouse X") * camRotateSpeed * Time.deltaTime;
			//rotationVector.z = Input.GetAxis("Mouse Y") * camRotateSpeed * Time.deltaTime;
			//deltaMouse.y = Input.GetAxis("Mouse Y");
			transform.RotateAround(guide.position, Vector3.up ,angle );
		}
		
		
	}
}
