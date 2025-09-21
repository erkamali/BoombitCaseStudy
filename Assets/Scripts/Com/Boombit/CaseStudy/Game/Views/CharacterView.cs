using Com.Boombit.CaseStudy.Game.Data;
using Com.Boombit.CaseStudy.Game.Utilities;
using UnityEngine;

namespace Com.Boombit.CaseStudy.Game.Views
{
    public abstract class CharacterView : MonoBehaviour, ICharacterView
    {
        //  MEMBERS
        protected int           _id;
        protected CharacterData _characterData;
        protected IGameManager  _gameManager;
        
        public int              ID              { get { return _id; } }
        public CharacterData    CharacterData   { get { return _characterData; } }
        
        //  METHODS
        public virtual void Init(CharacterData data, IGameManager gameManager)
        {
            _id             = data.ID;
            _characterData  = data;
            _gameManager    = gameManager;
        }

        public virtual void TakeDamage(float damage)
        {
            _gameManager.OnCharacterTakeDamage(_id, damage);
        }
    
        public abstract void Die();
        
        protected virtual void OnCharacterDied()
        {
            _gameManager.OnCharacterDied(_id);
        }
    }
}