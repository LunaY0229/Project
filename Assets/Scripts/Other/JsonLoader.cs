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
    public TextAsset jsonFile;  // �� Unity �༭���н� JSON �ļ��ϵ�����

    void Start()
    {
        // ��ȡ JSON ����
        string json = jsonFile.text;

        // ���� JSON ����
        SceneData sceneData = JsonUtility.FromJson<SceneData>(json);

        // ��ӡ��ȡ�Ľ��������Բ鿴����������ݣ�
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
