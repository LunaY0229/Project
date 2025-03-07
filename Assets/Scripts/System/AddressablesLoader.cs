using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AddressablesLoader : MonoSingle<AddressablesLoader>
{
    private const string ROW_PERFAB = "Assets/Res/Perfabs/Row.prefab";
    private const string SHOOT_PERFAB = "Assets/Res/Perfabs/Shoot.prefab";
    private const string Zombie_LXPERFAB = "Assets/Res/Perfabs/YS.prefab";
    private const string Zombie_BOPERFAB = "Assets/Res/Perfabs/Bo.prefab";
    private const string JQ_BOPERFAB = "Assets/Res/Perfabs/JQ.prefab";

    private List<string> AllPerfab = new List<string>()
    {
        ROW_PERFAB,
        SHOOT_PERFAB,
        Zombie_LXPERFAB,
        Zombie_BOPERFAB,
        JQ_BOPERFAB
    };

    private void Awake()
    {
        Application.targetFrameRate = 60;
        LoadPerfabs();
    }

    public async void LoadPerfabs()
    {
        List<AsyncOperationHandle<GameObject>> hanles = new List<AsyncOperationHandle<GameObject>>();

        for(int i = 0; i < AllPerfab.Count; i++)
        {
            var handle = Addressables.LoadAssetAsync<GameObject>(AllPerfab[i]);
            hanles.Add(handle);
        }
        
        for(int i = 0; i < hanles.Count; i++)
        {
            await hanles[i].Task;
        }
        
        for(int i = 0; i < hanles.Count; i++)
        {
            GameObjectPool.Instance.InitObj(hanles[i].Result,100);
        }
    }
}
