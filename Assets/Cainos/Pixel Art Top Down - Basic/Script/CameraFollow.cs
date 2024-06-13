using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cainos.PixelArtTopDown_Basic
{
    //let camera follow target
    public class CameraFollow : MonoBehaviour
    {

        
        public Transform target;
        public float lerpSpeed = 1.0f;

        private Vector3 offset;

        private Vector3 targetPos;

        private void Awake()
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;  // 根据标签找到玩家
            if (target == null)
            {
                Debug.LogError("Camera can't find the player!");
            }
        }

        private void Start()
        {
            if (target == null) return;

            offset = transform.position - target.position;
        }

        private void Update()
        {
            if (target == null) return;

            targetPos = target.position + offset;
            transform.position = Vector3.Lerp(transform.position, targetPos, lerpSpeed * Time.deltaTime);
        }
        
        void LateUpdate() {
        if (target != null) {
            transform.position = new Vector3(target.position.x, target.position.y, transform.position.z);
        }
    }

    }
}
