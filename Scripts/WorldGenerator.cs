using Godot;
using System;

public partial class WorldGenerator : Node
{
    [Export]
    private TileMap map;

    [Export]
    private WorldData world;

    System.Threading.Thread worldGen;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        worldGen = new System.Threading.Thread(generateWorld);
        worldGen.Start();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        if (!worldGen.IsAlive)
        {
            SceneTree tree = GetTree();

            map.GetParent().RemoveChild(map);

            GlobalData.map = map;

            tree.ChangeSceneToFile("res://Scenes/World.tscn");
        }
    }

    public void generateWorld()
    {
        initializeWorldGen();
    }

    public void initializeWorldGen()
    {
        for (int x = world.worldWidth/-2; x < world.worldWidth/2; x++)
        {
            for (int y = 0; y < world.worldDepth; y++)
            {
                world.setTile(x, y, 1);
            }
        }
    }
}
