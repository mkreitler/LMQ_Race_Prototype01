using UnityEngine;
using System.Collections;

public class RaceCamera : MonoBehaviour {
	[SerializeField]
	GameObject respawnPoint = null;

	[SerializeField]
	float speed = 100.0f;

	private Vector3 xDirection = new Vector3(1.0f, 0.0f, 0.0f);

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		gameObject.transform.position = gameObject.transform.position + xDirection * speed * Time.deltaTime;
	}

	public void OnTriggerEnter(Collider other) {
		Bounds otherBounds = other.bounds;
		float offsetX = gameObject.transform.position.x - (other.bounds.center.x - other.bounds.extents.x);

		gameObject.transform.position = respawnPoint.transform.position - offsetX * xDirection;
	}
}
