// Author: Kevin Mulliss (kam8ef@virginia.edu)

// K Dimensional Tree
// this tree works in two dimensions, x and z
public class KDTree {

    // root node of the KDTree
    private KDTreeNode root;
    // size of the tree
    private int size;

    // constructors

    // no parameter
    public KDTree() {

        this.root = null;
        this.size = 0;

    }

    // size
    public KDTree(int inputSize) {

        this.root = null;
        this.size = inputSize;

    }

    // root
    public KDTree(KDTreeNode inputRoot) {

        this.root = inputRoot;
        this.size = 0;

    }

    // size and root
    public KDTree(KDTreeNode inputRoot, int inputSize) {

        this.root = inputRoot;
        this.size = inputSize;

    }

    // getters

    // return the root
    public KDTreeNode getRoot() {

        return this.root;

    }

    // setters

    // set the root
    public void setRoot(KDTreeNode inputRoot) {

        this.root = inputRoot;

    }

    // other methods

    // insert a Unit into the tree
    public void insert(Unit inputUnit) {

        // if the root doesn't exist yet
        if (this.root == null) {

            // make the root have the Unit as data and 1 as its depth since it's the root
            this.root = new KDTreeNode(inputUnit, 1);

        } else { 

            // call insert at the root node
            this.root.insert(inputUnit);

        }

    }

    // return the minimum in the inputted dimension. true is x dimension and z is false
    public float findMinimum(bool x) {

        // if the root is a leaf
        if (this.root.isLeaf()) {

            // if we're searching for the minimum in the x dimension
            if (x) {

                // return the x value since it must be the minimum
                return this.root.getData().transform.position.x;

            } else { // else we're searching for the minimum in the z dimension

                // return the z value since it must be the minimum
                return this.root.getData().transform.position.z;

            }

        }
        //TODO: Actually, replace this with good code
        return 0;

    }

}