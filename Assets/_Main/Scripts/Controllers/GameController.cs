using _Main.Scripts.CardSystem;
using _Main.Scripts.Levels;
using _Main.Scripts.Utils;
using UnityEngine;

namespace _Main.Scripts.Controllers
{
    public class GameController : BaseSingleton<GameController>
    {
        [SerializeField] private CardView cardPrefab;
        [SerializeField] private LevelDataPool levelDataPool;
        [SerializeField] private CardStack mainCardStack;

        private int CurrentLevel
        {
            get => PlayerPrefs.GetInt("CurrentLevel", 0);
            set => PlayerPrefs.SetInt("CurrentLevel", value);
        }

        private void Start()
        {
            SpawnLevel();
            SubscribeToEvents();
        }

        private void SubscribeToEvents()
        {
            EventController.Subscribe<bool>(EventNames.OnLevelWin, OnLevelWin);
        }
        
        private void SpawnLevel()
        {
            if (CurrentLevel >= levelDataPool.Levels.Count)
            {
                CurrentLevel = 0;
            }

            var levelData = levelDataPool.Levels[CurrentLevel];
            foreach (var cardData in levelData.cards)
            {
                var card = Instantiate(cardPrefab, mainCardStack.transform);
                card.transform.localScale = Vector3.one;
                card.SetCardData(cardData);  
                mainCardStack.AddCard(card);
            }
        
            Debug.Log($"Level {CurrentLevel} spawned with {levelData.cards.Count} cards.");
        }

        private void OnLevelWin(bool value)
        {
            CurrentLevel++;
            SpawnLevel();
        }

     
    }
}