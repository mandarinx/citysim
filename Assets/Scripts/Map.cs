using UnityEngine;

[CreateAssetMenu(menuName = "Map", fileName = "Map.asset")]
public class Map : ScriptableObject {

    [SerializeField] private Vector3 position;
    [SerializeField] private int     width;
    [SerializeField] private int     length;
    [SerializeField] private Color   gridColor;
    [SerializeField] private Gradient dataColor;
    [SerializeField] private int[]   data;

    public void Reset() {
        data = new int[length * width];
    }

    public int[]   Data      => data;
    public Vector3 Position      => position;
    public int     Width         => width;
    public int     Length        => length;
    public Color   GridColor     => gridColor;
    public Gradient DataColor     => dataColor;

    public void SetData(int index, int value) {
        Debug.Log($"Set data at index: {index} to: {value} on map {name}");
        data[index] = value;
    }

    public int GetData(int index) {
        return data[index];
    }

    public static Vector2Int GetCoord(int index, Map map) {
        int x = index % map.Width;
        int y = (index - x) / map.Length;
        return new Vector2Int(x, y);
    }
}
