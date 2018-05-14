using UnityEngine;

public class MoveObject : MonoBehaviour {

    public float speedX;
    public float speedY;

	void Update () {
        transform.position = new Vector3(transform.position.x + speedX * Time.deltaTime,
                                         transform.position.y + speedY * Time.deltaTime);
	}
}
