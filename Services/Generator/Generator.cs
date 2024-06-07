using System;
using System.Collections.Generic;
using Godot;

enum RoomType {
    Default,
    StairUp,
    StairDown,
    Boss,
    Shop
}

public partial class Generator {
    private static Stack<Room> rooms = new();

    public static void generate(int rooms) {
        int floor = 0;

        for (int i = 0; i < rooms - 1; i++) {
            float random = new RandomNumberGenerator().Randf();

            if (random <= 0.025) {
                addRoom(RoomType.StairUp, floor);
                floor++;
                continue;
            }

            if (random <= 0.05) {
                addRoom(RoomType.StairDown, floor);
                floor--;
                continue;
            }
            
            if (random <= 0.055) {
                addRoom(RoomType.Shop, floor);
            }

            addRoom(RoomType.Default, floor);
        }

        addRoom(RoomType.Boss, floor);
    }

    public static void addRoom(RoomType type, int floor) {
        
    }
}
