﻿using UnityEngine;
using System.Collections;
using Battlehub.SplineEditor;

namespace LMQ_TrackTest {

    public class RaceCamera : MonoBehaviour {
		private const float SPEED_TO_MPH = 29.0f;
		private const float FORCE_BRAKE_DISTANCE = 5.0f;

        [SerializeField]
        float topSpeed = 6.0f;

        [SerializeField]
        float blendRate = 5.0f;

        [SerializeField]
        SplineFollow[] tracker = { null, null };

        [SerializeField]
        float accelBlend = 0.05f;

        [SerializeField]
        float brakeBlend = 0.5f;

		[SerializeField]
		TextMesh textSpeed = null;

		[SerializeField]
		GameObject warningSign = null;

		[SerializeField]
		float[] wallDetectionSpeed = { 150.0f, 180.0f };

		[SerializeField]
		float[] wallDownshiftSpeed = { 50.0f, 60.0f};

		[SerializeField]
		// float[] wallDamageSpeed = { 80.0f, 100.0f };

        public enum eSlot {INSIDE, OUTSIDE};

        private float speed = 0.0f;
        private float slot = 0.0f;
        private float wantSlot = 0.0f;
        private bool bBraking = false;
        private float[] length = { 1.0f, 1.0f };
		private bool bForceBrake = false;
        private Quaternion qLookBack = Quaternion.AngleAxis(180.0f, Vector3.up);
		private bool bWantsBrakeWarning = false;

	    // Use this for initialization
	    void Start () {
		    Init();
	    }
	
	    // Update is called once per frame
	    void Update () {
            ApplyAcceleration();

            slot = Mathf.Lerp(slot, wantSlot, blendRate * Time.deltaTime);
            slot = Mathf.Clamp(slot, 0.0f, 1.0f);

            Transform xFrmInside = tracker[(int)eSlot.INSIDE].gameObject.transform;
            Transform xFrmOutside = tracker[(int)eSlot.OUTSIDE].gameObject.transform;

            transform.position = Vector3.Lerp(xFrmInside.position, xFrmOutside.position, slot);
            transform.rotation = Quaternion.Slerp(xFrmInside.rotation, xFrmOutside.rotation, slot) * qLookBack;

			textSpeed.text = "" + SpeedToMPH ();
	    }

        // Interface ///////////////////////////////////////////////////////////////////////////////////
        public void Brake(bool bBrakesApplied) {
            bBraking = bBrakesApplied;
        }

        public void SetWantSlot(eSlot slot) {
            wantSlot = slot == eSlot.INSIDE ? 0.0f : 1.0f;
	    }

		public void NoWallDetected() {
			warningSign.SetActive (false);
			bForceBrake = false;
			bWantsBrakeWarning = true;
		}

		public int SpeedToMPH() {
			return (int)Mathf.Round (speed * SPEED_TO_MPH);
		}

		public void WallDetected(float distance) {
			if (SpeedToMPH () > wallDetectionSpeed [SlotIndex] && bWantsBrakeWarning) {
				warningSign.SetActive (true);

				if (distance < FORCE_BRAKE_DISTANCE && !bBraking) {
					bForceBrake = true;
				}
			}
			else if (bBraking && SpeedToMPH() < wallDetectionSpeed[SlotIndex] && bWantsBrakeWarning) {
				warningSign.SetActive (false);
				bWantsBrakeWarning = false;
			}
		}

        // Implementation ///////////////////////////////////////////////////////////////////////////////
		private int SlotIndex {
			get {
				return (int)Mathf.Round(Mathf.Clamp(slot, 0.0f, 1.0f));
			}
		}

		private int SlotIndexInside {
			get {
				return (int)eSlot.INSIDE;
			}
		}

		private int SlotIndexOutside {
			get {
				return (int)eSlot.OUTSIDE;
			}
		}

		private void Init() {
			slot = 0.0f;
			wantSlot = 0.0f;
			bForceBrake = false;
		}

        private void ApplyAcceleration() {
            if (bBraking) {
                speed = Mathf.Lerp(speed, 0.0f, brakeBlend * Time.deltaTime);
            }
			else if (bForceBrake) {
				speed = Mathf.Lerp(speed, wallDownshiftSpeed [SlotIndex] / SPEED_TO_MPH, brakeBlend * Time.deltaTime);
			}
            else if (speed < topSpeed) {
                speed = Mathf.Lerp(speed, topSpeed, accelBlend * Time.deltaTime);
            }

            if (length[(int)eSlot.INSIDE] == 0.0f || length[(int)eSlot.OUTSIDE] == 0.0f) {
                length[(int)eSlot.INSIDE] = tracker[(int)eSlot.INSIDE].Length;
                length[(int)eSlot.OUTSIDE] = tracker[(int)eSlot.OUTSIDE].Length;
            }

            float speedFactor = length[(int)eSlot.OUTSIDE] / length[(int)eSlot.INSIDE];
            tracker[(int)eSlot.INSIDE].Speed = speed;
            tracker[(int)eSlot.OUTSIDE].Speed = speed * speedFactor;
        }
    }

} // end namespace
