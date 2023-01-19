using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab4_ants
{
    internal class World
    {
        public int[,] DistanceMatrix;
        public double[,] PheromoneMatrix;

        public World(int numberOfNodes)
        {
            DistanceMatrix = new int[numberOfNodes, numberOfNodes];
            PheromoneMatrix = new double[numberOfNodes, numberOfNodes];
            FillMatrixWithRandom(numberOfNodes);
        }

        private void FillMatrixWithRandom(int numberOfNodes)
        {
            for (int i = 0; i < numberOfNodes; i++)
            {
                for (int j = 0; j < numberOfNodes; j++)
                {
                    DistanceMatrix[i, j] = 0;
                    PheromoneMatrix[i, j] = 0;
                }
            }

            for (int i = 0; i < numberOfNodes; i++)
            {
                for (int j = i + 1; j < numberOfNodes; j++)
                {
                    Random rnd = new Random();
                    DistanceMatrix[i, j] = rnd.Next(Constants.MinDistance, Constants.MaxDistance);
                    DistanceMatrix[j, i] = DistanceMatrix[i , j];
                    PheromoneMatrix[i, j] = Constants.StartPheremone;
                    PheromoneMatrix[j, i] = Constants.StartPheremone;
                }
            }
        }
        
        public int GreedySearch(int currentNode)
        {
            List<int> visitedNodes = new List<int>();
            int fullDist = 0;
            for (int i = 0; i < DistanceMatrix.GetLength(0) - 1; i++)
            {
                int nextNode = FindMin(DistanceMatrix, currentNode, visitedNodes.ToArray());
                fullDist += DistanceMatrix[currentNode , nextNode];
                visitedNodes.Add(currentNode);
                currentNode = nextNode;
            }

            fullDist += DistanceMatrix[currentNode , visitedNodes[0]];
            visitedNodes.Add(currentNode);
            return fullDist;
        }

        public int FindMin(int[,] distances, int column, int[] visitedNodes)
        {
            int minValue = Constants.MaxDistance + 1;
            int minIndex = 0;
            for (int i = 0; i < distances.GetLength(1); i++)
            {
                if (minValue > distances[column, i] && distances[column, i] > 0 && !visitedNodes.Contains(i))
                {
                    minValue = distances[column, i];
                    minIndex = i;
                }
            }
            return minIndex;
        }

        public void PrintGraph()
        {
            Console.WriteLine("Distance matrix:");
            for (int i = 0; i < DistanceMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < DistanceMatrix.GetLength(1); j++)
                {
                    Console.Write(DistanceMatrix[i, j] + " ");
                }
                Console.WriteLine();
            }
        }

        public void PrintPheromoneMatrix()
        {
            Console.WriteLine("The pheromone matrix is");
            for (int i = 0; i < PheromoneMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < PheromoneMatrix.GetLength(1); j++)
                {
                    Console.Write(PheromoneMatrix[i, j] + " ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }
}
