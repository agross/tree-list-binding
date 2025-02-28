using System;

using Xunit;

namespace Tree
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
}
