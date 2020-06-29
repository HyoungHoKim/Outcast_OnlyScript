using System;
using System.Collections;
using System.Collections.Generic;
using RootMotion.Demos;
using RootMotion.Dynamics;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;


public class EnemyController2 : MonoBehaviour
{
    public float enmeyHealth = 100f;

    private enum State
    {
        Patrol,
        Tracking,
        AttackBegin,
        Attacking,
        Dead,
        Idle,
        Alert,
        Attack
    }

    private State state; //현재 에너미 상황
    public ParticleSystem muzzleFlash;
    
    public NavMeshAgent EnemyNav;
    public Animator EnemyAni;
    public Transform EyeTransform; //눈의 위치
    public AudioSource AudioSource;
    public AudioClip hitClip;
    public AudioClip ShotClip;
    public AudioClip deathClip;
    public float runSpeed = 5f; // 이동 속도
    public float damage = 10f; //권총 공격 데미지
    public float attackDistance = 5f; //공격 시도 범위
    public float fieldOfView = 50f; //시야각
    public float viewDistance = 20f; //시야 범위
    public float partrolSpeed = 3f; //정찰 속도
    public float idelSpeed = 0f;
    public GameObject targetPlayer; //공격 타겟
    public LayerMask whatIsTarget;
    public Transform[] wayPoint = new Transform[3];
    public GameObject enemyBullet;
    public Transform bulletPos;
    public float shotSpeed = 0;
    public PuppetMaster puppetMaster;
    
    public bool hasTarget => targetPlayer != null;
    public float distance;
    private float timer = 0;
    public float shotDelay = 0;
    void Start()
    {
        state = State.Idle;
        EnemyNav.speed = idelSpeed; //처음 속도 = 정찰속도
        //EnemyNav.stoppingDistance = attackDistance; //공격범위에 들어오면 네비메시 정지거리
        StartCoroutine(UpdatePath());
    }

    void Update()
    {
        if (enmeyHealth <= 0)
        {
            state = State.Dead;
            
        }

    
        
   //     Debug.Log(state);
        if (state == State.Patrol)
        {
            
           
            EnemyNav.speed = partrolSpeed;
            EnemyAni.SetFloat("Speed", partrolSpeed);

            if (EnemyNav.remainingDistance < 1f)
            {
                EnemyNav.ResetPath();
                state = State.Idle;
            }
        }

        if (state == State.Idle)
        {
            
            EnemyNav.speed = idelSpeed;

            EnemyAni.SetFloat("Speed", idelSpeed);

            timer += Time.deltaTime;

            if (timer > 5)
            {
                state = State.Patrol;
              //  EnemyNav.SetDestination(wayPoint[Random.Range(0, 3)].transform.position);
                timer = 0;
            }
        }

        if (state == State.Dead) //사망상태라면
        {
            EnemyAni.enabled = false;
            puppetMaster.state = PuppetMaster.State.Dead;
            
            return; //업데이트 종료
        }

        if (state == State.Tracking) //추적상태라면
        {
            distance = Vector3.Distance(targetPlayer.transform.position, transform.position); //플레이어와의 거리 
           
            
           // if (distance <= attackDistance) //플레이어와 거리가 공격거리보다 짧다면
           //{
           //    BeginAttack(); //공격
           //}
           
            if (distance > 20)  //시야에서 벗어남
            {
                state = State.Idle;
                targetPlayer = null;
               //     EnemyNav.stoppingDistance = 0;
               EnemyNav.stoppingDistance = 0;
               
            }
        }
        
        if (state == State.Attack )
        {
            //  transform.LookAt(targetPlayer.transform.position);
            var lookRotation = Quaternion.LookRotation(targetPlayer.transform.position - transform.position); //타겟의 방향
            var targetAngleY = lookRotation.eulerAngles.y; //y축 기준으로 회전

            transform.eulerAngles = Vector3.up * targetAngleY;
            distance = Vector3.Distance(targetPlayer.transform.position, transform.position); //플레이어와의 거리  = 
            EnemyNav.speed = shotSpeed;
                ShotReady();
                if (distance > 5)
                {
                    state = State.Tracking;
                    EnemyAni.SetBool("Shot",false);
                }
             

        }
        
        EnemyAni.SetFloat("Speed", EnemyNav.velocity.magnitude);
    }

    void ShotReady()
    {
        
       // shotDelay += Time.deltaTime;
        
       // if (shotDelay > 3)
       // {
            
            Shot();
     //       shotDelay = 0;
     //   }
        
       
    }
    
   
    
    private void Shot()
    {
        //Instantiate(enemyBullet,bulletPos);
        EnemyAni.SetBool("Shot",true);
        //EnemyAni.SetBool("Shot", false);

    }
    
    

    void FixedUpdate()
    {
       // Debug.Log(distance);
        if (distance < 5 && targetPlayer != null)
        {
            state = State.Attack;
            
        }
        
            
        
        if (state == State.AttackBegin || state == State.Attacking) //공격상태라면
        {
            var lookRotation = Quaternion.LookRotation(targetPlayer.transform.position - transform.position); //타겟의 방향
            var targetAngleY = lookRotation.eulerAngles.y; //y축 기준으로 회전

            transform.eulerAngles = Vector3.up * targetAngleY;
        }
        

        if (state == State.Attacking)
        {
            var direction = transform.forward;
            var deltaDistance = EnemyNav.velocity.magnitude * Time.deltaTime;
        }
    }

    private void OnDrawGizmos() //적의 시야
    {
        var letfEyeRotation = Quaternion.AngleAxis(-fieldOfView * 0.5f, Vector3.up);
        var leftRayDirection = letfEyeRotation * transform.forward;
        //Handles.color = new Color(1f, 0.01f, 0f, 0.3f);
        //Handles.DrawSolidArc(EyeTransform.position, Vector3.up, leftRayDirection, fieldOfView, viewDistance);
    }

    private IEnumerator UpdatePath()
    {
        while (!(state == State.Dead)) //죽은 상태가 아니라면
        {
            if (targetPlayer != null)
            {
                state = State.Tracking; //추적 상태로 변경
                EnemyNav.speed = runSpeed; //달리기 속도로 변경
                EnemyNav.stoppingDistance = attackDistance; 
                EnemyNav.SetDestination(targetPlayer.transform.position); //목표지점을 플레이어로 지정
            }

            else
            {
                targetPlayer = null;
                // EnemyNav.SetDestination(wayPoint[1].transform.position);
                /*
                if (EnemyNav.remainingDistance < 1f) //만약 남은거리가 1이하일때
                {
                    var patrolTargetPosition = GetRandomPoiontOnNavMesh(transform.position, 20f, NavMesh.AllAreas); //현재 위치에서 20반경의 랜덤한 위치를 찍음
                    EnemyNav.SetDestination(patrolTargetPosition); //랜덤 위치로 이동
                }
                */
                var colliders = Physics.OverlapSphere(EyeTransform.position, viewDistance, whatIsTarget); // 눈을 기준으로 시야 거리의 둥근 콜라이더 전부 가져오기

                foreach (var collider in colliders) //모든 콜라이더 검사
                {
                    if (!IsTargetOnSight(collider.transform)) // 시야내에 있는게 아니면
                    {
                        continue; //무시하고 다음으로 넘어간다
                    }

                    var getcollider = collider;


                    if (getcollider.gameObject.CompareTag("PLAYER")) //콜라이더의 오브젝트가 플레이어태그면
                    {
                        targetPlayer = getcollider.gameObject; // 그 오브젝트를 타켓으로 지정하고


                        break; //foreach는 종료
                    }
                }
            }

            yield return new WaitForSeconds((0.2f));
        }
    }
    
    Vector3 GetRandomPoiontOnNavMesh(Vector3 center, float distance, int areaMask)
    {
        var randomPos = Random.insideUnitSphere * distance + center;

        NavMeshHit hit;

        NavMesh.SamplePosition(randomPos, out hit, distance, areaMask);

        return hit.position;
    }
    
    public bool IsTargetOnSight(Transform target) //시야내 플레이어가 있는지 검사
    {
        var direction = target.position - EyeTransform.position; //방향

        if (Vector3.Angle(direction, EyeTransform.forward) > fieldOfView * 0.5f) //시야각에서 벗어난다면
        {
            return false;
        }

        RaycastHit hit;
        if (Physics.Raycast(EyeTransform.position, direction, out hit, viewDistance))
        {
            if (hit.transform == target)

                return true;
        }

        return false;
    }

    public void BeginAttack()
    {
        state = State.AttackBegin;

        // EnemyNav.isStopped = true;
        EnemyAni.SetTrigger("Attack");
    }

    public void EnableAttack()
    {
        state = State.Attacking;
    }

    public void DisableAttack()
    {
        state = State.Tracking;

        EnemyNav.isStopped = false;
    }


    public void ShotEvent()
    {
        muzzleFlash.Play();
        AudioSource.clip = ShotClip;
        AudioSource.Play();
        Instantiate(enemyBullet,bulletPos);
        
    }
    
}