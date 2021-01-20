using System.Globalization;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private Mesh mesh;
    [SerializeField] private Material material;
    [SerializeField] private GameObject gameObjectPrefab;

    [SerializeField] int xSize = 10;
    [SerializeField] int ySize = 10;
    [Range(0.1f, 2f)]
    [SerializeField] private float spacing = 2f;

    private Entity _entityPrefab;
    private World _defaultWorld;
    private EntityManager _entityManager;

    void Start()
    {
        _defaultWorld = World.DefaultGameObjectInjectionWorld;
        _entityManager = _defaultWorld.EntityManager;

        //conversion workflow with editor
        GameObjectConversionSettings settings = GameObjectConversionSettings.FromWorld(_defaultWorld, null);
        _entityPrefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(gameObjectPrefab, settings);

        InstantiateEntityGrid(xSize, ySize, spacing);
    }

    private void InstantiateEntityGrid(int dimX, int dimY, float spacing = 1f)
    {
        for (int i = 0; i < dimX; i++)
        {
            for (int j = 0; j < dimY; j++)
            {
                InstantiateEntity(new float3(i * spacing, j * spacing, 0f));
            }
        }
    }

    private void InstantiateEntity(float3 position)
    {
        Entity myEntity = _entityManager.Instantiate(_entityPrefab);
        _entityManager.SetComponentData(myEntity, new Translation
        {
            Value = position
        });
    }


    //Pure ECS
    private void MakeEntity()
    {
        EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        EntityArchetype archetype = entityManager.CreateArchetype(
            typeof(Translation),
            typeof(Rotation),
            typeof(RenderMesh),
            typeof(RenderBounds),
            typeof(LocalToWorld)
        );

        Entity myEntity = entityManager.CreateEntity(archetype);
        entityManager.AddComponentData(myEntity, new Translation
        {
            Value = new float3(0f, 2f, 0f)
        });

        entityManager.AddSharedComponentData(myEntity, new RenderMesh
        {
            mesh = mesh,
            material = material
        });
    }
}
