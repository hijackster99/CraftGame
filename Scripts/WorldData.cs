using Godot;
using System;

public partial class WorldData : TileMap
{
	private byte[,] world;

	[Export]
	public int worldWidth { get; private set; }
    [Export]
    public int worldHeight { get; private set; }
    [Export]
    public int worldDepth { get; private set; }

    private bool load = false;
    
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        world = new byte[worldWidth, worldHeight + worldDepth];
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
        if (GetTree().CurrentScene.Name == "World" && !load)
        {
            for (int x = 0; x < worldWidth; x++)
            {
                for (int y = 0; y < worldHeight + worldDepth; y++)
                {
                    SetCell(0, new Vector2I(x - worldWidth / 2, y - worldHeight), 0, new Vector2I(world[x, y] % 16, world[x, y] / 16));
                }
            }
            load = true;
        }
	}

	public void setTile(int x, int y, byte tile)
	{
		world[x + worldWidth / 2, y + worldHeight] = tile;
	}

    public void setTile(Vector2I coords, byte tile)
    {
        world[coords.X + worldWidth / 2, coords.Y + worldHeight] = tile;
    }

    public byte getTile(int x, int y)
    {
        return world[x + worldWidth / 2, y + worldHeight];
    }

    public byte getTile(Vector2I coords)
    {
        return world[coords.X + worldWidth / 2, coords.Y + worldHeight];
    }
}
