using Godot;
using System;
using System.IO;
using FileAccess = Godot.FileAccess;

public partial class WorldGenerator : Node
{
    private byte[,] world;

    [Export]
    private int worldWidth;
    [Export]
    private int worldHeight;
    [Export]
    private int worldDepth;

    System.Threading.Thread worldGen;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        GlobalData.worldWidth = worldWidth;
        GlobalData.worldHeight = worldHeight;
        GlobalData.worldDepth = worldDepth;

        world = new byte[worldWidth * 32, (worldHeight + worldDepth) * 32];

        worldGen = new System.Threading.Thread(generateWorld);
        worldGen.Start();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        if (!worldGen.IsAlive)
        {
            //save();

            GetTree().ChangeSceneToFile("res://Scenes/World.tscn");
        }
    }

    public void generateWorld()
    {
        //initializeWorldGen();
    }

    public void initializeWorldGen()
    {
        for (int x = 0; x < worldWidth * 32; x++)
        {
            for (int y = 0; y < worldDepth * 32; y++)
            {
                world[x, y] = 1;
            }
        }
    }

    public void save()
    {
        for(int x = 0; x < worldWidth; x++)
        {
            for(int y = 0; y < worldHeight + worldDepth; y++)
            {
                using var file = FileAccess.Open("user://Saves/" + GlobalData.name + "/level/chunk" + x + "_" + y + ".dat", FileAccess.ModeFlags.Write);

                for(int i = 0; i < 1024; i++)
                {
                    file.Store8(world[x * 32 + i % 32, y * 32 + i / 32]);
                }
            }
        }

    }
}
