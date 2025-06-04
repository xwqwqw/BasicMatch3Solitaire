using System.Collections.Generic;
using System.Linq;
using _Main.Scripts.Levels;
using UnityEditor;
using UnityEngine;

namespace _Main.Scripts.Editor
{
    public class LevelEditorWindow : EditorWindow
    {
        private LevelDataPool _currentLevelDataPool;
        private string _levelsFolder = "Assets/Levels";
        private int _totalGenerateCount = 0;

        [MenuItem("Tools/Solitaire Level Editor")]
        public static void ShowWindow()
        {
            GetWindow<LevelEditorWindow>("Level Editor");
        }

        private void OnGUI()
        {
            GUILayout.Label("Level Data Editor", EditorStyles.boldLabel);

            _currentLevelDataPool = (LevelDataPool)EditorGUILayout.ObjectField("Level Data Pool SO", _currentLevelDataPool, typeof(LevelDataPool), false);

            _levelsFolder = EditorGUILayout.TextField("Levels Folder", _levelsFolder);
        
            _totalGenerateCount = EditorGUILayout.IntField("Total Levels to Generate", _totalGenerateCount);

            GUILayout.Space(10);

            if (GUILayout.Button("Create New Level SO"))
            {
                if (!AssetDatabase.IsValidFolder(_levelsFolder))
                {
                    EditorUtility.DisplayDialog("Error", $"Folder {_levelsFolder} does not exist!", "OK");
                    return;
                }

                if (_currentLevelDataPool == null)
                {
                    EditorUtility.DisplayDialog("Error", "Please assign a Level Data Pool ScriptableObject!", "OK");
                    return;
                }

                for (int i = 0; i < _totalGenerateCount; i++)
                {
                    CreateNewLevelSO(_levelsFolder, _currentLevelDataPool);
                }
            }

            GUILayout.Space(5);

            if (GUILayout.Button("Fetch All Levels From Project"))
            {
                if (_currentLevelDataPool == null)
                {
                    EditorUtility.DisplayDialog("Error", "Please assign a Level Data Pool ScriptableObject!", "OK");
                    return;
                }

                _currentLevelDataPool.FetchAllLevelsFromProject();

                _currentLevelDataPool.Levels.Sort((a, b) =>
                {
                    var aNum = ExtractNumberFromName(a.name);
                    var bNum = ExtractNumberFromName(b.name);
                    return aNum.CompareTo(bNum);
                });

                EditorUtility.SetDirty(_currentLevelDataPool);
                AssetDatabase.SaveAssets();
            }

            GUILayout.Space(10);
        }

        private int ExtractNumberFromName(string name)
        {
            var digits = new string(name.Where(char.IsDigit).ToArray());
            return int.TryParse(digits, out int number) ? number : 0;
        }

        private void CreateNewLevelSO(string folderPath, LevelDataPool pool)
        {
            var newLevelIndex = pool.Levels.Count + 1;
            var assetName = $"Level{newLevelIndex}.asset";
            var assetPath = $"{folderPath}/{assetName}";

            while (AssetDatabase.LoadAssetAtPath<LevelData>(assetPath) != null)
            {
                newLevelIndex++;
                assetName = $"Level{newLevelIndex}.asset";
                assetPath = $"{folderPath}/{assetName}";
            }

            var newLevel = CreateInstance<LevelData>();
            var randomizedTotalCards = Random.Range(6, 24);
            randomizedTotalCards = randomizedTotalCards / 3 * 3;
            GenerateLevelData(randomizedTotalCards, newLevel);
            AssetDatabase.CreateAsset(newLevel, assetPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            pool.Levels.Add(newLevel);
            EditorUtility.SetDirty(pool);
        }

        private void GenerateLevelData(int totalCards, LevelData levelData)
        {
            levelData.cards.Clear();

            if (totalCards % 3 != 0)
            {
                totalCards = totalCards / 3 * 3;
            }

            var uniqueIdsCount = totalCards / 3;
            var tempCards = new List<CardData>();

            for (int i = 0; i < uniqueIdsCount; i++)
            {
                var cardId = ((i % 3) + 1).ToString();

                for (int j = 0; j < 3; j++)
                {
                    var card = new CardData { cardId = cardId };
                    tempCards.Add(card);
                }
            }

            Shuffle(tempCards);
            levelData.cards.AddRange(tempCards);
        }


        private void Shuffle<T>(List<T> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                var rnd = Random.Range(i, list.Count);
                (list[rnd], list[i]) = (list[i], list[rnd]);
            }
        }
    }
}
