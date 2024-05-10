extends Control

signal succesfulConnection
signal ganeStart

signal upnp_completed(error)

signal getIP(code)
signal getCode(ip)

signal openLobby

signal updatePlayerCount(num)

const api = "https://webserver-zgik.onrender.com/"

var onHostCode 
var upnp
# var thread = null

const PORT = 11423

# export("int")
var numPlayers = 1;


var players = {

}
 
var gameScene = preload("res://game.tscn").instantiate()



func _upnp_setup(server_port):
	# UPNP queries take some time.
	upnp = UPNP.new()
	var err = upnp.discover()

	if err != OK:
		push_error(str(err))
		emit_signal("upnp_completed", err)
		return

	if upnp.get_gateway() and upnp.get_gateway().is_valid_gateway():
		upnp.add_port_mapping(server_port, server_port, ProjectSettings.get_setting("application/config/name"), "UDP")
		upnp.add_port_mapping(server_port, server_port, ProjectSettings.get_setting("application/config/name"), "TCP")
		emit_signal("upnp_completed", OK)

func _ready():
	multiplayer.connected_to_server.connect(connected)
	multiplayer.peer_connected.connect(onPeerConnected)



func onPeerConnected(id):
	numPlayers += 1
	emit_signal("updatePlayerCount", numPlayers)
	players[id] = -1

@rpc("any_peer", "call_local", "reliable")
func testRPC():
	print("RPC called")


# Called when the node enters the scene tree for the first time.
func hostGame():
	_upnp_setup(PORT)

func hostLocal():
	var server = ENetMultiplayerPeer.new()
	server.create_server(PORT, 4)
	multiplayer.multiplayer_peer = server
	print("Hosting game @")
	print(IP.get_local_addresses()[0] + ":" + str(PORT))
	print(IP.get_local_addresses())
	emit_signal("openLobby")
	numPlayers = 1
	players[1] = -1
	emit_signal("updatePlayerCount", numPlayers)
	

func joinLocal(ip):
	
	var client = ENetMultiplayerPeer.new()
	var err = client.create_client(ip, PORT)


	if (err == OK ):
		multiplayer.multiplayer_peer = client
		print(client)
		emit_signal("openLobby")
	else:
		print("Error connecting to server")
		print(err)






func connected():
	print("Connected to server")
	rpc("testRPC")
	rpc("connectionSuccess")	

	


func _on_upnp_completed(_error):
	var server = ENetMultiplayerPeer.new()
	server.create_server(PORT, 4)
	print("Hosting game @")
	print(PORT)
	print(upnp.query_external_address())
	getCode.emit(upnp.query_external_address() + ":" + PORT)

	
func joinGame(code):
	print("Joining game @")
	print(code)
	# var client = ENetMultiplayerPeer.new()
	getIP.emit(code)
	
func onCodeReceived(code):
	print("Code received")
	print(code)
	onHostCode = code

func onCharacterSelectCharacterChosen(index):
	print("any signals???")
	rpc("selectCharacter", index)

@rpc("any_peer", "call_local", "reliable")
func selectCharacter(index):
	print("Character selected" + str(index))
	if (index != -1 && index in players.values()):
		print("Character already selected")
		print(players)
		return 
	players[multiplayer.get_remote_sender_id()] = index
	print(players)



func StartGamePressed():

	if (-1 in players.values()):
		print("Not all players have selected a character")
		return
	print(players)
	rpc("GameStart")
	pass # Replace with function body.

@rpc("any_peer", "call_local", "reliable")
func GameStart():
	# emit_signal("gameStart")
	#delete the uis
	get_node("ConnectionMenu").queue_free()
	get_node("CharacterSelect").queue_free()
	# add game scene
	# get_tree().change_scene("res://scenes/Game.tscn")
	get_tree().root.add_child(gameScene)	
	pass # Replace with function body.