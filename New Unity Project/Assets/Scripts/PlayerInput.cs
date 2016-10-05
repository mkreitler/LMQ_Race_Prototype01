using UnityEngine;
using System.Collections;

namespace LMQ_TrackTest {

public class PlayerInput : MonoBehaviour {
	private const float ACTIVATE_TIME = 0.05f;

	[SerializeField]
	RaceCamera raceCamera = null;

	private float leftTimer = 0.0f;
	private float rightTimer = 0.0f;
	private bool bCanTriggerLeft = false;
	private bool bCanTriggerRight = false;
	private bool bLeftTriggered = false;
	private bool bRightTriggered = false;

	public void Update() {
		if (raceCamera != null) {
            bool bLeftDown = Input.GetButton("left");
            bool bRightDown = Input.GetButton("right");

			if (Input.GetButtonDown ("left")) {
				leftTimer = 0.0f;
				bCanTriggerLeft = false;
			} else if (bLeftDown) {
				if (leftTimer < ACTIVATE_TIME && leftTimer + Time.deltaTime >= ACTIVATE_TIME) {
					bCanTriggerLeft = true;
				}
				leftTimer += Time.deltaTime;
			} else if (Input.GetButtonUp ("left") && leftTimer > 0.0f) {
				bCanTriggerLeft = !bLeftTriggered;
				bLeftTriggered = false;
			}

			if (Input.GetButtonDown ("right")) {
				rightTimer = 0.0f;
				bCanTriggerRight = false;
			} else if (bRightDown) {
				if (rightTimer < ACTIVATE_TIME && rightTimer + Time.deltaTime >= ACTIVATE_TIME) {
					bCanTriggerRight = true;
				}
				rightTimer += Time.deltaTime;
			} else if (Input.GetButtonUp ("right") && rightTimer > 0.0f) {
				bCanTriggerRight = !bRightTriggered;
				bRightTriggered = false;
			}

            for (int i=0; i<Input.touchCount; ++i) {
					if (Input.GetTouch (i).phase != TouchPhase.Canceled || Input.GetTouch (i).phase != TouchPhase.Ended) {
						if (Input.GetTouch (i).position.x < Screen.width / 2) {
							if (Input.GetTouch (i).phase == TouchPhase.Began) {
								leftTimer = 0.0f;
								bCanTriggerLeft = false;
							} else {
								if (leftTimer < ACTIVATE_TIME && leftTimer + Time.deltaTime >= ACTIVATE_TIME) {
									bCanTriggerLeft = true;
								}
								leftTimer += Time.deltaTime;
							}

							bLeftDown = true;
						}
						if (Input.GetTouch (i).position.x > Screen.width / 2) {
							if (Input.GetTouch (i).phase == TouchPhase.Began) {
								rightTimer = 0.0f;
								bCanTriggerRight = false;
							} else {
								if (rightTimer < ACTIVATE_TIME && rightTimer + Time.deltaTime >= ACTIVATE_TIME) {
									bCanTriggerRight = true;
								}
								rightTimer += Time.deltaTime;
							}

							bRightDown = true;
						}
					}
					else {
						if (Input.GetTouch (i).position.x < Screen.width / 2) {
							bCanTriggerLeft = !bLeftTriggered;
							bLeftTriggered = false;
							bLeftDown = false;
						}
						if (Input.GetTouch (i).position.x > Screen.width / 2) {
							bCanTriggerRight = !bRightTriggered;
							bRightTriggered = false;
							bRightDown = false;
						}
					}
            }

            if (bLeftDown && bRightDown) {
                // Brake.
                raceCamera.Brake(true);
					bCanTriggerLeft = false;
					bCanTriggerRight = false;
            }
            else if (!bLeftDown && !bRightDown) {
                // Accelerate.
                raceCamera.Brake(false);
            }
            else {
				if (bCanTriggerLeft && !bRightDown) {
                    raceCamera.Brake(false);
                    raceCamera.SetWantSlot(LMQ_TrackTest.RaceCamera.eSlot.OUTSIDE);
					bLeftTriggered = true;
					bCanTriggerLeft = false;
                }
				else if (bCanTriggerRight && !bLeftDown) {
                    // bRightDown == true;
                    raceCamera.Brake(false);
                    raceCamera.SetWantSlot(LMQ_TrackTest.RaceCamera.eSlot.INSIDE);
					bRightTriggered = true;
					bCanTriggerRight = false;
                }
            }
		}
	}	
}

} // end namespace