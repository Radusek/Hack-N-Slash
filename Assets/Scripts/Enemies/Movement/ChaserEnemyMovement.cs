﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaserEnemyMovement : EnemyController
{
    protected override void InteractWithTarget()
    {
        agent.destination = target.position;
    }
}
