/*

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Playables;
using UnityEngine;
using UnityEngine.AI;


public class Enemy : MonoBehaviour
{
    public bool dead; //사망상태



    private enum State   // 상태 지정
    {
        Patrol, Tracking, AttackBegegin, Attacking
    }

    public float startingHealth = 100f; //초기체력
    public float currentHealth;



    private State state; //상태지정
    private NavMeshAgent EnemyNav;
    private Animator EnemyAni;

    public Transform attackRoot;
    public Transform eyeTransform; //적 캐릭터 시야기준점

    private AudioSource audioPlayer;
    public AudioClip hitClip;
    public AudioClip deatClip;


    public float runSpeed = 10f; //적 캐릭터 이동 속도
    [Range(0.01f, 2f)] public float turnSmoothTime = 0.1f; //방향 회전 지연시간
    private float turnSmoothVelocity; //현재 회전의 실시간 변화량

    public float damage = 30f; //적 캐릭터의 공격력
    public float attackRadius = 2f; //적 캐릭터 공격 반경
    private float attackDistance; //공격을 시도하는 거리

    public float fov = 50f; //적 캐릭터 시야각
    public float viewDistance = 10f; //적 캐릭터  시야거리
    public float patrolSpeed = 3f; //정찰 상태의 속도

    public GameObject Player; //타겟

    public LayerMask whatIsTarget; //적을 감지할때 사용할 레이어 필터

    private RaycastHit[] hits = new RaycastHit[10];

    private void OnDrawGizmosSelected() // 선택했을때 매 프레이 실행
    {
        if (attackRoot != null)
        {
            Gizmos.color = Color.red;


            Gizmos.DrawWireSphere(attackRoot.position, attackRadius); //기준점에서, 공격범위까지 그려줌

        }

        var leftEyeRotation = Quaternion.AngleAxis(-fov * 0.5f, Vector3.up); //왼쪽 끝지점을 향하는 각도
        var leftRayDirection = leftEyeRotation * transform.forward; //바라보는 방향

        Handles.color = new Color(1f, 1f, 1f, 0.2f);
        Handles.DrawSolidArc(eyeTransform.position, Vector3.up, leftRayDirection, fov, viewDistance);



    }

    private void Awake()
    {
        EnemyNav = GetComponent<NavMeshAgent>();
        EnemyAni = GetComponent<Animator>();
        audioPlayer = GetComponent<AudioSource>();

        var attackPivot = attackRoot.position;

        attackPivot.y = transform.position.y;

        attackDistance = Vector3.Distance(transform.position, attackRoot.position) + attackRadius;

        EnemyNav.stoppingDistance = attackDistance;
        EnemyNav.speed = patrolSpeed;
        currentHealth = startingHealth;
    }


    void Start()
    {
        StartCoroutine(UpdatePath());
    }

    void Update()
    {

    }
    private IEnumerator UpdatePath()
    {
        while (!dead) //적이 살아 있을때
        {

            if (findplayer) //플레이어를 발견했을때
            {
                if (state == State.Patrol)
                {
                    state = State.Tracking;
                    EnemyNav.speed = runSpeed;
                }
                EnemyNav.SetDestination(Player.transform.position);
            }
            else //플레이어를 발견못할경우
            {
                if (state != State.Patrol)
                {
                    state = State.Patrol;
                    EnemyNav.speed = patrolSpeed;
                }
                if (EnemyNav.remainingDistance <= 1f) //1미터 이내일때만 새로운 포지션 지정
                {
                    var patrolTargetPoSition = RandomPos.GetRandomPoInitOnNavMesh(transform.position, 20f, NavMesh.AllAreas); //현재위치에서 20만큼의 거리만큼 랜덤한 위치를 찍음 
                    EnemyNav.SetDestination(Player.transform.position);
                }
                var colliders = Physics.OverlapSphere(eyeTransform.position, viewDistance, whatIsTarget);

                foreach (var collider in colliders) //콜라이더내에서 플레이어 찾기
                {
                    if (!IsTargetOnSight(collider.transform))
                    {
                        continue;
                    }

                }

            }

        }
        yield return new WaitForSeconds(0.05f);
    }

    private bool IsTargetOnSight(Transform target)
    {
        RaycastHit hit;

        var direction = target.position - eyeTransform.position;
        direction.y = eyeTransform.forward.y; //수직 방향의 각도차이는 무시
        if (Vector3.Angle(direction, eyeTransform.forward) > fov * 0.5f)
        {
            return false;
        }
        if (Physics.Raycast(eyeTransform.position, direction, out hit, viewDistance, whatIsTarget))
        {
            if (hit.transform == target)
            {
                return true;
            }
        }
        return false;
    }
}

*/