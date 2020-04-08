using System;
using UnityEngine;

namespace Game_Of_Life.Scripts
{
    public class Cell : MonoBehaviour
    {
        private bool _isCellLivesNextStep;
        private bool _isCurrentlyActive;    
    
        [SerializeField] private GameObject activeCell;
        
        private void Start()
        {
            _isCurrentlyActive = false;
            _isCellLivesNextStep = false;
            GameEvents.Current.onRefreshCellState += DetectIfCellIsActive;
        }

        private void DetectIfCellIsActive()
        {
            if (_isCellLivesNextStep)
            {
                activeCell.SetActive(true);
                _isCurrentlyActive = true;
                _isCellLivesNextStep = false;
            }
            else
            {
                activeCell.SetActive(false);
            }
        }

        private void OnMouseUpAsButton()
        {
            activeCell.SetActive(!activeCell.activeInHierarchy);
            _isCurrentlyActive = true;
        }

        public void SetNextStepCellState(bool isLive)
        {
            _isCellLivesNextStep = isLive;
        }

        public bool GetVisibilityOfTheCell()
        {
            return _isCurrentlyActive;
        }
    }
}
