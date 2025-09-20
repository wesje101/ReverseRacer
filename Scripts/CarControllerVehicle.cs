using Godot;
using System;

public partial class CarControllerVehicle : VehicleBody3D
{
    [Export] float maxSteer = 0.9f;
    [Export] float steerSpeed = 10f;
    [Export] float enginePower = 300;

    public override void _PhysicsProcess(double delta)
    {
        Steering = (float)Mathf.MoveToward(Steering, Input.GetAxis("right", "left") * maxSteer, delta * steerSpeed);
        EngineForce = Input.GetAxis("down", "up") * enginePower;
    }
}
