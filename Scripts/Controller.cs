using Godot;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Security.Cryptography;

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

	public WorldData world;
	public TileMap map;

	List<Chunk> loadedChunks;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		loadedChunks = new List<Chunk>();
		world.printSize();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Vector2I chunk = map.LocalToMap(Position) / 32;
		List<Chunk> newChunks = new List<Chunk>();
		for (int i = 0; i < 9; i++)
		{
			Chunk c = new Chunk(i / 3 + chunk.X, i % 3 + chunk.Y, 0);
			if (loadedChunks.Contains(c))
			{
				c.load = 1;
                loadedChunks.Remove(c);
            }
            newChunks.Add(c);
        }
		foreach(Chunk c in loadedChunks)
		{
			c.load = -1;
			newChunks.Add(c);
		}
		loadedChunks = newChunks;
		LoadChunks();
		//if(chunk.X < 0 || chunk.Y < 0 || chunk.X > world.worldWidth/32 || chunk.Y > (world.worldHeight + world.worldDepth) / 32)

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

	private void LoadChunk(Chunk chunk)
	{
		if (chunk.x < 0 || chunk.y < 0 || chunk.x > world.worldWidth / 32 || chunk.y > (world.worldHeight + world.worldDepth) / 32 || chunk.load == 1) return;
        for (int i = chunk.x * 32; i < chunk.x * 32 + 32; i++)
		{
			for(int j = chunk.y * 32; j < chunk.y * 32 + 32; j++)
			{
				byte tile = world.getTileDirect(i, j);
				map.SetCell(chunk.load, world.DirecttoTile(new Vector2I(i, j)), 0, new Vector2I(tile % 16, tile / 16));
			}
		}
	}

	private void LoadChunks()
	{
		foreach (Chunk chunk in loadedChunks)
		{
			LoadChunk(chunk);
		}
	}

	private class Chunk
	{
		public int x { get; set; }
		public int y { get; set; }
		public int load { get; set; }

		public Chunk(int x, int y, int load)
		{
			this.x = x;
			this.y = y;
			this.load = load;
		}

		public static bool operator==(Chunk c1, Chunk c2)
		{
			return c1.x == c2.x && c1.y == c2.y;
		}

		public static bool operator !=(Chunk c1, Chunk c2)
		{
			return !(c1 == c2);
		}

		public override int GetHashCode() => (x, y).GetHashCode();

        public override bool Equals(object obj)
        {
            if(obj is Chunk)
			{
				Chunk c = obj as Chunk;
				return this == c;
			}
			return false;
        }
    }
}
