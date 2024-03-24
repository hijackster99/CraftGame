using Godot;
using System.Collections.Generic;

public partial class Controller : CharacterBody2D
{

	[Export]
	private Key left;
	[Export]
	private Key right;
	[Export]
	private Key jump;

	[Export]
	private float maxVelocity;
	[Export]
	private float terminalVelocity;
	[Export]
	private float accel;
	[Export]
	private float brake;
	[Export]
	private float jumpPower;
	[Export]
	private float gravity;

	float horizontalVelocity = 0;
	float verticalVelocity = 0;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

	}

	public override void _PhysicsProcess(double delta)
	{
		if (Input.IsPhysicalKeyPressed(left))
		{
			if (horizontalVelocity > 0) horizontalVelocity -= brake * (float)delta;
			else horizontalVelocity -= (accel * (float)delta);
			if (horizontalVelocity < -maxVelocity) horizontalVelocity = -maxVelocity;
		}
		if(Input.IsPhysicalKeyPressed(right))
		{
			if (horizontalVelocity < 0) horizontalVelocity += brake * (float)delta;
			else horizontalVelocity += (accel * (float)delta);
			if (horizontalVelocity > maxVelocity) horizontalVelocity = maxVelocity;
		}
		if(!Input.IsPhysicalKeyPressed(left) && !Input.IsPhysicalKeyPressed(right))
		{
			if (horizontalVelocity > -0.1 && horizontalVelocity < 0.1) horizontalVelocity = 0;
			if (horizontalVelocity < 0) horizontalVelocity += brake * (float)delta;
			else if (horizontalVelocity > 0) horizontalVelocity -= brake * (float)delta;
		}
		if (Input.IsPhysicalKeyPressed(jump) && verticalVelocity == 0)
		{
			verticalVelocity = jumpPower;
		}
		verticalVelocity += gravity * (float)delta;
		if (verticalVelocity > terminalVelocity) verticalVelocity = terminalVelocity;

		Vector2 hMotion = new Vector2(horizontalVelocity, 0);
		Vector2 vMotion = new Vector2(0, verticalVelocity);

		KinematicCollision2D hCol = MoveAndCollide(hMotion, true);
		KinematicCollision2D vCol = MoveAndCollide(vMotion, true);

		if (hCol != null)
		{
			MoveAndCollide(hMotion - (hMotion.Normalized() * hCol.GetDepth()));
		}else
		{
			MoveAndCollide(hMotion);
		}
		if (vCol != null)
		{
			MoveAndCollide(vMotion - (vMotion.Normalized() * vCol.GetDepth()));
			if (verticalVelocity > 0) verticalVelocity = 0;
		}
		else
		{
			MoveAndCollide(vMotion);
		}
	}
}