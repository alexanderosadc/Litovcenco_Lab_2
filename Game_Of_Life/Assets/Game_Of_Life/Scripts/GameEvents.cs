using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game_Of_Life.Scripts
{
    public class GameEvents : MonoBehaviour
    {
        public static GameEvents Current;

        private void Awake()
        {
            if (Current == null)
            {
                Current = this;
            }
        }

       
        public Action<int> onStartUserInput;
        public Action<GameManager.GameStates> onChangeGameState;
        public Action onRefreshCellState;
        public Action<bool> onUserSetEnviornment;

        public void UserSetEnviornment(bool isUserInputsEnviornment)
        {
            onUserSetEnviornment?.Invoke(isUserInputsEnviornment);
        }

        public void StartUserInput(int gridHeight)
        {
            onStartUserInput?.Invoke(gridHeight);
        }
        public void ChangeGameState(GameManager.GameStates currentGameState)
        {
            onChangeGameState?.Invoke(currentGameState);
        }

        public void RefreshCellState()
        {
            onRefreshCellState?.Invoke();   
        }
    }
}
