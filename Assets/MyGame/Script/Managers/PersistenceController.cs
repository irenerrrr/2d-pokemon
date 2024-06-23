using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class PersistenceController : MonoBehaviour
{
    public List<GameObject> objectsToPersist = new List<GameObject>(); // 持久化对象列表
    private static PersistenceController instance;
    private List<GameObject> beastsToPersist = new List<GameObject>(); // 专门存储Beast对象
    public Dictionary<string, SpiritualBeast> SpawnedBeasts { get; set; } = new Dictionary<string, SpiritualBeast>();
    public bool IsReturningFromBattle { get; set; } = false;

    public static PersistenceController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<PersistenceController>();
                if (instance == null)
                {
                    GameObject go = new GameObject("PersistenceController");
                    instance = go.AddComponent<PersistenceController>();
                    DontDestroyOnLoad(go);
                }
            }
            return instance;
        }
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this; // 设置唯一实例
            DontDestroyOnLoad(this.gameObject); // 使 PersistenceController 本身也持久化

            foreach (GameObject obj in objectsToPersist)
            {
                DontDestroyOnLoad(obj);
                Debug.Log("DontDestroyOnLoad: " + obj.name);
            }
        }
        else if (instance != this)
        {
            Destroy(this.gameObject); // 如果已经有一个持久化实例，销毁新的实例
            // Debug.Log("Destroying duplicate PersistenceController instance");
        }
    }


    public void AddBeastToPersist(GameObject obj)
    {
        if (!beastsToPersist.Contains(obj))
        {
            beastsToPersist.Add(obj);
            DontDestroyOnLoad(obj);
            // Debug.Log("Added Beast to DontDestroyOnLoad: " + obj.name);
        }
    }

    public void RemoveBeastFromPersist(GameObject obj)
    {
        if (beastsToPersist.Contains(obj))
        {
            beastsToPersist.Remove(obj);
            // Debug.Log("Removed Beast from DontDestroyOnLoad: " + obj.name);
        }
    }


    public void ClearPersistedObjects()
    {
        BeastComponent[] allBeasts = FindObjectsOfType<BeastComponent>();
        foreach (BeastComponent beast in allBeasts)
        {
            beast.MarkForDestruction();
            Destroy(beast.gameObject);
        }
        // Debug.Log("清理了所有Beast对象");
    }


    public void MoveBeastsToDontDestroyOnLoad(Dictionary<string, SpiritualBeast> spawnedBeasts)
    {
        // 获取场景中所有的BeastComponent对象
        BeastComponent[] allBeasts = FindObjectsOfType<BeastComponent>();
        // Debug.Log("开始保存，发现场景中有 " + allBeasts.Length + " 个Beast");

        foreach (BeastComponent beastComponent in allBeasts)
        {
            SpiritualBeast beast = beastComponent.beast;
            GameObject beastObject = beastComponent.gameObject;

            // 检查是否是已经遭遇的beast，不保存它
            if (beast == BeastComponent.encounteredBeast)
            {
                // Debug.Log("不保存已打败的敌人: " + beast.name);
                Destroy(beastObject);
                continue;
            }

            if (beastObject != null)
            {
                AddBeastToPersist(beastObject);
            }
        }

        // Debug.Log("保存结束: " + beastsToPersist.Count + " 个对象");
    }

    public void RestoreBeastsToScene()
    {
        foreach (var obj in beastsToPersist)
        {
            if (obj != null)
            {
                SceneManager.MoveGameObjectToScene(obj, SceneManager.GetActiveScene());
                obj.SetActive(true); // 确保对象被激活

                // 将对象重新附加到场景的根对象，移除DontDestroyOnLoad属性
                obj.transform.SetParent(null);
            }
        }
        // Debug.Log("恢复结束: " + beastsToPersist.Count + " 个对象");
        beastsToPersist.Clear();
       
    }
}
