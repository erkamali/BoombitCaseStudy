using Com.Boombit.CaseStudy.Game.Data;
using UnityEngine;

namespace Com.Boombit.CaseStudy.Main.Data
{
    [System.Serializable]
    public class EnemyTypeInfo
    {
        [Header("Enemy Configuration")]
        public GameObject   EnemyPrefab;
        public EnemyConfig  EnemyConfig;
        
        [Header("Spawn Settings")]
        [Range(0f, 1f)]
        public float SpawnWeight = 1f;
    }
}