using GASP.Genetics;
using GASP.Helper;

var random = new Random();
var module1 = new Tuple<int, int>(2, 1);
var module2 = new Tuple<int, int>(2, 3);
var module3 = new Tuple<int, int>(4, 4);
var problem = new Modules(new List<Tuple<int, int>>{module1,module1,module1,module2,module2});
var problem2 = new Modules(new List<Tuple<int, int>> { module1, module1, module1, module2, module2,module2,module2,module3,module3, module3 });

//var result = SlicingTree.FromPolishNotation("541H23VVH", out var tree);
//SlicingTree.FromPolishNotation("210HH34VV", out var optimalTree);
//SlicingTree.FromRandomSet(5, random, out var tree);
//Console.WriteLine(Modules.CalculateFloorArea(tree,problem));

for (int i = 0; i < 5; i++)
{
  var generations = (int)(100 * Math.Pow(10, i));
  var genetics = new GeneticAlgorithm(100, problem2);
  SlicingTree.ToPolishNotation(genetics.Fittest.Genome, out var polish);
  Console.WriteLine("Starting GASP with "+generations+" generations\n----------\nMinimal area for problem: " + problem2.MinimalArea + "\nBest genome after random initialization\n" + polish + " | Area:" + genetics.Fittest.Fitness + "\n");

  var watch = System.Diagnostics.Stopwatch.StartNew();
  var generationCount = genetics.StartEvolution(generations, problem2.MinimalArea);
  watch.Stop();

  SlicingTree.ToPolishNotation(genetics.Fittest.Genome, out var polish2);
  Console.WriteLine("Produced following result in " + watch.ElapsedMilliseconds + "ms after " + generationCount + " generations");
  Console.WriteLine(polish2 + " | Area:" + genetics.Fittest.Fitness);
  Console.WriteLine("\n|---------------------------------------------------------------------------------------|");
  Console.WriteLine("|---------------------------------------------------------------------------------------|");
  Console.WriteLine("|---------------------------------------------------------------------------------------|\n");
}

