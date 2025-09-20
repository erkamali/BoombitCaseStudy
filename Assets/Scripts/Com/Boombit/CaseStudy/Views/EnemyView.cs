using UnityEngine;
using UnityEngine.AI;
using Com.Boombit.CaseStudy.Constants;
using Com.Boombit.CaseStudy.Data;

namespace Com.Boombit.CaseStudy.Views
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class EnemyView : CharacterView
    {
        //  MEMBERS
        //      Editor
        [Header("References")]
        [SerializeField] private Animator _animator;
        
        //      Private
        private NavMeshAgent _navmeshAgent;
        private float _distanceToPlayer;
        private float _lastAttackTime;
        private float _lastPathUpdate;
        private bool _isAttacking = false;

        private Transform _player;
        private EnemyData _enemyData;
        
        //  METHODS
        protected override void Start()
        {
            base.Start();
            
            _enemyData = (EnemyData)CharacterData;
            
            InitComponents();
            SetupNavMeshAgent();
            FindPlayer();
            
            _lastAttackTime = -_enemyData.AttackCooldown;
            _lastPathUpdate = -_enemyData.PathUpdatePeriod;
        }

        void InitComponents()
        {
            //_animator = GetComponent<Animator>();
            _navmeshAgent = GetComponent<NavMeshAgent>();
        }

        void SetupNavMeshAgent()
        {
            _navmeshAgent.speed             = _enemyData.MoveSpeed;
            _navmeshAgent.stoppingDistance  = _enemyData.AttackRange * 0.8f;
            _navmeshAgent.acceleration      = 12f;
            _navmeshAgent.angularSpeed      = _enemyData.RotationSpeed;
        }

        void FindPlayer()
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                _player = playerObj.transform;
            }
            else
            {
                Debug.LogError("No player found. Check Player prefab tag.");
            }
        }
        
        void Update()
        {
            if (_enemyData.IsDead || 
                _player == null   || 
                _navmeshAgent == null)
            {
                return;
            }
            
            CalculateDistanceToPlayer();
            HandleAI();
            UpdateAnimator();
        }
        
        void CalculateDistanceToPlayer()
        {
            _distanceToPlayer = Vector3.Distance(transform.position, _player.position);
        }
        
        void HandleAI()
        {
            // Check if we should attack
            if (_distanceToPlayer <= _enemyData.AttackRange && 
                Time.time >= _lastAttackTime + _enemyData.AttackCooldown)
            {
                AttackPlayer();
            }
            // Follow player if not attacking
            else if (!_isAttacking)
            {
                FollowPlayer();
            }
        }
        
        void FollowPlayer()
        {
            // Set path to player
            if (Time.time >= _lastPathUpdate + _enemyData.PathUpdatePeriod)
            {
                if (_distanceToPlayer > _enemyData.AttackRange)
                {
                    _navmeshAgent.SetDestination(_player.position);
                }
                else
                {
                    // Stop moving when in attack range
                    _navmeshAgent.ResetPath();
                }
                _lastPathUpdate = Time.time;
            }
            
            if (_distanceToPlayer <= _enemyData.AttackRange + 1f)
            {
                LookAtPlayer();
            }
        }
        
        void LookAtPlayer()
        {
            Vector3 lookDirection = _player.position - transform.position;
            lookDirection.y = 0;
            
            if (lookDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _enemyData.RotationSpeed * Time.deltaTime);
            }
        }
        
        void AttackPlayer()
        {
            _isAttacking = true;
            _lastAttackTime = Time.time;
            
            _navmeshAgent.ResetPath();
            _navmeshAgent.velocity = Vector3.zero;
            
            LookAtPlayer();
            _animator.SetTrigger(EnemyAnimationParameters.ATTACK);
            
            // Stop the attack flag after animation duration (adjust based on your animation length)
            Invoke(nameof(StopAttacking), 1f); // Adjust this timing based on your attack animation length
        }
        
        void StopAttacking()
        {
            _isAttacking = false;
        }
        
        void UpdateAnimator()
        {
            float currentSpeed = _navmeshAgent.velocity.magnitude;
            _animator.SetFloat(EnemyAnimationParameters.SPEED, currentSpeed);
        }
        
        public override void Die()
        {
            Debug.Log("Enemy died!");

            _animator.SetTrigger(EnemyAnimationParameters.DIE);
            _animator.SetBool(EnemyAnimationParameters.IS_DEAD, true);
            
            if (_navmeshAgent != null)
            {
                _navmeshAgent.ResetPath();
                _navmeshAgent.enabled = false;
            }
            
            this.enabled = false;
            
            Collider collider = GetComponent<Collider>();
            if (collider != null)
            {
                collider.enabled = false;
            }
        }
    }
}