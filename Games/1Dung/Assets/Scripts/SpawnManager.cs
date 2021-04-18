using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 일정 시간마다 특정 프리팹을 복사해서 생성
/// 1. 얼마만큼의 시간(간격)인지? v
/// 2. 특정 프리팹을 가져와야겠네.. v
/// 3. 어디에 생성하지? -> 생성 영역 설정 v
/// </summary>
public class SpawnManager : MonoBehaviour
{
    //인스펙터에서 프리팹을 설정함
    public GameObject targetPrefab;

    private float _curTime;

    private void Update()
    {
        if (GameManager.isGameStart == true)
        {
            if (_curTime <= 0)
            {
                //카피한 게임오브젝트가 obj변수에 들어감
                GameObject obj = Instantiate(targetPrefab);
                obj.transform.position = new Vector3(Random.Range(-10f, 10f), 10, 0);
                Rigidbody2D targetRigid = obj.GetComponent<Rigidbody2D>();
                targetRigid.gravityScale = Random.Range(1f, 5f);
                _curTime = Random.Range(0.1f, 1f); // 0.1초 ~ 1초
            }
            else
            {
                _curTime -= Time.deltaTime;
            }
        }
    }
}
