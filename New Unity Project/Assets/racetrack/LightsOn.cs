using UnityEngine;
using System.Collections;

class LightsOn : MonoBehaviour
{
	void Start()
	{
		StartCoroutine("SwitchItUp");
	}
	
	IEnumerator SwitchItUp()
	{
		gameObject.SetActive(false);
		yield return new WaitForSeconds(1f);
		gameObject.SetActive(true);
		yield return new WaitForSeconds(1f);
	}
}