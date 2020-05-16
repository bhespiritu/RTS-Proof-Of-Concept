// Author: Kevin Mulliss (kam8ef@virginia.edu)

using UnityEngine;

// tester file for KDTest
public class KDTreeTest : MonoBehaviour {

    public void Start() {

        // array of all the Units in the fuckUnity scene
        Unit[] units = Object.FindObjectsOfType<Unit>();

        KDTree testTree = new KDTree();
        testTree.insert(units[9]);
        Debug.Log(testTree.getRoot().getData().transform.position);
        testTree.insert(units[8]);
        Debug.Log(testTree.getRoot().getLeft().getData().transform.position);
        testTree.insert(units[7]);
        Debug.Log(testTree.getRoot().getRight().getData().transform.position);
        testTree.insert(units[6]);
        Debug.Log(testTree.getRoot().getRight().getLeft().getData().transform.position);

    }

}