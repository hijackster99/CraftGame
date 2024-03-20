using Godot;
using System;

public partial class WorldData : TileMap
{
	private Chunk[,] world;

	[Export]
	public int worldWidth { get; private set; }
	[Export]
	public int worldHeight { get; private set; }
	[Export]
	public int worldDepth { get; private set; }
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		world = new Chunk[worldWidth, worldHeight + worldDepth];
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void save()
	{
		for(int x = 0; x < world.GetLength(0); x++)
		{
            for (int y = 0; y < world.GetLength(1); y++)
			{
				world[x, y].save(x, y);
			}
        }
	}
	public void load()
	{
        for (int x = 0; x < world.GetLength(0); x++)
        {
            for (int y = 0; y < world.GetLength(1); y++)
            {
                world[x, y].load(x, y);
            }
        }
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
		world[x / 32, y / 32].setTile(x % 32, y % 32, tile);
	}

	public void setTileDirect(Vector2I coords, byte tile)
	{
		world[coords.X / 32, coords.Y / 32].setTile(coords.X % 32, coords.Y % 32, tile);
	}

	public byte getTileDirect(int x, int y)
	{
		return world[x / 32, y / 32].getTile(x % 32, y % 32);
	}

	public byte getTileDirect(Vector2I coords)
	{
		return world[coords.X / 32, coords.Y / 32].getTile(coords.X % 32, coords.Y % 32);
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

	public class Chunk
	{
		private byte[,] chunk;

		public Chunk() 
		{
			chunk = null;
		}

		public void save(int x, int y)
		{
			if (chunk == null) return;
			using var file = FileAccess.Open("user://Saves//Test//level//" + x + "," + y, FileAccess.ModeFlags.Write);
			foreach(byte b in chunk)
			{
				file.Store8(b);
			}
			chunk = null;
		}

		public void load(int x, int y)
		{
			if (chunk != null) return;
			chunk = new byte[32, 32];
			using var file = FileAccess.Open("user://Saves//Test//level//" + x + "," + y, FileAccess.ModeFlags.Read);
			int i = 0, j = 0;
			while(file.GetPosition() < file.GetLength())
			{
				chunk[i, j] = file.Get8();
				i++;
				if(i > 31)
				{
					i = 0;
					j++;
				}
			}
        }

        public void init()
		{
			chunk = new byte[32, 32];
		}

		public void setTile(int x, int y, byte tile)
		{
			chunk[x, y] = tile;
		}

		public void setTile(Vector2I coords, byte tile)
		{
			chunk[coords.X, coords.Y] = tile;
		}

		public byte getTile(int x, int y)
		{
			return chunk[x, y];
		}

		public byte getTile(Vector2I coords)
		{
			return chunk[coords.X, coords.Y];
		}
	}
}
