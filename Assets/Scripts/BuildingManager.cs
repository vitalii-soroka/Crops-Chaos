using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class BuildingManager : MonoBehaviour
{
    [SerializeField] TileMapWrapper ground;
    [SerializeField] TileMapWrapper onGround;

    [SerializeField] BuildPreview buildPreview;

    [SerializeField] private BuildingSelectionMenu menu;

    [SerializeField] private GameObject buildParent;

    [SerializeField] private float collisionOffset = 0.2f;

    private GameObject buildPrefab = null;
    private TileMapWrapper buildTileMap = null;

    private Vector3 lastPreviewPosition;

    public TileBase testTile;

    public UnityEvent<TileBase, Vector3> OnTileBuild;

    void Start()
    {
        if (menu != null)
        {
            menu.BuildingSelected += OnSelect;
            menu.enabled = false;
        }
    }

    private bool IsMenuValidAndEnabled()
    {
        return menu != null && menu.gameObject.activeSelf;
    }

    private bool IsBuildingSelected() 
    { 
        return menu.GetSelectedBuilding() != null; 
    }

    void UpdatePreviewUI()
    {
        if (IsValidPlace())
        {
            if (buildPreview != null) buildPreview.SetPreviewColor(Color.white);
        }
        else
        {
            if (buildPreview != null) buildPreview.SetPreviewColor(Color.red);
        }
    }

    void Update()
    {
        // When build menu is disabled
        if (!IsMenuValidAndEnabled()) return;

        // When no Building
        if (!IsBuildingSelected()) return;

        // Stop Building when using UI
        if (IsMouseOverUI()) return;

        // Updates only when need
        if (lastPreviewPosition != buildPreview.GetPreviewPosition())
        {
            lastPreviewPosition = buildPreview.GetPreviewPosition();
            UpdatePreviewUI();
        }

        if (Input.GetMouseButtonDown(0) && IsValidPlace())
        {
            ConfirmBuild();
        }

        if (Input.GetMouseButtonDown(1))
        {
            CancelBuild();
        }
    }

    private bool IsMouseOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }

    private void ConfirmBuild()
    {
        if (buildPreview == null || !buildPreview.HasPreview()) return;

        // Build TileMap
        if (buildTileMap != null)
        {
            buildTileMap.
                SetTileNotify(
                buildTileMap.WorldToCell(buildPreview.GetPreviewPosition()), 
                buildPrefab.GetComponent<PreviewObject>().tile);

            // Changed Base Tile
            OnTileBuild.Invoke(testTile, buildPreview.GetPreviewPosition());
        }

        // Build other objects
        else
        {
            var build = Instantiate(buildPrefab, buildPreview.GetPreviewPosition(), Quaternion.identity);

            if (buildParent != null && build != null)
            {
                build.transform.SetParent(buildParent.transform, true);
            }
        }
    }

    private void CancelBuild()
    {
        if (buildPreview != null) buildPreview.Clear();
    }

    private bool IsValidPlace()
    {
        if (buildPreview == null || !buildPreview.HasPreview()) return false;

        // Case when we have tileMapWrapper object 
        if (buildTileMap != null)
        {
            // Can't place as same tile from same map already placed

            if (!buildTileMap.HasCollider()) return !buildTileMap.HasTile(lastPreviewPosition);

            // Second case for tilemaps with composite colliders
            if (buildTileMap.HasCollider())
            {
                return !IsCollisionSprite();
            }
        }

        if (buildPrefab.TryGetComponent<SpriteRenderer>(out var sprite))
        {
            if (IsCollisionSprite()) return false;
        }

        return true;
    }

    public bool IsCollisionSprite()
    {
        if (buildPrefab.TryGetComponent<SpriteRenderer>(out var sprite))
        {
            Vector2 boxSize = new Vector2(
                sprite.bounds.size.x - collisionOffset,
                sprite.bounds.size.y - collisionOffset
                );
            Vector2 boxCenter = lastPreviewPosition;


            // TODO add layer mask
            Collider2D hitCollider = Physics2D.OverlapBox(boxCenter, boxSize, 0f);
            if (hitCollider != null)
            {
                return true;
            }
        }

        return false;
    }

    private void OnSelect(GameObject prefab, TileMapWrapper map = null)
    {
        if (prefab == null) return;

        buildPrefab = prefab;

        if (buildPreview != null)
        {
            buildPreview.CreatePreview(prefab);
            lastPreviewPosition = buildPreview.GetPreviewPosition();
        }

        // For builds on TileMap
        if (map != null) buildTileMap = map;
        else buildTileMap = null;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        //if (buildPrefab != null && buildPrefab.TryGetComponent<SpriteRenderer>(out var sprite))
        //{
        //    Vector2 boxSize = new Vector2(sprite.bounds.size.x, sprite.bounds.size.y);
        //    Vector2 boxCenter = lastPreviewPosition;

        //    Gizmos.color = Color.green;

        //    Collider2D hitCollider = Physics2D.OverlapBox(boxCenter, boxSize, 0f);
        //    if (hitCollider != null)
        //    {
        //        Gizmos.color = Color.red; 
        //    }

        //    Gizmos.DrawWireCube(boxCenter, boxSize);
        //}
    }
#endif
}
