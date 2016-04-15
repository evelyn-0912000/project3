using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Meteen_Rotterdam;

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
                Tuple<Map, Map> closestTuple = new Tuple<Map, Map>(focusNode, focusNode);
                Map closestNode = focusNode;

                foreach (Map otherNode in nodes)
                {
                    if (focusNode.printPosition() != otherNode.printPosition())
                    {
                        Vector2 vectorDifference = Vector2.Subtract(node.printPosition(), beginningNode.printPosition());
                        double distance = Math.Sqrt(vectorDifference.X * vectorDifference.X + vectorDifference.Y * vectorDifference.Y);

                        if (distance < currentSmallestDistance)
                        {
                            Map closestNode = otherNode;
                            smallestDistance = distance;
                            closestTuple.Item2 = otherNode;
                        }
                    }
                }

                closestTuples.Add(closestTuple);

                if (closestNode.printPosition() != focusNode.printPosition())
                {
                    nodes.Remove(closestNode);
                }
            }

            return closestTuples();
        }

        Map createAbstractedNode(Map node1, Map node2)
        {
            Vector2 positionOne = node1.printPosition();
            Vector2 positionTwo = node2.printPositon();
            Vector2 avgPos = new Vector2((positionOne.X + positionTwo.X) / 2, (positionOne.X + positionTwo.Y) / 2);
            int weight;
            int abstraction = node1.getAbstraction() + 1;

            if (positionOne = positionTwo)
            {
                weight = node1.getWeight();
            } else
            {
                weight = node1.getWeight() + node2.getWeight();
            }

            Map abstractedNode = new Map(avgPos, Content.Load<Texture2D>("pointer.png"), weight, abstraction);
            return abstractedNode;
        }

        List<Map> createAbstractedMap(List<map> nodes)
        {
            List<Tuple<Map, Map>> closestTuples = produceClosestTuples(nodes);
            List<Map> abstractedMap = new List<Map>();

            foreach (Tuple<Map, Map> closestTuple in closestTuples)
            {
                Map abstractedNode = createAbstractedNode(closestTuple.Item1, closestTuple.Item2);
                abstractedMap.Add(abstractedNode);
            }
        }
    }
}
