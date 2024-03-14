using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapBuilder : MonoBehaviour
{
    private Grid _grid;
    private GameObject _tilePrefab;
    private GameObject _currentTileInstance;
    private Vector3 _cellWorldPosition;
    private Bounds _buildArea;
    private List<Vector3> _placePosition = new List<Vector3>();

    private void Awake()
    {
        _grid = GetComponent<Grid>();
        _buildArea = new Bounds(new Vector3(0, 0, 0), new Vector3(10, 0, 10));
    }

    private void Update()
    {
        PlacingTile();
        UpdateTileColor();

        if (Input.GetMouseButtonDown(0))
        {
            CreateTile();
        }
    }

    public void StartPlacingTile(GameObject tilePrefab)
    {
        _tilePrefab = tilePrefab;

        if (_currentTileInstance != null)
        {
            Destroy(_currentTileInstance);
        }

        _currentTileInstance = Instantiate(_tilePrefab, transform);
        _currentTileInstance.transform.localPosition = Vector3.zero;
    }

    private void PlacingTile()
    {
        if (_currentTileInstance == null)
            return;

        Vector3 mousePosition = Vector3.zero;
        var worldPosition = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(worldPosition, out var hit))
        {
            mousePosition = hit.point;
        }

        var cellPosition = _grid.LocalToCell(mousePosition);

        _cellWorldPosition = _grid.GetCellCenterWorld(cellPosition);

        _currentTileInstance.transform.position = _cellWorldPosition;
    }

    private void CreateTile()
    {
        if (_currentTileInstance == null)
            return;

        if (CanCreateTile())
        {
            Instantiate(_tilePrefab, _cellWorldPosition, Quaternion.identity);
            _placePosition.Add(_currentTileInstance.transform.position);
        }
    }

    private bool IsWithinBuildArea()
    {
        return _buildArea.Contains(_cellWorldPosition);
    }


    private bool IsTileOccupied()
    {
        return !_placePosition.Contains(_cellWorldPosition);
    }

    private bool CanCreateTile()
    {
        return IsWithinBuildArea() && IsTileOccupied();
    }


    private void UpdateTileColor()
    {
        if (_currentTileInstance == null && _tilePrefab == null)
            return;

        SetTileColor(CanCreateTile() ? Color.green : Color.red);
    }

    private void SetTileColor(Color color)
    {
        if (_currentTileInstance.TryGetComponent<Tile>(out Tile colorize))
        {
            colorize.ChangeColor(color);
        }
    }
}