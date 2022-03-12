using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachines;

public abstract class Movement : MonoBehaviour
{
    public enum Direction { LEFT, RIGHT }

    [SerializeField] protected Rigidbody2D rigidbody2d;
    [SerializeField] protected Animator animator;
    [SerializeField] private bool isDefaultFacingRight = true;

    public Rigidbody2D Rigidbody2d { get => rigidbody2d; }
    public Animator Animator { get => animator; }

    public bool IsFacingRight { get; private set; }
    public Vector2 CachedMovementDirection { get; protected set; }

    public abstract MovementStateMachine MovementStateMachine { get; }

    protected virtual void Awake() { }

    protected virtual void OnEnable() { }

    protected virtual void OnDisable() { }

    protected virtual void Start()
    {
        FlipDirection(isDefaultFacingRight ? Direction.RIGHT : Direction.LEFT);
        IsFacingRight = isDefaultFacingRight;
    }

    public abstract void UpdateMovement(Vector2 direction);

    public void FlipDirection(float toDirection)
    {
        Vector3 localScale = transform.localScale;
        if (toDirection < 0f && localScale.x > 0f || toDirection > 0f && localScale.x < 0f)
        {
            localScale.x = -localScale.x;
            transform.localScale = localScale;
        }

        IsFacingRight = localScale.x > 0f;
    }

    public void FlipDirection(Direction toDirection)
    {
        switch (toDirection)
        {
            case Direction.LEFT:
                FlipDirection(-1f);
                break;
            case Direction.RIGHT:
                FlipDirection(1f);
                break;
            default:
                throw new System.ArgumentException("Direction " +
                    $"{System.Enum.GetName(typeof(Direction), toDirection)} " +
                    "is not a valid direction to flip towards");
        }
    }

    public float GetStoppingDistanceFromNavTarget()
    {
        // TODO: make this value a serialized field once we're sure no one else is modifying prefabs
        return 1.4f;
    }
}