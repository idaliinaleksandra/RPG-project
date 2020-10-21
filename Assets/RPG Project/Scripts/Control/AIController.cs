using GameDevTV.Utils;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using RPG.Resources;
using UnityEngine;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] float chaseDistance = 5f;
        [SerializeField] float suspicionTime = 5f;
        [SerializeField] PatrolPath patrolPath;
        [SerializeField] float waypointTolerance = 1f;
        [SerializeField] float waypointTime = 3f;
        [Range(0,1)]
        [SerializeField] float patrolSpeedFraction = 0.2f;

        GameObject player;
        Fighter fighter;
        Health health;
        Move move;
        ActionScheduler actionScheduler; 

        LazyValue<Vector3> guardPosition;
        float timeSinceLastSawPlayer = Mathf.Infinity;
        float timeSinceArrivedAtWaypoint = Mathf.Infinity;
        int currentWaypointIndex = 0;

        private void Awake()
        {
            player = GameObject.FindGameObjectWithTag("Player");
            fighter = gameObject.GetComponent<Fighter>();
            health = gameObject.GetComponent<Health>();
            move = gameObject.GetComponent<Move>();
            actionScheduler = gameObject.GetComponent<ActionScheduler>();

            guardPosition = new LazyValue<Vector3>(GetInitialPosition);
        }

        private Vector3 GetInitialPosition()
        {
            return transform.position;
        }

        private void Start()
        {
            guardPosition.ForceInit();
        }

        private void Update()
        {
            if (health.IsDead()) return;

            if (InAttackRangeOfPlayer())
                StartChaseBehaviour();
            else if (timeSinceLastSawPlayer < suspicionTime)
                SuspiciousStateBehaviour();
            else
                StartPatrolBehaviour();

            UpdateTimers();
        }

        private void UpdateTimers()
        {
            timeSinceLastSawPlayer += Time.deltaTime;
            timeSinceArrivedAtWaypoint += Time.deltaTime;
        }

        private void SuspiciousStateBehaviour()
        {
            actionScheduler.CancelCurrentAction();
        }

        private bool InAttackRangeOfPlayer()
        {
            float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
            return distanceToPlayer < chaseDistance;
        }

        private void StartChaseBehaviour()
        {
            timeSinceLastSawPlayer = 0;

            if (fighter.CanAttack(player))
                fighter.Attack(player);
        }

        private void StartPatrolBehaviour()
        {
            Vector3 nextPosition = guardPosition.value;

            if (patrolPath != null)
            {
                if (AtWaypoint())
                {
                    timeSinceArrivedAtWaypoint = 0;
                    CycleWaypoint();
                }
                nextPosition = GetCurrentWaypoint();
            }

            if (timeSinceArrivedAtWaypoint > waypointTime)
                move.StartMoveAction(nextPosition, patrolSpeedFraction);
        }

        private bool AtWaypoint()
        {
            float distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWaypoint());
            return (distanceToWaypoint < waypointTolerance);
        }

        private void CycleWaypoint()
        {
            currentWaypointIndex = patrolPath.GetNextIndex(currentWaypointIndex);
        }

        private Vector3 GetCurrentWaypoint()
        {
            Vector3 waypoint = patrolPath.GetWaypoint(currentWaypointIndex);
            return waypoint;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;

            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }
    }
}