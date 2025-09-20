using UnityEngine;
using UnityEngine.AI;
using Com.Boombit.CaseStudy.Constants;

namespace Com.Boombit.CaseStudy.Views
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class EnemyView : MonoBehaviour
    {
        //  MEMBERS
        //      Editor
        [Header("AI Settings")]
        [SerializeField] private float _followSpeed = 3f;
        [SerializeField] private float _attackRange = 2f;
        [SerializeField] private float _attackCooldown = 2f;
        [SerializeField] private float _rotationSpeed = 360f;
        [SerializeField] private float _pathUpdatePeriod = 0.2f; // How often to recalculate path to player
        
        [Header("References")]
        [SerializeField] private Animator _animator;
        [SerializeField] private Transform _player; // Drag your player here, or we'll find it automatically
        
        //      Private
        private NavMeshAgent _navmeshAgent;
        private float _distanceToPlayer;
        private float _lastAttackTime;
        private float _lastPathUpdate;
        private bool _isDead = false;
        private bool _isAttacking = false;
        
        //  METHODS
        void Start()
        {
            if (_animator == null)
            {
                _animator = GetComponent<Animator>();
            }
                
            if (_navmeshAgent == null)
            {
                _navmeshAgent = GetComponent<NavMeshAgent>();
            }
            
            // Configure NavMeshAgent
            _navmeshAgent.speed = _followSpeed;
            _navmeshAgent.stoppingDistance = _attackRange * 0.8f; // Stop slightly before attack range
            _navmeshAgent.acceleration = 12f;
            _navmeshAgent.angularSpeed = _rotationSpeed;
            
            // Find player automatically if not assigned
            if (_player == null)
            {
                GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
                if (playerObj != null)
                    _player = playerObj.transform;
                else
                    Debug.LogError("No player found! Make sure player has 'Player' tag or assign manually.");
            }
            
            _lastAttackTime = -_attackCooldown; // Allow immediate first attack
            _lastPathUpdate = -_pathUpdatePeriod; // Allow immediate first path calculation
        }
        
        void Update()
        {
            if (_isDead || _player == null || _navmeshAgent == null) return;
            
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
            if (_distanceToPlayer <= _attackRange && Time.time >= _lastAttackTime + _attackCooldown)
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
            // Update path to player periodically (not every frame for performance)
            if (Time.time >= _lastPathUpdate + _pathUpdatePeriod)
            {
                if (_distanceToPlayer > _attackRange)
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
            
            // Rotate to face player when close (NavMesh handles rotation while moving)
            if (_distanceToPlayer <= _attackRange + 1f)
            {
                LookAtPlayer();
            }
        }
        
        void LookAtPlayer()
        {
            Vector3 lookDirection = _player.position - transform.position;
            lookDirection.y = 0; // Keep only horizontal rotation
            
            if (lookDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
            }
        }
        
        void AttackPlayer()
        {
            _isAttacking = true;
            _lastAttackTime = Time.time;
            
            // Stop moving during attack
            _navmeshAgent.ResetPath();
            _navmeshAgent.velocity = Vector3.zero;
            
            // Face player and attack
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
            // Set speed parameter based on NavMeshAgent velocity
            float currentSpeed = _navmeshAgent.velocity.magnitude;
            _animator.SetFloat(EnemyAnimationParameters.SPEED, currentSpeed);
        }
        
        public void Die()
        {
            if (_isDead) return;
            
            _isDead = true;
            _animator.SetTrigger(EnemyAnimationParameters.DIE);
            _animator.SetBool(EnemyAnimationParameters.IS_DEAD, true);
            
            // Stop NavMesh movement
            if (_navmeshAgent != null)
            {
                _navmeshAgent.ResetPath();
                _navmeshAgent.enabled = false;
            }
            
            // Disable AI behavior
            this.enabled = false;
            
            // Optional: Disable collider so player can walk through
            Collider col = GetComponent<Collider>();
            if (col != null) col.enabled = false;
        }
    }
}