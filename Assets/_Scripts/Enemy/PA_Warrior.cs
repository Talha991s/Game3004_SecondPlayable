using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PA_Warrior : MonoBehaviour
{
    [SerializeField]


    NavMeshAgent NavAgent;
    public Transform player;
    public LayerMask GroundDetection, PlayerDetection; //Whats ground and whats player. 
    bool IsPointSet; // is the new walking position set?
    public Vector3 PointSet;  // New walking position to go to.
     public float ATKStall; //How long between attacks.
    bool ATKCooldown; //Has Already attacked, chill out!
    private Animator anim;
    public float PointRange; // How far new walking point can be. (Keep Reasonable)
    bool isATKStall;  // Has the enemy attacked yet if able to?, 
    

    public float AggroRange;  // Distance of aggro, detection.
    public float CombatDistance; // How far it can swing.
    public bool IsAggro, IsCombatDistance; // is in aggro range,  is it in combat range?
                                           
    // Start is called before the first frame update
    void Start()
    {

        anim = gameObject.GetComponent<Animator>();
    }
    private void Awake()
    {
        player = GameObject.Find("Stylized Astronaut").transform;
        NavAgent = GetComponent<NavMeshAgent>();

    }
    // Update is called once per frame
    void Update()
    {
        IsAggro = Physics.CheckSphere(transform.position, AggroRange, PlayerDetection);
        IsCombatDistance = Physics.CheckSphere(transform.position, CombatDistance, PlayerDetection);

        if (!IsAggro && !IsCombatDistance)
        {
            Patrol();
        }
        if (IsAggro && !IsCombatDistance)
        {
            Hunt();
        }
        if (IsCombatDistance && IsAggro)
        {

            AttackPlayer();
        }
        if (!IsCombatDistance)
        {
            anim.SetBool("Attack", false);
        }
    }
    private void ResetAttack()
    {

        ATKCooldown = false;

    }
    private void AttackPlayer()
    {
        NavAgent.SetDestination(transform.position);
        transform.LookAt(player);
        anim.SetBool("Attack", true);

        if (!isATKStall)
        {
            isATKStall = true;
            Invoke(nameof(ResetAttack), ATKStall);

        }

    }
    private void SearchWalkPoint()
    {
        
        float RandposX = Random.Range(-PointRange, PointRange);
        

        PointSet = new Vector3(transform.position.x + RandposX, transform.position.z);
        if (Physics.Raycast(PointSet, -transform.up, 2f, GroundDetection))
        {
            IsPointSet = true;

            anim.SetBool("Moving", true);
        }
    }

    private void Patrol()
    {
        if (!IsPointSet) SearchWalkPoint();
        if (IsPointSet)
        {
            NavAgent.SetDestination(PointSet);
            Vector3 distance = transform.position - PointSet;

            if (distance.magnitude < 1f)
                IsPointSet = false;
        }
    }

    private void Hunt()
    {
        NavAgent.SetDestination(player.position);
        anim.SetBool("Moving", true);
    }

 










}


