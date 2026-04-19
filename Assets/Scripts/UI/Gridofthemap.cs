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
    {1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,1,1,1,1,1},
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

            // If the cell is WALKABLE (The Yellow Path)
            if (grid[(height - 1) - y, x] == 0)
            {
                // Make these look like "Step Stones"
                cell.transform.localScale = Vector3.one * 0.85f; // Smaller for a "button" look
                
                // Use a soft white/gold glow
                cellSr.color = new Color(1f, 1f, 1f, 0.2f); 
                cellSr.sortingOrder = 1; // Put it slightly ABOVE the ground
            }
            else // It is a WALL
            {
                // Instead of a box, make it a faint "Shadow"
                cell.transform.localScale = Vector3.one; 
                cellSr.color = new Color(0f, 0f, 0f, 0.4f); // Darker shadow
                cellSr.sortingOrder = -1; // Keep it behind the player
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