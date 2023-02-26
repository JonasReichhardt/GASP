namespace GASP.Helper
{
  public class SlicingTree
  {
    public string Value {  get;  set; }
    public SlicingTree? Left { get; set; }
    public SlicingTree? Right { get; set; }

    public SlicingTree(string value, SlicingTree left, SlicingTree right)
    {
      Value = value;
      Left = left;
      Right = right;
    }

    public SlicingTree(string value)
    {
      Value = value;
      Left = null;
      Right = null;
    }

    public static bool FromRandomSet(int size,Random rand, out SlicingTree slicingTree)
    {
      if (size > 10)
      {
        slicingTree = new SlicingTree("");
        return false;
      }

      var modules = new List<int>(size);
      while (modules.Count < size)
      {
        var candidate = rand.Next(0, size);
        while (modules.Contains(candidate))
        {
          candidate = rand.Next(0, size);
        }
        modules.Add(candidate);
      }

      slicingTree = new SlicingTree(RandomOperator(rand));

      while (modules.Count > 0)
      {
        var module = modules.FirstOrDefault();
        slicingTree.InsertLeaf(rand, module.ToString());
        modules.Remove(module);
      }

      return true;
    }

    private void InsertLeaf(Random rand, string value)
    {
      if (rand.NextSingle() < 0.5)
      {
        if (Left == null)
        {
          Left = new SlicingTree(value);
          return;
        }
        if(Left.IsLeaf())
        {
          var n = Left.Value;
          Left = new SlicingTree(RandomOperator(rand), new SlicingTree(n), new SlicingTree(value));
          return;
        }

        Left.InsertLeaf(rand, value);
      }
      else
      {
        if (Right == null)
        {
          Right = new SlicingTree(value);
          return;
        }
        if (Right.IsLeaf())
        {
          var n = Right.Value;
          Right = new SlicingTree(RandomOperator(rand), new SlicingTree(n), new SlicingTree(value));
          return;
        }

        Right.InsertLeaf(rand, value);
      }
    }

    public List<SlicingTree> GetSubtrees()
    {
      var items = new List<SlicingTree>();

      if (IsLeaf()) return items;

      if (Left != null && !Left.IsLeaf())
      {
        items.Add(Left);
        items.AddRange(Left.GetSubtrees());
      }

      if (Right != null && !Right.IsLeaf())
      {
        items.Add(Right);
        items.AddRange(Right.GetSubtrees());
      }
      return items;
    }

    public bool Contains(string value)
    {
      if (Value == value)
      {
        return true;
      }

      if (Left == null || Right == null)
      {
        return false;
      }
      var resultLeft = Left.Contains(value);
      var resultRight = Right.Contains(value);
      if (resultLeft || resultRight)
      {
        return true;
      }

      return false;
    }

    public SlicingTree? FindParentOfSubtree(SlicingTree subtree)
    {
      if (Left != null && Left == subtree)
      {
        return this;
      }
      if (Right != null && Right == subtree)
      {
        return this;
      }

      var ret1 = Left?.FindParentOfSubtree(subtree);
      var ret2 = Right?.FindParentOfSubtree(subtree);
      if (ret1 != null && ret2 == null)
      {
        return ret1;
      }
      if (ret1 == null && ret2 != null)
      {
        return ret2;
      }

      return null;
    }

    private static string RandomOperator(Random random)
    {
      return random.NextSingle() < 0.5 ? "V" : "H";
    }

    public static SlicingTree? SelectRandomValue(SlicingTree root, Random random)
    {
      var current = root;
      while (current != null && !current.IsLeaf())
      {
        current = random.NextSingle() < 0.5 ? current.Left : current.Right;
      }

      return current;
    }

    public static SlicingTree SelectRandomSubtree(SlicingTree root, Random random)
    {
      var subtrees = new List<SlicingTree>();
      subtrees.AddRange(root.GetSubtrees());

      var subtreeArr = subtrees.ToArray();
      return subtreeArr[random.Next(0, subtreeArr.Length)];
    }

    public static bool FromPolishNotation(string polishNotation, out SlicingTree slicingTree)
    {
      var stack = new Stack<SlicingTree>();
      foreach (var c in polishNotation)
      {
        if (c is 'V' or 'H')
        {
          var right = stack.Pop();
          var left = stack.Pop();
          stack.Push(new SlicingTree(c.ToString(),left,right));
        }
        else
        {
          stack.Push(new SlicingTree(c.ToString()));
        }
      }

      if (stack.Count == 1)
      {
        slicingTree = stack.Pop();
        return true;
      }

      slicingTree = new SlicingTree("");
      return false;
    }

    public static bool ToPolishNotation(SlicingTree? slicingTree, out string polishNotation)
    {
      polishNotation = string.Empty;
      if (slicingTree == null)
      {
        return true;
      }

      if (!slicingTree.IsWellformed())
      {
        return false;
      }

      if (slicingTree.IsLeaf())
      {
        polishNotation = slicingTree.Value;
        return true;
      }

      if (!ToPolishNotation(slicingTree.Left, out var polish1) || 
          !ToPolishNotation(slicingTree.Right, out var polish2))
      {
        return false;
      }

      polishNotation = polish1 + polish2 + slicingTree.Value;
      return true;
    }

    public bool IsLeaf()
    {
      return Value != "V" && Value != "H";
    }

    public void ComplementOperator()
    {
      if (IsLeaf()) return;
      Value = Value == "V" ? "H" : "V";
    }

    public bool IsWellformed()
    {
      if (IsLeaf())
      {
        return Left == null && Right == null;
      }
      return (Left != null && Right != null) && Left.IsWellformed() && Right.IsWellformed();
    }

    public int Size()
    {
      var size = 0;
      if (Left != null)
      {
        size += Left.Size();
      }
      if (Right != null)
      {
        size += Right.Size();
      }
      if(!string.IsNullOrEmpty(Value))
      {
        size += 1;
      }
      return size;
    }
  }
}
