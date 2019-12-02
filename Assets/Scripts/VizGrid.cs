using UnityEngine;

public class VizGrid : MonoBehaviour {

    [SerializeField] private Map[] maps;
    
    private Material mat;
    
    private void Awake() {
        mat = new Material(Shader.Find("Mobile/Particles/Additive"));
    }

    private void OnPostRender() {

        for (int m = 0; m < maps.Length; ++m) {
            Map map = maps[m];
            GL.PushMatrix();
            mat.SetPass(0);
            GL.LoadOrtho();

            GL.Begin(GL.LINES);
            GL.Color(map.GridColor);
            Camera cam = Camera.main;
            Vector3 bottomLeft = map.Position - new Vector3(map.Width * 0.5f,
                                                            0f,
                                                            map.Length * 0.5f);
            for (int i = 0; i < map.Width + 1; ++i) {
                GL.Vertex(W2VP(bottomLeft + new Vector3(i, 0f, map.Length), cam));
                GL.Vertex(W2VP(bottomLeft + new Vector3(i, 0f, 0f),         cam));
            }

            for (int i = 0; i < map.Length + 1; ++i) {
                GL.Vertex(W2VP(bottomLeft + new Vector3(0f, 0f, i),         cam));
                GL.Vertex(W2VP(bottomLeft + new Vector3(map.Length, 0f, i), cam));
            }
        
            GL.End();
            GL.PopMatrix();

            GL.PushMatrix();
            mat.SetPass(0);
            GL.LoadOrtho();
            GL.Begin(GL.QUADS);
        
            for (int i = 0; i < map.Data.Length; ++i) {
                int e = map.Data[i];
                if (e == 0) {
                    continue;
                }
                
                GL.Color(map.DataColor.Evaluate((float)e / 255));
                int     x    = i % map.Width;
                int     y    = (i - x) / map.Length;
                Vector3 cell = new Vector3(x, 0f, y);
                GL.Vertex(W2VP(bottomLeft + cell + new Vector3(0f, 0f, 0f), cam));
                GL.Vertex(W2VP(bottomLeft + cell + new Vector3(0f, 0f, 1f), cam));
                GL.Vertex(W2VP(bottomLeft + cell + new Vector3(1f, 0f, 1f), cam));
                GL.Vertex(W2VP(bottomLeft + cell + new Vector3(1f, 0f, 0f), cam));
            }
        
            GL.End();
            GL.PopMatrix();
            
        }
    }

    private static Vector3 W2VP(Vector3 v3, Camera cam) {
        Vector3 v = cam.WorldToViewportPoint(v3);
        v.z = 0f;
        return v;
    }
}
