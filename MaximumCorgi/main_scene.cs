using Godot;
using System;

public partial class main_scene : Node
{
	[Export]
	public PackedScene MobScene {get; set;}
	[Export]
	public PackedScene TreatScene {get; set;}
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
		GD.Print("New Game");
		var hud = GetNode<HUD>("HUD");
		hud.UpdateScore(_score);
		hud.ShowMessage("Klar parat.. Go!");
		_score = 0;
		var player = GetNode<Player>("Player");
		var startPosition = GetNode<Marker2D>("StartPosition");
		player.Start(startPosition.Position);
		GD.Print("Timer start");
		GetNode<Timer>("StartTimer").Start();
		
	}
	
	private void GameOver()
	{
			GetTree().CallGroup("Mobs", Node.MethodName.QueueFree);
			GetTree().CallGroup("Treats", Node.MethodName.QueueFree);
			GetNode<HUD>("HUD").ShowGameOver();
			GetNode<Timer>("MobTimer").Stop();
			GetNode<Timer>("TreatTimer").Stop();
	}
	
	private void SpawnMonster()
	{
		mob monster = MobScene.Instantiate<mob>();
		var mobSpawnLocation = GetNode<PathFollow2D>("MobPath/MobSpawnLocation");
   		mobSpawnLocation.ProgressRatio = GD.Randf();
		float direction = mobSpawnLocation.Rotation + Mathf.Pi / 2;
		monster.Position = mobSpawnLocation.Position;
		direction += (float)GD.RandRange(-Mathf.Pi / 4, Mathf.Pi / 4);
		var velocity = new Vector2((float)GD.RandRange(25.0, 100.0), 0);
		monster.LinearVelocity = velocity.Rotated(direction);
		monster.GetNode<RigidBody2D>("AngryZone").LinearVelocity = monster.LinearVelocity;
		AddChild(monster);
	}
	private void EatTreat()
	{
		_score++;
		GetNode<HUD>("HUD").UpdateScore(_score);
	}
	private void _on_mob_timer_timeout()
	{
		SpawnMonster();
	}
	
	private void _on_treat_timer_timeout()
	{
		treat t = TreatScene.Instantiate<treat>();
		t.Position =  new Vector2(
				x: Mathf.Clamp((float)GD.RandRange(20, 1100), 0, 1200),
				y: Mathf.Clamp((float)GD.RandRange(20, 700), 0, 800)
			);
		AddChild(t);
	}
	


	private void _on_start_timer_timeout()
	{
		GD.Print("GO");
		GetNode<Timer>("MobTimer").Start();
		GetNode<Timer>("TreatTimer").Start();
	}
}





