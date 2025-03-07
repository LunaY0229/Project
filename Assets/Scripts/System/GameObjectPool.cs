using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectPool : MonoSingle<GameObjectPool>
{
    private Dictionary<string,Queue<GameObject>> allPoolObj = new Dictionary<string, Queue<GameObject>>();
    public void InitObj(GameObject obj, int num)
    {
        for(int i = 0; i < num; i++)
        {
            var newObj = Instantiate(obj,Vector3.zero,Quaternion.identity);
            newObj.transform.SetParent(transform);
            newObj.SetActive(false);
            string name = newObj.name;
            name = name.Replace("(Clone)","");

            if (allPoolObj.ContainsKey(name))
            {
                allPoolObj[name].Enqueue(newObj);
            }
            else
            {
                Debug.Log(name);
                allPoolObj.Add(name,new Queue<GameObject>());
                allPoolObj[name].Enqueue(newObj);
            }
        }
    }

    public GameObject GetGameObjectByName(string name)
    {
        if (allPoolObj.ContainsKey(name))
        {
            if (allPoolObj[name].Count > 0)
            {
                var res = allPoolObj[name].Dequeue();
                res.SetActive(true);
                return res;
            }
            else
            {
                Debug.LogError("对象池为" + name + "的物品不足");
            }
        }

        return null;
    }

    public void PushObj(GameObject obj)
    {
        obj.SetActive(false); 
        var name = obj.name.Replace("(Clone)","");
        allPoolObj[name].Enqueue(obj);
    }
}
