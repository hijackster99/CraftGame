using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
public partial class WorldData : TileMap
{
	private List<Chunk> world;

	[Export]
	public CharacterBody2D player;

	public int worldWidth { get; private set; }
	public int worldHeight { get; private set; }
	public int worldDepth { get; private set; }
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		world = new List<Chunk>();

		worldWidth = GlobalData.worldWidth;
		worldHeight = GlobalData.worldHeight;
		worldDepth = GlobalData.worldDepth;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Vector2I mapPos = LocalToMap(player.Position);
		//Unload old chunks
		foreach(Chunk c in world)
		{
			if(c.x - (mapPos.X/32) + (worldWidth/2) > 2 || c.y - (mapPos.Y/32 + worldHeight) > 2 || c.x - (mapPos.X / 32) + (worldWidth / 2) < -2 || c.y - (mapPos.Y / 32 + worldHeight) < -2)
			{
				c.save();
				unloadChunk(c.x, c.y);
			}
		}

		//Load new chunks
		for(int i = -2; i < 3; i++)
		{
			for(int j = -2; j < 3; j++)
			{
				if(!world.Contains(new Chunk(i + (mapPos.X/32) + (worldWidth / 2), j + (mapPos.Y / 32) + worldHeight)))
				{
					Chunk c = readChunk(i + (mapPos.X/32) + (worldWidth / 2), j + (mapPos.Y/32) + worldHeight);

					if (c != null)
					{
						loadChunk(c);
					}
				}
			}
		}
	}

	public void writeChunk(int x, int y)
	{
		Chunk chunk = world.Find(c => (c.x == x && c.y == y));
		if (chunk.x != -1)
			chunk.save();
	}
	public Chunk readChunk(int x, int y)
	{
		Chunk c = new Chunk(x, y);

		if (!world.Contains<Chunk>(c))
		{
			c.load();
			return c;
		}
		return null;
	}

	public void loadChunk(Chunk chunk)
	{
		for (int x = 0; x < 32; x++)
		{
			for(int y = 0; y < 32; y++)
			{
				addTileToMap(chunk.x * 32 + x, chunk.y * 32 + y, chunk.getTile(x, y));
			}
		}
		world.Add(chunk);
	}

	public void unloadChunk(int x, int y)
	{
		for (int i = 0; i < 32; i++)
		{
			for (int j = 0; j < 32; j++)
			{
				removeTileFromMap(x*32 + i, y*32 + j);
			}
		}
	}
	public void addTileToMap(int x, int y, byte tile)
	{
		SetCell(0, new Vector2I(x - worldWidth*16, y - worldHeight*32), 0, new Vector2I(tile%16, tile/16));
	}

	public void removeTileFromMap(int x, int y)
	{
		EraseCell(0, new Vector2I(x - worldWidth * 16, y - worldHeight * 32));
	}

	public void setTile(int x, int y, byte tile)
	{
		setTileDirect(x + worldWidth * 16, y + worldHeight * 32, tile);
	}

	public void setTile(Vector2I coords, byte tile)
	{
		setTileDirect(coords.X + worldWidth * 16, coords.Y + worldHeight * 32, tile);
	}

	public byte getTile(int x, int y)
	{
		return getTileDirect(x + worldWidth * 16, y + worldHeight * 32);
	}

	public byte getTile(Vector2I coords)
	{
		return getTileDirect(coords.X + worldWidth * 16, coords.Y + worldHeight * 32);
	}

	public void setTileDirect(int x, int y, byte tile)
	{
		Chunk chunk = world.Find(c => (c.x == x/32 && c.y == y/32));
		chunk.setTile(x % 32, y % 32, tile);
	}

	public void setTileDirect(Vector2I coords, byte tile)
	{
		Chunk chunk = world.Find(c => (c.x == coords.X / 32 && c.y == coords.Y / 32));
		chunk.setTile(coords.X % 32, coords.Y % 32, tile);
	}

	public byte getTileDirect(int x, int y)
	{
		Chunk chunk = world.Find(c => (c.x == x / 32 && c.y == y / 32));
		return chunk.getTile(x % 32, y % 32);
	}

	public byte getTileDirect(Vector2I coords)
	{
		Chunk chunk = world.Find(c => (c.x == coords.X / 32 && c.y == coords.Y / 32));
		return chunk.getTile(coords.X % 32, coords.Y % 32);
	}

	public Vector2I TiletoDirect(Vector2I tileCoords)
	{
		return new Vector2I(tileCoords.X + worldWidth / 2, tileCoords.Y + worldHeight);
	}

	public Vector2I DirecttoTile(Vector2I directCoords)
	{
		return new Vector2I(directCoords.X - worldWidth / 2, directCoords.Y - worldHeight);
	}
}
