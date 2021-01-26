using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public static class MapGenerator
{
    public static List<Room> GenerateMap()
    {
        Random random = new Random();
        return GenerateMap(random.Next());
    }

    public static List<Room> GenerateMap(int seed)
    {
        Random random = new Random(seed);

        int mainLength = random.Next(7, 10);
        int maxBranchLength = 5;

        var map = new List<int>();

        for (int i = 0; i < mainLength; i++)
        {
            double rand = random.NextDouble();

            if (rand < 0.2)
                map.Add(3);
            else if (rand < 0.6)
                map.Add(2);
            else
                map.Add(1);
        }

        int lastCount = 0;
        var stepCount = new int[6];
        stepCount[0] = mainLength;

        for (int j = lastCount; j < mainLength; j++)
        {
            int toGenerate = map[j] - 1;
            for (int k = 0; k < toGenerate; k++)
            {
                double rand = random.NextDouble();

                if (rand < 0.3 - 0.15)
                    map.Add(2);
                else if (rand < 0.6 - 0.15)
                    map.Add(1);
                else
                    map.Add(0);

                stepCount[1]++;
            }
        }

        lastCount = mainLength;

        for (int i = 1; i < maxBranchLength; i++)
        {
            int currentCount = map.Count;
            for (int j = lastCount; j < currentCount; j++)
            {
                int toGenerate = map[j];
                for (int k = 0; k < toGenerate; k++)
                {
                    double rand = random.NextDouble();

                    if (rand < 0.3 - 0.075 * i)
                        map.Add(2);
                    else if (rand < 0.6 - 0.15 * i)
                        map.Add(1);
                    else
                        map.Add(0);

                    stepCount[i + 1]++;
                }
            }
            lastCount = currentCount;
        }

        var rooms = new List<List<Tuple<char, int>>>();

        var previous = new Tuple<char, int>[map.Count + 1];
        previous[0] = new Tuple<char, int>('R', -1);
        var currentChild = stepCount[0];
        var doors = new List<char> { 'L', 'R', 'T', 'B' };

        for (int i = 0; i < stepCount[0]; i++)
        {
            var shuffled = doors.OrderBy(item => random.Next()).ToList();
            var room = new List<Tuple<char, int>>();

            switch (previous[i].Item1)
            {
                case 'R':
                    room.Add(new Tuple<char, int>('L', previous[i].Item2 + 1));
                    shuffled.Remove('L');
                    break;
                case 'L':
                    room.Add(new Tuple<char, int>('R', previous[i].Item2 + 1));
                    shuffled.Remove('R');
                    break;
                case 'T':
                    room.Add(new Tuple<char, int>('B', previous[i].Item2 + 1));
                    shuffled.Remove('B');
                    break;
                case 'B':
                    room.Add(new Tuple<char, int>('T', previous[i].Item2 + 1));
                    shuffled.Remove('T');
                    break;
                default:
                    break;
            }
            if (i != stepCount[0] - 1)
            {
                room.Add(new Tuple<char, int>(shuffled[0], i + 2));
                previous[i + 1] = new Tuple<char, int>(room[1].Item1, i);
            }
            else
            {
                room.Add(new Tuple<char, int>(shuffled[0], map.Count + 1));
                previous[map.Count] = new Tuple<char, int>(room[1].Item1, i + 1);
            }

            for (int j = 1; j < map[i]; j++)
            {
                room.Add(new Tuple<char, int>(shuffled[j], currentChild + 1));
                previous[currentChild] = new Tuple<char, int>(room[j + 1].Item1, i);
                currentChild++;
            }

            rooms.Add(room);
        }

        for (int i = stepCount[0]; i < map.Count; i++)
        {
            var shuffled = doors.OrderBy(item => random.Next()).ToList();
            var room = new List<Tuple<char, int>>();

            switch (previous[i].Item1)
            {
                case 'R':
                    room.Add(new Tuple<char, int>('L', previous[i].Item2 + 1));
                    shuffled.Remove('L');
                    break;
                case 'L':
                    room.Add(new Tuple<char, int>('R', previous[i].Item2 + 1));
                    shuffled.Remove('R');
                    break;
                case 'T':
                    room.Add(new Tuple<char, int>('B', previous[i].Item2 + 1));
                    shuffled.Remove('B');
                    break;
                case 'B':
                    room.Add(new Tuple<char, int>('T', previous[i].Item2 + 1));
                    shuffled.Remove('T');
                    break;
                default:
                    break;
            }
            for (int j = 0; j < map[i]; j++)
            {
                room.Add(new Tuple<char, int>(shuffled[j], currentChild + 1));
                previous[currentChild] = new Tuple<char, int>(room[j + 1].Item1, i);
                currentChild++;
            }
            rooms.Add(room);
        }

        var listOfRooms = new List<Room>();

        listOfRooms.Add(new Room("start", rooms.Count + 1));
        for (int i = 0; i < rooms.Count; i++)
        {
            rooms[i] = rooms[i].OrderBy(room => room.Item1).ToList();
            //listOfRooms.Add(new Room(rooms[i], random.Next(0, 3)));
            listOfRooms.Add(new Room(rooms[i], 0));
        }
        listOfRooms.Add(new Room("boss", rooms.Count + 1));

        return listOfRooms;
    }
}

public class Room
{
    public int Bottom { get; set; }
    public int Left { get; set; }
    public int Right { get; set; }
    public int Top { get; set; }
    public string Code { get; set; }
    public bool Cleared { get; set; }

    public Room(List<Tuple<char, int>> tuples, int type)
    {
        Bottom = Left = Right = Top = -1;

        foreach (var tuple in tuples)
        {
            switch (tuple.Item1)
            {
                case 'R':
                    Right = tuple.Item2;
                    break;
                case 'L':
                    Left = tuple.Item2;
                    break;
                case 'T':
                    Top = tuple.Item2;
                    break;
                case 'B':
                    Bottom = tuple.Item2;
                    break;
                default:
                    break;
            }

            Code += tuple.Item1;
        }

        Code += " " + type;
        Cleared = false;
    }

    public Room(string special, int listLength)
    {
        Bottom = Left = Right = Top = -1;

        if (special == "start")
        {
            Cleared = true;
            Right = 1;
            Code = "Start";
        }
        else
        {
            Cleared = false;
            Code = "Boss";
        }
    }

    public override string ToString()
    {
        return $"{Bottom} {Left} {Right} {Top} \"{Code}\"";
    }
}
