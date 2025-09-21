using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Com.Boombit.CaseStudy.Game.Views
{
    public class MainMenuUIView : BaseUIView
    {
        //  MEMBERS
        //      Editor
        [Header("UI Elements")]
        [SerializeField] private Button     _startButton;
        [SerializeField] private Button     _quitButton;
        [SerializeField] private TMP_Text   _titleText;

        [SerializeField] private Transform  _panel;

        //  Private
        private Tween _showTween;
        private Tween _hideTween;
        private float _hideOffset = 200f;
        
        //  MEMBERS
        protected override void SetupUI()
        {
            _startButton.onClick.AddListener(() => _gameManager.StartGame());
            _quitButton.onClick.AddListener(() =>  _gameManager.QuitGame());
               
            _titleText.text = "Mobile Shooter";
        }
        
        protected override void OnShow()
        {
            KillShowTween();

            Canvas canvas = _panel.GetComponentInParent<Canvas>();
            RectTransform canvasRect = canvas.GetComponent<RectTransform>();

            Vector3 targetPos = Vector3.zero;
            Vector3 startPos = new Vector3(0, -canvasRect.rect.height, 0);

            _showTween = _panel.DOLocalMove(targetPos, 0.6f)
                               .SetEase(Ease.OutBack, 1.7f)
                               .OnComplete(() => {
                                    _panel.DOPunchScale(Vector3.one * 0.08f, 0.2f);
                                    _showTween = null;
                               }
            );
        }

        protected override void OnHide()
        {
            KillHideTween();

            Vector3 currentPos  = _panel.localPosition;
            Vector3 upPos       = currentPos + Vector3.up * 20f;
            Vector3 hidePos     = currentPos + Vector3.down * _hideOffset;
            
            Sequence hideSequence = DOTween.Sequence();
            hideSequence.Append(_panel.DOLocalMove(upPos, 0.15f).SetEase(Ease.OutQuad));
            hideSequence.Append(_panel.DOLocalMove(hidePos, 0.4f).SetEase(Ease.InBack, 1.2f));
            hideSequence.OnComplete(() => _hideTween = null);
            
            _hideTween = hideSequence;
        }

        private void KillShowTween()
        {
            if (_showTween != null)
            {
                _showTween.Kill();
                _showTween = null;
            }
        }

        public void KillHideTween()
        {
            if (_hideTween != null)
            {
                _hideTween.Kill();
                _hideTween = null;
            }
        }

        private void OnDestroy()
        {
            KillShowTween();
            KillHideTween();
        }
    }
}