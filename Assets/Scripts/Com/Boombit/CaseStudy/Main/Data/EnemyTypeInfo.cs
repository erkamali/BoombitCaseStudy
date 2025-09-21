using Com.Boombit.CaseStudy.Game.Data;
using UnityEngine;

namespace Com.Boombit.CaseStudy.Main.Data
{
    [System.Serializable]
    public class EnemyTypeInfo
    {
        //  MEMBERS
        //      Editor
        public EnemyData  EnemyData;
        public GameObject EnemyPrefab;
        [Range(0f, 1f)]
        public float    SpawnWeight    = 1f;
        public float    MinSpawnDelay  = 0f;
    }
}