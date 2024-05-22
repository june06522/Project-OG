using UnityEngine;

public class MovingWall : MonoBehaviour
{
    [SerializeField] float offset;
    [SerializeField] Vector2 moveDir;

    Vector2 anchorPos;
    float minX;
    float maxX;
    float minY;
    float maxY;

    private void Start()
    {

        anchorPos = transform.position;
        moveDir.Normalize();

        minX = anchorPos.x - moveDir.x * offset;
        maxX = anchorPos.x + moveDir.x * offset;

        minY = anchorPos.y - moveDir.y * offset;
        maxY = anchorPos.y + moveDir.y * offset;

    }

    private void Update()
    {

        float ratio = (Mathf.Sin(Time.time) + 1f) * 0.5f;

        float x = Mathf.Lerp(minX, maxX, ratio);
        float y = Mathf.Lerp(minY, maxY, ratio);

        transform.position = new Vector2(x, y);

    }
}
