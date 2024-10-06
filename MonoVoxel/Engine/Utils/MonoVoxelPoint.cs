namespace MonoVoxel.Engine.Utils {
    
    public struct MonoVoxelPoint {

        public int X;
        public int Y;
        public int Z;

        public MonoVoxelPoint( int x, int y, int z ) {
            X = x;
            Y = y;
            Z = z;
        }

        public override string ToString( )
            => $"[ x : {X}, y : {Y}, z : {Z}]";

        public static MonoVoxelPoint operator +( MonoVoxelPoint a, MonoVoxelPoint b )
            => new( a.X + b.X, a.Y + b.Y, a.Z + b.Z );

        public static MonoVoxelPoint operator -( MonoVoxelPoint a, MonoVoxelPoint b )
            => new( a.X - b.X, a.Y - b.Y, a.Z - b.Z );

        public static MonoVoxelPoint operator *( MonoVoxelPoint point, int scale )
            => new( point.X * scale, point.Y * scale, point.Z * scale );

    }

}
