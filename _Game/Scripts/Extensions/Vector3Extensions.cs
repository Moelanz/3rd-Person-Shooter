using System;
using UnityEngine;

namespace Extensions
{
	public static class Vector3Extensions
	{
        /// <summary>
        /// Detect ground under object
        /// </summary>
        /// <param name="position">position</param>
        /// <returns>ground position under object</returns>
        public static Vector3 DetectGround (this Vector3 position)
	    {
		    //Detect ground to spawn items on
		    RaycastHit hit;

		    if (Physics.Raycast (position, -Vector3.up, out hit, 1000.0f)) 
		    {
			    return hit.point;
		    }

		    return position;
	    }
	}
}

