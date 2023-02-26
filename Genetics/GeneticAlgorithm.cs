using GASP.Helper;

namespace GASP.Genetics
{
  public class GeneticAlgorithm
  {
    public DNA[] Population { get; set; }
    public int MaxPopulation { get; }
    public Modules Modules { get; }
    public DNA Fittest => GetFittest();

    private readonly int[] _elitism = new int[5];
    private readonly Random _random = new();
    private const int Factor = 3;
    private const int SelectionSize = 2;

    public GeneticAlgorithm(int maxPopulation, Modules modules)
    {
      MaxPopulation = maxPopulation;
      Modules = modules;
      Population = InitPopulation();
    }

    public int StartEvolution(int maxGenerations, int threshold)
    {
      for (var i = 0; i < maxGenerations; i++)
      {
        FlagElite();

        var crossoverCandidates = RandomSelection(SelectionSize);
        Crossover(crossoverCandidates);

        var mutationCandidates = RandomSelection(SelectionSize);
        Mutation(mutationCandidates);

        FitnessEval();

        // exit criteria
        if (Fittest.Fitness <= threshold)
        {
          Console.WriteLine("Reached threshold in generation "+i+"...");
          return i;
        }
        //Console.WriteLine("Generation "+(i+1) +" processed");
      }

      return maxGenerations;
    }

    private void FlagElite()
    {
      var fitnessList = Population.Select(dna => dna.Fitness).ToList();
      for (var i = 0; i < _elitism.Length; i++)
      {
        var minIndex = fitnessList.IndexOf(fitnessList.Min());
        _elitism[i] = minIndex;
        fitnessList.RemoveAt(minIndex);
      }
    }

    private int[] RandomSelection(int selectionSize)
    {
      if (selectionSize > Population.Length)
        throw new ArgumentOutOfRangeException();
      var candidatesIndices = new int[selectionSize];
      for (var i = 0; i < selectionSize; i++)
      {
        var c = GetRandomPopulationMember();
        while (candidatesIndices.Contains(c) || _elitism.Contains(c))
        {
          c = GetRandomPopulationMember();
        }
        candidatesIndices[i]=c;
      }
      return candidatesIndices;
    }

    private void Crossover(int[] parents)
    {

    }

    private void Mutation(int[] genomeIndices)
    {
      foreach (var index in genomeIndices)
      {
        Population[index].Mutate(_random);
        CalculateFitness(Population[index]);
      }
    }

    private void FitnessEval()
    {
      foreach (var dna in Population)
      {
        CalculateFitness(dna);
      }
    }
    private int GetRandomPopulationMember()
    {
      return _random.Next(0, Population.Length);
    }

    private DNA GetFittest()
    {
      var min = Population.Select(dna => dna.Fitness).Min();
      return Population.ToList().Find(dna => dna.Fitness == min)!;
    }

    private DNA[] InitPopulation()
    {
      var population = new DNA[MaxPopulation];
      for (var i = 0; i < MaxPopulation; i++)
      {
        var dna = new DNA(Modules.ModuleList.Count, _random);
        CalculateFitness(dna);
        population[i] = dna;
      }
      return population;
    }

    private int CalculateFitness(DNA dna)
    {
      var area = Modules.CalculateFloorArea(dna.Genome, Modules);
      dna.Fitness = area > Modules.MinimalArea * Factor ? int.MaxValue : area;
      return dna.Fitness;
    }
  }
}
