using System.Text.Json;
using System.Text.Json.Serialization;

using Shouldly;

using Xunit;

namespace Tests
{
  public class TreeTests
  {
    [Fact]
    public void ist_anfangs_leer()
    {
      var tree = new TreeNode<object>();

      tree.Nodes.ShouldBeEmpty();
    }

    [Fact]
    public void enthält_hinzugefügtes_Element()
    {
      var tree = new TreeNode<object>();
      var element = new TreeNode<object>();

      tree.Add(element);

      tree.Nodes.ShouldContain(element);
    }

    [Fact]
    public void enthält_hinzugefügtes_verschachteltes_Element()
    {
      var root = new TreeNode<object>();
      var parent = new TreeNode<object>();
      var child = new TreeNode<object>();

      parent.Add(child);
      root.Add(parent);

      root.Nodes.ShouldContain(parent);
      parent.Nodes.ShouldContain(child);
    }

    [Fact]
    public void löscht_Elemente_mit_Kindern()
    {
      var root = new TreeNode<object>();
      var parent = new TreeNode<object>();
      var child = new TreeNode<object>();
      parent.Add(child);
      root.Add(parent);

      root.Remove(parent);

      root.Nodes.ShouldBeEmpty();
    }

    [Fact]
    public void kann_als_JSON_serialisiert_werden()
    {
      var vater = new Person("Donald Trump");
      var sohn = new Person("Eric Trump");

      var vaterNode = new TreeNode<Person>(vater);
      vaterNode.Add(new TreeNode<Person>(sohn));

      var tree = new TreeNode<Person>();
      tree.Add(vaterNode);

      var json = JsonSerializer.Serialize(tree);

      var deserialized = JsonSerializer
        .Deserialize<TreeNode<Person>>(json);

      tree.ShouldBeEquivalentTo(deserialized);
    }
  }

  public class TreeNode<T>
  {
    readonly List<TreeNode<T>> _nodes = new();
    public T Daten { get; init;  }

    public TreeNode()
    {
    }

    public TreeNode(T daten)
    {
      Daten = daten;
    }

    public IEnumerable<TreeNode<T>> Nodes
    {
      get => _nodes;
      init => _nodes = new List<TreeNode<T>>(value);
    }

    public void Add(TreeNode<T> element)
    {
      _nodes.Add(element);
    }

    public void Remove(TreeNode<T> element)
    {
      _nodes.Remove(element);
    }
  }

  class Person
  {
    [JsonConstructor]
    Person()
    {
    }

    public Person(string name)
    {
      Name = name;
    }

    public string Name { get; init; }
  }
}
