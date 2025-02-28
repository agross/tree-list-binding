using Shouldly;

using Xunit;

namespace Tests
{
  public class TreeTests
  {
    [Fact]
    public void ist_anfangs_leer()
    {
      var tree = new TreeNode();

      tree.Nodes.ShouldBeEmpty();
    }

    [Fact]
    public void enthält_hinzugefügtes_Element()
    {
      var tree = new TreeNode();
      var element = new TreeNode();

      tree.Add(element);

      tree.Nodes.ShouldContain(element);
    }

    [Fact]
    public void enthält_hinzugefügtes_verschachteltes_Element()
    {
      var root = new TreeNode();
      var parent = new TreeNode();
      var child = new TreeNode();

      parent.Add(child);
      root.Add(parent);

      root.Nodes.ShouldContain(parent);
      parent.Nodes.ShouldContain(child);
    }
  }

  public class TreeNode
  {
    readonly List<object> _nodes = new();

    public IEnumerable<object> Nodes => _nodes;

    public void Add(object element)
    {
      _nodes.Add(element);
    }
  }
}
