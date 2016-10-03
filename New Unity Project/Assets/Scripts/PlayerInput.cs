using UnityEngine;
using System.Collections;

namespace LMQ_TrackTest {

public class PlayerInput : MonoBehaviour {
	[SerializeField]
	RaceCamera raceCamera = null;

	public void Update() {
		if (raceCamera != null) {
            bool bLeftDown = Input.GetButton("left");
            bool bRightDown = Input.GetButton("right");

            for (int i=0; i<Input.touchCount; ++i) {
                if (Input.GetTouch(i).phase != TouchPhase.Canceled || Input.GetTouch(i).phase != TouchPhase.Ended) {
                    if (Input.GetTouch(i).position.x < Screen.width / 2) {
                        bLeftDown = true;
                    }
                    if (Input.GetTouch(i).position.x > Screen.width / 2) {
                        bRightDown = true;
                    }
                }
            }

            if (bLeftDown && bRightDown) {
                // Brake.
                raceCamera.Brake(true);
            }
            else if (!bLeftDown && !bRightDown) {
                // Accelerate.
                raceCamera.Brake(false);
            }
            else {
                if (bLeftDown) {
                    raceCamera.Brake(false);
                    raceCamera.SetWantSlot(LMQ_TrackTest.RaceCamera.eSlot.OUTSIDE);
                }
                else {
                    // bRightDown == true;
                    raceCamera.Brake(false);
                    raceCamera.SetWantSlot(LMQ_TrackTest.RaceCamera.eSlot.INSIDE);
                }
            }
		}
	}	
}

} // end namespace