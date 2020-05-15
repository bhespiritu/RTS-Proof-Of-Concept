public class KDTree {

    private KDTreeNode root;
    private int size;

    public KDTree() {

        this.root = null;
        this.size = 0;

    }

    public KDTree(int inputSize) {

        this.root = null;
        this.size = inputSize;

    }

    public KDTree(KDTreeNode inputRoot) {

        this.root = inputRoot;
        this.size = 0;

    }

    public KDTree(KDTreeNode inputRoot, int inputSize) {

        this.root = inputRoot;
        this.size = inputSize;

    }

    public KDTreeNode getRoot() {

        return this.root;

    }

    public void setRoot(KDTreeNode inputRoot) {

        this.root = inputRoot;

    }

    public void insert(Unit inputUnit) {

        if (this.root == null) {

            this.root = new KDTreeNode(inputUnit, 1);

        } else {

            this.root.insert(inputUnit);

        }

    }

}