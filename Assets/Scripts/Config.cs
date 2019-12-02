using UnityEngine;

[CreateAssetMenu(menuName = "Config", fileName = "Config.asset")]
public class Config : ScriptableObject {

    [SerializeField] private Map[] maps;

    public Map[] Maps => maps;

    public Map Get(string name) {
        Map map = null;
        for (int i = 0; i < maps.Length; ++i) {
            if (maps[i].name == name) {
                map = maps[i];
            }
        }
        return map;
    }
}
