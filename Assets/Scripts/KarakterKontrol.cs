using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KarakterKontrol : MonoBehaviour
{

    public int speed;
    public int jumpSpeed;
    public int damage;

    public bool canJump = true;
    public bool faceRight = true;
    public bool canAttack;

    public Vector2 forward;
    public Vector3 offset;

    public GameObject magBallR,magBallL;
    Vector2 magBallPos;
    public float magBallOffset;
    public float fireRate = 0.5f;
    float nextFire = 0.0f;

    Animator animator;
    Rigidbody2D rb;

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        float moveInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);

        if(moveInput>0 || moveInput < 0)
        {
            animator.SetBool("isRunning", true);
        }
        else
        {
            animator.SetBool("isRunning", false);
        }

        if(faceRight==true && moveInput < 0)
        {
            Flip();
        }
        else if(faceRight==false && moveInput>0)
        {
            Flip();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();

        }
        if (Input.GetKeyDown(KeyCode.F)&& canAttack)
        {
            Attack();
        }
        if (Input.GetKeyDown(KeyCode.G) && Time.time>nextFire)
        {
            nextFire = Time.time + fireRate;
            Fire();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Platform")
        {
            canJump = true;
        }
        else if (collision.transform.tag == "Enemy")
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position + offset, forward, 0.5f);
            hit.transform.GetComponent<KarakterStat>().Damage(1);
        }
    }

    private void Jump()
    {
        if (canJump)
        {
            rb.AddForce(Vector2.up * jumpSpeed);
            canJump = false;
        }
    }

    private void Flip()
    {
        faceRight = !faceRight;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }

    private void Attack()
    {
        canAttack = false;

        if (!faceRight)
        {
            forward = transform.TransformDirection(Vector2.right * -1);
        }
        else
        {
            forward = transform.TransformDirection(Vector2.right * 1);
        }

        RaycastHit2D hit = Physics2D.Raycast(transform.position + offset, forward, 2.0f);

        if (hit)
        {
            if (hit.transform.tag == "Enemy")
            {
                hit.transform.GetComponent<Enemy>().GetDamage(damage);
            }
        }

        animator.SetTrigger("attack");
        StartCoroutine(AttackDelay());

    }

    IEnumerator AttackDelay()
    {
        yield return new WaitForSeconds(0.3f);
        canAttack = true;
    }

    private void Fire()
    {
        magBallPos = transform.position;
        if (faceRight)
        {

            magBallPos += new Vector2(0.2f,magBallPos.y+ magBallOffset);
            Instantiate(magBallR, magBallPos, Quaternion.identity);

        }
        else
        {
            magBallPos += new Vector2(-0.2f,magBallPos.y+ magBallOffset);
            Instantiate(magBallL,magBallPos,Quaternion.identity);
        }
        animator.SetTrigger("magicAttack");
        StartCoroutine(magAttackDelay());

       
    }

    IEnumerator magAttackDelay()
    {
        yield return new WaitForSeconds(0.3f);
        
    }
}
