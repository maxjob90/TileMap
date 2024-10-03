using UnityEngine;

public class MapBuilder : MonoBehaviour
{
    [SerializeField] private GameObject _plane;
    [SerializeField] private Grid _grid;
    private Camera _camera;
    private Tile _currentTile;


    private void Awake()
    {
        _camera = Camera.main;
    }


    /// <summary>
    /// Данный метод вызывается автоматически при клике на кнопки с изображениями тайлов.
   /// </summary>
    public void StartPlacingTile(GameObject tilePrefab)
    {
        if (_currentTile)
        {
            Destroy(_currentTile.gameObject);
        }

        var tileObject = Instantiate(tilePrefab, _plane.transform);
        _currentTile = tileObject.GetComponent<Tile>();
    }


    private void Update()
    {
        if (_currentTile == null)
        {
            return;
        }

        var mousePosition = Input.mousePosition;
        var ray = _camera.ScreenPointToRay(mousePosition);

        if (Physics.Raycast(ray, out var hitInfo))
        {
            var worldPosition = hitInfo.point;
            var cellPosition = _grid.WorldToCell(worldPosition);
            var cellCenterWorld = _grid.GetCellCenterWorld(cellPosition);
            _currentTile.transform.position = cellCenterWorld;

            _currentTile.ChangeColor(IsTileWithinPlane(_currentTile), IsTilePlaced(_currentTile));

            if (Input.GetMouseButtonDown(0) && IsTileWithinPlane(_currentTile) && IsTilePlaced(_currentTile))
            {
                var tilePosition = _currentTile.transform.position;
                _currentTile.transform.position = new Vector3(tilePosition.x, 0, tilePosition.z);
                _currentTile.ResetColor();
                _currentTile = null;
            }
        }
    }

    private bool IsTileWithinPlane(Tile tile)
    {
        var positionTile = tile.transform.position;
        return positionTile.x >= -4.5 && positionTile.x <= 4.5 && positionTile.z >= -4.5 && positionTile.z <= 4.5;
    }

    private bool IsTilePlaced(Tile tile)
    {
        var tiles = _plane.GetComponentsInChildren<Tile>();

        for (var i = 0; i < tiles.Length - 1; i++)
        {
            if (tiles[i].transform.position == tile.transform.position)
            {
                return false;
            }
        }
        return true;
    }
}