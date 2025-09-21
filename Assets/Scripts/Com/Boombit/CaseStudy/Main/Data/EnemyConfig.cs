using UnityEngine;

namespace Com.Boombit.CaseStudy.Main.Data
{
    [CreateAssetMenu(fileName = "New Enemy Data", menuName = "Character Data/Enemy Data")]
    public class EnemyConfig : CharacterConfig
    {
        [Header("Enemy Specific")]
        [SerializeField] private float _pathUpdatePeriod = 0.2f;
        [SerializeField] private float _attackCooldown   = 1f;
        
        public float PathUpdatePeriod   { get { return _pathUpdatePeriod; } }
        public float AttackCooldown     { get { return _attackCooldown; } }
    }
}