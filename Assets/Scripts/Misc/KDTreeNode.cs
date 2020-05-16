// Author: Kevin Mulliss (kam8ef@virginia.edu)

// individual node of KDTree
public class KDTreeNode {

    // left child node
    private KDTreeNode left;
    // right child node
    private KDTreeNode right;
    // depth of an indvidiual node. if it is odd, we're sorting by x and if it's even we're sorting by z
    private int depth;
    // the Unit stored in the node
    private Unit data;

    // constructors

    // no parameter
    public KDTreeNode() {

        this.left = null;
        this.right = null;
        this.depth = 0;
        this.data = null;

    }

    // data
    public KDTreeNode(Unit inputData) {
       
        this.left = null;
        this.right = null;
        this.depth = 0;
        this.data = inputData;

    }

    // data and depth
    public KDTreeNode(Unit inputData, int inputDepth) {

        this.left = null;
        this.right = null;
        this.data = inputData;
        this.depth = inputDepth;

    }
    
    // all parameter constructor
    public KDTreeNode(KDTreeNode inputLeft, KDTreeNode inputRight, int inputDepth, Unit inputData) {

        this.left = inputLeft;
        this.right = inputRight;
        this.depth = inputDepth;
        this.data = inputData;

    }

    // getters

    // return the left node
    public KDTreeNode getLeft() {

        return this.left;

    }

    // return the right node
    public KDTreeNode getRight() {

        return this.right;

    }

    // return the depth
    public int getDepth() {

        return this.depth;

    }

    // return the Unit data
    public Unit getData() {

        return this.data;

    }

    // set the left node
    public void setLeft(KDTreeNode inputLeft) {

        this.left = inputLeft; 

    }

    // set the right node
    public void setRight(KDTreeNode inputRight) {

        this.right = inputRight;

    }

    // set the input depth
    public void setDepth(int inputDepth) {

        // the depth cannot be less than 0
        if (inputDepth >= 0) {

            // if it's not, set it
            this.depth = inputDepth;

        }

    }

    // insert a new node under this node
    public void insert(Unit inputUnit) {

        // x/z location of the thing we are inserting
        float unitLocation;
        // x/z location of the thing at the current node
        float dataLocation;

        // if the depth is even (in effect, if we comparing in the z dimension)
        if ((depth % 2) == 0) {

            // make our two variables reflect their respective z values
            unitLocation = inputUnit.transform.position.z;
            dataLocation = this.data.transform.position.z;

        } else { // else it must be odd

            // make our two variables reflect their respective x values
            unitLocation = inputUnit.transform.position.x;
            dataLocation = this.data.transform.position.x;

        }

        // if the new node should go to the left
        if (unitLocation < dataLocation) {

            // if the left child does not exist
            if (this.left == null) {

                // make it a new node with the inputted Unit and depth+1 because the node is one node down
                this.left = new KDTreeNode(inputUnit, this.depth + 1);

            } else { // else the left child must exist

                // so we insert at that node
                this.left.insert(inputUnit);

            }

        } else { // if ithe values are equal or greater

            // if the right child node does not exist
            if (this.right == null) {

                // make it a new node with the inputted Unit depth+1
                this.right = new KDTreeNode(inputUnit, this.depth + 1);

            } else { // else the node must exist and be occupied

                // so insert at that node
                this.right.insert(inputUnit);

            }

        }

    }

    // return whether or not this node is a leaf
    public bool isLeaf() {

        return (this.left == null && this.right == null);

    }

}