using System.Collections.Generic;
using Com.Boombit.CaseStudy.Game.Views;
using UnityEngine;

namespace Com.Boombit.CaseStudy.Game.Utilities
{
    public class BulletPool : MonoBehaviour
    {
        //  MEMBERS
        //      Editor
        [Header("Config")]
        public GameObject   BulletPrefab;
        public int          InitialPoolSize = 20;
        public int          MaxPoolSize     = 50;

        public static BulletPool Instance { get; private set; }
        
        //      Private
        private readonly Queue<BulletView> _bulletPool      = new Queue<BulletView>();
        private readonly List<BulletView>  _activeBullets   = new List<BulletView>();
        
        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                InitializePool();
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        void InitializePool()
        {
            if (BulletPrefab == null)
            {
                Debug.LogError("Bullet prefab not assigned!");
                return;
            }
            
            for (int i = 0; i < InitialPoolSize; i++)
            {
                CreateNewBullet();
            }
        }
        
        BulletView CreateNewBullet()
        {
            GameObject bulletObj = Instantiate(BulletPrefab, transform);
            BulletView bullet    = bulletObj.GetComponent<BulletView>();
            
            bulletObj.SetActive(false);
            _bulletPool.Enqueue(bullet);
            return bullet;
        }
        
        public BulletView GetBullet()
        {
            BulletView bullet;
            
            if (_bulletPool.Count > 0)
            {
                bullet = _bulletPool.Dequeue();
            }
            else if (_activeBullets.Count < MaxPoolSize)
            {
                // Create new bullet if under limit
                bullet = CreateNewBullet();
                _bulletPool.Dequeue();
            }
            else
            {
                // Pool is at max capacity, reuse oldest active bullet
                bullet = _activeBullets[0];
                _activeBullets.RemoveAt(0);
                bullet.gameObject.SetActive(false);
            }
            
            _activeBullets.Add(bullet);
            return bullet;
        }
        
        public void ReturnBullet(BulletView bullet)
        {
            if (_activeBullets.Contains(bullet))
            {
                _activeBullets.Remove(bullet);
                _bulletPool.Enqueue(bullet);
            }
        }
    }
}