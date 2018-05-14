using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveOffset : MonoBehaviour
{
    public float speed;
    private float offset;
    private Material currentMaterial;

    void Start()
    {
        currentMaterial = GetComponent<Renderer>().material; //Load the material from the current GameObject
    }
    
    void Update()
    {
        offset += speed * Time.deltaTime;    //Adaptive speed based in the framerate
        currentMaterial.SetTextureOffset("_MainTex", new Vector2(offset, 0));
    }
}
