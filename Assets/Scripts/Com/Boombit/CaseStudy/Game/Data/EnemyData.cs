using UnityEngine;

namespace Com.Boombit.CaseStudy.Game.Data
{
    [CreateAssetMenu(fileName = "New Enemy Data", menuName = "Character Data/Enemy Data")]
    public class EnemyData : CharacterData
    {
        [Header("Enemy Specific")]
        [SerializeField] private float _pathUpdatePeriod = 0.2f;
        [SerializeField] private float _attackCooldown   = 1f;
        
        public float PathUpdatePeriod   { get { return _pathUpdatePeriod; } }
        public float AttackCooldown     { get { return _attackCooldown; } }
        
        public override void Initialize()
        {
            Debug.Log($"Enemy initialized: {MaxHealth} HP, {MoveSpeed} speed, {AttackDamage} damage");
        }
    }
}