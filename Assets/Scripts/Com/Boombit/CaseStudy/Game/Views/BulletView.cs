using Com.Boombit.CaseStudy.Game.Utilities;
using UnityEngine;

namespace Com.Boombit.CaseStudy.Game.Views
{
    public class BulletView : MonoBehaviour
    {
        //  MEMBERS
        //      Editor
        [Header("Config")]
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private Collider  _collider;
        public float _speed     = 20f;
        public float _lifetime  = 3f;
        
        //      Private
        private float   _damage;
        private Vector3 _direction;
        private float   _timer;
        private bool    _isActive;
        
        void FixedUpdate()
        {
            if (!_isActive) 
            {
                return;
            }
        
            _timer += Time.fixedDeltaTime;
            if (_timer >= _lifetime)
            {
                ReturnToPool();
                return;
            }
            
            /*
            Vector3 newPosition = transform.position + _direction * _speed * Time.fixedDeltaTime;
            Debug.Log($"Moving from {transform.position} to {newPosition}, direction: {_direction}, speed: {_speed}");
    
            _rigidbody.MovePosition(newPosition);
            */

            Vector3 oldPos = transform.position;
            Vector3 newPosition = transform.position + _direction * _speed * Time.fixedDeltaTime;
            
            // Test with simple transform movement instead of rigidbody
            transform.position = newPosition;
                    }
        
        public void Initialize(Vector3 startPosition, Vector3 moveDirection, float bulletDamage)
        {
            transform.position = startPosition;
            transform.rotation = Quaternion.LookRotation(moveDirection);
            
            _direction  = moveDirection.normalized;
            _damage     = bulletDamage;
            
            _timer      = 0f;
            _isActive   = true;
            
            gameObject.SetActive(true);
            _collider.enabled           = true;
            _rigidbody.linearVelocity   = Vector3.zero;
        }

        void OnTriggerEnter(Collider other)
        {            
            if (!_isActive) 
            {
                return;
            }
            
            EnemyView enemy = other.GetComponent<EnemyView>();
            if (enemy != null)
            {
                enemy.TakeDamage(_damage);
                ReturnToPool();
            }
            else
            {
                Debug.Log($"Object hit has no EnemyView component");
            }
            
            /*
            if (other.CompareTag("Obstacle"))
            {
                Debug.Log("Hit obstacle");
                ReturnToPool();
            }
            */
        }
        
        void ReturnToPool()
        {
            _isActive = false;
            gameObject.SetActive(false);
            BulletPool.Instance.ReturnBullet(this);
        }
    }
}