using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class player : MonoBehaviour
{
    public int health = 3;
    public static float movement;
    public float speed;
    public float jumpForce;

    private Vector3 respawnPoint;
    public GameObject fallDetector;
    
    private bool isJumping;
    private bool isAtk;

    private Rigidbody2D rig;
    private Animator anim;

    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        
        GameController.instance.UpdateLives(health);

        respawnPoint = transform.position;
    }

    void Update()
    {
        Move();
        Jump();

        if (Input.GetKeyDown(KeyCode.X))
        {
            StartCoroutine("Attack");
        }
    }

    void Move()
    {
        movement = Input.GetAxis("Horizontal");

        rig.velocity = new Vector2(movement * speed, rig.velocity.y);

        if (movement > 0)
        {
            if (!isJumping && !isAtk)
            {
                anim.SetInteger("transition", 1);
            }
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else if (movement < 0)
        {
            if (!isJumping && !isAtk)
            {
                anim.SetInteger("transition", 1);
            }
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else if (movement == 0 && !isJumping && !isAtk)
        {
            anim.SetInteger("transition", 0);
        }
    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (!isJumping && !isAtk)
            {
                anim.SetInteger("transition", 2);
                rig.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
                isJumping = true;
            }
        }
    }

    IEnumerator Attack()
    {
        isAtk = true;
        anim.SetInteger("transition", 3);

        yield return new WaitForSeconds(1.1f);

        isAtk = false;
        anim.SetInteger("transition", 0);
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.layer == 6)
        {
            isJumping = false;
        }
    }

    public void Damage(int dmg)
    {
        health -= dmg;
        GameController.instance.UpdateLives(health);
        anim.SetTrigger("hit");

        if (transform.rotation.y == 0)
        {
            transform.position += new Vector3(-2,0,0);
        }    
        
        if (transform.rotation.y == 180)
        {
            transform.position += new Vector3(2,0,0);
        } 

        if (health <= 0)
        {
            GameController.instance.GameOver();
        }
    }

    public void IncreaseLife(int value)
    {
        health += value;
        GameController.instance.UpdateLives(health);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "FallDetector")
        {
            transform.position = respawnPoint;
        }
        else if (col.tag == "Checkpoint")
        {
            respawnPoint = transform.position;
        }
    }
}






