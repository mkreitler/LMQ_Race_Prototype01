using UnityEngine;
using System.Collections;
using LMQ_TrackTest;

public class DetectCurve : MonoBehaviour {
	RaceCamera raceCamera = null;

	[SerializeField]
	float lookAheadDist = 20.0f;

	private const string CURVE_DETECTION_LAYER = "CurveDetection";

	// Use this for initialization
	void Start () {
		raceCamera = gameObject.GetComponent<RaceCamera> ();
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 vRayDir = gameObject.transform.forward * -1.0f;
		RaycastHit hitInfo;

		if (Physics.Raycast (gameObject.transform.position, vRayDir, out hitInfo, lookAheadDist, 1 << LayerMask.NameToLayer (CURVE_DETECTION_LAYER))) {
			raceCamera.WallDetected (hitInfo.distance);
		}
		else {
			raceCamera.NoWallDetected ();
		}
	}
}
