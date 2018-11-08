using UnityEngine;
using System.Collections;

public class SimplePlatformController : MonoBehaviour
{

	[HideInInspector] public bool facingRight = true;
	[HideInInspector] public bool jump = false;
	public float moveForce = 365f;
	public float maxSpeed = 5f;
	public float jumpForce = 1000f;
	public Transform groundCheck;


	private bool grounded = false;
	private Animator anim;
	private Rigidbody2D rb2d;


	// Use this for initialization
	void Awake()
	{
		anim = GetComponent<Animator>();
		rb2d = GetComponent<Rigidbody2D>();
	}

	// Update is called once per frame
	void Update()
	{

		grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));
		anim.SetBool("grounded", grounded);

		if (Input.GetButtonDown("Jump") && grounded)
		{
			Debug.Log("jump");
			anim.SetTrigger("jump");
		}
	}

	void FixedUpdate()
	{

		float h = Input.GetAxis("Horizontal");

		anim.SetFloat("walkSpeed",Mathf.Abs(h)*2);

		if (h * rb2d.velocity.x < maxSpeed)
			rb2d.AddForce(Vector2.right * h * moveForce);

		if (Mathf.Abs(rb2d.velocity.x) > maxSpeed)
			rb2d.velocity = new Vector2(Mathf.Sign(rb2d.velocity.x) * maxSpeed, rb2d.velocity.y);

		if (h > 0 && !facingRight)
			Flip();
		else if (h < 0 && facingRight)
			Flip();

		if (jump)
		{
			
		}
	}

	void OnJumpAnim()
	{
		Debug.Log("actual jump");
		rb2d.AddForce(new Vector2(0f, jumpForce));
		jump = false;
	}

	void Flip()
	{
		facingRight = !facingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
}