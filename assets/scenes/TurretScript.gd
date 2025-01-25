extends Sprite2D

@onready var bubbleShot = preload("res://assets/scenes/BubbleShot.tscn")

@export var shoot_player: AudioStreamPlayer2D
@export var rotate_player: AudioStreamPlayer2D

#var rocketBody : RigidBody2D
# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	pass
	#rocketBody = get_parent().get_node("RocketBody")

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _physics_process(delta: float) -> void:
	#set_rotation(clamp(rad_to_deg(get_rotation()),-180,180))
	if Input.is_action_pressed("TurretCW"):
		rotate(rad_to_deg(1)*0.1*delta)
		rotate_player.play();
	if Input.is_action_pressed("TurretCCW"):
		rotate(-rad_to_deg(1)*0.1*delta)
		rotate_player.play();
	if Input.is_action_just_pressed("ui_accept"):
		var shot = bubbleShot.instantiate()
		$Marker2D.add_child(shot)
		shoot_player.play()
		
		#get_parent().set_freeze_enabled(false)
		get_parent().apply_impulse(Vector2(0,-10))
		#get_parent().set_freeze_enabled(true)

		shot.reparent(get_parent().get_parent())
		shot.apply_impulse(Vector2(400,0),$Marker2D.get_position())
