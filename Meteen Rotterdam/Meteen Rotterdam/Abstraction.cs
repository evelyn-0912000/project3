using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Meteen_Rotterdam;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace Meteen_Rotterdam {
	class Abstraction {
		// produce all tuples of two nodes that are closest to each other. heavier nodes have stronger 'attraction power'.
		public static List<Tuple<Map, Map>> produceClosestTuples(List<Map> nodes) {
			List<Tuple<Map, Map>> closestTuples = new List<Tuple<Map, Map>>();
			List<Map> skippableNodes = new List<Map>();

			foreach (Map focusNode in nodes) {
				double smallestDistance = double.PositiveInfinity;
				Map closestNode = focusNode;

				int count = 0;

				foreach (Map otherNode in nodes) {
					if (focusNode.printPosition() != otherNode.printPosition()) {
						Vector2 vectorDifference = Vector2.Subtract(focusNode.printPosition(), otherNode.printPosition());
						double distance = Math.Sqrt(vectorDifference.X * vectorDifference.X + vectorDifference.Y * vectorDifference.Y);
						double weightedDistance = distance * 1 / (Math.Pow(focusNode.weight, .8));

						// uncomment the next line to have unweighed coalescion
						// weightedDistance = distance;

						bool skipped = false;

						foreach (Map skippableNode in skippableNodes) {
							if (skippableNode.printPosition() == focusNode.printPosition() || otherNode.printPosition() == skippableNode.printPosition() || focusNode.printPosition() == skippableNode.printPosition()) {
								skipped = true;
							}
						}

						if (weightedDistance < smallestDistance && weightedDistance != 0 && !skipped) {
							count++;
							// Console.WriteLine(count.ToString());
							closestNode = otherNode;
							smallestDistance = weightedDistance;
						}
					}
				}

				if (!closestTuples.Contains(new Tuple<Map, Map>(closestNode, focusNode)) && focusNode.printPosition() != closestNode.printPosition()) {
					Tuple<Map, Map> closestTuple = new Tuple<Map, Map>(focusNode, closestNode);
					closestTuples.Add(closestTuple);
				}

				// uncomment the next lines to have non-coalesced results
				// skippableNodes.Add(closestNode);
				// skippableNodes.Add(focusNode);
			}

			int singleTupleCount = 0;
			int uniqueTupleCount = 0;

			foreach (Tuple<Map, Map> closestTuple in closestTuples) {
				if (closestTuple.Item1.printPosition() == closestTuple.Item2.printPosition()) {
					singleTupleCount++;
				}
				else {
					uniqueTupleCount++;
				}
			}

			return closestTuples;
		}

		// total average calculated in which heavier nodes weigh more. takes more than two nodes.
		public static Map coalescePolyNode(ContentManager Content, List<Map> nodes) {
			Vector2 avgPos = new Vector2(0, 0);

			int totalWeight = 0;
			foreach (Map node in nodes) {
				for (int i = 0; i < node.weight; i++) {
					avgPos = Vector2.Add(avgPos, node.printPosition());
					totalWeight++;
				}
			}
			avgPos.X = avgPos.X / totalWeight;
			avgPos.Y = avgPos.Y / totalWeight;

			Map coalescedNode = new Map(avgPos, Content.Load<Texture2D>("heat1.png"), "1", totalWeight);

			return coalescedNode;
		}

		// average positions, add weight, join together as one node.
		public static Map createAbstractedNode(ContentManager Content, Map node1, Map node2) {
			Vector2 positionOne = node1.printPosition();
			Vector2 positionTwo = node2.printPosition();
			Vector2 avgPos = new Vector2((positionOne.X + positionTwo.X) / 2, (positionOne.Y + positionTwo.Y) / 2);
			int weight;

			if (positionOne == positionTwo) {
				weight = node1.weight;
			}
			else {
				weight = node1.weight + node2.weight;
			}

			Map abstractedNode = new Map(avgPos, Content.Load<Texture2D>("heat2.png"), "1", weight);
			return abstractedNode;
		}

		public static List<Map> createAbstractedMap(List<Map> nodes, ContentManager Content) {
			List<Tuple<Map, Map>> closestTuples = produceClosestTuples(nodes);
			List<Map> abstractedMap = new List<Map>();
			List<List<Tuple<Map, Map>>> coalescableNodeTupleListsWithDupes = new List<List<Tuple<Map, Map>>>();
			List<List<Tuple<Map, Map>>> dupelessCoalescableLists = coalescableNodeTupleListsWithDupes;
			List<List<Tuple<Map, Map>>> dupeAddables = coalescableNodeTupleListsWithDupes;

			// if tuples have one of their nodes in common, they're added in a common list.
			foreach (Tuple<Map, Map> closestTuple in closestTuples) {
				foreach (Tuple<Map, Map> otherTuple in closestTuples) {
					if (closestTuple != otherTuple && (closestTuple.Item1 == otherTuple.Item1 || closestTuple.Item1 == otherTuple.Item2 || closestTuple.Item2 == otherTuple.Item1 || closestTuple.Item2 == otherTuple.Item2)) {
						bool inLists = false;

						// add to list if in there.
						for (int i = 0; i < dupeAddables.Count; i++) {
							if (inLists) {
								break;
							}

							if (dupeAddables[i].Contains(closestTuple)) {
								if (dupeAddables[i].Contains(otherTuple)) {
									dupeAddables[i].Add(otherTuple);
									inLists = true;
								}
							}
							else if (dupeAddables[i].Contains(otherTuple)) {
								dupeAddables[i].Add(closestTuple);
								inLists = true;
							}
						}

						// make new list and add said list if not.
						if (!inLists) {
							List<Tuple<Map, Map>> newList = new List<Tuple<Map, Map>>();
							newList.Add(closestTuple);
							newList.Add(otherTuple);
							dupeAddables.Add(newList);
						}
					}
				}
			}

			int coalescableCount = 0;

			// remove the coalescable nodes from the overal tuple lists, coalesce the nodes, add to map.
			foreach (List<Tuple<Map, Map>> dupelessList in dupeAddables) {
				List<Map> coalescableNodes = new List<Map>();
				foreach (Tuple<Map, Map> coalescableTuple in dupelessList) {
					if (closestTuples.Contains(coalescableTuple)) {
						closestTuples.Remove(coalescableTuple);
					}

					coalescableNodes.Add(coalescableTuple.Item1);
					coalescableNodes.Add(coalescableTuple.Item2);
				}

				coalescableCount++;
				coalescableNodes = coalescableNodes.Distinct().ToList();
				abstractedMap.Add(coalescePolyNode(Content, coalescableNodes));
			}

			// abstract the rest of the node pairs.
			foreach (Tuple<Map, Map> closestTuple in closestTuples) {
				Map abstractedNode = createAbstractedNode(Content, closestTuple.Item1, closestTuple.Item2);
				abstractedMap.Add(abstractedNode);
			}

			foreach (Map node in abstractedMap) {
			}

			return abstractedMap;
		}
	}
	class AbstractionButton : IButton {
		public int abstractionLevel;
		public Vector2 pos;
		private Texture2D texture;
		private List<Texture2D> textureList = new List<Texture2D>();

		public AbstractionButton(buttonOverlay overlay, GraphicsDeviceManager graphics, ContentManager content) {
			this.abstractionLevel = 0;
			float posx, posy;
			for (int i = 0; i < 3; i++) {
				textureList.Add(content.Load<Texture2D>("buttons/abstraction" + i.ToString() + ".png"));
			}

			texture = textureList[abstractionLevel];

			if (overlay.rightstatus == true) {
				posx = graphics.PreferredBackBufferWidth - (overlay.width - 50);
			}
			else {
				posx = 50;
			}
			posy = (Game1.GetCenter(texture, graphics).Y + 305);
			pos = new Vector2(posx, posy);
		}

		public bool checkMouse(MouseState mouseState) {
			Rectangle area = new Rectangle((int)pos.X, (int)pos.Y, (int)texture.Width, (int)texture.Height);
			if (area.Contains(mouseState.Position)) {
				return true;
			}
			else {
				return false;
			}
		}

		public void click() {
			abstractionLevel++;
			if (abstractionLevel == 3) {
				abstractionLevel = 0;
			}
			texture = textureList[abstractionLevel];
		}

		public void Draw(SpriteBatch spriteBatch) {
			spriteBatch.Draw(texture, pos, Color.White);
		}
		public string printValue() {
			return abstractionLevel.ToString();
		}
		public void Update(MouseState mousestate, MouseState oldmousestate) {
			if (checkMouse(mousestate)) {
				if (mousestate.LeftButton == ButtonState.Pressed && oldmousestate.LeftButton == ButtonState.Released) {
					click();
				}
			}
		}
	}
}