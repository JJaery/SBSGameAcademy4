using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    public Text tweenTarget; // subtitle

    private void Start()
    {
        //코루틴
        //일반적으로 메소드안에서 무한 루틴(무한 반복문)을 동작시키면
        //게임/프로그램이 멈춥니다.
        //코루틴은 메소드를 일시정지 시킬 수 있는 기능을 가지고 있는 메소드
        StartCoroutine(TweenAnimation());
    }

    private void Update()
    {
        if(Input.anyKeyDown == true) //아무키나 눌렸을 때
        {
            //게임 선택씬으로 이동
            SceneManager.LoadScene("Scenes/2SelectGame");
        }
    }

    //코루틴
    //절대로 Update문 (반복문) 안에서 실행시키면 안된다.
    //private IEnumerable 가 아닙니다.
    private IEnumerator TweenAnimation()
    {
        tweenTarget.color = new Color(tweenTarget.color.r, tweenTarget.color.g, tweenTarget.color.b, 0.5f); //투명도 50% 적용
        float alpha = tweenTarget.color.a;

        while (true)
        {
            tweenTarget.color = new Color(tweenTarget.color.r, tweenTarget.color.g, tweenTarget.color.b, alpha);
            
            yield return null; // 한프레임 일시정지 -> 화면을 그려라.
            //yield return new WaitForSeconds(1.5f); // 1.5초 일시정지
            //yield return new WaitForSecondsRealtime(2f); // 게임 속도와 관계없는 실제 2초 일시정지

            if (alpha >= 1)
            {
                alpha = 0.5f;
            }

            alpha += 0.5f * Time.deltaTime;
        }
    }
}
