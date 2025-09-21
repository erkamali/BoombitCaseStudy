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
        
        //  MEMBERS
        protected override void SetupUI()
        {
            _startButton.onClick.AddListener(() => _gameManager.StartGame());
            _quitButton.onClick.AddListener(() =>  _gameManager.QuitGame());
                
            _titleText.text = "Mobile Shooter";
        }
        
        protected override void OnShow()
        {
            //TODO: Animation
        }
    }
}