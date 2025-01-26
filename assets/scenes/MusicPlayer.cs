using Godot;
using System;

public partial class MusicPlayer : Node
{
	[Export] public AudioStreamPlayer[] bgms;

	public void SetSections(Double[] vols) {
		for (int i=0; i<3; i++){
			Tween tw = CreateTween();
			tw.TweenProperty(
				bgms[i], "volume_db",
				Mathf.LinearToDb(vols[i]), 0.5
			);
			tw.Play();
		}
	}
}
