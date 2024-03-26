using Godot;
using System;

public class Chunk
{
	private byte[,] chunk;
	public int x { get; private set; }
	public int y { get; private set; }

	public bool dirty { get; set; }

	public int state { get; set; }

	public Chunk()
	{
		x = -1;
		y = -1;
		dirty = false;
		state = 0;
	}

	public Chunk(int x, int y)
	{
		this.x = x;
		this.y = y;
		dirty = false;
		state = 0;
	}

	public void save()
	{
		if (chunk == null) return;
		using var file = FileAccess.Open("user://Saves/" + GlobalData.name + "/level/chunk" + x + "_" + y + ".dat", FileAccess.ModeFlags.Write);
		foreach (byte b in chunk)
		{
			file.Store8(b);
		}
	}

	public void load()
	{
		chunk = new byte[32, 32];
		using var file = FileAccess.Open("user://Saves/" + GlobalData.name + "/level/chunk" + x + "_" + y + ".dat", FileAccess.ModeFlags.Read);
		if (file == null) return;
		int i = 0, j = 0;
		while (file.GetPosition() < file.GetLength())
		{
			chunk[i, j] = file.Get8();
			i++;
			if (i > 31)
			{
				i = 0;
				j++;
			}
		}
	}

	public static bool operator ==(Chunk c1, Chunk c2)
	{
		if ((object)c1 == null && (object)c2 == null) return true;
		if ((object)c1 == null || (object)c2 == null) return false;
		return c1.x == c2.x && c1.y == c2.y;
	}

	public static bool operator !=(Chunk c1, Chunk c2)
	{
		if ((object)c1 == null && (object)c2 == null) return false;
		if ((object)c1 == null || (object)c2 == null) return true;
		return c1.x != c2.x || c1.y != c2.y;
	}

	public override bool Equals(object obj)
	{
		if (obj is Chunk)
		{
			Chunk c = obj as Chunk;
			return this.x == c.x && this.y == c.y;
		}
		return false;
	}

	public override int GetHashCode() => (x, y).GetHashCode();

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
