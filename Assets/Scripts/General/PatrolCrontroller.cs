using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class PatrolCrontroller : MonoBehaviour
{
    [SerializeField] private Transform[] patrolPoints;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Animator animator;
    [SerializeField] private Transform target;
    [SerializeField] private bool isMoving;
    [SerializeField] private bool movingRight;

    [SerializeField] private Vector3 destination;
    [SerializeField] private Vector3 currentPosition;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        agent.updateRotation = false;
        agent.angularSpeed = 0;
    }

    private void Start()
    {
        currentPosition = transform.position;
        PatrolMovement();
    }

    private void Update()
    {
        animator.SetBool("moving", agent.velocity.magnitude > 0.1f);

        if (isMoving && agent.remainingDistance < 0.1)
        {
            isMoving = false;
            currentPosition = transform.position;
            StartCoroutine(WaitAndChooseTrack());
        }
    }

    private void PatrolMovement()
    {
        destination = target.position;
        HandleFacingDirection();
        HandleWalkingAnimation();
        agent.SetDestination(target.position);
        isMoving = true;
    }

    private IEnumerator WaitAndChooseTrack()
    {
        yield return new WaitForSeconds(1f);
        ChooseNextTrack();
    }

    private void ChooseNextTrack()
    {
        int index = Random.Range(0, patrolPoints.Length);
        var currentTrack = patrolPoints[index];
        if(currentTrack.position == transform.position)
        {
            ChooseNextTrack();
        }
        else
        {
            target.position = currentTrack.position;
            PatrolMovement();
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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            GameManager.instance.LoadSceneByName("GameOver");
        }
    }
}
