using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab4_ants
{
    internal class Algorithm
    {
        private World model;
        private int numOfNodes;
        private double minL;
        private double bestL;
        private int bestAnt;

        public Algorithm(World world, int num)
        {
            model = world;
            numOfNodes = num;
            minL = Constants.MaxDistance * numOfNodes;
            FindMinL();
            bestL = Constants.MaxDistance * this.numOfNodes;
            bestAnt = 0;
        }
        public void Execute()
        {
            List<Ant> ants = SetAnts();
            for (int t = 1; t < Constants.MaxT + 1; t++)
            {
                foreach (Ant ant in ants)
                {
                    ant.way = new List<int>();
                    ant.way.Add(ant._position);
                    while (ant.way.Count < numOfNodes)
                    {
                        int new_node = FindNewNode(ant);
                        ant.way.Add(new_node);
                    }
                }
                (int best_length, int best_ant) = FindMinWay(ants);
                if (best_length < bestL)
                {
                    bestL = best_length;
                    bestAnt = best_ant;
                }
                UpdatePheromone(ants);
                if (t % Constants.ITerT == 0)
                {
                    Console.WriteLine($"On the {t} iteration, the best lenght is: {bestL}");
                    string line = "";
                    foreach (int node in ants[bestAnt].way)
                    {
                        line += node + " ";
                    }
                    Console.WriteLine("And way is [{0}]", line);
                    //self.__model.PrintPheromoneMatrix()
                }
            }
        }
        
        public void FindMinL()
        {
            for (int i = 0; i < numOfNodes; i++)
            {
                int new_value = model.GreedySearch(i);
                if (new_value < minL)
                {
                    minL = new_value;
                }
            }
            Console.WriteLine("L-min = "+ minL);
        }
        private int FindNewNode(Ant ant)
        {
            if (ant._is_wild)
            {
                List<int> possible_node = new List<int>();
                for (int i = 0; i < numOfNodes; i++)
                {
                    if (!ant.way.Contains(i))
                    {
                        possible_node.Add(i);
                    }
                }
                Random random = new Random();
                int new_node = possible_node[random.Next(possible_node.Count)];
                return new_node;
            }
            else
            {
                int cur = ant.way[ant.way.Count - 1];
                List<double> probabilities = new List<double>();
                for (int i = 0; i < numOfNodes; i++)
                {
                    if (ant.way.Contains(i))
                    {
                        probabilities.Add(0);
                    }
                    else
                    {
                        probabilities.Add(Math.Pow(model.PheromoneMatrix[cur, i], Constants.Alpha) *
                                          Math.Pow(1.0 / model.DistanceMatrix[cur, i], Constants.Beta));
                    }
                }

                double prob_sum = probabilities.Sum();
                for (int i = 0; i < probabilities.Count; i++)
                {
                    probabilities[i] /= prob_sum;
                }
                Random random = new Random();
                double rand_num = random.NextDouble();
                double sum_num = 0;
                for (int i = 0; i < probabilities.Count; i++)
                {
                    sum_num += probabilities[i];
                    if (rand_num < sum_num)
                    {
                        return i;
                    }
                }
                return probabilities.Count - 1;
            }
        }
        private (int, int) FindMinWay(List<Ant> ants)
        {
            int min_length = Constants.MaxDistance * numOfNodes;
            int ant_index = 0;
            foreach (Ant ant in ants)
            {
                int way_len = ant.get_way_length(model);
                if (way_len < min_length)
                {
                    min_length = way_len;
                    ant_index = ants.IndexOf(ant);
                }
            }
            return (min_length, ant_index);
        }
        
        private List<Ant> SetAnts()
        {
            List<int> nodes_in_use = new List<int>();
            List<Ant> ants = new List<Ant>();
            int num_of_wild = 0;
            Random rnd = new Random();
            while (ants.Count < Constants.NumOfAnts)
            {
                int new_position = rnd.Next(0, numOfNodes);
                if (!nodes_in_use.Contains(new_position))
                {
                    if (num_of_wild < Constants.NumOfWildAnts)
                    {
                        ants.Add(new Ant(true, new_position));
                        num_of_wild++;
                    }
                    else
                    {
                        ants.Add(new Ant(false, new_position));
                    }
                    nodes_in_use.Add(new_position);
                }
            }
            return ants;
        }
        private void UpdatePheromone(List<Ant> ants)
        {
            for (int i = 0; i < numOfNodes; i++)
            {
                for (int j = 0; j < numOfNodes; j++)
                {
                    model.PheromoneMatrix[i, j] *= (1.0 - Constants.R);
                }
            }
            foreach (Ant ant in ants)
            {
                double pheromone_of_ant = minL / ant.get_way_length(model);
                for (int i = 0; i < ant.way.Count - 1; i++)
                {
                    model.PheromoneMatrix[ant.way[i], ant.way[i + 1]] += pheromone_of_ant;
                    model.PheromoneMatrix[ant.way[i + 1], ant.way[i]] += pheromone_of_ant;
                }
                model.PheromoneMatrix[ant.way[ant.way.Count - 1], ant.way[0]] += pheromone_of_ant;
                model.PheromoneMatrix[ant.way[0], ant.way[ant.way.Count - 1]] += pheromone_of_ant;
            }
        }
    }
}
