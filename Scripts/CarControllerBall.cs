using Godot;
using System;

public partial class CarControllerBall : RigidBody3D
{
    [ExportGroup("Properties")]
    [Export] Vector3 sphereOffset = Vector3.Zero;
    [Export] float acceleration = 35f;
    [Export] float steering = 18f;
    [Export] float turnSpeed = 4f;
    [Export] float turnStopLimit = .75f;
    [Export] float bodyTilt = 35;

    [ExportGroup("References")]
    [Export] MeshInstance3D carMesh;
    [Export] Node3D frontRightWheel;
    [Export] Node3D frontLeftWheel;
    [Export] Node3D backRightWheel;
    [Export] Node3D backLeftWheel;
    [Export] RayCast3D groundRay;

    float speedInput = 0f;
    float turnInput = 0f;

    public override void _PhysicsProcess(double delta)
    {
        carMesh.GlobalPosition = Position + sphereOffset;

        if (groundRay.IsColliding())
        {
            ApplyCentralForce(carMesh.GlobalTransform.Basis.X * speedInput);
        }
    }

    public override void _Process(double delta)
    {

        if (!groundRay.IsColliding())
        {
            return;
        }

        speedInput = Input.GetAxis("down", "up") * acceleration;
        turnInput = Input.GetAxis("right", "left") * Mathf.DegToRad(steering);

        Vector3 newWheelRotation = new Vector3(0, turnInput, 0);

        frontRightWheel.Rotation = newWheelRotation;
        frontLeftWheel.Rotation = newWheelRotation;


        if (LinearVelocity.Length() > turnStopLimit)
        {
            Basis newBasis = carMesh.GlobalTransform.Basis.Rotated(carMesh.GlobalTransform.Basis.Y, turnInput);
            Transform3D newTransform = carMesh.GlobalTransform;
            newTransform.Basis = carMesh.GlobalTransform.Basis.Slerp(newBasis, (float)(turnSpeed * delta));
            carMesh.GlobalTransform = newTransform;
            carMesh.GlobalTransform = carMesh.GlobalTransform.Orthonormalized();

            float tilt = -turnInput * LinearVelocity.Length() / bodyTilt;
            Vector3 newCarRotation = new Vector3(Mathf.Lerp(carMesh.Rotation.X, tilt, (float)(10 * delta)), carMesh.Rotation.Y, carMesh.Rotation.Z);
            carMesh.Rotation = newCarRotation;

            //align car with ground
            if (groundRay.IsColliding())
            {
                Vector3 n = groundRay.GetCollisionNormal();
                Transform3D xform = alignWithY(carMesh.GlobalTransform, n);
                carMesh.GlobalTransform = carMesh.GlobalTransform.InterpolateWith(xform, (float)(10.0 * delta));
            }
        }
    }

    private Transform3D alignWithY(Transform3D xform, Vector3 newY)
    {
        xform.Basis.Y = newY;
        xform.Basis.X = -xform.Basis.Z.Cross(newY);
        xform.Basis = xform.Basis.Orthonormalized();
        return xform.Orthonormalized();
    }
}
