using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class terrainElement : MonoBehaviour
{
    public int width = 10;
    private Mesh mesh;
    private MeshFilter meshFilter;
    private Vector3[] coords;
    private Vector3[] verts;
    private Color[] colors;
    private int[] tris;
    private Vector2[] uvs;

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
        CreateCoords();
        CreateMesh();
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

    private bool TriangulationCheck(Vector3 coord0, Vector3 coord1)
    {
        if(coord0.y == coord1.y)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    
    private Color VertexColor(float height)
    {
        if(height < 1.5)
        {
            return Color.black;
        }
        else if(height < 3.5)
        {
            return Color.red;
        }
        else
        {
            return Color.green;
        }
    }

    private void CreateMesh()
    {
        meshFilter = this.GetComponent<MeshFilter>();
        mesh = new Mesh();
        meshFilter.mesh = mesh;

        verts = new Vector3[width * width * 6];
        colors = new Color[verts.Length];
        uvs = new Vector2[verts.Length];
        tris = new int[width * width * 6];
    
        for (int i = 0, z = 0; z < width; z++) 
        {
            //outer loop for z-axis
			for (int x = 0; x < width; x++, i += 6) 
            {
                //setting verts
                verts[i] =   coords[x * (width + 1) + z];
                verts[i+1] = coords[(x +1) * (width+1) + z];
                verts[i+2] = coords[(x +1) * (width+1) + z + 1];
                verts[i+3] = coords[x * (width+1) + z + 1];

                colors[i] =  VertexColor(coords[x * (width + 1) + z].y);
                colors[i+1] = VertexColor(coords[(x +1) * (width+1) + z].y);
                colors[i+2] = VertexColor(coords[(x +1) * (width+1) + z + 1].y);
                colors[i+3] = VertexColor(coords[x * (width+1) + z + 1].y);
                
                if(TriangulationCheck( coords[x * (width + 1) + z],coords[(x +1) * (width+1) + z + 1]))
                {
                    //setting extra vertices
                    verts[i+4] = coords[x * (width + 1) + z];
                    verts[i+5] = coords[(x +1) * (width+1) + z + 1];

                    //setting vertex colors
                    colors[i+4] = VertexColor(coords[x * (width + 1) + z].y);
                    colors[i+5] = VertexColor(coords[(x +1) * (width+1) + z + 1].y);

                    //setting tris
                    tris[i] = i;
                    tris[i +1] = i +1;
                    tris[i +2] = i +2;
                    tris[i +3] = i +4;
                    tris[i +4] = i +5;
                    tris[i +5] = i +3;
                    //setting uvs
                    uvs[i] = new Vector2(0, 0);
                    uvs[i+1] = new Vector2(0,1 );
                    uvs[i+2] = new Vector2(1,1);
                    uvs[i+3] = new Vector2(1,0);
                    uvs[i+4] = new Vector2(0,0);
                    uvs[i+5] = new Vector2(1,1);
                }
                else
                {
                    //setting extra vertices
                    verts[i+4] = coords[x * (width+1) + z + 1];
                    verts[i+5] = coords[(x +1) * (width+1) + z];

                    //setting vertex colors
                    colors[i+4] = VertexColor(coords[x * (width+1) + z + 1].y);
                    colors[i+5] = VertexColor(coords[(x +1) * (width+1) + z].y);

                    //setting tris
                    tris[i] = i;
                    tris[i +1] = i +1;
                    tris[i +2] = i +3;
                    tris[i +3] = i +4;
                    tris[i +4] = i +5;
                    tris[i +5] = i +2;
                    //setting uvs
                    uvs[i] = new Vector2(0, 0);
                    uvs[i+1] = new Vector2(0,1 );
                    uvs[i+2] = new Vector2(1,1);
                    uvs[i+3] = new Vector2(1,0);
                    uvs[i+4] = new Vector2(1,0);
                    uvs[i+5] = new Vector2(0,1);
                }
                               
			}
		}
        mesh.vertices = verts;
        mesh.colors = colors;
        mesh.uv = uvs;
        mesh.triangles = tris;
        mesh.RecalculateNormals();
    }
}
