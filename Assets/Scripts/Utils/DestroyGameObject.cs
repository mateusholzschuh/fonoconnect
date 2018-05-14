using UnityEngine;

public class DestroyGameObject : MonoBehaviour {

    public float timeUntilDestroy = 1;

    public void Start() {
        Destroy(this.gameObject, timeUntilDestroy);
    }

}
