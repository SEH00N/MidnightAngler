using UnityEngine;

public class AgentMovement : MonoBehaviour
{
    [SerializeField] float movementSpeed = 1;

    private Vector2 moveVelocity = Vector2.zero;
    private float rotateSpeed = 10f;

    private void FixedUpdate()
    {
        Vector3 dir = moveVelocity * movementSpeed * Time.fixedDeltaTime;
        transform.position += dir;

        if(moveVelocity.sqrMagnitude > 0f)
        {
            Vector3 normal = moveVelocity.normalized;
            float angle = Mathf.Atan2(normal.y, normal.x) * Mathf.Rad2Deg;
            Quaternion rotate = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotate, Time.fixedDeltaTime * rotateSpeed);

            if(normal.x >= 0.1f)
            {
                if(transform.localScale.y != 1)
                    transform.localScale = new Vector3(1, 1, 1);
            }
            else if(normal.x <= 0.1f)
            {
                if(transform.localScale.y != -1)
                    transform.localScale = new Vector3(1, -1, 1);
            }
        }
    }

    public void SetVelocity(Vector2 velocity) => this.moveVelocity = velocity;
    public void SetVelocity(float x, float y) => this.moveVelocity = new Vector2(x, y);
    public void SetRotateSpeed(float speed) => rotateSpeed = speed;
}
