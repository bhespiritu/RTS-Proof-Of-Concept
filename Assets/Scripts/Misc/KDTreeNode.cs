public class KDTreeNode {

    private KDTreeNode left;
    private KDTreeNode right;
    private int depth;
    private Unit data;

    public KDTreeNode() {

        this.left = null;
        this.right = null;
        this.depth = 0;
        this.data = null;

    }

    public KDTreeNode(Unit inputData) {
       
        this.left = null;
        this.right = null;
        this.depth = 0;
        this.data = inputData;

    }

    public KDTreeNode(Unit inputData, int inputDepth) {

        this.left = null;
        this.right = null;
        this.data = inputData;
        this.depth = inputDepth;

    }
    
    public KDTreeNode(KDTreeNode inputLeft, KDTreeNode inputRight, int inputDepth, Unit inputData) {

        this.left = inputLeft;
        this.right = inputRight;
        this.depth = inputDepth;
        this.data = inputData;

    }

    public KDTreeNode getLeft() {

        return this.left;

    }

    public KDTreeNode getRight() {

        return this.right;

    }

    public int getDepth() {

        return this.depth;

    }

    public Unit getData() {

        return this.data;

    }

    public void setLeft(KDTreeNode inputLeft) {

        this.left = inputLeft; 

    }

    public void setRight(KDTreeNode inputRight) {

        this.right = inputRight;

    }

    public void setDepth(int inputDepth) {

        if (inputDepth >= 0) {

            this.depth = inputDepth;

        }

    }

    public void insert(Unit inputUnit) {

        float unitLocation;
        float dataLocation;

        if ((depth % 2) == 0) {

            unitLocation = inputUnit.transform.position.z;
            dataLocation = this.data.transform.position.z;

        } else {

            unitLocation = inputUnit.transform.position.x;
            dataLocation = this.data.transform.position.x;

        }

        if (unitLocation < dataLocation) {

            if (this.left == null) {

                this.left = new KDTreeNode(inputUnit, this.depth + 1);

            } else {

                this.left.insert(inputUnit);

            }

        } else {

            if (this.right == null) {

                this.right = new KDTreeNode(inputUnit, this.depth + 1);

            } else {

                this.right.insert(inputUnit);

            }

        }

    }

}