using System;

namespace Lab4_ants
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter number of nodes:");
            int number_of_nodes = Convert.ToInt32(Console.ReadLine());
            World world = new World(number_of_nodes);
            world.PrintGraph();
            DateTime start = DateTime.Now;
            Algorithm algorithm = new Algorithm(world, number_of_nodes);
            algorithm.Execute();
            DateTime end = DateTime.Now;
            Console.WriteLine("Execution time: " + (end - start).TotalSeconds + " s");
        }
    }
}
