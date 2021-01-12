using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class MovementSystem : ComponentSystem
{
    protected override void OnUpdate()
    {
        float deltaTime = Time.DeltaTime;

        Entities.ForEach((ref Translation translation, ref Rotation rotation, ref MovementData movementData) =>
        {
            rotation.Value = Quaternion.Slerp(rotation.Value, Quaternion.LookRotation(movementData.velocity), 0.15f);
            translation.Value += movementData.velocity * (movementData.speed * deltaTime);
        });
    }
}
