using Godot;
using System;

public partial class Player : Area2D
{
	[Signal]
	public delegate void HitEventHandler();
	[Signal]
	public delegate void TreatEventHandler();
	[Export]
	public int Speed{ get; set;} = 400;
	public Vector2 ScreenSize;
	
	private bool _hasMoved = false;
	public void Start(Vector2 position)
	{
		Position = position;
		Show();
		GetNode<CollisionPolygon2D>("CollisionPolygon2D").Disabled = false;

	}
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ScreenSize = GetViewportRect().Size;
		Start(new Vector2(ScreenSize.X/2, ScreenSize.Y/2));
		var animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		animatedSprite2D.Animation = "IntroIdle";
		animatedSprite2D.Play();

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

			Vector2 velocity = Input.GetVector("Move_Left", "Move_Right", "Move_Up", "Move_Down");
			var animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");

			if (velocity.Length() > 0)
			{
				velocity = velocity.Normalized() * Speed;
				animatedSprite2D.Animation = "Walk";
				_hasMoved = true;
				
			}
			else
			{
				if(_hasMoved)
				{
					var random = GD.RandRange(0,2);
					if(random < 1)
					{
						animatedSprite2D.Animation = "Idle";	
					}
					else
					{
						animatedSprite2D.Animation = "IntroIdle";	
					}		
					_hasMoved = false;		
				}
			}
			animatedSprite2D.Play();
			Position += velocity * (float)delta;
			Position = new Vector2(
				x: Mathf.Clamp(Position.X, 0, ScreenSize.X),
				y: Mathf.Clamp(Position.Y, 50, ScreenSize.Y-100)
			);
			if (velocity.X != 0)
			{
				animatedSprite2D.FlipV = false;
				animatedSprite2D.FlipH = velocity.X < 0;
			}
	}
	
	private void _on_body_entered(Node2D body)
	{
		if(body.GetType() == typeof(mob))
		{
			GD.Print("Polygon");
			Hide(); 
			EmitSignal(SignalName.Hit);
			GetNode<CollisionPolygon2D>("CollisionPolygon2D").SetDeferred(CollisionPolygon2D.PropertyName.Disabled, true);			
		}
		else if(body.GetType() == typeof(angryzone))
		{
			var monster = (mob)((angryzone)body).GetParent();
			monster.Angry();
		}
		else if(body.GetType() == typeof(treat))
		{
			EmitSignal(SignalName.Treat);
			var treat = (treat)body;
			treat.EatTreat();
		}
	}
	
	
	private void _on_body_exited(Node2D body)
	{
		if(body.GetType() == typeof(angryzone))
		{
			var monster = (mob)((angryzone)body).GetParent();
			monster.Idle();
		}
	}
}





