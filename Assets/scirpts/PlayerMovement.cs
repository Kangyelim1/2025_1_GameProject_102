using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Runtime.InteropServices;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("기본 이동 설정")]
    public float moveSpeed = 5.0f;
    public float jumpForce = 7.0f;
    public float turnSpeed = 10f;

    [Header("점프 개선 설정")]
    public float fallMultiplier = 2.5f;
    public float lowjumpMultiplier = 2.0f;

    [Header("지면 감지 설정")]
    public float coyoTime = 0.15f;
    public float coyoteTimCounter;
    public bool realGrouned = true;

    [Header("글라이더 설정")]
    public GameObject gliderObject;
    public float gliderFallSpeed = 1.0f;
    public float gliderMoveSpeed = 7.0f;
    public float gliderMaxTime = 5.0f;
    public float gliderTimeLeft;
    public bool isGliding = false;

    public bool isGrounded = true;

    public int coinCcunt = 0;
    public int totalCoins = 5;

    public Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        if (gliderObject != null)
        {
            gliderObject.SetActive(false);
        }
        gliderTimeLeft = gliderMaxTime;

        coyoteTimCounter = 0;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateGroundedState();

        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0, moveVertical);
        if (movement.magnitude > 0.1f)
        {
            Quaternion targeeRotation = Quaternion.LookRotation(movement);
            transform.rotation = Quaternion.Slerp(transform.rotation, targeeRotation, turnSpeed * Time.deltaTime);
        }

        if(Input.GetKey(KeyCode.G) && !isGrounded && gliderTimeLeft > 0)
        {
            if(!isGliding)
            {
                //글라이더 활설화 함수 (아래 정의)
                EnableGlider();
            }

            gliderTimeLeft -= Time.deltaTime;

            if(gliderTimeLeft <=0)
            {
                //글라이더 비활성화 함수 (아래  정의)
                DisableGlider();
            }
            else if (isGliding)
            {
                //G키를 때면 글라이더 비활성화
                DisableGlider();
            }

            if(isGliding)
            {
                ApplyGliderMovement(moveHorizontal, moveVertical);
            }
            else
            {
                rb.velocity  = new Vector3(moveHorizontal * moveSpeed, rb.velocity.y, moveVertical * moveSpeed);
                if (rb.velocity.y < 0)
                {
                    rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
                }
                else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
                {
                    rb.velocity += Vector3.up * Physics.gravity.y * (lowjumpMultiplier - 1) * Time.deltaTime;
                }
            }

            if(isGliding)
            {
                if (isGliding)
                {
                    DisableGlider();
                }

                gliderTimeLeft = gliderMaxTime;
            }
                   
        }


        rb.velocity = new Vector3(moveHorizontal * moveSpeed, rb.velocity.y, moveVertical * moveSpeed);

        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if(rb.velocity.y > 0 && !Input.GetButton("Jump"))
         {
            rb.velocity += Vector3.up * Physics.gravity.y * (lowjumpMultiplier - 1) * Time.deltaTime;
         }


        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
            realGrouned = false;
            coyoteTimCounter = 0;
        }


    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
           realGrouned = true;
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            realGrouned = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            realGrouned = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coin"))
        {
            coinCcunt++;
            Destroy(other.gameObject);
            Debug.Log($"코인 수집 : {coinCcunt}/{totalCoins}");

        }

        if(other.CompareTag("Door") && coinCcunt >= totalCoins)
        {
            Debug.Log("게임 클리어");
        }
    }

    void UpdateGroundedState()
    {
        if(realGrouned)
        {
            coyoteTimCounter = coyoTime;
            isGrounded = true;
        }
        else 
        {
            isGrounded = false;
        }

    }

    void EnableGlider()
    {
        isGliding = true;

        if (gliderObject != null)
        {
            gliderObject.SetActive(true);
        }

        rb.velocity = new Vector3(rb.velocity.x, -gliderFallSpeed, rb.velocity.z);
    }

    void DisableGlider()
    {
        isGliding = false;

        if(gliderObject != null)
        {
            gliderObject.SetActive(false);
        }

        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
    }

    void ApplyGliderMovement(float horizontal , float vertical)
    {
        Vector3 gliderVelocity = new Vector3(horizontal * gliderMoveSpeed, gliderFallSpeed, vertical * gliderMoveSpeed);
        rb.velocity = gliderVelocity;
    }
}
