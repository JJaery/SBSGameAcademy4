using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Title : MonoBehaviour
{
    public GameManager manager;

    void Update()
    {
        if(Input.anyKeyDown)
        {
            manager.LoadGameScene();
            Destroy(this); // 현재 스크립트삭제
        }
    }
}
