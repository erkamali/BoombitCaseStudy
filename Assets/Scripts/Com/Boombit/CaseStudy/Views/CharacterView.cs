using Com.Boombit.CaseStudy.Data;
using UnityEngine;

namespace Com.Boombit.CaseStudy.Views
{
    public abstract class CharacterView : MonoBehaviour, ICharacterView
    {
        //  MEMBERS
        [SerializeField] protected CharacterData _characterData;
        
        public CharacterData CharacterData { get { return _characterData; } }
        
        //  METHODS
        protected virtual void Start()
        {
            if (_characterData != null)
            {
                Init(_characterData);
                SubscribeToEvents();
            }
        }
        
        public virtual void Init(CharacterData data)
        {
            _characterData = data;
            data.Initialize();
        }
        
        protected virtual void SubscribeToEvents()
        {
            _characterData.OnDamageTaken    += OnDamageTaken;
            _characterData.OnDeath          += OnDeath;
        }
        
        protected virtual void OnDestroy()
        {
            UnsubscribeFromEvents();
        }
        
        protected virtual void UnsubscribeFromEvents()
        {
            _characterData.OnDamageTaken    -= OnDamageTaken;
            _characterData.OnDeath          -= OnDeath;
        }
        
        public virtual void TakeDamage(float damage)
        {
            _characterData.TakeDamage(damage);
        }
        
        public abstract void Die();
        
        protected virtual void OnDamageTaken()
        {
            // TODO: Play take damage anim
        }
        
        protected virtual void OnDeath()
        {
            Die();
        }
    }
}