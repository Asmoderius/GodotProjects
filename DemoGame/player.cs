using Godot;
using System;


public partial class Player : Area2D
{
	[Signal]
	public delegate void HitEventHandler();
	[Export]
	public int Speed{ get; set;} = 400;
	public Vector2 ScreenSize;
	
	public void Start(Vector2 position)
	{
		Position = position;
		Show();
		GetNode<CollisionShape2D>("CollisionShape2D").Disabled = false;
	}
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ScreenSize = GetViewportRect().Size;
		Start(new Vector2(ScreenSize.X/2, ScreenSize.Y/2));
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

			Vector2 velocity = Input.GetVector("move_left", "move_right", "move_up", "move_down");
			var animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");

			if (velocity.Length() > 0)
			{
				velocity = velocity.Normalized() * Speed;
				animatedSprite2D.Play();
			}
			else
			{
				animatedSprite2D.Stop();
			}
			Position += velocity * (float)delta;
			Position = new Vector2(
				x: Mathf.Clamp(Position.X, 0, ScreenSize.X),
				y: Mathf.Clamp(Position.Y, 0, ScreenSize.Y)
			);
			if (velocity.X != 0)
			{
				animatedSprite2D.Animation = "walk";
				animatedSprite2D.FlipV = false;
				// See the note below about boolean assignment.
				animatedSprite2D.FlipH = velocity.X < 0;
			}
			else if (velocity.Y != 0)
			{
				animatedSprite2D.Animation = "up";
				animatedSprite2D.FlipV = velocity.Y > 0;
			}
	}
	
	private void _on_body_entered(Node2D body)
	{
		Hide(); // Player disappears after being hit.
		EmitSignal(SignalName.Hit);
		// Must be deferred as we can't change physics properties on a physics callback.
		GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
	}
}


