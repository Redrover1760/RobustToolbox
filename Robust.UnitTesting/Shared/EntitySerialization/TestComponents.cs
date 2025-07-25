using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Robust.Shared.GameObjects;
using Robust.Shared.Map;
using Robust.Shared.Maths;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.Manager.Attributes;
using Robust.Shared.Utility;

namespace Robust.UnitTesting.Shared.EntitySerialization;

[RegisterComponent]
public sealed partial class EntitySaveTestComponent : Component
{
    /// <summary>
    /// Give each entity a unique id to identify them across map saves & loads.
    /// </summary>
    [DataField] public string? Id;

    [DataField] public EntityUid? Entity;

    [DataField, AlwaysPushInheritance] public List<int> List = [];

    /// <summary>
    /// Find an entity with a <see cref="EntitySaveTestComponent"/> with the matching id.
    /// </summary>
    public static Entity<TransformComponent, EntitySaveTestComponent> Find(string id, IEntityManager entMan)
    {
        var ents = entMan.AllEntities<EntitySaveTestComponent>();
        var matching = ents.Where(x => x.Comp.Id == id).ToArray();
        Assert.That(matching, Has.Length.EqualTo(1));
        return (matching[0].Owner, entMan.GetComponent<TransformComponent>(matching[0].Owner), matching[0].Comp);
    }

    public static Entity<TransformComponent, EntitySaveTestComponent> Get(EntityUid uid, IEntityManager entMan)
    {
        return new Entity<TransformComponent, EntitySaveTestComponent>(
            uid,
            entMan.GetComponent<TransformComponent>(uid),
            entMan.EnsureComponent<EntitySaveTestComponent>(uid));
    }
}

/// <summary>
/// Dummy tile definition for serializing grids.
/// </summary>
[Prototype("testTileDef")]
public sealed partial class TileDef : ITileDefinition
{
    public ushort TileId { get; set; }
    public string Name => ID;

    [IdDataField]
    public string ID { get; private set; } = default!;
    public ResPath? Sprite => null;
    public Dictionary<Direction, ResPath> EdgeSprites => new();
    public int EdgeSpritePriority => 0;
    public float Friction => 0;
    public byte Variants => 0;
    public void AssignTileId(ushort id) => TileId = id;
}
