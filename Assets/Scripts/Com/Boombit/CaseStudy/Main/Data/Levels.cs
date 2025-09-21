using UnityEngine;

namespace Com.Boombit.CaseStudy.Main.Data
{
    [CreateAssetMenu(fileName = "New Level Sequence", menuName = "Level System/Levels")]
    public class Levels : ScriptableObject
    {
        //  MEMBERS
        //      Editor
        public LevelData[] LevelsArray;
    }
}