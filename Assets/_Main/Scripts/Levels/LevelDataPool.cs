using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace _Main.Scripts.Levels
{
    [CreateAssetMenu(fileName = "LevelPool", menuName = "Config/LevelPool", order = 1)]
    public class LevelDataPool : ScriptableObject
    {
        private static LevelDataPool _firebaseConfig;
        public static LevelDataPool Instance
        {
            get
            {
                if (_firebaseConfig == null)
                {
                    _firebaseConfig = Resources.Load<LevelDataPool>($"Config/{nameof(LevelDataPool)}");
                }

                return _firebaseConfig;
            }
        }
    
        [field: SerializeField] public List<LevelData> Levels { get; private set; } = new();

#if UNITY_EDITOR
        public void FetchAllLevelsFromProject()
        {
            Levels.Clear();
      
            var allLevels = AssetDatabase.FindAssets("t:LevelData")
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(AssetDatabase.LoadAssetAtPath<LevelData>)
                .Where(level => level != null)
                .ToList();

            Levels.AddRange(allLevels);
        }
#endif

    }
}