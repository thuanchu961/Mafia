using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Pathfinding {
	
	/// <summary>
	/// Sets the destination of an AI to the position of a specified object.
	/// This component should be attached to a GameObject together with a movement script such as AIPath, RichAI or AILerp.
	/// This component will then make the AI move towards the <see cref="target"/> set on this component.
	///
	/// See: <see cref="Pathfinding.IAstarAI.destination"/>
	///
	/// [Open online documentation to see images]
	/// </summary>
	[UniqueComponent(tag = "ai.destination")]
	[HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_a_i_destination_setter.php")]
	public class AIDestinationSetter : VersionedMonoBehaviour {
		/// <summary>The object that the AI should move to</summary>
		public Transform target;
		//public Transform target2;
		public List<Transform> target1 = new List<Transform>();
		Vector2 test;
		Rigidbody2D rigi;
		IAstarAI ai;
		
		void OnEnable () {
			ai = GetComponent<IAstarAI>();
			if (ai != null) ai.onSearchPath += Update;
		}

		void OnDisable () {
			if (ai != null) ai.onSearchPath -= Update;
		}

		
		void Update () {

			move();

			
		}
		public void move()
        {

			if (target != null && ai != null) ai.destination = target.position;
	
			
		}
      

    }
}
