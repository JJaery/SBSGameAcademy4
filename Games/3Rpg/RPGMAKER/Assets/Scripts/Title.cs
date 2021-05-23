using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Ÿ��Ʋ ������ ���ξ����� �Ѿ�� ����� ����ϴ� Ŭ����
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
        
        Debug.Log("�� �ε� ����!");
        while (ao.isDone == false)
        {
            Debug.Log(ao.progress);// 0.0~1.0f;
            yield return null;
        }

        Debug.Log("�� �ε� ��!");

        yield return SceneManager.UnloadSceneAsync("Title");
    }
}
