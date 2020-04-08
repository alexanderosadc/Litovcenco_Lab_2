using System;
using UnityEngine;

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
                }
            }
        }

        private void NextGeneration()
        {
           // PopulateGrid();
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

        /*private void PopulateGrid()
    {
        for (int i = 0; i < _gridWidth; i++)
        {
            for (int j = 0; j < gridHeight; j++)
            {

                Instantiate(cellPrefab, cellPosition, Quaternion.identity, transform);
            }
        }
    }

    private void DeleteDeadCells()
    {
        foreach (Transform item in transform)
        {
            Destroy(item);
        }
    }
*/
        private void ApplyRules()
        {
            GameObject[,] nextGenGrid = new GameObject[_gridSize, _gridSize];
            for (int i = 0; i < _gridSize; i++)
            {
                for (int j = 0; j < _gridSize; j++)
                {
                    int livingNeighbours = CountLivingNeighbours(i, j);
                        if (livingNeighbours == 3)
                        {
                            Debug.Log("Se naste");
                            _grid[i, j].GetComponent<Cell>().SetNextStepCellState(true);
                        }
                        else if(livingNeighbours == 2 && _grid[i, j].GetComponent<Cell>().GetVisibilityOfTheCell())
                        {
                            _grid[i, j].GetComponent<Cell>().SetNextStepCellState(true);
                        }
                        else
                        {
                            _grid[i, j].GetComponent<Cell>().SetNextStepCellState(false);
                        }
                }
            }

            GameEvents.Current.onRefreshCellState();
        }

        private int CountLivingNeighbours(int i, int j)
        {
            int result = 0;
            for (int iNeigh = i - 1; iNeigh < i + 2; iNeigh++)
            {
                for (int jNeigh = j - 1; jNeigh < j + 2; jNeigh++)
                {
                    if (iNeigh == i && jNeigh == j) continue;
                    try
                    {
                        if (_grid[iNeigh, jNeigh].GetComponent<Cell>().GetVisibilityOfTheCell())
                            result++;
                    }
                    catch {}
                }
            }
            return result;
        }
    }
}
