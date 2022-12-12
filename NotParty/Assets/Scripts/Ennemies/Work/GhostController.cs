using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostController : WorkController
{
    public override float ActivateAbility() {
        //The Ghost doesn't actually have a special ability
        return specialAbilityDuration;
    }

}
