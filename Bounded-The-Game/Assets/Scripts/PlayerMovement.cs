using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    CharacterController characterController;
    Vector3 tempMovement;
    public float gravity = -9.81f;
    public float groundedGravity = -2f;
    float gravityTracker = 0;
    float moveSpeed = 5f;
    float jumpForce = 10f;

    public AudioClip winClip;
    AudioSource audioSource;
    bool hasWon = false;
    public AudioClip treasureClip;

    int points = 0;

    public CameraFollow CameraFollow;
    Vector3 camForward;
    Vector3 camRight;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        audioSource = GetComponent<AudioSource>();
        // Cursor.visible = false;
        // Cursor.lockState = CursorLockMode.Locked;

    }

    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        camForward = Camera.main.transform.forward;
        camRight = Camera.main.transform.right;
        camForward.y = 0;
        camRight.y = 0;

        Vector3 moveDir = (camForward * v + camRight * h).normalized;



        Vector3 finalMove = moveDir * moveSpeed + new Vector3(0, gravityTracker, 0);
        characterController.Move(finalMove * Time.deltaTime);

        if (camForward.magnitude > 0.1f)
        {
            transform.rotation = Quaternion.LookRotation(camForward);
        }

        if (characterController.isGrounded)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                gravityTracker = jumpForce;
            }
            else
            {
                gravityTracker = groundedGravity;
            }
        }
        else
        {
            gravityTracker += gravity * Time.deltaTime;
        }


    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Objective"))
        {
            Collider treasureCollider = hit.gameObject.GetComponent<Collider>();
            if (treasureCollider != null && treasureCollider.enabled)
            {
                treasureCollider.enabled = false;
                Destroy(hit.gameObject);
                points++;
                Debug.Log(points);

                // if (points < 3)
                // {
                //     audioSource.PlayOneShot(treasureClip);
                // }

            }
        }

        // if (points == 3 && !hasWon)
        // {
        //     hasWon = true;
        //     StartCoroutine(DelayReset());
        // }

    }

    public void ResetGame()
    {
        // SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    IEnumerator DelayReset()
    {
        audioSource.PlayOneShot(winClip);
        yield return new WaitForSeconds(3f);
        ResetGame();
    }



    public void SimulateGravity()
    {
        if (characterController.isGrounded)
        {
            gravityTracker = -1;
        }

        gravityTracker += gravity * Time.deltaTime;

        characterController.Move(new Vector3(0, gravityTracker, 0) * Time.deltaTime);
    }

    public void Move(Vector3 unitMovement)
    {
        GetComponent<CharacterController>().Move(unitMovement * 5f * Time.deltaTime);
    }

}