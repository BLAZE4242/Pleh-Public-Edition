using UnityEngine;
using System.Collections;

public class SimplySpaceship : MonoBehaviour {

	float speed = 5f;

	void Update () {
		transform.Translate( new Vector3(
			Input.GetAxis("Horizontal") * speed,
			Input.GetAxis("Vertical") * speed,
			0) * Time.deltaTime );
	}
}
