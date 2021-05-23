using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 타이틀 씬에서 메인씬으로 넘어가는 기능을 담당하는 클래스
/// </summary>
public class Title : MonoBehaviour
{

    public void OnClickGameStartButton()
    {
        this.StartCoroutine(SceneLoadRoutine());
    }

    IEnumerator SceneLoadRoutine()
    {
        AsyncOperation ao = SceneManager.LoadSceneAsync("Game", LoadSceneMode.Additive);
        
        Debug.Log("씬 로딩 시작!");
        while (ao.isDone == false)
        {
            Debug.Log(ao.progress);// 0.0~1.0f;
            yield return null;
        }

        Debug.Log("씬 로딩 끝!");

        yield return SceneManager.UnloadSceneAsync("Title");
    }
}
