using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detection : MonoBehaviour
{
    private static readonly float STRAIGHT_ANGLE_LEFT = 270f;
    private static readonly float STRAIGHT_ANGLE_RIGHT = 90f;

    [SerializeField] private float fov;
    [SerializeField] private float viewDistance;
    [SerializeField] private LayerMask viewLayer;

    private Transform actorTransform;
    private float halfFov;

    public float ViewDistance { get => viewDistance; }

    /// <summary>
    /// The clockwise angle from the global up direction to whichever direction the actor is looking towards.
    /// </summary>
    public float CurrViewAngle { get; private set; }

    private void Awake()
    {
        actorTransform = transform;
        halfFov = fov / 2f;
    }

    public bool IsVisible(Collider2D target)
    {
        Vector2 actorPosition = actorTransform.position;
        Vector2 targetPosition = target.bounds.center;
        if (Vector2.Distance(actorPosition, targetPosition) > viewDistance)
        {
            // target beyond view distance
            return false;
        }

        Vector2 directionToTarget = (targetPosition - actorPosition);
        float angleToTarget = GeneralUtility.ConvertDirectionToAngle(directionToTarget);
        if (angleToTarget < CurrViewAngle - halfFov || angleToTarget > CurrViewAngle + halfFov)
        {
            // target not within field of view angle
            return false;
        }

        RaycastHit2D raycastToTarget = Physics2D.Raycast(
            actorPosition, directionToTarget, viewDistance, viewLayer);
        
        return raycastToTarget.collider == target;
    }

    public void LookAt(Vector2 position)
    {
        CurrViewAngle = GeneralUtility.ConvertDirectionToAngle(position - (Vector2)transform.position);
    }

    public void LookStraight(bool isFacingRight)
    {
        CurrViewAngle = isFacingRight ? STRAIGHT_ANGLE_RIGHT : STRAIGHT_ANGLE_LEFT;
    }
}