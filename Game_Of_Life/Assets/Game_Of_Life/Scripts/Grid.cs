using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game_Of_Life.Scripts
{
    public class Grid : MonoBehaviour
    {
        [SerializeField] private GameObject cellPrefab;
        private int _gridSize;
    
        private float _generationInterval = 1.0f;
        private GameObject[,] _grid;

        private void Start()
        {
            GameEvents.Current.onStartUserInput += GenerateGrid;
            GameEvents.Current.onChangeGameState += GameStateController;
        }

        private void GameStateController(GameManager.GameStates state)
        {
            switch (state)
            {
                case GameManager.GameStates.Menu:
                    break;
                case GameManager.GameStates.UserInteractsWithGrid:
                    break;
                case GameManager.GameStates.Gameplay:
                    StartCellGenerator();
                    break;
                case GameManager.GameStates.Finish:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }

        private void StartCellGenerator()
        {
            InvokeRepeating(nameof(NextGeneration), _generationInterval, _generationInterval);
        }

        private void GenerateGrid(int gridHeight)
        {
            Debug.Log("Ahtung");
            _gridSize = gridHeight;
            _grid = new GameObject[_gridSize, _gridSize];
            Vector2 position;
            for (int i = 0; i < _gridSize; i++)
            {
                for (int j = 0; j < _gridSize; j++)
                {
                    position = new Vector2(j, i);
                    _grid[i, j] = Instantiate(cellPrefab, position, Quaternion.identity, transform);
                    _grid[i, j].GetComponent<Cell>().SetEnviornment((GameManager.Enviornment)Random.Range(0, 4));
                }
            }
        }

        private void NextGeneration()
        {
            ApplyRules();
        }

        private void PopulateGrid()
        {
            for (int i = 0; i < _gridSize; i++)
            {
                for (int j = 0; j < _gridSize; j++)
                {
                }
            }
        }
        
        private void ApplyRules()
        {
            for (int i = 0; i < _gridSize; i++)
            {
                for (int j = 0; j < _gridSize; j++)
                {
                    GameManager.Role roleInNextStage = GameManager.Role.Green;
                    List<Cell> cells = CountLivingNeighbours(i, j);
                    int livingNeighboursRed = DetectCountOfRoles(cells, GameManager.Role.Red);
                    int livingNeighboursGreen = DetectCountOfRoles(cells, GameManager.Role.Green);
                    int livingNeighboursBlue = DetectCountOfRoles(cells, GameManager.Role.Blue);

                    if (livingNeighboursRed >= livingNeighboursGreen &&
                        livingNeighboursRed >= livingNeighboursBlue)
                    {
                        SetNextCells(livingNeighboursRed, GameManager.Role.Red, i, j, cells);
                    }
                    else if (livingNeighboursGreen >= livingNeighboursRed &&
                             livingNeighboursGreen >= livingNeighboursBlue)
                    {
                        SetNextCells(livingNeighboursGreen, GameManager.Role.Green, i, j, cells);
                    }
                    else if (livingNeighboursBlue >= livingNeighboursRed &&
                             livingNeighboursBlue >= livingNeighboursGreen)
                    {
                        SetNextCells(livingNeighboursBlue, GameManager.Role.Blue, i, j, cells);
                    }
                }
            }

            GameEvents.Current.onRefreshCellState();
        }

        private void SetNextCells(int livingNeighbours, GameManager.Role roleInNextStage, int i, int j, List<Cell> cells)
        {
            Cell currentCell = _grid[i, j].GetComponent<Cell>();
            bool isCellDead = false;
            if (currentCell.GetEnviornment() == GameManager.Enviornment.Water)
            {
                switch (roleInNextStage)
                {
                    case GameManager.Role.Blue:
                        break;
                    case GameManager.Role.Red:
                        isCellDead = true;
                        break;
                    case GameManager.Role.Green:
                        /*isCellDead = true;
                        foreach (Cell cell in cells)
                        {
                            if (cell.GetRole() == GameManager.Role.Green &&
                                cell.GetEnviornment() == GameManager.Enviornment.Desert)
                            {
                                isCellDead = false;
                                break;
                            }
                        }*/
                        Debug.Log(isCellDead);
                        break;
                }
            }

            if (!isCellDead)
            {
                if (livingNeighbours == 3)
                {
                    if (roleInNextStage == GameManager.Role.Red)
                    {
                        foreach (Cell cell in cells)
                        {
                            if (cell.GetRole() != GameManager.Role.Red)
                            {
                                cell.SetNextStepCellState(true, GameManager.Role.Red); 
                                break;
                            }
                        }
                    }

                    if (roleInNextStage == GameManager.Role.Blue)
                    {
                        ActivateRandomBlueCells(1);
                    }
                    currentCell.GetComponent<Cell>().SetNextStepCellState(true, roleInNextStage);
                }
                else if(livingNeighbours == 2 && currentCell.GetIfCurrentlyActive())
                {
                    currentCell.GetComponent<Cell>().SetNextStepCellState(true, roleInNextStage);
                }
                else
                {
                    currentCell.GetComponent<Cell>().SetNextStepCellState(false, roleInNextStage);
                }
            }
            else
            {
                currentCell.GetComponent<Cell>().SetNextStepCellState(false, roleInNextStage);
            }
        }

        private int DetectCountOfRoles(List<Cell> cells, GameManager.Role role)
        {
            int result = 0;
            foreach (Cell cell in cells)
            {
                if (cell.GetRole() == role && !cell.GetIfIsAliveLonger())
                    result++;
            }

            return result;
        }

        private void ActivateRandomBlueCells(int howMany)
        {
            while (howMany > 0)
            {
                int randomI = Random.Range(0, _gridSize);
                int randomJ = Random.Range(0, _gridSize);

                Cell cell = _grid[randomI, randomJ].GetComponent<Cell>();
                if (!cell.GetIfCurrentlyActive())
                {
                    cell.SetNextStepCellState(true, GameManager.Role.Blue);
                    howMany--;
                }
            }
        }

        private List<Cell> CountLivingNeighbours(int i, int j)
        {
            List<Cell> result = new List<Cell>();
            for (int iNeigh = i - 1; iNeigh < i + 2; iNeigh++)
            {
                for (int jNeigh = j - 1; jNeigh < j + 2; jNeigh++)
                {
                    if (iNeigh == i && jNeigh == j) continue;
                    try
                    {
                        if (_grid[iNeigh, jNeigh].GetComponent<Cell>().GetIfCurrentlyActive())
                            result.Add(_grid[iNeigh, jNeigh].GetComponent<Cell>());
                    }
                    catch {}
                }
            }
            return result;
        }
    }
}
