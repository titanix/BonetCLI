using System;
using System.Collections.Generic;
using System.Linq;

using Leger;

namespace BonetIDE
{
    class ComplexCharacterSearch
    {
        public List<string> Search(IGraph graph, bool includeSelf, params string[] input)
        {
            GraphObjectTypeInfo composeRelationship = new(Guid.Parse("98b96e12-95d7-42ec-9b30-a8433904789d"), "Compose", "", GraphObjectKind.Edge, true);

            List<IVertexContent> inputVertices = new();
            foreach (string str in input)
            {
                inputVertices.Add(graph.SearchVertices("", str)?.First());
            }

            List<IVertexContent>[] predecessorsLists = new List<IVertexContent>[inputVertices.Count];

            for (int i = 0; i < predecessorsLists.Length; i++)
            {
                predecessorsLists[i] = 
                    GetPredecessors(graph, inputVertices[i], composeRelationship)
                    .Where(v => IsKanji(v.ToString())).ToList();
                if (includeSelf)
                {
                    predecessorsLists[i].Add(inputVertices[i]);
                }
            }

            List<List<string>> allSets = new List<List<string>>();
            foreach (List<IVertexContent> vl in predecessorsLists)
            {
                List<string> resultSet = new List<string>();
                foreach (IVertexContent v in vl)
                {
                    HashSet<string> temp = new HashSet<string>(
                        GetSuccessors(graph, v, composeRelationship)
                        .Select(a => a.ToString())
                        );
                    resultSet = resultSet.Union(temp).ToList();
                }
                allSets.Add(resultSet);
            }

            List<string> candidates = allSets[0];
            for (int i = 1; i < allSets.Count; i++)
            {
                candidates = candidates.Intersect(allSets[i]).ToList();
            }

            return candidates;
        }

        private static List<IVertexContent> GetPredecessors(IGraph graph, IVertexContent vertex, GraphObjectTypeInfo edgeType)
        {
            List<IVertexContent> result = new();


            foreach (IEdgeContent edge in graph.GetAdjacentEdges(vertex))
            {
                if (edge.TypeIdentity.Equals(edgeType))
                {
                    var linkedObjects = graph.GetAdjacentNodes(edge);
                    if (linkedObjects.Count != 2)
                    {
                        throw new ArgumentException("Edge relationship (parameter 2) is not binary.");
                    }
                    IVertexContent v1 = linkedObjects[0];
                    IVertexContent v2 = linkedObjects[1];
                    if (v2.Equals(vertex))
                    {
                        result.Add(v1);
                    }
                }
            }

            return result;
        }

        private static List<IVertexContent> GetSuccessors(IGraph graph, IVertexContent vertex, GraphObjectTypeInfo edgeType)
        {
            List<IVertexContent> result = new();

            foreach (IEdgeContent edge in graph.GetAdjacentEdges(vertex))
            {
                if (edge.TypeIdentity.Equals(edgeType))
                {
                    var linkedObjects = graph.GetAdjacentNodes(edge);

                    if (linkedObjects.Count != 2)
                    {
                        throw new ArgumentException("Edge relationship (parameter 2) is not binary.");
                    }
                    IVertexContent v1 = linkedObjects[0];
                    IVertexContent v2 = linkedObjects[1];
                    if (v1.Equals(vertex))
                    {
                        result.Add(v2);
                    }
                }
            }

            return result;
        }

        public static bool IsKanji(char c)
        {
            return c > 0x3400 && c < 0x9FC3;
        }

        public static bool IsKanji(string str)
        {
            foreach (char c in str.ToCharArray())
            {
                if (!IsKanji(c))
                    return false;
            }
            return true;
        }

        public static bool IsRomaji(string str)
        {
            foreach (char c in str.ToCharArray())
            {
                if (!(c < 0x024F))
                    return false;
            }
            return true;
        }
    }
}
