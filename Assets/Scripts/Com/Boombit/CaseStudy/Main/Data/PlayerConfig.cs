using UnityEngine;

namespace Com.Boombit.CaseStudy.Main.Data
{
    [CreateAssetMenu(fileName = "New Player Data", menuName = "Character Data/Player Data")]
    public class PlayerConfig : CharacterConfig
    {
        //  MEMBERS
        [Header("Player Specific")]
        [SerializeField] private float _inputDeadZone   = 0.1f;
        [SerializeField] private float _weaponFireRate  = 0.1f;
        
        public float InputDeadZone  { get { return _inputDeadZone; } }
        public float WeaponFireRate { get { return _weaponFireRate; } }
    }
}
