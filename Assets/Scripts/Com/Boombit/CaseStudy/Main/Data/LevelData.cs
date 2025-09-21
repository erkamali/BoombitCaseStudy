using UnityEngine;

namespace Com.Boombit.CaseStudy.Main.Data
{
    [CreateAssetMenu(fileName = "New Level Data", menuName = "Level System/Level Data")]
    public class LevelData : ScriptableObject
    {
        [Header("Level Info")]
        public string   LevelName       = "Level 1";
        public float    LevelDuration   = 60f;
        
        [Header("Level Layout")]
        public GameObject LevelLayoutPrefab; // Contains plane, obstacles, etc.
        
        [Header("Player Setup")]
        public GameObject   PlayerPrefab;
        public PlayerConfig PlayerConfig;
        public Transform    PlayerSpawnTransform;
        
        [Header("Enemy Configuration")]
        public EnemyTypeInfo[]  EnemyTypes;
        public Transform[]      EnemySpawnPoints;
        public int              MaxEnemies = 20;
        public float            EnemySpawnInterval = 2f;
    }
}