namespace GASP.Helper
{
  public class Modules
  {
    public List<Tuple<int,int>> ModuleList { get;  }
    public int MinimalArea {  get;  }

    public Modules(List<Tuple<int, int>> moduleList)
    {
      ModuleList = moduleList;
      MinimalArea = CalculateArea();
    }

    private int CalculateArea()
    {
      return ModuleList.Sum(module => module.Item1 * module.Item2);
    }

    public static Modules RandomModules(int num, int maxWidth, int maxHeight,Random random)
    {
      var list = new List<Tuple<int,int>>();
      for (var i = 0; i < num; i++)
      {
        list.Add(new Tuple<int, int>(random.Next(1,maxWidth),random.Next(1,maxHeight)));
      }

      return new Modules(list);
    }

    public static int CalculateFloorArea(SlicingTree tree, Modules modules)
    {
      return GetHeight(tree, modules) * GetWidth(tree, modules);
    }

    private static int GetHeight(SlicingTree? tree, Modules modules)
    {
      if (tree == null)
      {
        return 0;
      }
      if (tree.IsLeaf())
      {
        return modules.ModuleList[int.Parse(tree.Value)].Item2;
      }
      switch (tree.Value)
      {
        case "V":
          return Math.Max(GetHeight(tree.Left, modules), GetHeight(tree.Right, modules));
        case "H":
          return GetHeight(tree.Left, modules) + GetHeight(tree.Right, modules);
      }

      return 0;
    }

    private static int GetWidth(SlicingTree? tree, Modules modules)
    {
      if (tree == null)
      {
        return 0;
      }
      if (tree.IsLeaf())
      {
        return modules.ModuleList[int.Parse(tree.Value)].Item1;
      }
      switch (tree.Value)
      {
        case "V":
          return GetWidth(tree.Left, modules) + GetWidth(tree.Right, modules);
        case "H":
          return Math.Max(GetWidth(tree.Left, modules), GetWidth(tree.Right, modules));
      }

      return 0;
    }
  }
}
