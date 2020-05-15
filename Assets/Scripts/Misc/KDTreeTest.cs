using UnityEngine;

public class KDTreeTest : MonoBehaviour {

    public void Start() {

        Debug.Log("hi");

        Unit[] units = Object.FindObjectsOfType<Unit>();
        //Debug.Log(units[8].transform.position);

        KDTree testTree = new KDTree();
        testTree.insert(units[9]);
        Debug.Log(testTree.getRoot().getData().transform.position);
        testTree.insert(units[8]);
        Debug.Log(testTree.getRoot().getLeft().getData().transform.position);
        testTree.insert(units[7]);
        Debug.Log(testTree.getRoot().getRight().getData().transform.position);
       // Debug.Log(units[6].transform.position);
        testTree.insert(units[6]);
        Debug.Log(testTree.getRoot().getRight().getLeft().getData().transform.position);

    }

}