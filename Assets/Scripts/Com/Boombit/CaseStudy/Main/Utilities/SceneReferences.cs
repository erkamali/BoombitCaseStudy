using UnityEngine;

namespace Com.Boombit.CaseStudy.Main.Utilities
{
    public class SceneReferences : MonoBehaviour
    {
        //  MEMBERS
        //      From Editor
        [SerializeField] public GameObject _uiViewContainer;

        //      Properties
        public GameObject UIViewContainer { get { return _uiViewContainer; } }
        
    }
}
