using UnityEngine;

namespace Com.Boombit.CaseStudy.Data
{
    [CreateAssetMenu(fileName = "New Player Data", menuName = "Character Data/Player Data")]
    public class PlayerData : CharacterData
    {
        //  MEMBERS
        [Header("Player Specific")]
        [SerializeField] private float _inputDeadZone = 0.1f;
        
        public float InputDeadZone { get { return _inputDeadZone; } }
        
        public override void Initialize()
        {
            Debug.Log($"Player initialized: {MaxHealth} HP, {MoveSpeed} speed, {AttackDamage} damage");
        }
    }
}
