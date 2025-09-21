using System;
using UnityEngine;

namespace Com.Boombit.CaseStudy.Main.Data
{
    [System.Serializable]
    public abstract class CharacterConfig : ScriptableObject
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
        
        // Properties
        public float MaxHealth      { get { return _maxHealth; } }
        public float CurrentHealth  { get { return _currentHealth; } }
        public float MoveSpeed      { get { return _moveSpeed; } }
        public float RotationSpeed  { get { return _rotationSpeed; } }
        public float AttackDamage   { get { return _attackDamage; } }
        public float AttackRange    { get { return _attackRange; } }
    }
}