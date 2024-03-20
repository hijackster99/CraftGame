using Godot;
using System;

public partial class WorldData : TileMap
{
	private byte[,] world;

	public byte this[int x, int y]
	{
		get { return world[x + worldWidth/2, y + worldHeight]; }
		set { world[x + worldWidth / 2, y + worldHeight] = value; }
	}

	[Export]
	public int worldWidth { get; private set; }
	[Export]
	public int worldHeight { get; private set; }
	[Export]
	public int worldDepth { get; private set; }
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		world = new byte[worldWidth, worldHeight + worldDepth];
		GD.Print(world.Length);
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void setTile(int x, int y, byte tile)
	{
		this[x, y] = tile;
	}

	public void setTile(Vector2I coords, byte tile)
	{
		this[coords.X, coords.Y] = tile;
	}

	public byte getTile(int x, int y)
	{
		return this[x, y];
	}

	public byte getTile(Vector2I coords)
	{
		return this[coords.X, coords.Y];
	}

    public void setTileDirect(int x, int y, byte tile)
    {
        world[x, y] = tile;
    }

    public void setTileDirect(Vector2I coords, byte tile)
    {
        world[coords.X, coords.Y] = tile;
    }

    public byte getTileDirect(int x, int y)
    {
        return world[x, y];
    }

    public byte getTileDirect(Vector2I coords)
    {
        return world[coords.X, coords.Y];
    }

	public Vector2I TiletoDirect(Vector2I tileCoords)
	{
		return new Vector2I(tileCoords.X + worldWidth / 2, tileCoords.Y + worldHeight);
	}

    public Vector2I DirecttoTile(Vector2I directCoords)
    {
		return new Vector2I(directCoords.X - worldWidth / 2, directCoords.Y - worldHeight);
    }

	public void printSize()
	{
		GD.Print(world.Length);
	}
}
