using UnityEngine;

namespace Com.Boombit.CaseStudy.Main.Data
{
    [CreateAssetMenu(fileName = "New Level Data", menuName = "Level System/Level Data")]
    public class LevelData : ScriptableObject
    {
        //  MEMBERS
        //      Editor
        [Header("Level Info")]
        public string LevelName = "Level 1";
        public float  LevelDuration = 60f;
        
        [Header("Enemy Spawn")]
        public EnemyTypeInfo[]  EnemyTypes;
        public float            SpawnInterval       = 2f;
        public int              MaxEnemiesAtOnce    = 10;
        public int              MaxEnemies          = 20;
        
        [Range(0f, 2f)]
        public float            SpawnRateMultiplier = 1f;
    }
}