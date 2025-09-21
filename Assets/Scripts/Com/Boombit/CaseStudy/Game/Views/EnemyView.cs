using UnityEngine;
using UnityEngine.AI;
using Com.Boombit.CaseStudy.Game.Constants;
using Com.Boombit.CaseStudy.Game.Data;
using Com.Boombit.CaseStudy.Game.Utilities;
using System.Collections;

namespace Com.Boombit.CaseStudy.Game.Views
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
        private float        _distanceToPlayer;
        private float        _lastAttackTime;
        private float        _lastPathUpdate;
        private bool         _isAttacking = false;

        private Transform _player;
        private EnemyData _enemyData;
        
        //  METHODS
        public void Initialize(EnemyData enemyData, Transform playerTransform, IGameManager gameManager)
        {
            _player = playerTransform;
            Init(enemyData, gameManager);
            
            InitComponents();
            SetupNavMeshAgent();
            
            _lastAttackTime = -_enemyData.AttackCooldown;
            _lastPathUpdate = -_enemyData.PathUpdatePeriod;
        }
        
        public override void Init(CharacterData data, IGameManager gameManager)
        {
            base.Init(data, gameManager);
            
            EnemyData enemyData = (EnemyData)data;
            _enemyData = enemyData;
        }

        private void InitComponents()
        {
            //_animator = GetComponent<Animator>();
            _navmeshAgent = GetComponent<NavMeshAgent>();
        }

        private void SetupNavMeshAgent()
        {
            _navmeshAgent.speed             = _enemyData.MoveSpeed;
            _navmeshAgent.stoppingDistance  = _enemyData.AttackRange * 0.8f;
            _navmeshAgent.acceleration      = 12f;
            _navmeshAgent.angularSpeed      = _enemyData.RotationSpeed;
        }
        
        private void Update()
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
        
        private void CalculateDistanceToPlayer()
        {
            _distanceToPlayer = Vector3.Distance(transform.position, _player.position);
        }
        
        private void HandleAI()
        {
            // Check if enemy should attack
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
        
        private void FollowPlayer()
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
        
        private void LookAtPlayer()
        {
            Vector3 lookDirection = _player.position - transform.position;
            lookDirection.y = 0;
            
            if (lookDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _enemyData.RotationSpeed * Time.deltaTime);
            }
        }
        
        private void AttackPlayer()
        {
            _isAttacking = true;
            _lastAttackTime = Time.time;
            
            _navmeshAgent.ResetPath();
            _navmeshAgent.velocity = Vector3.zero;
            
            LookAtPlayer();
            _animator.SetTrigger(EnemyAnimationParameters.ATTACK);
            
            StartCoroutine(AttackWithDelay());
        
            Invoke(nameof(StopAttacking), 1f);
        }

        private IEnumerator AttackWithDelay()
        {
            yield return new WaitForSeconds(0.5f);
            
            // Check if player is still in range before dealing damage
            if (_distanceToPlayer <= _enemyData.AttackRange)
            {
                // Deal damage to player directly
                PlayerView playerView = _player.GetComponent<PlayerView>();
                if (playerView != null)
                {
                    playerView.TakeDamage(_enemyData.AttackDamage);
                }
            }
        }
        
        private void StopAttacking()
        {
            _isAttacking = false;
        }
        
        private void UpdateAnimator()
        {
            float currentSpeed = _navmeshAgent.velocity.magnitude;
            _animator.SetFloat(EnemyAnimationParameters.SPEED, currentSpeed);
        }

#region ICharacterView implementations

        public override void TakeDamage(float damage)
        {
            base.TakeDamage(damage);
        }
        
        public override void Die()
        {
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
            
            OnCharacterDied();
        }

#endregion

        public void Stop()
        {
            if (_navmeshAgent != null)
            {
                _navmeshAgent.ResetPath();
                _navmeshAgent.velocity = Vector3.zero;
                _navmeshAgent.enabled = false;
            }
            
            StopAttacking();
            
            StopAllCoroutines();
            
            CancelInvoke(nameof(StopAttacking));
            
            if (_animator != null)
            {
                _animator.SetFloat(EnemyAnimationParameters.SPEED, 0f);
            }
            
            this.enabled = false;
        }

    }
}