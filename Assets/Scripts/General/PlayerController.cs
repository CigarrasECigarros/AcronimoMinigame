using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    [Header("----Input System----")]
    [SerializeField] private InputActionAsset inputActions;
    private InputAction playerMove;
    private InputAction movePosition;

    [Header("----Navigation----")]
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private bool canMove;
    [SerializeField] private bool movingRight;
    [SerializeField] private Animator animator;
    [SerializeField] private Vector3 currentPosition;
    [SerializeField] private Vector3 destination;
    [SerializeField] private GameObject wayPoint;

    private void OnEnable()
    {
        inputActions.FindActionMap("Player").Enable();
    }

    private void OnDestroy()
    {
        playerMove.performed -= PlayerNavigation;
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        playerMove = InputSystem.actions.FindAction("Move");
        movePosition = InputSystem.actions.FindAction("MovePosition");

        playerMove.performed += PlayerNavigation;
    }

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.angularSpeed = 0;
        canMove = true;
        movingRight = true;

        animator = GetComponentInChildren<Animator>();
        wayPoint = GameObject.Find("WayPoint");
        wayPoint.SetActive(false);
    }

    private void Update()
    {
        animator.SetBool("moving", agent.velocity.magnitude > 0.1f);

        HideWayPoint();
    }

    private void PlayerNavigation(InputAction.CallbackContext context)
    {
        if (canMove)
        {
            StartCoroutine(NavigationCoroutine());
        }
        else return;
    }

    private IEnumerator NavigationCoroutine()
    {
        yield return null;
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            Ray ray = Camera.main.ScreenPointToRay(movePosition.ReadValue<Vector2>());
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (((1 << hit.collider.gameObject.layer) & groundLayer) != 0)
                {
                    currentPosition = transform.position;
                    destination = hit.point;
                    HandleFacingDirection();
                    HandleWalkingAnimation();
                    agent.SetDestination(hit.point);
                    ShowWayPoint();
                }
            }
        }
    }

    private void HandleFacingDirection()
    {
        if (destination.x > currentPosition.x)
        {
            movingRight = true;
        }
        else
        {
            movingRight = false;
        }
    }

    private void HandleWalkingAnimation()
    {
        if (movingRight)
        {
            animator.SetBool("walkLeft", false);
            animator.SetBool("walkRight", true);
        }
        else
        {
            animator.SetBool("walkRight", false);
            animator.SetBool("walkLeft", true);
        }
    }

    public void SetCanMove(bool moveStatus)
    {
        canMove = moveStatus;
    }

    private void ShowWayPoint()
    {
        wayPoint.transform.position = destination + new Vector3(0, 0.3f, 0);

        if (movingRight)
        {
            wayPoint.transform.eulerAngles = new Vector3(90, 90, 0);
        }
        else
        {
            wayPoint.transform.eulerAngles = new Vector3(90, -90, 0);
        }
        wayPoint.SetActive(true);
    }

    private void HideWayPoint()
    {
        if (agent.remainingDistance < 0.1f && wayPoint.activeInHierarchy)
        {
            wayPoint.SetActive(false);
        }
    }
}
