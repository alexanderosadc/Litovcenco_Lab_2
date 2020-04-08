using System;
using System.Collections;
using UnityEngine;

namespace Game_Of_Life.Scripts
{
    public class GameManager: MonoBehaviour
    {
        public enum GameStates
        {
            Menu,
            UserInteractsWithGrid,
            Gameplay,
            Finish
        }
        public enum Enviornment
        {
            Forest,
            Desert,
            Water,
            Plain
        }

        public enum CellType
        {
        
        }
        
        
        
        [SerializeField] private int gridHeight;

        public static GameStates StateOfTheGame = GameStates.Menu;
        
        public static GameStates CurrentGameState
        {
            get => StateOfTheGame;
            set
            {
                StateOfTheGame = value;
                GameEvents.Current.ChangeGameState(StateOfTheGame);
            }
        }

        private void Start()
        {
            StartCoroutine(nameof(PublishEvent));
        }

        private IEnumerator PublishEvent()
        {
            yield return new WaitForSeconds(0.2f);
            GameEvents.Current.StartUserInput(gridHeight);
        }
    }
}
