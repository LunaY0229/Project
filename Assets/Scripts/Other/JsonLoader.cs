using UnityEngine;

[System.Serializable]
public class MapInfo
{
    public Shelf[] shelves;
}

[System.Serializable]
public class Shelf
{
    public float[] pos;
    public string type;
}

[System.Serializable]
public class SceneData
{
    public float[] sceneSize;
    public MapInfo[] mapInfo;
}


public class JsonLoader : MonoBehaviour
{
    public TextAsset jsonFile;  // 在 Unity 编辑器中将 JSON 文件拖到这里

    void Start()
    {
        // 读取 JSON 数据
        string json = jsonFile.text;

        // 解析 JSON 数据
        SceneData sceneData = JsonUtility.FromJson<SceneData>(json);

        // 打印读取的结果（你可以查看解析后的数据）
        Debug.Log("Scene Size: " + sceneData.sceneSize[0] + ", " + sceneData.sceneSize[1]);
        foreach (var mapInfo in sceneData.mapInfo)
        {
            foreach (var shelf in mapInfo.shelves)
            {
                Debug.Log("Shelf at position: " + shelf.pos[0] + ", " + shelf.pos[1] + " with type: " + shelf.type);
            }
        }
    }
}
