using Godot;
using System;

public partial class generate : TileMap
{
	System.Threading.Thread worldGen;

	[Export]
	private TileMap map;

	[Export]
	private int worldWidth;
    [Export]
    private int worldHeight;
    [Export]
    private int worldDepth;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		worldGen = new System.Threading.Thread(generateWorld);
		worldGen.Start();

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void generateWorld()
	{
        initializeWorldGen();
	}

	public void initializeWorldGen()
	{
		for (int x = 0; x < worldWidth; x++)
		{
			for(int y = 0; y < worldDepth; y++)
			{
				map.SetCell(0, new Vector2I(x - worldWidth/2, y), 0, new Vector2I(0, 0));
			}
		}
	}
}
