using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    CharacterController characterController;
    Vector3 tempMovement;
    public float gravity = -9.81f;
    public float groundedGravity = -2f;
    float gravityTracker = 0;
    float moveSpeed = 3f;
    float jumpForce = 10f;

    public AudioClip winClip;
    public AudioClip fireballClip;
    public GameObject fireballPrefab;
    public float fireballSpeed = 10f;
    AudioSource audioSource;
    bool hasWon = false;
    public AudioClip treasureClip;

    int points = 0;

    public CameraFollow CameraFollow;
    Vector3 camForward;
    Vector3 camRight;

    private Animator animator;

    public Texture2D cursorTexture;
    public Vector2 cursorHotspot = Vector2.zero;

    void Start()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        audioSource = GetComponent<AudioSource>();

        if (cursorTexture != null)
        {
            Cursor.SetCursor(cursorTexture, cursorHotspot, CursorMode.Auto);
        }
    }

    public bool IsMovingBackward { get; private set; }
    public bool IsMovingForward { get; private set; } 
    public bool IsMovingLeft { get; private set; }
    public bool IsMovingRight { get; private set; }

    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        if (Input.GetMouseButtonDown(0))
        {
            if ((MainMenuManager._ == null || !MainMenuManager._.container.activeSelf)
                && !(EventSystem.current != null && EventSystem.current.IsPointerOverGameObject()))
            {
                ShootFireball();
            }
        }
        float v = Input.GetAxisRaw("Vertical");
        camForward = Camera.main.transform.forward;
        camRight = Camera.main.transform.right;
        camForward.y = 0;
        camRight.y = 0;

        Vector3 moveDir = (camForward * v + camRight * h).normalized;

        IsMovingForward = (v > 0.01f);
        IsMovingBackward = (v < -0.01f);
        IsMovingLeft = (h < -0.01f);
        IsMovingRight = (h > 0.01f);

        if (IsMovingForward)
        {
            animator.SetBool("isMoving", true);
        }
        else
        {
            animator.SetBool("isMoving", false);
        }

        if (IsMovingBackward)
        {
            animator.SetBool("isMovingBackward", true);
        }
        else
        {
            animator.SetBool("isMovingBackward", false);
            
        }

        if (IsMovingLeft)
        {
            animator.SetBool("isMovingLeft", true);

        }
        else
        {
            animator.SetBool("isMovingLeft", false);
        }

        if (IsMovingRight)
        {
            animator.SetBool("isMovingRight", true);
        }
        else
        {
            animator.SetBool("isMovingRight", false);
        }

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
                animator.SetBool("isJumping", true);
                gravityTracker = jumpForce;
            }
            else
            {
                animator.SetBool("isJumping", false);
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

        if (hit.gameObject.CompareTag("Exit"))
        {
            Debug.Log("Player collided with Exit object");
            SceneManager.LoadScene("MainMenu");
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

    void ShootFireball()
    {
        if (fireballPrefab == null) return;
        Vector3 spawnPos = transform.position + transform.forward * 1.5f + Vector3.up * 1f;
        GameObject fireball = Instantiate(fireballPrefab, spawnPos, Quaternion.identity);
        FireballMovement movement = fireball.GetComponent<FireballMovement>();
        if (movement != null)
        {
            movement.speed = fireballSpeed;
            movement.SetDirection(transform.forward);
        }

        if (fireballClip != null && audioSource != null)
        {
            audioSource.PlayOneShot(fireballClip);
        }
    }

}