using UnityEngine;
using UnityEngine.AI;

public class GhostAI : MonoBehaviour
{
    public enum GhostState
    {
        Idle,   // Đứng yên
        Patrol, // Đi tuần
        Chase   // Truy đuổi
    }

    public Transform player; // Nhân vật mà con ma sẽ truy đuổi
    public float patrolSpeed = 2f; // Tốc độ di chuyển khi đi tuần
    public float chaseSpeed = 5f; // Tốc độ di chuyển khi truy đuổi
    public Transform[] patrolPoints; // Các điểm đi tuần
    public float chaseRange = 10f; // Khoảng cách để kích hoạt truy đuổi
    public float chaseDuration = 3f; // Thời gian truy đuổi liên tục
    public float idleDuration = 2f; // Thời gian đứng yên sau khi truy đuổi

    private NavMeshAgent navMeshAgent;
    private int currentPatrolIndex = 0;
    private GhostState currentState = GhostState.Idle;
    private Animator animator;
    private float chaseTimer = 0f;
    private float idleTimer = 0f;
    private bool isChasingContinuously = false; // Kiểm tra xem có đang trong chế độ chase liên tục không

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        // Khởi tạo trạng thái ban đầu
        SetState(GhostState.Idle);
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Kiểm tra và chuyển đổi trạng thái dựa trên khoảng cách
        if (distanceToPlayer < chaseRange && currentState != GhostState.Chase)
        {
            SetState(GhostState.Chase);
        }
        else if (distanceToPlayer > chaseRange && currentState == GhostState.Chase)
        {
            SetState(GhostState.Patrol);
        }

        // Xử lý logic cho từng trạng thái
        switch (currentState)
        {
            case GhostState.Idle:
                Idle();
                break;
            case GhostState.Patrol:
                Patrol();
                break;
            case GhostState.Chase:
                Chase();
                break;
        }
    }

    void SetState(GhostState newState)
    {
        if (currentState == newState) return;

        currentState = newState;

        // Cập nhật Animator dựa trên trạng thái mới
        animator.SetBool("isIdle", currentState == GhostState.Idle);
        animator.SetBool("isPatrol", currentState == GhostState.Patrol);
        animator.SetBool("isChasing", currentState == GhostState.Chase);

        // Điều chỉnh tốc độ của NavMeshAgent dựa trên trạng thái
        switch (currentState)
        {
            case GhostState.Idle:
                navMeshAgent.speed = 0;
                break;
            case GhostState.Patrol:
                navMeshAgent.speed = patrolSpeed;
                break;
            case GhostState.Chase:
                navMeshAgent.speed = chaseSpeed;
                chaseTimer = 0f; // Reset chase timer
                break;
        }
    }

    void Idle()
    {
        // Đếm thời gian đứng yên
        idleTimer += Time.deltaTime;

        // Nếu hết thời gian đứng yên, chuyển sang Patrol
        if (idleTimer >= idleDuration)
        {
            idleTimer = 0f;
            SetState(GhostState.Patrol);
        }
    }

    void Patrol()
    {
        if (patrolPoints.Length == 0) return;

        // Di chuyển đến điểm tuần tra tiếp theo
        navMeshAgent.SetDestination(patrolPoints[currentPatrolIndex].position);

        // Kiểm tra nếu đã đến điểm tuần tra hiện tại
        if (navMeshAgent.remainingDistance < 0.1f && !navMeshAgent.pathPending)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
        }
    }

    void Chase()
    {
        // Di chuyển về phía nhân vật
        navMeshAgent.SetDestination(player.position);

        // Đếm thời gian truy đuổi
        chaseTimer += Time.deltaTime;

        // Nếu hết thời gian truy đuổi
        if (chaseTimer >= chaseDuration)
        {
            if (isChasingContinuously)
            {
                // Chuyển sang Idle trong một khoảng thời gian
                SetState(GhostState.Idle);
            }
            else
            {
                // Quay lại Patrol
                SetState(GhostState.Patrol);
            }

            // Đảo trạng thái chase liên tục
            isChasingContinuously = !isChasingContinuously;
        }
    }
}