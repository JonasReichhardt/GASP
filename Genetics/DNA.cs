using GASP.Helper;

 namespace GASP.Genetics
{
  public class DNA
  {
    public SlicingTree Genome { get; set; }
    public int Fitness { get; set; }

    private readonly Random _random;

    public DNA(int size,Random random)
    {
      _random = random;
      SlicingTree.FromRandomSet(size, _random, out var tree);
      Genome = tree;
    }
    public DNA(SlicingTree genome, Random random)
    {
      Genome = genome;
      _random = random;
    }

    public DNA Crossover(DNA otherParent)
    {
      var child = new DNA(new SlicingTree("") , _random);

      return child;
    }
    public void Mutate(Random random)
    {
      var mutationOperator = 1;//_random.Next(0, 2);
      var size = Genome.Size();
      switch (mutationOperator)
      {
        case 0:
          this.ComplementMutation(_random);
          break;
        case 1:
          this.SubtreeLeafSwitchMutation(_random);
          break;
      }
    }
  }
}
