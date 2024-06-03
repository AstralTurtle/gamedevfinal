using System;
using System.Collections;
using System.Collections.Generic;
using Godot;

public partial class Generator {
	public static Stack<Room> rooms = new Stack<Room>();

	public static void generateRooms(int rooms) {
		for (int i = 0; i < rooms; i++) {
			if (calculateBoss(i)) {
				addBoss();
				continue;
			};

			if (calculateStair()) {
				addStairUp();
				continue;
			};

			if (calculateStair()) {
				addStairDown();
				continue;
			};

			addRoom();
		}
	}

	public static void addStairUp() {

	}

	public static void addStairDown() {
		
	}

	public static void addBoss() {
		
	}

	public static void addRoom() {
		
	}

	public static bool calculateBoss(int number) {
		number -= 10;
		float random = new RandomNumberGenerator().Randf() * 2000000;
		float chance = (float) Math.Pow(Math.E, (double) number); 

		if (number < 0) return false;
		return (random > chance);
	}

	public static bool calculateStair() {
		float random = new RandomNumberGenerator().Randf();

		return (random > 0.05);
	}
}
