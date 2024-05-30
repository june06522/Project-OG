using UnityEngine;

public class RotateWall : MonoBehaviour
{
    [SerializeField] float rotateSpeed;



    private void Update()
    {

        transform.Rotate(new Vector3(0, 0, rotateSpeed * Time.deltaTime));

    }
}
