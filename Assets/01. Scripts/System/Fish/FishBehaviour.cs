using UnityEngine;

public class FishBehaviour : MonoBehaviour
{
    private AgentMovement movement;

    private void Awake()
    {
        movement = GetComponent<AgentMovement>();
    }

    private void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        movement.SetVelocity(x, y);

        // if(new Vector2(x, y).sqrMagnitude > 0)
        // {
        //     transform.right = Vector3.Lerp(transform.right, new Vector3(x, y), Time.deltaTime * 10f);
        // }
    }
}
