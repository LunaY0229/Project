using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


namespace GamePlay.HumanFSM
{
    public class HumanFlyLocamotion : BaseSubState<FlyState>
    {
        private float maxHeight = 7f;
        public HumanFlyLocamotion(FSM fsm) : base(fsm)
        {
            //allChildState.Add(FlyState.Attack,);
        }

        public override void OnEnter()
        {
            (fsm as HumanFSM).ChangeAlpha_1();
            animator.CrossFadeInFixedTime("FlyStart",0.35f);
            (fsm as HumanFSM).isHasGravity = false;
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            (fsm as HumanFSM).characterController.Move(UnityEngine.Vector3.zero);

            if (fsm.transform.position.y < maxHeight)
            {
                fsm.transform.position += UnityEngine.Vector3.up * 3f;
            }
        }

        public override void OnExit()
        {
            (fsm as HumanFSM).isHasGravity = true;
        }
    }

    public enum FlyState
    {
        None,
        Attack
    }
}
