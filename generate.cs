using Godot;
using System;

public partial class generate : Node
{
	System.Threading.Thread worldGen;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		worldGen = new System.Threading.Thread(() => worldGen.Start());
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}


}
