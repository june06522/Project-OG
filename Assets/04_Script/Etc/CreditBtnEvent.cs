using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditBtnEvent : MonoBehaviour
{
    public void BtnClick()
    {
        AsyncSceneLoader.LoadScene("Intro");
    }
}
