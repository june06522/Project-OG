using UnityEngine;

public class MovingWall : MonoBehaviour
{
    [SerializeField] float offset;
    [SerializeField] float speed;
    [SerializeField] Vector2 moveDir;
    [SerializeField] LineRenderer line;

    Vector2 anchorPos;
    float minX;
    float maxX;
    float minY;
    float maxY;

    private void Start()
    {

        Init();

    }

    [ContextMenu("Test")]
    private void Init()
    {

        anchorPos = transform.position;
        moveDir.Normalize();

        minX = anchorPos.x - moveDir.x * offset;
        maxX = anchorPos.x + moveDir.x * offset;

        minY = anchorPos.y - moveDir.y * offset;
        maxY = anchorPos.y + moveDir.y * offset;

        line.SetPosition(0, new Vector2(minX, minY));
        line.SetPosition(1, new Vector2(maxX, maxY));
    }

    private void Update()
    {

        float ratio = (Mathf.Sin(Time.time * speed) + 1f) * 0.5f;

        float x = Mathf.Lerp(minX, maxX, ratio);
        float y = Mathf.Lerp(minY, maxY, ratio);

        transform.position = new Vector2(x, y);

    }
}
