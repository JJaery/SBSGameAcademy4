using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    public Text tweenTarget; // subtitle

    private void Start()
    {
        //�ڷ�ƾ
        //�Ϲ������� �޼ҵ�ȿ��� ���� ��ƾ(���� �ݺ���)�� ���۽�Ű��
        //����/���α׷��� ����ϴ�.
        //�ڷ�ƾ�� �޼ҵ带 �Ͻ����� ��ų �� �ִ� ����� ������ �ִ� �޼ҵ�
        StartCoroutine(TweenAnimation());
    }

    private void Update()
    {
        if(Input.anyKeyDown == true) //�ƹ�Ű�� ������ ��
        {
            //���� ���þ����� �̵�
            SceneManager.LoadScene("Scenes/2SelectGame");
        }
    }

    //�ڷ�ƾ
    //����� Update�� (�ݺ���) �ȿ��� �����Ű�� �ȵȴ�.
    //private IEnumerable �� �ƴմϴ�.
    private IEnumerator TweenAnimation()
    {
        tweenTarget.color = new Color(tweenTarget.color.r, tweenTarget.color.g, tweenTarget.color.b, 0.5f); //������ 50% ����
        float alpha = tweenTarget.color.a;

        while (true)
        {
            tweenTarget.color = new Color(tweenTarget.color.r, tweenTarget.color.g, tweenTarget.color.b, alpha);
            
            yield return null; // �������� �Ͻ����� -> ȭ���� �׷���.
            //yield return new WaitForSeconds(1.5f); // 1.5�� �Ͻ�����
            //yield return new WaitForSecondsRealtime(2f); // ���� �ӵ��� ������� ���� 2�� �Ͻ�����

            if (alpha >= 1)
            {
                alpha = 0.5f;
            }

            alpha += 0.5f * Time.deltaTime;
        }
    }
}