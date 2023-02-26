using System.Reflection.Metadata.Ecma335;
using GASP.Helper;

namespace GASP.Genetics
{
  public static class DNAExtension
  {
    /// <summary>
    /// Complements a random chain of operators. 
    /// </summary>
    /// <param name="dna"></param>
    /// <param name="random"></param>
    public static void ComplementMutation(this DNA dna, Random random)
    {
      var root = dna.Genome;
      while (root != null && !root.IsLeaf())
      {
        root.ComplementOperator();
        root = random.NextSingle() < 0.5 ? root.Left : root.Right;
      }
    }

    public static void SubtreeLeafSwitchMutation(this DNA dna, Random random)
    {
      var root = dna.Genome;
      var size = root.Size();
      SlicingTree.ToPolishNotation(root, out var polish);

      SelectSwitchingPositions(root,random, out var subtree, out var value);

      var parentOfSubtree = root.FindParentOfSubtree(subtree);
      var parentOfLeaf = root.FindParentOfSubtree(value);

      if (parentOfLeaf == null)
      {
        Console.WriteLine("Could not find parent of leaf" + value.Value);
        return;
      }
      if (parentOfSubtree == null)
      {
        Console.WriteLine("Could not find parent of leaf" + value.Value);
        return;
      }

      // switching operation
      if (parentOfSubtree.Left == subtree)
      {
        parentOfSubtree.Left = value;
      }
      else
      {
        parentOfSubtree.Right = value;
      }

      if (parentOfLeaf.Left == value)
      {
        parentOfLeaf.Left = subtree;
      }
      else
      {
        parentOfLeaf.Right = subtree;
      }

      if (!root.IsWellformed() || root.Size() != size)
      {
        SlicingTree.ToPolishNotation(root, out var polish2);
        Console.Write(polish);
        Console.WriteLine(polish2);
      }
      
    }

    private static void SelectSwitchingPositions(SlicingTree root, Random random, out SlicingTree subtree, out SlicingTree value)
    {
      subtree = SlicingTree.SelectRandomSubtree(root, random);
      value = SlicingTree.SelectRandomValue(root, random);
      while (value == null)
      {
        SlicingTree.SelectRandomValue(root, random);
      }

      for (var i = 0; i < 10; i++)
      {
        for (var j = 0; j < 10; j++)
        {
          if (!subtree.Contains(value.Value))
          {
            return;
          }
          subtree = SlicingTree.SelectRandomSubtree(root, random);
        }
        value = SlicingTree.SelectRandomValue(root,random) ?? throw new InvalidOperationException();
      }

    }
  }
}
