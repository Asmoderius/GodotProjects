using Godot;
using System;

public partial class main : Node
{
	[Export]
	public PackedScene MobScene {get; set;}
	private int _score;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	
	private void NewGame()
	{
		GetNode<AudioStreamPlayer>("Music").Play();
		var hud = GetNode<HUD>("HUD");
		hud.UpdateScore(_score);
		hud.ShowMessage("Get Ready!");
		_score = 0;
		var player = GetNode<Player>("Player");
		var startPosition = GetNode<Marker2D>("StartPosition");
		player.Start(startPosition.Position);

		GetNode<Timer>("StartTimer").Start();
		
	}
	
	private void GameOver()
	{
			GetNode<AudioStreamPlayer>("Music").Stop();
   			GetNode<AudioStreamPlayer>("DeathSound").Play();
			GetTree().CallGroup("Mobs", Node.MethodName.QueueFree);
			GetNode<HUD>("HUD").ShowGameOver();
			GetNode<Timer>("MobTimer").Stop();
			GetNode<Timer>("ScoreTimer").Stop();
	}
	
	private void _on_mob_timer_timeout()
	{
	 	Mob mob = MobScene.Instantiate<Mob>();
		var mobSpawnLocation = GetNode<PathFollow2D>("MobPath/MobSpawnLocation");
   		mobSpawnLocation.ProgressRatio = GD.Randf();
		float direction = mobSpawnLocation.Rotation + Mathf.Pi / 2;
		mob.Position = mobSpawnLocation.Position;
		direction += (float)GD.RandRange(-Mathf.Pi / 4, Mathf.Pi / 4);
   		mob.Rotation = direction;
		var velocity = new Vector2((float)GD.RandRange(150.0, 250.0), 0);
		mob.LinearVelocity = velocity.Rotated(direction);
		AddChild(mob);
	
	}


	private void _on_score_timer_timeout()
	{
		 _score++;
		GetNode<HUD>("HUD").UpdateScore(_score);
	}


	private void _on_start_timer_timeout()
	{
		 GetNode<Timer>("MobTimer").Start();
		 GetNode<Timer>("ScoreTimer").Start();
	}
}










