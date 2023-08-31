using UnityEngine;

public class FishBehaviour : MonoBehaviour
{
    [SerializeField] FishBehaviourDataSO behaviourData = null;
    private AgentMovement movement = null;

    private Vector3 moveDirection = Vector3.zero;
    private float decisionTime = 0f;
    private bool onDanger = false;

    private void Awake()
    {
        movement = GetComponent<AgentMovement>();
    }

    private void Start()
    {
        movement.SetRotateSpeed(behaviourData.rotateSpeed);
    }

    private void Update()
    {
        Vector3 decidedVelocity = DecideDirection();
        if(decidedVelocity.sqrMagnitude > 0)
        {
            moveDirection = decidedVelocity;
            movement.SetVelocity(moveDirection);
        }
    }

    private Vector3 DecideDirection()
    {
        Vector3 dir = Vector3.zero;

        if(NearDanger(out Vector3 dangerDir))
        {
            if(onDanger == false)
            {
                onDanger = true;
                dir = -dangerDir.normalized * behaviourData.moveSpeed * behaviourData.speedFactor;
            }

            // 근처에 위험한 놈이 있으면 방향 결정 시간 연장
            decisionTime += Time.deltaTime;
        }
        else
        {
            if(onDanger)
                onDanger = false;

            // (1 / 활동성) -+ 활동성 마다 방향 바꾸기
            if(Time.time > decisionTime)
            {
                // 방향 다시 정하기
                
                float activeness = Mathf.Max(behaviourData.activeness, 0.01f);
                decisionTime = Time.time + ((1f / activeness) + Random.Range(-activeness, activeness));

                dir = Random.insideUnitCircle.normalized * behaviourData.moveSpeed;
            }
        }

        return dir;
    }

    private bool NearDanger(out Vector3 dangerDirection)
    {
        Collider2D[] dangers = Physics2D.OverlapCircleAll(transform.position, behaviourData.sight, behaviourData.enemyLayer);
        bool result = dangers.Length > 0;

        if(result)
            dangerDirection = dangers[0].transform.position - transform.position;
        else 
            dangerDirection = Vector3.zero;

        return result;
    }

    #if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        // if(UnityEditor.Selection.activeGameObject != gameObject)
        //     return;

        if(behaviourData == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, behaviourData.sight);
    }
    #endif
}
