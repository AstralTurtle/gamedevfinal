extends Control

signal succesfulConnection
signal ganeStart

signal upnp_completed(error)

signal getIP(code)
signal getCode(ip)

signal openLobby


const api = "https://webserver-zgik.onrender.com/"

var onHostCode 
var upnp
var thread = null

const PORT = 11423


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
	thread = Thread.new()
	thread.call_deferred("_upnp_setup",PORT)


func _exit_tree():
	# Wait for thread finish here to handle game exit while the thread is running.
	thread.wait_to_finish()


# Called when the node enters the scene tree for the first time.
func hostGame():
	_upnp_setup(PORT)


func _on_upnp_completed(error):
	var server = ENetMultiplayerPeer.new()
	server.create_server(PORT, 4)
	print("Hosting game @")
	print(PORT)
	print(upnp.query_external_address())
	getCode.emit(upnp.query_external_address())

	
func joinGame(code):
	print("Joining game @")
	print(code)
	# var client = ENetMultiplayerPeer.new()
	getIP.emit(code)
	
func onCodeReceived(code):
	print("Code received")
	print(code)
	onHostCode = code