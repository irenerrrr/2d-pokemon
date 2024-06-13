using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistenceController : MonoBehaviour
{
    public GameObject[] objectsToPersist;  // 在Inspector中设置想要持久化的对象数组

    private static bool isPersisted = false; // 静态变量用于跟踪是否已经执行过持久化

    void Awake()
    {
        if (!isPersisted)
        {
            DontDestroyOnLoad(this.gameObject); // 使 PersistenceController 本身也持久化

            foreach (GameObject obj in objectsToPersist)
            {
                DontDestroyOnLoad(obj);
            }
            isPersisted = true; // 设置标志，防止再次持久化
        }
        else
        {
            Destroy(this.gameObject); // 如果已经持久化过，销毁新的实例
        }
    }
}
