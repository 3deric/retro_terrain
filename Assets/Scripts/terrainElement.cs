using UnityEngine;

public class terrainElement : MonoBehaviour
{
    public int width = 10;
    private Vector3[] coords;

    void OnDrawGizmosSelected()
    {
        foreach(Vector3 vec3 in coords)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(vec3, .1f);
        }
    }

    void Start()
    { 
        CreateCoords();
    }

    private void CreateCoords()
    {
        coords = new Vector3[(width + 1) * (width + 1)];
        /*
        every side needs to be 1 unity longer
        */
        for (int i = 0, z = 0; z <= width; z++)
        {
            //outer loop, z-axis
            for (int x = 0; x <= width; x++, i++) 
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
}
