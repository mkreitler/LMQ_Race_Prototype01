using UnityEngine;
using System.Collections;

namespace LMQ_TrackTest {

public class PlayerInput : MonoBehaviour {
	[SerializeField]
	RaceCamera raceCamera = null;

	public void Update() {
		if (raceCamera != null) {
			if (Input.GetButtonDown("left")) {
				raceCamera.SetState(LMQ_TrackTest.RaceCamera.eSlotState.SLOT_LEFT);
			}
			else if (Input.GetButtonDown("right")) {
				raceCamera.SetState(LMQ_TrackTest.RaceCamera.eSlotState.SLOT_RIGHT);
			}
			else if (Input.touchCount == 1) {
				if (Input.GetTouch(0).phase == TouchPhase.Began) {
					if (Input.GetTouch(0).position.x < Screen.width / 2) {
						raceCamera.SetState(LMQ_TrackTest.RaceCamera.eSlotState.SLOT_LEFT);
					}
					else {
						raceCamera.SetState(LMQ_TrackTest.RaceCamera.eSlotState.SLOT_RIGHT);
					}
				}
			}
			else if (Input.touchCount == 2) {
				if ((Input.GetTouch(0).position.x < Screen.width / 2 && Input.GetTouch(1).position.x > Screen.width / 2) ||
					(Input.GetTouch(0).position.x > Screen.width / 2 && Input.GetTouch(1).position.x < Screen.width / 2)) {
					// Apply brakes.
				}
			}
			else {
				// Accelerate, if we can.
			}
		}
	}	
}

} // end namespace