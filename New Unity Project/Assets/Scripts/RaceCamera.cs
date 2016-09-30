using UnityEngine;
using System.Collections;

namespace LMQ_TrackTest {

public class RaceCamera : MonoBehaviour {
	public enum eSlotState {UNKNOWN,
							SLOT_RIGHT,
							SLOT_LEFT,
							TRANSITIONING};
	[SerializeField]
	GameObject respawnPoint = null;

	[SerializeField]
	float speed = 100.0f;

	[SerializeField]
	float blendRate = 20.0f;

	[SerializeField]
	float slotTolerance = 0.001f;

	private Vector3 xDirection = new Vector3(1.0f, 0.0f, 0.0f);
	private float[] camPositions = {0.33f, -0.33f, 0.0f};
	private float destZ = 0.0f;
	private eSlotState curSlotState = eSlotState.UNKNOWN;
	private eSlotState wantSlotState = eSlotState.UNKNOWN;
	private Vector3 vNewPos = Vector3.zero;
	private float curZ = 0.0f;

	// Use this for initialization
	void Start () {
		Init();
	}
	
	// Update is called once per frame
	void Update () {
		gameObject.transform.position = gameObject.transform.position + xDirection * speed * Time.deltaTime;

		float newZ = gameObject.transform.position.z;
		if (curSlotState == eSlotState.TRANSITIONING) {
			newZ = Mathf.Lerp(newZ, destZ, Mathf.Clamp(blendRate * Time.deltaTime, 0.0f, 1.0f));

			if (Mathf.Abs(destZ - newZ) < slotTolerance) {
				newZ = destZ;

				curSlotState = wantSlotState;
			}

			SetSlotPos(newZ);

			curZ = newZ;
		}
	}

	public void OnTriggerEnter(Collider other) {
		Bounds otherBounds = other.bounds;
		float offsetX = gameObject.transform.position.x - (other.bounds.center.x - other.bounds.extents.x);

		gameObject.transform.position = respawnPoint.transform.position - offsetX * xDirection;
		SetSlotPos(curZ);
	}

	public void SetState(eSlotState slotState) {
		if (slotState != curSlotState &&
			slotState > eSlotState.UNKNOWN &&
			slotState < eSlotState.TRANSITIONING) {
			destZ = camPositions[(int)slotState - 1];
			curSlotState = eSlotState.TRANSITIONING;
			wantSlotState = slotState;
		}
	}

	private void Init() {
		curSlotState = eSlotState.SLOT_RIGHT;
		destZ = camPositions[(int)eSlotState.SLOT_RIGHT - 1];
		gameObject.transform.position = new Vector3(gameObject.transform.position.x,
													gameObject.transform.position.y,
													destZ);
		curZ = destZ;
		wantSlotState = curSlotState;

	}

	private void SetSlotPos(float newZ) {
		vNewPos.x = gameObject.transform.position.x;
		vNewPos.y = gameObject.transform.position.y;
		vNewPos.z = newZ;

		gameObject.transform.position = vNewPos;
	}
}

} // end namespace
