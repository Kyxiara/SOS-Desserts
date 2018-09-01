﻿using System.Collections;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{

    private Rigidbody2D myRigidbody;

    private Animator myAnimator;

    [SerializeField]
    private float speed;

    private bool facingRight;

    [SerializeField]
    private Transform[] groundPoints;

    [SerializeField]
    private float groundRadius;

    [SerializeField]
    private LayerMask whatIsGround;

    private bool isGrounded;

    private bool jump;

    [SerializeField]
    private GameObject sugarPrefab;

    [SerializeField]
    private GameObject lifePrefab;

    [SerializeField]
    private float jumpForce;

    [SerializeField]
    private int health;

    private bool flashActive;

    [SerializeField]
    private float flashLength;

    private float flashCounter;

    private SpriteRenderer playerSprite;

    private GameObject[] lifes = new GameObject[5];

    void Start ()
    {
        facingRight = true;
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        playerSprite = GetComponent<SpriteRenderer>();
        CreateLifes();
    }

    private void Update()
    {
        HandleInput();
    }

    void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        isGrounded = IsGrounded();
        if (!IsDead())
        {
            HandleMovement(horizontal);
            Flip(horizontal);
        }
        HandleLayers();
        ResetValues();
        if (flashActive) SystemFlash();

    }

    private void CreateLifes()
    {
        for (int i = 0; i < 5; i++)
        {
            lifes[i] = Instantiate(lifePrefab, new Vector3(i * 40.0F - 375.0f, 115 + 3.5f, 0), Quaternion.identity);
            lifes[i].transform.SetParent(GameObject.FindGameObjectWithTag("FixedScreen").transform, false);
        }
    }

    private void SystemFlash()
    {
        if (flashCounter > flashLength * .80f)
            playerSprite.color = new Color(playerSprite.color.r, playerSprite.color.g, playerSprite.color.b, 0f);
        else if (flashCounter > flashLength * .60f)
            playerSprite.color = new Color(playerSprite.color.r, playerSprite.color.g, playerSprite.color.b, 1f);
        else if (flashCounter > flashLength * .40f)
            playerSprite.color = new Color(playerSprite.color.r, playerSprite.color.g, playerSprite.color.b, 0f);
        else if (flashCounter > flashLength * .20f)
            playerSprite.color = new Color(playerSprite.color.r, playerSprite.color.g, playerSprite.color.b, 1f);
        else if (flashCounter > 0f)
            playerSprite.color = new Color(playerSprite.color.r, playerSprite.color.g, playerSprite.color.b, 0f);
        else
        {
            playerSprite.color = new Color(playerSprite.color.r, playerSprite.color.g, playerSprite.color.b, 1f);
            flashActive = false;
        }
        flashCounter -= Time.deltaTime;
    }

    private void HandleMovement(float horizontal)
    {
        if (myRigidbody.velocity.y < 0)
        {
            myAnimator.SetBool("land", true);
        }
        if (isGrounded && jump)
        {
            isGrounded = false;
            myRigidbody.AddForce(new Vector2(0, jumpForce));
            myAnimator.SetTrigger("jump");
        }
        myRigidbody.velocity = new Vector2(horizontal * speed, myRigidbody.velocity.y);
        myAnimator.SetFloat("speed", Mathf.Abs(horizontal));
    }

    private void HandleInput()
    {
        if (!IsDead())
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                jump = true;
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Shoot(0);
            }
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    private void Flip(float horizontal)
    {
        if (horizontal > 0 && !facingRight || horizontal < 0 && facingRight)
        {
            facingRight = !facingRight;
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }

    private void ResetValues()
    {
        jump = false;
    }

    private bool IsGrounded()
    {
        if (myRigidbody.velocity.y <= 0)
        {
            foreach (Transform point in groundPoints)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(point.position, groundRadius, whatIsGround);
                for (int i = 0; i < colliders.Length; i++)
                {
                    if (colliders[i].gameObject != gameObject)
                    {
                        myAnimator.ResetTrigger("jump");
                        myAnimator.SetBool("land", false);
                        return true;
                    }
                }
            }
        }
        return false;
    }

    private void HandleLayers()
    {
        if (!isGrounded)
        {
            myAnimator.SetLayerWeight(1, 1);
        }
        else
        {
            myAnimator.SetLayerWeight(1, 0);
        }
    }

    private void Shoot(int value)
    {
        if (facingRight)
        {
            GameObject tmp = Instantiate(sugarPrefab, new Vector3(transform.position.x + 0.558f, transform.position.y - 0.611f, transform.position.z), Quaternion.identity);
            tmp.GetComponent<Sugar>().Initialize(Vector2.right);
        }
        else
        {
            GameObject tmp = Instantiate(sugarPrefab, new Vector3(transform.position.x - 0.558f, transform.position.y - 0.611f, transform.position.z), Quaternion.identity);
            tmp.GetComponent<Sugar>().Initialize(Vector2.left);
        }
    }

    public IEnumerator TakeDamage()
    {
        health -= 1;
        Destroy(lifes[health]);
        if (!IsDead())
        {
            flashActive = true;
            flashCounter = flashLength;
        }
        else
        {
            myRigidbody.velocity = Vector3.zero;
            myAnimator.SetTrigger("die");
            yield return null;
        }
    }

    public bool IsDead()
    {
        return health <= 0;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Carot")
        {
            StartCoroutine(TakeDamage());
        }
    }
}
