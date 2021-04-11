using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputOuput : MonoBehaviour
{
    int i = 0;

    //스크립트가 시작될때 한번 호출됩니다. 
    void Start()
    {
        Debug.Log("일반로그");
        Debug.LogWarning("경고로그");
        Debug.LogError("에러로그");
    }
    //매 프레임마다 호출됩니다. e.g) 1000 fps -> 1초에 1000번 실행 60fps -> 1초에 60번실행
    void Update()
    {
        // 해당 키가 눌렸는지?
        if(Input.GetKeyDown(KeyCode.A) == true)
        {
            Debug.Log("A");
        }

        if(Input.GetKey(KeyCode.D) == true)
        {
            Debug.Log("D");
        }

        if(Input.GetKeyUp(KeyCode.D) == true)
        {
            Debug.Log("D up");
        }
    }
    public void OnClickButton()
    {
        Debug.Log($"버튼이 {++i}회 눌렸습니다.");
    }

    public void OnChangeSliderValue(Slider sender)
    {
        Debug.Log($"ChangeValue - {sender.value}");
    }

    public void OnChangeScrollBarValue(Scrollbar sender)
    {
        Debug.Log($"[ScrollBar] ChangeValue - {sender.value}");
    }

    public void OnChangeDDBoxValue(Dropdown sender)
    {
        Debug.Log($"[Dropdown] ChangeValue - {sender.value}");
    }

    public void OnChangeInputFieldValue(InputField sender)
    {
        Debug.Log($"[InputField] ChangeValue - {sender.text}");
    }

    public void OnEndEditInputField(InputField sender)
    {
        Debug.Log($"[InputField] EndEdit - {sender.text}");
    }

    public void OnChangeScrollRect(ScrollRect sender)
    {
        Debug.Log($"[ScrollRect] ValueChanged - {sender.normalizedPosition}");
    }
}
