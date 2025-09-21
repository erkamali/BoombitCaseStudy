using Com.Boombit.CaseStudy.Game.Utilities;
using UnityEngine;

namespace Com.Boombit.CaseStudy.Game.Views
{
    public abstract class BaseUIView : MonoBehaviour
    {
        //  MEMBERS
        protected IGameManager _gameManager;
        
        //  METHODS
        public virtual void Init(IGameManager gameManager)
        {
            _gameManager = gameManager;
            SetupUI();
        }
        
        public virtual void Show()
        {
            gameObject.SetActive(true);
            OnShow();
        }
        
        public virtual void Hide()
        {
            gameObject.SetActive(false);
            OnHide();
        }
        
        protected abstract void SetupUI();
        protected virtual void  OnShow() { }
        protected virtual void  OnHide() { }
    }
}