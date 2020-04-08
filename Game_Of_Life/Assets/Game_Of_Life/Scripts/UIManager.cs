using UnityEngine;

namespace Game_Of_Life.Scripts
{
    public class UIManager : MonoBehaviour
    {
        public void GoToGameplay()
        {
            GameManager.CurrentGameState = GameManager.GameStates.Gameplay;
        }
    }
}
