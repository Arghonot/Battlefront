using System;
using System.Collections.Generic;
using System.Linq;

namespace Genetic
{
    /*class Program
    {
        static void Main(string[] args)
        {
            // Déclare la classe manager
            Genetik genetik = new Genetik();
            genetik.Initialize(10, 10, 0.05f);
            // Déclare les Genes
            genetik.SetGenes(new List<Gene>()
            {
                new Gene("Speed",0.5f, 2.5f),
                new Gene("Agressivity", 0.0f, 1.0f)
            });
            // ici ta fonction de calcule du score (retourne un float)
            genetik.SetFitnessFunction((dna) =>
            {
                return 0;
            });

            // génère la première population random
            genetik.GenerateRandomPopulation();

            for (int i = 0; i < genetik.Generations; i++)
            {
                // tu fais ta game....
                // quand ta fini la game, tu call
                genetik.ProcessPopulation();
            }

            // fin de toutes les games
            genetik.GetElites(2); // les 2 plus forts
        }
    }*/

    public class Gene
    {
        public string Name;
        public float Value;
        public float Max;
        public float Min;

        public Gene(string _name)
        {
            Name = _name;
            Min = 0f;
            Max = 1f;
        }
        public Gene(string _name, float _min, float _max)
        {
            Name = _name;
            Min = _min;
            Max = _max;
        }

        public Gene GetRandom(Random rand)
        {
            Value = ((float)rand.NextDouble() * (Max - Min)) + Min;
            return this;
        }
    }

    public class DNA
    {
        public string Name;
        public Dictionary<string, Gene> Genes = new Dictionary<string, Gene>();
        public float Fitness { get { return ProcessingFunction(this); } }
        private Func<DNA, float> ProcessingFunction;

        public void SetFitnessFunction(Func<DNA, float> _ProcessingFunction)
        {
            ProcessingFunction = _ProcessingFunction;
        }

        public DNA GetRandomDNA(Random rand, List<Gene> _Gennes)
        {
            foreach (var gen in _Gennes)
                Genes.Add(gen.Name, new Gene(gen.Name).GetRandom(rand));
            return this;
        }
    }

    public class Genetik
    {
        public Genetik instance;
        private int PopulationQuantity;
        private float MutationRate;
        public int Generations;
        private int ElitismeRate;
        private List<DNA> Population = new List<DNA>();
        private Random Rand;
        private List<Gene> Genes = new List<Gene>();
        private List<DNA> Elites = new List<DNA>();
        private Func<DNA, float> ProcessingFunction;
        private bool _working = false;
        public bool isWorking { get { return _working; } }
        public void StartWorking()
        {
            _working = true;
        }
        public void StopWorking()
        {
            _working = false;
        }
        void Start()
        {
            instance = this;
        }

        public void SetFitnessFunction(Func<DNA, float> _ProcessingFunction)
        {
            ProcessingFunction = _ProcessingFunction;
        }

        /// <summary>
        /// Create new Genetik Instance
        /// </summary>
        /// <param name="_populationQuantity">The quantity of DNA by Population</param>
        /// <param name="_generations">The number of Generations</param>
        /// <param name="_mutationRate">The rate of DNA Mutation</param>
        public void Initialize(int _populationQuantity, int _generations, float _mutationRate)
        {
            PopulationQuantity = _populationQuantity;
            Generations = _generations;
            MutationRate = _mutationRate;
            Rand = new Random();
        }

        public void SetGenes(List<Gene> genes)
        {
            Genes = genes;
        }

        public DNA GetDNA(string DNAName)
        {
            DNA dna = null;
            foreach (var d in Population)
                if (d.Name == DNAName)
                {
                    dna = d;
                    break;
                }
            return dna;
        }

        public void GenerateRandomPopulation()
        {
            Population.Clear();
            for (int i = 0; i < PopulationQuantity; i++)
                Population.Add(new DNA().GetRandomDNA(Rand, Genes));
        }

        public void ProcessPopulation()
        {
            Population = Population.OrderByDescending(dna => dna.Fitness).ToList();
            Elites.Clear();
            for (int i = 0; i < ElitismeRate; i++)
            {
                Elites.Add(Population[0]);
                Population.RemoveAt(0);
            }

            List<DNA> newPopulation = new List<DNA>();
            // Cross Elites
            for (int i = 0; i < ElitismeRate; i++)
                newPopulation.Add(Crossover(PickRandomElite(), PickRandomElite()));
            for (int i = 0; i < Population.Count; i++)
                newPopulation.Add(Crossover(PickRandomPopulation(), PickRandomPopulation()));
            Population = newPopulation.OrderBy(a => Guid.NewGuid()).ToList();
            MutatePopulation();
        }

        public DNA PickRandomElite()
        {
            return Population[Rand.Next(Population.Count - 1)];
        }

        public DNA PickRandomPopulation()
        {
            return Elites[Rand.Next(Elites.Count - 1)];
        }

        public DNA Crossover(DNA a, DNA b)
        {
            DNA c = new DNA();
            foreach (var gene in Genes)
            {
                //50-50 chance of selection
                float random = Rand.Next(0, 1);
                if (random < 0.5)
                    c.Genes[gene.Name] = a.Genes[gene.Name];
                else
                    c.Genes[gene.Name] = b.Genes[gene.Name];
            }
            return c;
        }

        public List<DNA> GetElites(int nbElites)
        {
            Population = Population.OrderByDescending(dna => dna.Fitness).ToList();
            Elites.Clear();
            for (int i = 0; i < nbElites; i++)
            {
                Elites.Add(Population[0]);
                Population.RemoveAt(0);
            }
            return Elites;
        }

        public void MutatePopulation()
        {
            foreach (var individu in Population)
                foreach (var gen in individu.Genes)
                    if (Rand.NextDouble() < MutationRate)
                        gen.Value.GetRandom(Rand);
        }
    }
}