﻿using System.Collections.ObjectModel;
using System.ComponentModel;
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

      tree.ShouldBeEmpty();
    }

    [Fact]
    public void enthält_hinzugefügtes_Element()
    {
      var tree = new TreeNode<object>();
      var element = new TreeNode<object>();

      tree.Add(element);

      tree.ShouldContain(element);
    }

    [Fact]
    public void enthält_hinzugefügtes_verschachteltes_Element()
    {
      var root = new TreeNode<object>();
      var parent = new TreeNode<object>();
      var child = new TreeNode<object>();

      parent.Add(child);
      root.Add(parent);

      root.ShouldContain(parent);
      parent.ShouldContain(child);
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

      root.ShouldBeEmpty();
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

    [Fact]
    public void kann_als_BindingList_abgebildet_werden()
    {
      var vater = new Person("Donald Trump");
      var sohn = new Person("Eric Trump");
      var vaterNode = new TreeNode<Person>(vater);
      var sohnNode = new TreeNode<Person>(sohn);
      vaterNode.Add(sohnNode);
      var root = new TreeNode<Person>();
      root.Add(vaterNode);

      var list = new BindingList<BindingListRecord>();
      new Visitor<Person>(list, null).Visit(root);

      list.ShouldContain(new BindingListRecord(root.GetHashCode(), null, null));
      list.ShouldContain(new BindingListRecord(vaterNode.GetHashCode(),
        root.GetHashCode(),
        new Person("Donald Trump")));
      list.ShouldContain(new BindingListRecord(sohnNode.GetHashCode(),
        vaterNode.GetHashCode(),
        new Person("Eric Trump")));

      // bool wasChanged = false;
      // list.ListChanged += (sender, args) =>
      // {
      //   wasChanged = true;
      // };

      // root.PropertyChanged += () => {
      //   list.Clear();
      //   list.RaiseListChangedEvents = false;
      //   new Visitor<Person>(list, null).Visit(root)
      //   list.RaiseListChangedEvents = true;
      // }
      // root.Add(new TreeNode<Person>(new Person("Alex")));
      //
      // wasChanged.ShouldBeTrue();
    }

    [Fact]
    public void erhält_Reihenfolge_des_Einfügens()
    {
     var tree = new TreeNode<int>();
      tree.Add(new TreeNode<int>(1));
      tree.Add(new TreeNode<int>(2));
      tree.Add(new TreeNode<int>(3));
      tree.RemoveAt(1);
      tree.Add(new TreeNode<int>(4));

      tree.Select(x => x.Daten).ShouldBe([1, 3, 4]);
    }
  }

  public class Visitor<T>
  {
    readonly BindingList<BindingListRecord> _list;
    readonly TreeNode<T>? _parent;

    public Visitor(BindingList<BindingListRecord> list, TreeNode<T>? parent)
    {
      _list = list;
      _parent = parent;
    }

    public void Visit(TreeNode<T> treeNode)
    {
      _list.Add(new BindingListRecord(
        treeNode.GetHashCode(),
        _parent?.GetHashCode() ?? null,
        treeNode.Daten));

      foreach (var child in treeNode)
      {
        new Visitor<T>(_list, treeNode).Visit(child);
      }
    }
  }

  public record BindingListRecord(int Id, int? ParentId, object Daten);

  public class TreeNode<T> : KeyedCollection<TreeNode<T>,TreeNode<T>>
  {
    public TreeNode()
    {
    }

    public TreeNode(T daten)
    {
      Daten = daten;
    }

    public T? Daten { get; set; }

    protected override TreeNode<T> GetKeyForItem(TreeNode<T> item) => item;
  }

  // record Person(string Name);

  class Person : IEquatable<Person>
  {
    public bool Equals(Person? other)
    {
      if (other is null)
      {
        return false;
      }

      if (ReferenceEquals(this, other))
      {
        return true;
      }

      return Name == other.Name;
    }

    public override bool Equals(object? obj)
    {
      if (obj is null)
      {
        return false;
      }

      if (ReferenceEquals(this, obj))
      {
        return true;
      }

      if (obj.GetType() != GetType())
      {
        return false;
      }

      return Equals((Person) obj);
    }

    public override int GetHashCode() => Name.GetHashCode();

    public static bool operator ==(Person? left, Person? right) => Equals(left, right);

    public static bool operator !=(Person? left, Person? right) => !Equals(left, right);

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
