using UnityEngine;

namespace Com.Boombit.CaseStudy.Main.Data
{
    [CreateAssetMenu(fileName = "New Level Data", menuName = "Level System/Level Data")]
    public class LevelData : ScriptableObject
    {
        //  MEMBERS
        //      Editor
        [Header("Level Info")]
        [SerializeField] private string _levelName = "Level 1";
        [SerializeField] private float  _levelDuration = 60f;
        
        [Header("Enemy Spawn")]
        [SerializeField] private EnemyTypeInfo[]    _enemyTypes;
        [SerializeField] private float              _baseSpawnInterval   = 2f;
        [SerializeField] private int                _maxEnemiesAtOnce    = 10;
        
        [Range(0f, 2f)]
        [SerializeField] private float              _spawnRateMultiplier = 1f;
    }
}