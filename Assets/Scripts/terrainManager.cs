using UnityEngine;

public class terrainManager : MonoBehaviour {

    public static terrainManager instance;
	public int terrainWidth, elementWidth;
	public terrainElement terrainPrefab;
	[HideInInspector]
    public Vector3[] coords;
    [HideInInspector]
	public terrainElement[] terrainElements;

    
	// void OnDrawGizmosSelected()
    // {
    //     foreach(Vector3 vec3 in coords)
    //     {
    //         Gizmos.color = Color.red;
    //         Gizmos.DrawSphere(vec3, .1f);
    //     }
    // }

    void Start()
    { 
        instance = this;
        CreateCoords();
		CreateTerrainElements();
    }

    private void CreateCoords()
    {
        coords = new Vector3[(terrainWidth + 1) * (terrainWidth + 1)];
        /*
        every side needs to be 1 unity longer
        */
        for (int i = 0, z = 0; z <= terrainWidth; z++)
        {
            //outer loop, z-axis
            for (int x = 0; x <= terrainWidth; x++, i++) 
            {
                /* 
                inner loop, x-axis
                setting height value based on perlin noise
                needs to be optimized low, mid and high frequency noise
                */
                float y = Mathf.PerlinNoise((float)x / 20, (float)z / 20) *10;
                y = Mathf.Floor(y) / 2;
                coords[i] = new Vector3(x, y, z);
            } 
        }

    }

	private void CreateTerrainElements()
	{
		int tilesPerSide = terrainWidth / elementWidth;
		terrainElements = new terrainElement[tilesPerSide*tilesPerSide];

		for (int i = 0, z = 0; z < tilesPerSide; z++)
        {
            //outer loop, z-axis
            for (int x = 0; x < tilesPerSide; x++, i++) 
            {

                terrainElement elementInstance = Instantiate(terrainPrefab, this.transform);
                elementInstance.Initialize(x,z);
                terrainElements[i] = elementInstance;
            }
        }
	}
	
}
