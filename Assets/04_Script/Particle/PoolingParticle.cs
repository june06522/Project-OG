using FD.Dev;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolingParticle : MonoBehaviour
{

    public void Push()
    {

        FAED.InsertPool(gameObject);

    }


}
