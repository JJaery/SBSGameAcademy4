using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEventListener : MonoBehaviour
{
    public Character script;

    public void OnEvent()
    {
        script.OnHitEvent();
    }
}
