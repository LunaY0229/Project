using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterLocamotion : BaseSubState<MonsterLocamotionState>
{
    public MonsterLocamotion(FSM fsm) : base(fsm)
    {
        allChildState.Add(MonsterLocamotionState.Run,new MonsterRunStates(fsm,this));
    }

    public override void OnEnter()
    {
        ChangeState(MonsterLocamotionState.Run);
    }

    public override void OnExit()
    {
        allChildState[currentState].OnExit();
    }
}

public enum MonsterLocamotionState
{
    Idel,
    Run
}
