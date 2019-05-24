using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour 
{
	[System.Serializable]
	public class CameraRig
	{
		public Vector3 cameraOffset;
		public float crouchHeight;
		public float damping;
	}

	[SerializeField] CameraRig defaultCamera;
	[SerializeField] CameraRig aimCamera;

	Transform cameraLookTarget;
    Vector3 destination = Vector3.zero;
	private Player localPlayer;

	// Use this for initialization
	void Awake () 
	{
		GameManager.Instance.OnLocalPlayerJoined += delegate(Player player) {
			localPlayer = player;
			cameraLookTarget = localPlayer.transform.Find("AimingPivot");

			if(cameraLookTarget == null)
				cameraLookTarget = localPlayer.transform;
        };
	}
	
	// Update is called once per frame
	void LateUpdate () 
	{
		if (localPlayer == null)
			return;

		CameraRig cameraRig = defaultCamera;

		if (localPlayer.PlayerState.weaponState == PlayerState.EWeaponState.AIMING || localPlayer.PlayerState.weaponState == PlayerState.EWeaponState.AIMED_FIRING)
			cameraRig = aimCamera;

		float targetHeight = cameraRig.cameraOffset.y + (localPlayer.PlayerState.moveState == PlayerState.EMoveState.CROUCHING ? cameraRig.crouchHeight : 0);

		//Camera follow player
		Vector3 targetPosition = cameraLookTarget.position + localPlayer.transform.forward * cameraRig.cameraOffset.z + 
			localPlayer.transform.up * targetHeight +
			localPlayer.transform.right * cameraRig.cameraOffset.x;

        //Camera Collision fix V1
        //Move camera away from walls
        Vector3 collisionCheckEnd = cameraLookTarget.position + localPlayer.transform.up * targetHeight - localPlayer.transform.forward * .5f;
        HandleCameraCollision (collisionCheckEnd, ref targetPosition);

        transform.position = Vector3 .Lerp(transform.position, targetPosition, cameraRig.damping * Time.deltaTime);
		transform.rotation = Quaternion.Lerp (transform.rotation, cameraLookTarget.rotation, cameraRig.damping * Time.deltaTime);
	}

	private void HandleCameraCollision(Vector3 toTarget, ref Vector3 fromTarget)
	{
		RaycastHit hit;
		if (Physics.Linecast (toTarget, fromTarget, out hit)) 
		{
			//hitPoint Fixes wall clipping
			Vector3 hitPoint = new Vector3 (hit.point.x + hit.normal.x * .2f, hit.point.y, hit.point.z + hit.normal.z * .2f);
			//Change position in front of wall.
			fromTarget = new Vector3 (hitPoint.x, fromTarget.y, hitPoint.z);
		}
	}
}
