using Godot;
using System;

public partial class InitWorld : Node
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Node player = GetNode("Player");

		player.GetParent().RemoveChild(player);

		Controller con = player as Controller;

		GD.Print(GlobalData.map);

		con.map = GlobalData.map;
		con.world = GlobalData.map as WorldData;

		GlobalData.map.AddChild(player);

		GetTree().Root.AddChild(GlobalData.map);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
