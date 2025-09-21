using UnityEngine;

namespace Com.Boombit.CaseStudy.Main.Data
{
    [CreateAssetMenu(fileName = "New Level Sequence", menuName = "Level System/Levels")]
    public class Levels : ScriptableObject
    {
        public LevelData[] LevelsArray;
        
        public LevelData GetLevel(int index)
        {
            if (index >= 0 && index < LevelsArray.Length)
            {
                return LevelsArray[index];
            }
            return null;
        }
        
        public int GetLevelCount()
        {
            return LevelsArray != null ? LevelsArray.Length : 0;
        }
        
        public bool IsValidLevelIndex(int index)
        {
            return index >= 0 && index < GetLevelCount();
        }
    }
}