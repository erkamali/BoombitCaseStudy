using System;
using UnityEngine;

namespace Com.Boombit.CaseStudy.Game.Data
{
    [System.Serializable]
    public abstract class CharacterData : ScriptableObject
    {
        //  MEMBERS
        //      Editor
        [Header("Health")]
        [SerializeField] protected float _maxHealth = 100f;
        [SerializeField] protected float _currentHealth;
        
        [Header("Movement")]
        [SerializeField] protected float _moveSpeed = 5f;
        [SerializeField] protected float _rotationSpeed = 360f;
        
        [Header("Combat")]
        [SerializeField] protected float _attackDamage = 10f;
        [SerializeField] protected float _attackRange = 2f;
        
        // Events
        public event Action OnDamageTaken;
        public event Action OnDeath;
        
        // Properties
        public float MaxHealth      { get { return _maxHealth; } }
        public float CurrentHealth  { get { return _currentHealth; } }
        public float MoveSpeed      { get { return _moveSpeed; } }
        public float RotationSpeed  { get { return _rotationSpeed; } }
        public float AttackDamage   { get { return _attackDamage; } }
        public float AttackRange    { get { return _attackRange; } }
        
        public bool IsDead => _currentHealth <= 0f;
        public float HealthPercentage => _currentHealth / _maxHealth;
        
        //  METHODS
        protected virtual void OnEnable()
        {
            _currentHealth = _maxHealth;
        }
        
        // Health management methods
        public virtual void TakeDamage(float damage)
        {
            if (IsDead) return;
            
            _currentHealth = Mathf.Max(0f, _currentHealth - damage);
            OnDamageTaken?.Invoke();
            
            if (IsDead)
            {
                OnDeath?.Invoke();
            }
        }
        
        public virtual void ResetHealth()
        {
            _currentHealth = _maxHealth;
        }
        
        public abstract void Initialize();
    }
}