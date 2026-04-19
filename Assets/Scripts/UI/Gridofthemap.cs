using UnityEngine;

public class Gridofthemap : MonoBehaviour
{
    public static Gridofthemap instance;

    [Header("Grid Settings")]
    public int width = 20;
    public int height = 20;
    public float cellSize = 3f;
    public Vector2 gridOrigin = Vector2.zero;

    public GameObject gridCellPrefab;

    public int[,] grid = new int[20, 20]
    {
    {1,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
    {1,1,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
    {1,1,1,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
    {1,1,1,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
    {1,1,1,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1},

    {1,1,1,1,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
    {1,1,1,1,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
    {1,1,1,1,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
    {1,1,1,1,1,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1},
    {1,1,1,1,1,1,0,0,1,1,1,1,1,1,1,1,1,1,1,1},

    {1,1,1,1,1,1,1,0,0,1,1,1,1,1,1,1,1,1,1,1},
    {1,1,1,1,1,1,1,0,0,0,1,1,1,1,1,1,1,1,1,1},
    {1,1,1,1,1,1,1,0,0,0,1,1,1,1,1,1,1,1,1,1},
    {1,1,1,1,1,1,1,1,1,0,0,1,1,1,1,1,1,1,1,1},

    {1,1,1,1,1,1,1,1,1,0,0,1,1,1,1,1,1,1,1,1},
    {1,1,1,1,1,1,1,1,1,0,0,0,0,0,1,1,1,1,1,1},
    {1,1,1,1,1,1,1,1,1,1,0,0,0,0,1,1,1,1,1,1},
    {1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,1,1,1,1},

    {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1},
    {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1}
    };
    // Inside Gridofthemap.cs
    void Awake()
    {
        instance = this;

        // --- MOVE CALCULATIONS HERE ---
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        float worldWidth = sr.bounds.size.x;
        float worldHeight = sr.bounds.size.y;

        cellSize = worldWidth / width;

        // This defines the bottom-left corner of your grid in world space
        gridOrigin = transform.position - new Vector3(
            worldWidth / 2f,
            worldHeight / 2f
        );

        Debug.Log($"Grid Initialized. Origin: {gridOrigin}, CellSize: {cellSize}");
    }
    void Start()
    {
        GenerateGridVisual();
    }
void GenerateGridVisual()
{
    for (int x = 0; x < width; x++)
    {
        for (int y = 0; y < height; y++)
        {
            Vector3 pos = new Vector3(
                x * cellSize + gridOrigin.x + cellSize / 2f,
                y * cellSize + gridOrigin.y + cellSize / 2f,
                0
            );

            GameObject cell = Instantiate(gridCellPrefab, pos, Quaternion.identity);
            SpriteRenderer cellSr = cell.GetComponent<SpriteRenderer>();

            // Scale the cell down slightly so you see gaps (creating a grid effect)
            cell.transform.localScale = Vector3.one * 0.95f;

            if (grid[(height - 1) - y, x] == 0)
            {
                bool isOffset = (x + y) % 2 == 0;
                
                // Boost alpha slightly so you can actually see it
                float alpha = isOffset ? 0.1f : 0.18f; 
                
                // Soft Gold color
                cellSr.color = new Color(1f, 0.9f, 0.5f, alpha);
                cellSr.sortingOrder = -1;
            }
            else
            {
                // To keep the "borders" of the path visible, 
                // we can show the walls as very faint dark shadows
                cellSr.color = new Color(0, 0, 0, 0.2f); 
                // OR keep them off if you want the forest clean:
                // cellSr.enabled = false;
            }
        }
    }
}
    public bool IsWalkable(Vector2Int pos)
    {
        if (pos.x < 0 || pos.x >= width || pos.y < 0 || pos.y >= height)
            return false;

        // (height - 1) - pos.y flips the array so index 0 is the TOP
        return grid[(height - 1) - pos.y, pos.x] == 0;
    }
}