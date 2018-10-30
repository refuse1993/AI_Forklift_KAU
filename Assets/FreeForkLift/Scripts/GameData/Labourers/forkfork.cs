using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class forkfork : forklift
{
    /**
	 * Our only goal will ever be to make tools.
	 * The ForgeTooldAction will be able to fulfill this goal.
	 */
    public override HashSet<KeyValuePair<string, object>> createGoalState()
    {
        HashSet<KeyValuePair<string, object>> goal = new HashSet<KeyValuePair<string, object>>();

        goal.Add(new KeyValuePair<string, object>("complete", true));
        return goal;
    }
}

