using System.Collections;
using UnityEngine;

/// <summary>
/// State becomes active to trigger the path traverse animation and waits for it to complete
/// before looping back to the unit selection state.
/// </summary>
public class MoveSequenceState : BattleState {

    public override void Enter() {
        base.Enter();
        StartCoroutine("Sequence");
    }

    IEnumerator Sequence() {
        Movement move = owner.currentUnit.GetComponent<Movement>();
        yield return StartCoroutine(move.Traverse(owner.currentTile));
        owner.ChangeState<SelectUnitState>();
    }

}
