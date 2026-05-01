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
    {1,1,1,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
    {1,1,1,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
    {1,1,1,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
    {1,1,1,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
    {1,1,1,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},

    {1,1,1,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
    {1,1,1,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
    {1,1,1,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
    {1,1,1,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
    {1,1,1,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},

    {1,1,1,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
    {1,1,1,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
    {1,1,1,1,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
    {1,1,1,1,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1},

    {1,1,1,1,1,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1},
    {1,1,1,1,1,1,1,0,0,0,0,1,1,1,1,1,1,1,1,1},
    {1,1,1,1,1,1,1,1,1,0,0,0,0,1,1,1,1,1,1,1},
    {1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,1,1,1,1,1},

    {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1},
    {1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,1,1,1,1,1}
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
            if (grid[(height - 1) - y, x] != 0) continue;

            Vector3 pos = new Vector3(
                x * cellSize + gridOrigin.x + cellSize / 2f,
                y * cellSize + gridOrigin.y + cellSize / 2f,
                0
            );

            GameObject dust = Instantiate(gridCellPrefab, pos, Quaternion.identity);
            
            // SCALE: Make it small! This turns big snowflakes into tiny dust.
            dust.transform.localScale = Vector3.one * (cellSize * 0.3f); 

            // ROTATION: Keep this to make the trail look "fluffy"
            dust.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));

            SpriteRenderer sr = dust.GetComponent<SpriteRenderer>();
            
            // ALPHA: Make it very low (0.1f is 10% visibility)
            sr.color = new Color(1f, 1f, 1f, 0.1f); 
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