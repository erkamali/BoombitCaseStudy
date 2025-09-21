using UnityEngine;

namespace Com.Boombit.CaseStudy.Game.Data
{
    public abstract class CharacterData
    {
        //  MEMBERS
        //      Protected
        protected float maxHealth;
        protected float currentHealth;
        protected float moveSpeed;
        protected float rotationSpeed;
        protected float attackDamage;
        protected float attackRange;
        
        //      Properties
        public float MaxHealth      { get { return maxHealth; } }
        public float CurrentHealth  { get { return currentHealth; } }
        public float MoveSpeed      { get { return moveSpeed; } }
        public float RotationSpeed  { get { return rotationSpeed; } }
        public float AttackDamage   { get { return attackDamage; } }
        public float AttackRange    { get { return attackRange; } }
        
        public bool IsDead { get { return CurrentHealth <= 0; } }
        
        //  CONSTRUCTORS
        public CharacterData(float maxHealth, float currentHealth, float moveSpeed, float rotationSpeed, float attackDamage, float attackRange)
        {
            this.maxHealth      = maxHealth;
            this.currentHealth  = currentHealth;
            this.moveSpeed      = moveSpeed;
            this.rotationSpeed  = rotationSpeed;
            this.attackDamage   = attackDamage;
            this.attackRange    = attackRange;
        }
        
        //  METHODS
        public virtual void TakeDamage(float damage)
        {
            if (IsDead)
            {
                return;
            }
            
            currentHealth -= damage;
            currentHealth = Mathf.Max(0f, currentHealth);
        }
        
        public virtual void ResetHealth()
        {
            currentHealth = maxHealth;
        }
    }
}