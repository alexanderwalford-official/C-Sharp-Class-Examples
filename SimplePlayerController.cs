using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]

public class SimplePlayerController : MonoBehaviour
{
    public Camera playerCameraidle;
    public Camera playerCamerawalk;
    public Camera playerCameraaim;
    public float walkSpeed = 1.15f;
    public float runSpeed = 4.0f;
    public float lookSpeed = 2.0f;
    public float lookXLimit = 60.0f;
    public float gravity = 150.0f;
    public float crouchdistance = 0.5f;
    public GameObject crouchingicon;
    public GameObject shootingingicon;
    bool iscrouched = false;
    bool isholdinggun = false;
    bool iswalking = false;
    public AudioSource footstepsource;

    public GameObject PlayerIdleAnim;
    public GameObject PlayerWalkAnim;
    public GameObject PlayerAimAnim;
    public bool GunPickedUp = false;

    CharacterController characterController;
    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;
    private bool canMove = true;

    void OnEnable()
    {
        Start();
    }

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        StartCoroutine(StepWait());
    }

    void Update()
    {
        // camera and player character movement
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float curSpeedX = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Vertical") : 0;
        float curSpeedY = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Horizontal") : 0;
        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        // ground check
        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }
        if (!isholdinggun)
        {
            characterController.Move(moveDirection * Time.deltaTime);
        }

        if (canMove)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamerawalk.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            playerCameraidle.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            playerCameraaim.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }

        // walking

        if (Input.GetKey(KeyCode.W) && !isholdinggun)
        {
            PlayerWalkAnim.SetActive(true);
            PlayerIdleAnim.SetActive(false);
            PlayerAimAnim.SetActive(false);
            iswalking = true;
        }

        else if (Input.GetKey(KeyCode.A) && !isholdinggun)
        {
            PlayerWalkAnim.SetActive(true);
            PlayerIdleAnim.SetActive(false);
            PlayerAimAnim.SetActive(false);
            iswalking = true;
        }

        else if (Input.GetKey(KeyCode.S) && !isholdinggun)
        {
            PlayerWalkAnim.SetActive(true);
            PlayerIdleAnim.SetActive(false);
            PlayerAimAnim.SetActive(false);
            iswalking = true;         
        }

        else if (Input.GetKey(KeyCode.D) && !isholdinggun)
        {
            PlayerWalkAnim.SetActive(true);
            PlayerIdleAnim.SetActive(false);
            PlayerAimAnim.SetActive(false);
            iswalking = true;
        }
        // idle
        else if (!isholdinggun)
        {
            PlayerIdleAnim.SetActive(true);
            PlayerWalkAnim.SetActive(false);
            PlayerAimAnim.SetActive(false);
            iswalking = false;
            isholdinggun = false;
        }

        // crouching
        if (Input.GetKeyDown(KeyCode.C) && !isholdinggun)
        {
            if (iscrouched)
            {
                playerCameraidle.transform.position = new Vector3(playerCameraidle.transform.position.x, playerCameraidle.transform.position.y + crouchdistance, playerCameraidle.transform.position.z);
                iscrouched = false;
                iswalking = false;
                crouchingicon.SetActive(false);
            }
            else
            {
                playerCameraidle.transform.position = new Vector3(playerCameraidle.transform.position.x, playerCameraidle.transform.position.y - crouchdistance, playerCameraidle.transform.position.z);
                //playerCamerawalk.transform.position = new Vector3(playerCamerawalk.transform.position.x, playerCamerawalk.transform.position.y + crouchdistance, playerCamerawalk.transform.position.z);

                iscrouched = true;
                crouchingicon.SetActive(true);
            }

        }

        // pistol aim
        if (Input.GetKeyDown(KeyCode.Alpha1) && GunPickedUp)
        {
            // pressed 1
            if (isholdinggun)
            {
                PlayerIdleAnim.SetActive(true);
                PlayerWalkAnim.SetActive(false);
                PlayerAimAnim.SetActive(false);
                shootingingicon.SetActive(false);
                isholdinggun = false;
            }
            else
            {
                PlayerIdleAnim.SetActive(false);
                PlayerWalkAnim.SetActive(false);
                PlayerAimAnim.SetActive(true);
                shootingingicon.SetActive(true);
                isholdinggun = true;
            }

        }
    }

    // step sound fx
    protected IEnumerator StepWait()
    {
        if (iswalking)
        {
            footstepsource.pitch = Random.Range(0.15f, 0.1f);
            footstepsource.Play();
        }
        yield return new WaitForSeconds(Random.Range(2, 3));
        StartCoroutine(StepWait());
    }

}
