namespace Com.Boombit.CaseStudy.Game.Data
{
    public class EnemyData : CharacterData
    {
        //  MEMBERS
        //      Private
        private float _pathUpdatePeriod;
        private float _attackCooldown;
        
        //      Properties
        public float PathUpdatePeriod { get { return _pathUpdatePeriod; } }
        public float AttackCooldown   { get { return _attackCooldown; } }
        
        //  CONSTRUCTORS
        public EnemyData(float maxHealth, float currentHealth, float moveSpeed, float rotationSpeed, float attackDamage, float attackRange, float pathUpdatePeriod, float attackCooldown)
            : base(maxHealth, currentHealth, moveSpeed, rotationSpeed, attackDamage, attackRange)
        {
            _pathUpdatePeriod   = pathUpdatePeriod;
            _attackCooldown     = attackCooldown;
        }
    }
}