using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

    public Animator animator;
    public Rigidbody2D playerRB;
    public Transform GroundCheck;
    public LayerMask whatIsGround;
    public Transform playerCollider;

    public AudioSource audio;
    public AudioClip soundJump;
    public AudioClip soundSlide;

    private bool sliding;
    private bool grounded;
    public int forceJump;
    public float slideDuration;
    private float timeTemporary;
    public static bool died;
    
    //Used to down the collider when is sliding
    private float offsetColliderWhenSlide = 0.3f;

    // Use this for initialization
    void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        //Trigger to jump - if it's on the ground
        if (Input.GetButtonDown("Jump") && grounded) {
            playerRB.AddForce(new Vector2(0, forceJump));
            playerRB.velocity = new Vector2(0, 0); // n estava aqui antes
            audio.PlayOneShot(soundJump); //Play audio
            audio.volume = 1;
            sliding = false;
        }
        //Trigger to slide - if it's in the ground and isn't sliding
        else if (Input.GetButtonDown("Slide") && grounded && !sliding) {
            //Change the y-axis of the collisor - down the collisor
            playerCollider.position = new Vector3(playerCollider.position.x, playerCollider.position.y - offsetColliderWhenSlide, playerCollider.position.z);
            audio.PlayOneShot(soundSlide); //Play audio
            audio.volume = 0.5f;
            sliding = true;
            timeTemporary = 0; //Restart counter
        }

        //Check if it's on the ground - if it is colliding then true else false
        grounded = Physics2D.OverlapCircle(GroundCheck.position, 0.2f, whatIsGround);

        //Wait for the slide duration
        if (sliding) { 
            timeTemporary += Time.deltaTime; //Add time counter
            //If has passed the duration then stops the animation
            if (timeTemporary >= slideDuration) {
                //Stops the 'slide' animation
                sliding = false;
                //When it stops sliding, change the y-axis of the collisor - up the collisor
                playerCollider.position = new Vector3(playerCollider.position.x, playerCollider.position.y + offsetColliderWhenSlide, playerCollider.position.z);
            }
        }

        //Set the animations
        animator.SetBool("jump", !grounded);
        animator.SetBool("slide", sliding);
    }
}
