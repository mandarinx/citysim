using UnityEngine;

public class Main : MonoBehaviour {

    [SerializeField] private Config config;

    private static Main self;
    public static Config Config => self.config;
    
    private void Awake() {
        self = this;
        for (int i = 0; i < config.Maps.Length; ++i) {
            config.Maps[i].Reset();
        }
    }

    private void Update() {
        if (!Input.GetMouseButtonUp(0)) {
            return;
        }

        Map mapEntities = config.Get("Entities");
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 localPos = mousePos - (mapEntities.Position - new Vector3(mapEntities.Width * 0.5f,
                                                                  0f,
                                                                  mapEntities.Length * 0.5f));
        int y = Mathf.FloorToInt(localPos.z);
        if (y < 0
            || y >= mapEntities.Length) {
            return;
        }
        int x = Mathf.FloorToInt(localPos.x);
        if (x < 0
            || x >= mapEntities.Width) {
            return;
        }

        int index = y * mapEntities.Width + x;
        mapEntities.SetData(index, 1);

        Map mapPollution = config.Get("Pollution");
        int pollutionLen = mapPollution.Data.Length;
        for (int i = 0; i < pollutionLen; ++i) {
            mapPollution.SetData(i, mapEntities.GetData(i) > 0 ? 255 : 0);
        }
        
        // Sweep up
        for (int i = 0; i < pollutionLen; ++i) {
            Compare(mapPollution, 
                    Map.GetCoord(i, mapPollution), 
                    new Vector2Int(1, 0), 
                    mapPollution.Width,
                    mapPollution.Length);
        }

        // Sweep down
        // for (int i = pollutionLen - 1; i >= 0; --i) {
        //     Compare(config.MapPollution.Data, 
        //             Map.GetCoord(i, config.MapPollution), 
        //             new Vector2Int(1, 0), 
        //             config.MapPollution.Width,
        //             config.MapPollution.Length);
        // }
    }
    
    //compares a pixel for the sweep, and updates it with a new distance if necessary
    private static void Compare(Map map, Vector2Int coord, Vector2Int offset, int mapWidth, int mapLength) {
        //calculate the location of the other pixel, and bail if in valid
        int otherx = coord.x + offset.x;
        int othery = coord.y + offset.y;
        if (otherx < 0
            || otherx >= mapWidth) {
            return;
        }
        if (othery < 0
            || othery >= mapLength) {
            return;
        }

        //read the distance values stored in both this and the other cell
        int curr_dist  = map.Data[coord.y * mapWidth + coord.x];
        int other_dist = map.Data[othery * mapWidth + otherx];

        //calculate a potential new distance, using the one stored in the other pixel,
        //PLUS the distance to the other pixel
        const float maxDist = 4f;
        float offsetDist = Mathf.Sqrt(offset.x * offset.x + offset.y * offset.y);
        int offsetVal = (int)(Mathf.Clamp01(offsetDist / maxDist) * 255f);
        int new_dist = other_dist + offsetVal;

        //if the potential new distance is better than our current one, update!
        if (new_dist < curr_dist) {
            map.SetData(coord.y * mapWidth + coord.x, new_dist);
        }
    }

}
