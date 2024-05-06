extends Control

signal succesfulConnection
signal ganeStart

const PORT = 11423

# Called when the node enters the scene tree for the first time.
func _hostGame():
	var server = ENetMultiplayerPeer.new()
	server.create_server(PORT, 4)
	