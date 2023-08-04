using UnityEngine;

[CreateAssetMenu(menuName = "SO/FishBehaviourData")]
public class FishBehaviourDataSO : ScriptableObject
{
    public LayerMask enemyLayer;
    public float sight;
    public float moveSpeed;
    public float speedFactor;
    public float rotateSpeed;
    public float activeness;
}
