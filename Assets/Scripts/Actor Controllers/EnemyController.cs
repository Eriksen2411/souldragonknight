using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyController : ActorController
{
    [Tooltip("This toggle is for allowing enemy visibility during debugging only.")]
    [SerializeField] private bool hideVisibility;
    [SerializeField] protected Visibility visibility;
    [SerializeField] protected float hurtRevealDuration;

    public BehaviorTreesManager BehaviorTreesManager { get; private set; }

    protected virtual void OnEnable()
    {
        if (photonView.IsMine)
        {
            Combat.HurtEvent.AddListener(HandleHurtEvent);
            Combat.DeathEvent.AddListener(HandleDeathEvent);
        }
    }

    protected virtual void OnDisable()
    {
        if (photonView.IsMine)
        {
            Combat.HurtEvent.RemoveListener(HandleHurtEvent);
            Combat.DeathEvent.RemoveListener(HandleDeathEvent);
        }
    }

    protected virtual void Start()
    {
        if (photonView.IsMine)
        {
            if (hideVisibility)
            {
                visibility.Hide();
            }

            BehaviorTreesManager = InitializeBehaviorTreesManager();
        }
    }

    protected override void Update()
    {
        base.Update();
        if (photonView.IsMine)
        {
            BehaviorTreesManager.UpdateActiveTree();
        }
    }

    protected abstract BehaviorTreesManager InitializeBehaviorTreesManager();

    protected virtual void HandleHurtEvent()
    {
        if (hideVisibility)
        {
            visibility.RevealBriefly(hurtRevealDuration);
        }
    }

    protected virtual void HandleDeathEvent()
    {
        BehaviorTreesManager.SwitchActiveTree(AiBehaviorTrees.BehaviorTree.Function.NONE);
        visibility.Reveal();
    }
}
