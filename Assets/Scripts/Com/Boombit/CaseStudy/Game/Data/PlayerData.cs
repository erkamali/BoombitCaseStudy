namespace Com.Boombit.CaseStudy.Game.Data
{
    public class PlayerData : CharacterData
    {
        //  MEMBERS
        //      Private
        private float _inputDeadZone;
        private float _weaponFireRate;
        
        //      Properties
        public float InputDeadZone  { get { return _inputDeadZone; } }
        public float WeaponFireRate { get { return _weaponFireRate; } }
        
        //  CONSTRUCTORS
        public PlayerData(float maxHealth, float currentHealth, float moveSpeed, float rotationSpeed, float attackDamage, float attackRange, float inputDeadZone, float weaponFireRate)
            : base(maxHealth, currentHealth, moveSpeed, rotationSpeed, attackDamage, attackRange)
        {
            _inputDeadZone  = inputDeadZone;
            _weaponFireRate = weaponFireRate;
        }
    }
}