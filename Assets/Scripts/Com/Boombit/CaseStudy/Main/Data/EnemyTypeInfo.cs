using Com.Boombit.CaseStudy.Game.Data;
using UnityEngine;

namespace Com.Boombit.CaseStudy.Main.Data
{
    [System.Serializable]
    public class EnemyTypeInfo
    {
        //  MEMBERS
        //      Editor
        [SerializeField] private EnemyData  _enemyData;
        [SerializeField] private GameObject _enemyPrefab;
        [Range(0f, 1f)]
        [SerializeField] private float      _spawnWeight    = 1f;
        [SerializeField] private float      _minSpawnDelay  = 0f;
    }
}