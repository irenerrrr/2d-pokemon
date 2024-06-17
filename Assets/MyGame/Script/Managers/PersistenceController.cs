using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistenceController : MonoBehaviour
{
    public GameObject[] objectsToPersist;  // 在Inspector中设置想要持久化的对象数组
    private static PersistenceController instance; // 静态变量用于跟踪唯一


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
            Debug.Log("Destroying duplicate PersistenceController instance");
        }
        
    }
}
