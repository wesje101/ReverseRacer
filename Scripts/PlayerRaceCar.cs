using Godot;
using System;

namespace ReverseRacer.Scripts
{
    public static class Globals
    {
        public static float MAX_STEER = 0.8f;
        public static float ENGINE_POWER = 300f;
    }

    public partial class PlayerRaceCar : VehicleBody3D
    {
        public override void _Process(double delta)
        {
            Steering = (float) Mathf.MoveToward(
                Steering, 
                Input.GetAxis("ui_right", "ui_left") * Globals.MAX_STEER, 
                delta * 2.5);
            EngineForce = Input.GetAxis("ui_down", "ui_up") * Globals.ENGINE_POWER;
        }

        public override void _Ready()
        {
            Input.SetMouseMode(Input.MouseModeEnum.Captured);
        }
    }
}