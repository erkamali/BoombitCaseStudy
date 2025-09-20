using UnityEngine;

namespace Com.Boombit.CaseStudy.Data
{
    [CreateAssetMenu(fileName = "New Enemy Data", menuName = "Character Data/Enemy Data")]
    public class EnemyData : CharacterData
    {
        [Header("Enemy Specific")]
        [SerializeField] private float _pathUpdatePeriod = 0.2f;
        
        public float PathUpdatePeriod { get { return _pathUpdatePeriod; } }
        
        public override void Initialize()
        {
            Debug.Log($"Enemy initialized: {MaxHealth} HP, {MoveSpeed} speed, {AttackDamage} damage");
        }
    }
}