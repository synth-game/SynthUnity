using UnityEngine;
using System.Collections;

public class SimplePlatformController : MonoBehaviour
{

	[HideInInspector] public bool facingRight = true;
	[HideInInspector] public bool jump = false;
	public float moveForce = 100f;
	public float maxSpeed = 5f;
	public float jumpForce = 10f;
	public Transform groundCheck;


	private bool grounded = false;
	private Animator anim;
	private Rigidbody2D rb2d;

	private bool crawling = false;


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


		if (Input.GetKeyUp("c"))
		{
			crawling = !crawling;
		}

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

		// Crawl
		Vector2 groundDir = Vector2.right;
		if (crawling)
		{
			rb2d.gravityScale = 0f;
			RaycastHit2D lrc = Physics2D.Raycast(transform.position - transform.right * 0.5f,
										-transform.up,
										15f);
			RaycastHit2D rrc = Physics2D.Raycast(transform.position + transform.right * 0.5f,
										-transform.up,
										15f);
			Debug.DrawLine(lrc.point, rrc.point, Color.red, 1f, false);

			groundDir = (lrc.point - rrc.point).normalized;
			Vector2 n = (new Vector2(-groundDir.y, groundDir.x)).normalized;
			Debug.DrawRay(transform.position, n, Color.red);

			float angle = Vector3.SignedAngle(-n, transform.up, Vector3.forward);
			transform.Rotate(new Vector3(0,0,1), -angle);

			rb2d.AddForce(n * 30f);
		} else
		{
			rb2d.gravityScale = 1f;
		}
		// end crawl

		if (h * rb2d.velocity.x < maxSpeed)
		{
			if (crawling)
			{
				rb2d.AddForce(groundDir * h * moveForce);
			} else
			{
				rb2d.AddForce(Vector2.right * h * moveForce);
			}
			
		}
			

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