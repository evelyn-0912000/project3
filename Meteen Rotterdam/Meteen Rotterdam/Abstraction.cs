using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Meteen_Rotterdam;
using Microsoft.Xna.Framework.Graphics;

namespace Meteen_Rotterdam
{
    class Abstraction
    {
        List<Tuple<Map, Map>> produceClosestTuples(List<Map> nodes)
        {
            List<Tuple<Map, Map>> closestTuples = new List<Tuple<Map, Map>>();

            foreach (Map focusNode in nodes)
            {
                double smallestDistance = 100000000000000000000000000000000000.0;
                Map closestNode = focusNode;

                foreach (Map otherNode in nodes)
                {
                    if (focusNode.printPosition() != otherNode.printPosition())
                    {
                        Vector2 vectorDifference = Vector2.Subtract(focusNode.printPosition(), otherNode.printPosition());
                        double distance = Math.Sqrt(vectorDifference.X * vectorDifference.X + vectorDifference.Y * vectorDifference.Y);

                        if (distance < smallestDistance)
                        {
                            closestNode = otherNode;
                            smallestDistance = distance;
                        }
                    }
                }

                Tuple<Map, Map> closestTuple = new Tuple<Map, Map>(focusNode, closestNode);
                closestTuples.Add(closestTuple);

                if (closestNode.printPosition() != focusNode.printPosition())
                {
                    nodes.Remove(closestNode);
                }
            }

            return closestTuples;
        }

        Map createAbstractedNode(Map node1, Map node2)
        {
            Vector2 positionOne = node1.printPosition();
            Vector2 positionTwo = node2.printPosition();
            Vector2 avgPos = new Vector2((positionOne.X + positionTwo.X) / 2, (positionOne.X + positionTwo.Y) / 2);
            int weight;
            int abstraction = node1.abstraction + 1;

            if (positionOne == positionTwo)
            {
                weight = node1.weight;
            } else
            {
                weight = node1.weight + node2.weight;
            }

            Map abstractedNode = new Map(avgPos, Content.Load<Texture2D>("pointer.png"), weight, abstraction);
            return abstractedNode;
        }

        List<Map> createAbstractedMap(List<Map> nodes)
        {
            List<Tuple<Map, Map>> closestTuples = produceClosestTuples(nodes);
            List<Map> abstractedMap = new List<Map>();

            foreach (Tuple<Map, Map> closestTuple in closestTuples)
            {
                Map abstractedNode = createAbstractedNode(closestTuple.Item1, closestTuple.Item2);
                abstractedMap.Add(abstractedNode);
            }

            return abstractedMap;
        }
    }
}
