using UnityEngine;

namespace Com.Boombit.CaseStudy.Main.Data
{
    public class EnemyCleanup : MonoBehaviour
    {
        private LevelManager levelManager;
        
        public void Initialize(LevelManager manager)
        {
            levelManager = manager;
        }
        
        private void OnDestroy()
        {
            if (levelManager != null)
            {
                levelManager.UnregisterEnemy(gameObject);
            }
        }
    }
}