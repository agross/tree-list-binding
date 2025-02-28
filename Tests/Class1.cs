using System;

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
  }

  public class Tree
  {
    public IEnumerable<object> Nodes { get; } = new List<object>();
  }
}
