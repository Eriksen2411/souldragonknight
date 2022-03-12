using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CombatStates;
using AiBehaviorTreeNodes;
using AiBehaviorTreeBlackboards;

namespace AiBehaviorTrees
{
    public static class CombatTreeConstructor
    {
        public static BehaviorTree ConstructGroundCombatTree(Movement movement, MeleeCombat combat)
        {
            return new BehaviorTree(
                BehaviorTree.Function.COMBAT,
                new SequenceNode(new List<BehaviorNode>()
                {
                    new GetVisibleCombatTargetNode(combat),
                    new SelectorNode(new List<BehaviorNode>()
                    {
                        // in combat state, engaging target
                        new SequenceNode(new List<BehaviorNode>()
                        {
                            // in ready-attack state
                            new IsStateMachineInStateNode(combat.CombatStateMachine, typeof(ReadyAttackState)),
                            new AttackNode(combat)
                        }),
                        new IsStateMachineInStateNode(combat.CombatStateMachine, typeof(CombatState)),
                        // chasing target
                        new SequenceNode(new List<BehaviorNode>()
                        {
                            new SetCombatTargetPosNode(),
                            new GoToNavTargetNode(movement, true),
                            new StopMovingNode(movement),
                            new FaceNavTargetNode(movement),
                            new ReadyAttackNode(combat)
                        })
                    })
                }));
        }

        public static BehaviorTree ConstructAirCombatTree(Movement movement, ChargeCombat combat)
        {
            return new BehaviorTree(
                BehaviorTree.Function.COMBAT,
                new SequenceNode(new List<BehaviorNode>()
                {
                    new GetVisibleCombatTargetNode(combat),
                    new SelectorNode(new List<BehaviorNode>()
                    {
                        // in combat state, engaging target
                        new SequenceNode(new List<BehaviorNode>()
                        {
                            new SetCombatTargetPosNode(),
                            new SequenceNode(new List<BehaviorNode>()
                            {
                                // in ready-attack state
                                new IsStateMachineInStateNode(combat.CombatStateMachine, typeof(ReadyAttackState)),
                                new SelectorNode(new List<BehaviorNode>()
                                {
                                    // turn to face combat target if charge direction not yet locked
                                    new HasLockedTargetPositionNode(combat),
                                    new FaceNavTargetNode(movement)
                                }),
                                new AttackNode(combat)
                            })
                        }),
                        new IsStateMachineInStateNode(combat.CombatStateMachine, typeof(CombatState)),
                        // chasing target
                        new SequenceNode(new List<BehaviorNode>()
                        {
                            new SetReadyAttackPosNode(combat),
                            new GoToNavTargetNode(movement, false),
                            new StopMovingNode(movement),
                            new ReadyAttackNode(combat)
                        })
                    })
                }));
        }
    }
}