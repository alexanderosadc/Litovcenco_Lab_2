using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game_Of_Life.Scripts
{
    public class Cell : MonoBehaviour
    {
        private bool _isUserInputsEnviornment;
        private bool _isCellLivesNextStep;
        private bool _isCurrentlyActive;


        private GameManager.Enviornment _enviornment;
        private SpriteRenderer _renderer;
        private GameManager.Role _role;

        [SerializeField] private GameObject activeCell;
        [SerializeField] private List<Sprite> enviornmentSprites;

        private void Awake()
        {
            _isUserInputsEnviornment = false;
            _renderer = GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            _isCurrentlyActive = false;
            _isCellLivesNextStep = false;
            GameEvents.Current.onRefreshCellState += DetectIfCellIsActive;
            GameEvents.Current.onUserSetEnviornment += UserClickedOnUIButtons;
        }

        private void UserClickedOnUIButtons(bool isUserInputsEnviornment)
        {
            _isUserInputsEnviornment = isUserInputsEnviornment;
        }

        private void DetectIfCellIsActive()
        {
            if (_isCellLivesNextStep)
            {
                _isCurrentlyActive = true;
                _isCellLivesNextStep = false;
                activeCell.SetActive(true);
            }
            else
            {
                _isCurrentlyActive = false;
                _isCellLivesNextStep = false; 
                activeCell.SetActive(false);
            }
        }

        private void OnMouseUpAsButton()
        {
            if (!_isUserInputsEnviornment)
            {
                activeCell.SetActive(!activeCell.activeInHierarchy);
                _isCurrentlyActive = true;
                _role = GameManager.CurrentInputUserRole;
                SelectColor();
            }
            else
            {
                SetEnviornment(GameManager.CurrentInputEnviornment);
            }
        }

        private void SelectColor()
        {
            Color color = Color.blue;
            switch (_role)
            {
                case GameManager.Role.Blue:
                    color = Color.blue;
                    break;
                case GameManager.Role.Red:
                    color = Color.red;
                    break;
                case GameManager.Role.Green:
                    color = Color.green;
                    break;
            }

            activeCell.GetComponent<SpriteRenderer>().color = color;
        }

        public void SetNextStepCellState(bool isLive, GameManager.Role nextRole)
        {
            _isCellLivesNextStep = isLive;
            _role = nextRole;
            SelectColor();
        }

        public bool GetIfCurrentlyActive()
        {
            return _isCurrentlyActive;
        }

        public void SetEnviornment(GameManager.Enviornment enviornment)
        {
            _enviornment = enviornment;
            _renderer.sprite = enviornmentSprites[(int) enviornment];
        }

        public GameManager.Enviornment GetEnviornment()
        {
            return _enviornment;
        }

        public GameManager.Role GetRole()
        {
            return _role;
        }
    }
}
