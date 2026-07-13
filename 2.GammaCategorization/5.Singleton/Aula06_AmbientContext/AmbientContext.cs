using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.VisualBasic;
using System.Threading;         
using System.Threading.Tasks;
using System.Text;   

namespace Aula06_AmbientContext
{
    public class BuildingContext : IDisposable
    {
        public int WallHeight;
        private static Stack<BuildingContext> stack = new Stack<BuildingContext>();

        static BuildingContext()
        {
            stack.Push(new BuildingContext(0));
        }

        public BuildingContext(int wallHeight)
        {
            WallHeight = wallHeight;
            stack.Push(this);
        }

        public static BuildingContext Current => stack.Peek();

        public void Dispose()
        {
            if (stack.Count > 1)
            {
                stack.Pop();
            }
        }
    }

    public struct Point
    {
        private int x, y;

        public Point(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        // [FIX] Moved printing logic from here to Building where 'Walls' actually exists
        public override string ToString()
        {
            return $"({x}, {y})";
        }
    } // [FIX] Added missing closing brace for Point struct

    public class Wall
    {
        public Point Start, End;
        public int Height;

        public Wall(Point start, Point end)
        {
            Start = start;
            End = end;
            Height = BuildingContext.Current.WallHeight; // Context handles the height automatically
        }

        public override string ToString()
        {
            return $"Wall from {Start} to {End} with Height {Height}";
        }
    }

    public class Building
    {
        public List<Wall> Walls = new List<Wall>();

        // [FIX] Fixed loop logic inside the class that actually contains the collection
        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var wall in Walls)
            {
                sb.AppendLine(wall.ToString()); // [FIX] Capitalized 'ToString'
            }
            return sb.ToString();
        }
    }

    public class Demo
    {
        public static void Main(string[] args)
        {
            var house = new Building();

            // [FIX] Removed 'height' parameter since Ambient Context handles it implicitly
            using (new BuildingContext(3000))
            {
                house.Walls.Add(new Wall(new Point(0, 0), new Point(5000, 0)));
                house.Walls.Add(new Wall(new Point(0, 0), new Point(0, 4000)));
            }

            using (new BuildingContext(3500))
            {
                house.Walls.Add(new Wall(new Point(0, 0), new Point(6000, 0)));
                house.Walls.Add(new Wall(new Point(0, 0), new Point(0, 4000)));
            }

            using (new BuildingContext(3000))
            {
                house.Walls.Add(new Wall(new Point(5000, 0), new Point(5000, 4000)));
            }

            Console.WriteLine(house);
        }
    }
}
