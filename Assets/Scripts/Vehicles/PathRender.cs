using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class PathRender : MonoBehaviour {

    public Color lineColor;

    private List<Transform> nodes = new List<Transform>();

    // Find the nodes and add to the list
    // This function executes inside the editor (similar to Update function in game)
    void OnDrawGizmos() {
        // Sets the color for the gizmos that will be drawn next.
        Gizmos.color = lineColor;

        // We can get all the transforms of all the nodes (children) using GetComponentsInChildren<Transform>()
        Transform[] pathTransform = GetComponentsInChildren<Transform>();
        nodes = new List<Transform>();

        // However, this function also takes its own transform so we need to filter this one
        for (int i = 0; i < pathTransform.Length; i++) {
            if (pathTransform[i] != transform) { // not equal to our own transform
                nodes.Add(pathTransform[i]);
            }
        }

        // Let's draw a line between nodes
        for (int i = 0; i < nodes.Count; i++) {
            Vector3 currentNode = nodes[i].position;
            Vector3 previousNode = Vector3.zero;

            if (i > 0) {
                previousNode = nodes[i - 1].position;
            }
            else if (i == 0 && nodes.Count > 1) {
                previousNode = nodes[nodes.Count - 1].position;
            }

            Gizmos.DrawLine(currentNode, previousNode);
            Gizmos.DrawWireSphere(currentNode, 0.3f);
        }
    }
}
