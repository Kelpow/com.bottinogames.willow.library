using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Willow
{
    public static class Utilities
    {
        public static IEnumerator TransitionCoroutine(float length, Action<float> action)
        {
            float time = length;
            while (time > 0)
            {
                float t = (length - time) / length;
                action.Invoke(t);
                yield return null;
                time -= Time.deltaTime;
            }
            action.Invoke(1f);
        }

        public static void MakeLineMesh(Mesh basemesh, Mesh outmesh)
        {
            HashSet<(int a, int b)> edges = new HashSet<(int a, int b)>();
            HashSet<(int a, int b)> doubled = new HashSet<(int a, int b)>();
            var triangles = basemesh.triangles;
            for (int i = 0; i < triangles.Length; i += 3)
            {
                int a = triangles[i];
                int b = triangles[i + 1];
                int c = triangles[i + 2];

                if (edges.Contains((b, a)) || edges.Contains((a, b)))
                    doubled.Add((a, b));
                else
                    edges.Add((a, b));

                if (edges.Contains((c, b)) || edges.Contains((b, c)))
                    doubled.Add((b, c));
                else
                    edges.Add((b, c));

                if (edges.Contains((a, c)) || edges.Contains((c, a)))
                    doubled.Add((c, a));
                else
                    edges.Add((c, a));
            }

            List<int> newEdges = new List<int>();
            foreach (var edge in edges)
            {
                if (doubled.Contains((edge.a, edge.b)) || doubled.Contains((edge.b, edge.a)))
                    continue;

                newEdges.Add(edge.a);
                newEdges.Add(edge.b);
            }
            outmesh.SetTriangles(new int[0], 0);
            outmesh.SetVertices(basemesh.vertices);
            outmesh.SetNormals(basemesh.normals);
            outmesh.SetColors(basemesh.colors);
            outmesh.SetUVs(0, basemesh.uv);
            outmesh.SetIndices(newEdges, MeshTopology.Lines, 0);
        }

        public static Mesh MakeNewLineMesh(Mesh basemesh)
        {
            if (basemesh == null)
                return null;

            Mesh newMesh = new Mesh();
            newMesh.name = basemesh.name + "_EDGE";

            MakeLineMesh(basemesh, newMesh);
            return newMesh;
        }
    }
}