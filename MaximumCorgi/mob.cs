using Godot;
using System;

public partial class mob : RigidBody2D
{
	public override void _Ready()
	{
		var animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		animatedSprite2D.Animation = "Idle";
		animatedSprite2D.Play();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	public void Idle()
	{
			var animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		animatedSprite2D.Animation = "Idle";
		animatedSprite2D.Play();
	}
	
	public void Angry()
	{
			var animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		animatedSprite2D.Animation = "Angry";
		animatedSprite2D.Play();
	}
	private void _on_visible_on_screen_notifier_2d_screen_exited()
	{
		QueueFree();
	}
}

