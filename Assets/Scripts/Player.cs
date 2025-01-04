using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Vector3 currentJumpVelocity;
    bool isJumping;

    public float speed = 5f;
    public float boostedSpeed = 10f; 
    public float boostDuration = 3f;
    private float currentSpeed; 
    private Coroutine boostCoroutine;



    IEnumerator SpeedBoost()
    {
        currentSpeed = boostedSpeed;
        yield return new WaitForSeconds(boostDuration);
        currentSpeed = speed;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<Pickable>())
        {
            other.gameObject.SetActive(false);
             if (boostCoroutine != null)
            {
                StopCoroutine(boostCoroutine);
            }
                boostCoroutine = StartCoroutine(SpeedBoost());
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        currentSpeed = speed;
    }

    // Update is called once per frame
    void Update()
    {
        CharacterController controller = GetComponent<CharacterController>();
        Vector3 moveVelocity = Vector3.zero;
        
        //moveVelocity.x = Input.GetAxis("Horizontal");  // deleted will be ignored
        moveVelocity = transform.forward * Input.GetAxis("Vertical") * currentSpeed;

        // Turn LEFT
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Vector3 currentRotation = transform.eulerAngles;
            currentRotation.y = currentRotation.y-90f;
            transform.eulerAngles = currentRotation;
        }
        //

        //Turn Right
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            Vector3 currentRotation = transform.eulerAngles;
            currentRotation.y = currentRotation.y+90f;
            transform.eulerAngles = currentRotation;
        }
        //

        if(Input.GetButtonDown("Jump"))
        {
            if(!isJumping)
            {
                isJumping = true;
                currentJumpVelocity = Vector3.up * 5;
            }

        }
        if(isJumping)
        {
            controller.Move((moveVelocity + currentJumpVelocity) * Time.deltaTime);
            currentJumpVelocity += Physics.gravity * Time.deltaTime;

            if(controller.isGrounded)
            {
                isJumping = false;
            }
        }
        else
        {
            controller.SimpleMove(moveVelocity);
        }
    }
}
