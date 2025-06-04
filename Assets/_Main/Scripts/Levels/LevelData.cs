using System.Collections.Generic;
using UnityEngine;

namespace _Main.Scripts.Levels
{
    [CreateAssetMenu(fileName = "NewLevelData", menuName = "Solitaire/Level Data")]
    public class LevelData : ScriptableObject
    {
        public List<CardData> cards = new();
    }

    [System.Serializable]
    public class CardData
    {
        public string cardId;
    }
}