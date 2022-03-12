using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CombatStates
{
    public class ClashState : CombatKnockbackState
    {
        private new readonly MeleeCombat owner;

        private float timeSinceKnockbackEnded = 0f;

        public ClashState(MeleeCombat owner, Vector2 knockbackDirection) : base(owner, knockbackDirection)
        {
            this.owner = owner;
        }

        public override void Enter()
        {
            base.Enter();
            owner.Animator.SetBool("isClashing", true);
        }

        public override void Execute()
        {
            base.Execute();
            if (hasKnockbackEnded)
            {
                if (timeSinceKnockbackEnded > owner.PostClashKnockbackRecoveryTime)
                {
                    owner.CombatStateMachine.Exit();
                }
                else
                {
                    timeSinceKnockbackEnded += Time.deltaTime;
                }
            }
        }

        public override void Exit()
        {
            base.Exit();
            owner.Animator.SetBool("isClashing", false);
        }
    }
}