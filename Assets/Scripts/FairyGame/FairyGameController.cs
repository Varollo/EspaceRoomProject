using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FairyGameController : MonoBehaviour, IInteractable
{
    [Header("Settings")]
    [SerializeField] private int _randomizeAmt;
    [SerializeField] private Material _highlightMaterial;
    [SerializeField] private InventoryItem _fairyDustItem;
    [Header("References")]
    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private CameraController _cameraController;
    [SerializeField] private ItemDisplayer _itemDisplayer;
    [SerializeField] private UIShowHide _itemShowHide;
    [SerializeField] private Inventory _inventory;
    [SerializeField] private FairyTile[] _fairyTiles;
    [SerializeField] private Renderer _renderer;

    private Renderer[] _tileRenderers;
    private Material _originalMaterial;
    private Material[] _originalTileMaterials;

    private FairyTile[,] _grid;
    private string[,] _winGrid;

    private int[] _emptyTile;

    private bool IsTileOffGrid(int i, int j) => i >= _grid.GetLength(0) || j >= _grid.GetLength(1) || i < 0 || j < 0;

    private bool IsTileEmpty(int i, int j)
    {
        if (IsTileOffGrid(i, j)) return false;

        return i == _emptyTile[0] && j == _emptyTile[1];
    }

    private void Start()
    {
        _originalMaterial = _renderer.material;

        _tileRenderers = new Renderer[_fairyTiles.Length];
        _originalTileMaterials = new Material[_fairyTiles.Length];

        for (int i = 0; i < _fairyTiles.Length; i++)
        {
            _tileRenderers[i] = _fairyTiles[i].GetComponent<Renderer>();
            _originalTileMaterials[i] = _tileRenderers[i].material;
        }

        FillGrid();

    }

    private void StartGame()
    {
        RandomizeTiles();
    }

    private void FillGrid()
    {
        _grid = new FairyTile[3, 3];
        _winGrid = new string[3, 3];
        _emptyTile = new int[2];

        for (int i = 0; i < _grid.GetLength(0); i++)
        {
            for (int j = 0; j < _grid.GetLength(1); j++)
            {
                if (i == 2 && j == 2)
                {
                    _emptyTile[0] = i;
                    _emptyTile[1] = j;
                    break;
                }

                _grid[i, j] = _fairyTiles[(j * _grid.GetLength(0)) + i];
                _winGrid[i, j] = _grid[i, j].name;
            }
        }
    }

    private int[][] GetOccupiedNeighbours(int i, int j)
    {
        List<int[]> neighbours = new List<int[]>();

        int[] west = { i - 1, j };
        int[] east = { i + 1, j };
        int[] north = { i, j + 1 };
        int[] south = { i, j - 1 };

        if (!IsTileEmpty(west[0], west[1]) && !IsTileOffGrid(west[0], west[1]))
        {
            neighbours.Add(west);
        }
        if (!IsTileEmpty(east[0], east[1]) && !IsTileOffGrid(east[0], east[1]))
        {
            neighbours.Add(east);
        }
        if (!IsTileEmpty(north[0], north[1]) && !IsTileOffGrid(north[0], north[1]))
        {
            neighbours.Add(north);
        }
        if (!IsTileEmpty(south[0], south[1]) && !IsTileOffGrid(south[0], south[1]))
        {
            neighbours.Add(south);
        }

        return neighbours.ToArray();
    }

    private int[] GetEmptyNeighbour(int i, int j)
    {
        int[] west = { i - 1, j };
        int[] east = { i + 1, j };
        int[] north = { i, j + 1 };
        int[] south = { i, j - 1 };

        if (IsTileEmpty(west[0], west[1]))
        {
            return west;
        }
        else if (IsTileEmpty(east[0], east[1]))
        {
            return east;
        }
        else if (IsTileEmpty(north[0], north[1]))
        {
            return north;
        }
        else if (IsTileEmpty(south[0], south[1]))
        {
            return south;
        }
        else
        {
            return new int[2] { -1, -1 };
        }
    }

    public void MoveTile(FairyTile tile, bool isRandomizing = false)
    {
        int[] tilePosition = GetTilePosition(tile);
        if (tilePosition[0] == -1) return;

        int[] emptyNeighbour = GetEmptyNeighbour(tilePosition[0], tilePosition[1]);
        if (emptyNeighbour[0] == -1) return;

        _grid[emptyNeighbour[0], emptyNeighbour[1]] = tile;
        _grid[tilePosition[0], tilePosition[1]] = null;

        _emptyTile[0] = tilePosition[0];
        _emptyTile[1] = tilePosition[1];

        tile.Move(new Vector3(tilePosition[1] - emptyNeighbour[1], 0, tilePosition[0] - emptyNeighbour[0]));

        if (isRandomizing) return;

        if (IsComplete())
        {
            EndGame();
        }
    }

    public int[] GetTilePosition(FairyTile tile)
    {
        for (int i = 0; i < _grid.GetLength(0); i++)
        {
            for (int j = 0; j < _grid.GetLength(1); j++)
            {
                if (IsTileEmpty(i, j)) continue;

                if (tile.name == _grid[i, j].name)
                {
                    return new int[2] { i, j };
                }

            }
        }

        return new int[2] { -1, -1 };
    }

    private bool IsComplete()
    {
        for (int i = 0; i < _grid.GetLength(0); i++)
        {
            for (int j = 0; j < _grid.GetLength(0); j++)
            {
                if (_grid[i, j] == null) continue;

                if (_grid[i, j].name != _winGrid[i, j])
                {
                    return false;
                }
            }
        }

        return true;
    }

    private void RandomizeTiles()
    {
        for (int c = 0; c < _randomizeAmt; c++)
        {
            int i1 = _emptyTile[0];
            int j1 = _emptyTile[1];

            int[][] neighbours = GetOccupiedNeighbours(i1, j1);

            int r = UnityEngine.Random.Range(0, neighbours.Length);

            int i2 = neighbours[r][0];
            int j2 = neighbours[r][1];

            MoveTile(_grid[i2, j2], isRandomizing: true);
        }
    }

    private void EndGame()
    {
        foreach (FairyTile tile in _fairyTiles)
        {
            tile.GetComponent<Collider>().enabled = false;
        }

        _inventory.AddItem(_fairyDustItem);
        _itemDisplayer.DisplayItem(_fairyDustItem);

        _itemShowHide.OnHide.AddListener(OnCloseItemPopup);
    }

    private void OnCloseItemPopup()
    {
        PlayerMovement.CanMove = true;
        _playerMovement.MoveToCamera(3);
        _inventory.ShowHud();
        _itemShowHide.OnHide.RemoveListener(OnCloseItemPopup);
    }

    public void OnHighlight()
    {
        _renderer.material = _highlightMaterial;

        foreach (Renderer renderer in _tileRenderers)
        {
            renderer.material = _highlightMaterial;
        }
    }

    public void OnInteract()
    {
        GetComponent<Collider>().enabled = false;
        _cameraController.ActiveCamera = "CameraFairy";
        _inventory.HideHud();
        StartGame();
    }

    public void OnResetHighlight()
    {
        _renderer.material = _originalMaterial;

        for (int i = 0; i < _tileRenderers.Length; i++)
        {
            _tileRenderers[i].material = _originalTileMaterials[i];
        }
    }
}
