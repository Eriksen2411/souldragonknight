using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AiBehaviorTreeNodes;
using AiBehaviorTreeBlackboards;

namespace AiBehaviorTrees
{
    public static class CombatTreeConstructor
    {
        public static BehaviorTree ConstructGroundCombatTree(Movement movement, Combat combat)
        {
            return new BehaviorTree(
                BehaviorTree.Function.COMBAT,
                new SequenceNode(new List<BehaviorNode>()
                {
                    new GetVisibleCombatTargetNode(),
                    new SetCombatTargetPosNode(),
                    new GoToNavTargetNode(movement, true),
                    new StopMovingNode(movement),
                    new UpdateTimeVariableNode(CombatBlackboardKeys.TIME_SINCE_LAST_ATTACK),
                    new AttackTargetNode(combat)
                }));
        }
    }
}
