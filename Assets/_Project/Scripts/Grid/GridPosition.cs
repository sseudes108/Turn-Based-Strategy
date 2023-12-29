using System;
public struct GridPosition: IEquatable<GridPosition>{
    public int _x, _z;

    public GridPosition(int x, int z){
        this._x = x;
        this._z = z;
    }

    public override readonly int GetHashCode(){
        return HashCode.Combine(_x, _z);
    }

    public override readonly bool Equals(object obj){
        return obj is GridPosition position &&
            _x == position._x &&
            _z == position._z;
    }
    
    public readonly bool Equals(GridPosition other){
        return this == other;
    }

    public static bool operator == (GridPosition a, GridPosition b){
        return a._x == b._x && a._z == b._z;
    }

    public static bool operator != (GridPosition a, GridPosition b){
        return !(a == b);
    }

    public static GridPosition operator + (GridPosition a, GridPosition b){
        return new GridPosition{
            _x = a._x + b._x,
            _z = a._z + b._z
        };
    }

    public static GridPosition operator - (GridPosition a, GridPosition b){
        return new GridPosition{
            _x = a._x - b._x,
            _z = a._z - b._z
        };
    }

    public override readonly string ToString(){
        return $"x:{_x}, z:{_z}";
    }
}