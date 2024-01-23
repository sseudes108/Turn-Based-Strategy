using System;
public struct GridPosition: IEquatable<GridPosition>{
    public int x, z;

    public GridPosition(int x, int z){
        this.x = x;
        this.z = z;
    }

    public override readonly int GetHashCode(){
        return HashCode.Combine(x, z);
    }

    public override readonly bool Equals(object obj){
        return obj is GridPosition position &&
            x == position.x &&
            z == position.z;
    }
    
    public readonly bool Equals(GridPosition other){
        return this == other;
    }

    public static bool operator == (GridPosition a, GridPosition b){
        return a.x == b.x && a.z == b.z;
    }

    public static bool operator != (GridPosition a, GridPosition b){
        return !(a == b);
    }

    public static GridPosition operator + (GridPosition a, GridPosition b){
        return new GridPosition{
            x = a.x + b.x,
            z = a.z + b.z
        };
    }

    public static GridPosition operator - (GridPosition a, GridPosition b){
        return new GridPosition{
            x = a.x - b.x,
            z = a.z - b.z
        };
    }

    public override readonly string ToString(){
        return $"x:{x}, z:{z}";
    }
}