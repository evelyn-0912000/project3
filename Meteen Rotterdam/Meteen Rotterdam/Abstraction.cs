using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Meteen_Rotterdam;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Meteen_Rotterdam
{
    class Abstraction
    {
        List<Tuple<Map, Map>> produceClosestTuples(List<Map> nodes)
        {
            List<Tuple<Map, Map>> closestTuples = new List<Tuple<Map, Map>>();
            List<Map> skippableNodes = new List<Map>();

            foreach (Map focusNode in nodes)
            {
                double smallestDistance = double.PositiveInfinity;
                Map closestNode = focusNode;

                foreach (Map otherNode in nodes)
                {
                    if (focusNode.printPosition() != otherNode.printPosition() && !skippableNodes.Contains(otherNode))
                    {
                        Vector2 vectorDifference = Vector2.Subtract(focusNode.printPosition(), otherNode.printPosition());
                        double distance = Math.Sqrt(vectorDifference.X * vectorDifference.X + vectorDifference.Y * vectorDifference.Y);
                        double weightedDistance = distance * 1 / (Math.Pow(focusNode.weight, .8));

                        // uncomment the next line to have unweighed coalescion
                        // weightedDistance = distance;

                        if (weightedDistance < smallestDistance)
                        {
                            closestNode = otherNode;
                            smallestDistance = weightedDistance;
                        }
                    }
                }

                Tuple<Map, Map> closestTuple = new Tuple<Map, Map>(focusNode, closestNode);
                closestTuples.Add(closestTuple);

                // uncomment the next line to have non-coalesced results
                skippableNodes.Add(closestNode);
            }

            return closestTuples;
        }

        Map coalescePolyNode(ContentManager Content, List<Map> nodes)
        {
            Vector2 avgPos = new Vector2(0, 0);

            int totalWeight = 0;
            foreach (Map node in nodes)
            {
                for (int i = 0; i < node.weight; i++)
                {
                    avgPos = Vector2.Add(avgPos, node.printPosition());
                    totalWeight++;
                }
            }
            avgPos.X = avgPos.X / totalWeight;
            avgPos.Y = avgPos.Y / totalWeight;

            Map coalescedNode = new Map(avgPos, Content.Load<Texture2D>("pointer.png"), "1",totalWeight);

            return coalescedNode;
        }

        Map createAbstractedNode(ContentManager Content, Map node1, Map node2)
        {
            Vector2 positionOne = node1.printPosition();
            Vector2 positionTwo = node2.printPosition();
            Vector2 avgPos = new Vector2((positionOne.X + positionTwo.X) / 2, (positionOne.Y + positionTwo.Y) / 2);
            int weight;

            if (positionOne == positionTwo)
            {
                weight = node1.weight;
            } else
            {
                weight = node1.weight + node2.weight;
            }

            Map abstractedNode = new Map(avgPos, Content.Load<Texture2D>("pointer.png"), "1",weight);
            return abstractedNode;
        }

        List<Map> createAbstractedMap(List<Map> nodes, ContentManager Content)
        {
            List<Tuple<Map, Map>> closestTuples = produceClosestTuples(nodes);
            List<Map> abstractedMap = new List<Map>();
            List<List<Tuple<Map, Map>>> coalescableNodeTupleListsWithDupes = new List<List<Tuple<Map, Map>>>();
            List<List<Tuple<Map, Map>>> dupelessCoalescableLists = coalescableNodeTupleListsWithDupes;

            foreach (Tuple<Map, Map> closestTuple in closestTuples)
            {
                foreach (Tuple<Map, Map> otherTuple in closestTuples)
                {
                    if (closestTuple.Item1 == otherTuple.Item1 || closestTuple.Item1 == otherTuple.Item2 || closestTuple.Item2 == otherTuple.Item1 || closestTuple.Item2 == otherTuple.Item2)
                    {
                        foreach (List<Tuple<Map, Map>> coalescableNodeTupleList in coalescableNodeTupleListsWithDupes)
                        {
                            if (coalescableNodeTupleList.Contains(closestTuple))
                            {
                                if (!coalescableNodeTupleList.Contains(otherTuple))
                                {
                                    coalescableNodeTupleList.Add(otherTuple);
                                }
                            } else if (coalescableNodeTupleList.Contains(otherTuple))
                            {
                                coalescableNodeTupleList.Add(closestTuple);
                            }
                        }
                    }
                }
            }

            foreach (List<Tuple<Map, Map>> coalescableNodeTupleListWithDupes in coalescableNodeTupleListsWithDupes)
            {
                List<Tuple<Map, Map>> dupelessList = new List<Tuple<Map, Map>>();
                dupelessList = coalescableNodeTupleListWithDupes.Distinct().ToList();
                dupelessCoalescableLists.Add(dupelessList);
            }

            foreach (List<Tuple<Map, Map>> dupelessList in dupelessCoalescableLists)
            {
                List<Map> coalescableNodes = new List<Map>();
                foreach (Tuple<Map, Map> coalescableTuple in dupelessList)
                {
                    if (closestTuples.Contains(coalescableTuple)) {
                        closestTuples.Remove(coalescableTuple);
                    }

                    coalescableNodes.Add(coalescableTuple.Item1);
                    coalescableNodes.Add(coalescableTuple.Item2);
                }

                coalescableNodes = coalescableNodes.Distinct().ToList();
                abstractedMap.Add(coalescePolyNode(Content, coalescableNodes));
            }

            foreach (Tuple<Map, Map> closestTuple in closestTuples)
            {
                Map abstractedNode = createAbstractedNode(Content, closestTuple.Item1, closestTuple.Item2);
                abstractedMap.Add(abstractedNode);
            }

            return abstractedMap;
        }
    }
}