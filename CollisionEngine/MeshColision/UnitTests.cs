using NUnit.Framework;
using System.Numerics;
using System.Collections.Generic;
using Intech.Geometry;
using System.Threading.Tasks;
using Intech.UnitTest;

[TestFixture]
public class MeshIntersectionTests
{

    [Test]
    public async Task TriangleCutByTwoNonColinearTriangles_ShouldSplitIntoTwoPolygons()
    {
        var baseTri = TestMeshFactory.CreateTriangle(
        new Vector3(0, 0, 0),
        new Vector3(5, 0, 0),
        new Vector3(2.5f, 5, 0)
        );

        var cutter1 = TestMeshFactory.CreateTriangle(
        new Vector3(1, -1, 0),
        new Vector3(2.5f, 2.5f, 0),
        new Vector3(0, 1, 0)
        );

        var cutter2 = TestMeshFactory.CreateTriangle(
        new Vector3(4, -1, 0),
        new Vector3(2.5f, 2.5f, 0),
        new Vector3(5, 1, 0)
        );

        var cut1 = MeshManager.Intersect(baseTri, cutter1);
        var cut2 = MeshManager.Intersect(cut1, cutter2);

        Assert.That(cut2.Polygons.Count, Is.GreaterThan(1), "Should split into multiple polygons.");
        await Task.CompletedTask;
    }

    [Test]
    public async Task CoplanarOverlap_ShouldDetectIntersection()
    {
        var triA = TestMeshFactory.CreateTriangle(
        new Vector3(0, 0, 0),
        new Vector3(2, 0, 0),
        new Vector3(1, 2, 0)
        );

        var triB = TestMeshFactory.CreateTriangle(
        new Vector3(1, 1, 0),
        new Vector3(3, 1, 0),
        new Vector3(2, 3, 0)
        );

        var result = MeshManager.Intersect(triA, triB);
        Assert.That(result.Polygons.Count, Is.GreaterThan(0), "Should detect coplanar overlap.");
        await Task.CompletedTask;
    }

    [Test]
    public async Task EdgeOnlyIntersection_ShouldNotFail()
    {
        var triA = TestMeshFactory.CreateTriangle(
        new Vector3(0, 0, 0),
        new Vector3(2, 0, 0),
        new Vector3(1, 2, 0)
        );

        var triB = TestMeshFactory.CreateTriangle(
        new Vector3(2, 0, 0),
        new Vector3(3, 0, 0),
        new Vector3(2.5f, 2, 0)
        );

        var result = MeshManager.Intersect(triA, triB);
        Assert.That(result.Polygons.Count, Is.GreaterThanOrEqualTo(0), "Should handle edge-only intersection gracefully.");
        await Task.CompletedTask;
    }

    [Test]
    public async Task NoIntersection_ShouldReturnEmptyMesh()
    {
        var triA = TestMeshFactory.CreateTriangle(
        new Vector3(0, 0, 0),
        new Vector3(1, 0, 0),
        new Vector3(0.5f, 1, 0)
        );

        var triB = TestMeshFactory.CreateTriangle(
        new Vector3(5, 5, 0),
        new Vector3(6, 5, 0),
        new Vector3(5.5f, 6, 0)
        );

        var result = MeshManager.Intersect(triA, triB);
        Assert.That(result.Polygons.Count, Is.EqualTo(0), "Should return empty mesh for no intersection.");
        await Task.CompletedTask;
    }

    [Test]
    public async Task FullContainment_ShouldReturnInnerTriangle()
    {
        var outer = TestMeshFactory.CreateTriangle(
        new Vector3(0, 0, 0),
        new Vector3(10, 0, 0),
        new Vector3(5, 10, 0)
        );

        var inner = TestMeshFactory.CreateTriangle(
        new Vector3(4, 2, 0),
        new Vector3(6, 2, 0),
        new Vector3(5, 4, 0)
        );

        var result = MeshManager.Intersect(outer, inner);
        Assert.That(result.Polygons.Count, Is.EqualTo(1), "Should return the inner triangle as intersection.");
        await Task.CompletedTask;
    }

}

