using Shouldly;

using Xunit;

namespace Tests
{
  public class TreeTests
  {
    [Fact]
    public void ist_anfangs_leer()
    {
      var tree = new Tree();

      tree.Nodes.ShouldBeEmpty();
    }

    [Fact]
    public void enthält_hinzugefügtes_Element()
    {
      var tree = new Tree();
      var element = new object();

      tree.Add(element);

      tree.Nodes.ShouldContain(element);
    }
  }

  public class Tree
  {
    readonly List<object> _nodes = new();

    public IEnumerable<object> Nodes => _nodes;

    public void Add(object element)
    {
      _nodes.Add(element);
    }
  }
}
