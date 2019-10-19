using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{

    public float velX = 5f;
    public float velY = 0f;
    public int damage;
    Rigidbody2D rb;
    Animator animator;



    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = new Vector2(velX, velY);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Enemy")
        {
            Destroy(gameObject);
            damage = 1;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(1,0) , 1.0f);
            hit.transform.GetComponent<Enemy>().GetDamage(damage);

            animator.SetTrigger("onHit");

        }
    }
}
