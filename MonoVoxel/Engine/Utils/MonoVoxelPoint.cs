using Microsoft.Xna.Framework;

namespace MonoVoxel.Engine.Utils {
    
    public struct MonoVoxelPoint {

        public int X;
        public int Y;
        public int Z;

        /// <summary>
        /// Constructor
        /// </summary>
        public MonoVoxelPoint( )
            : this( 0, 0, 0 ) 
        { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="x" >Point X value</param>
        /// <param name="y" >Point Y value</param>
        /// <param name="z" >Point Z value</param>
        public MonoVoxelPoint( int x, int y, int z ) {
            X = x;
            Y = y;
            Z = z;
        }

        /// <summary>
        /// Convert point to string.
        /// </summary>
        /// <returns>String version of the point</returns>
        public override string ToString( )
            => $"[ x : {X}, y : {Y}, z : {Z}]";

        /// <summary>
        /// Cast 3d integer point to 3d vector.
        /// </summary>
        /// <returns>Float vector that represent the point</returns>
        public Vector3 ToVector3( )
            => new( X, Y, Z );

        /// <summary>
        /// Addition operator.
        /// </summary>
        /// <param name="a" >Point A</param>
        /// <param name="b" >Point B</param>
        /// <returns>Sum of point A and B</returns>
        public static MonoVoxelPoint operator +( MonoVoxelPoint a, MonoVoxelPoint b )
            => new( a.X + b.X, a.Y + b.Y, a.Z + b.Z );

        /// <summary>
        /// Subtraction operator.
        /// </summary>
        /// <param name="a" >Point A</param>
        /// <param name="b" >Point B</param>
        /// <returns>Difference of point A and B</returns>
        public static MonoVoxelPoint operator -( MonoVoxelPoint a, MonoVoxelPoint b )
            => new( a.X - b.X, a.Y - b.Y, a.Z - b.Z );

        /// <summary>
        /// Scale operator.
        /// </summary>
        /// <param name="point" >Point to scale</param>
        /// <param name="scale" >Scaling value</param>
        /// <returns>Point scaled.</returns>
        public static MonoVoxelPoint operator *( MonoVoxelPoint point, int scale )
            => new( point.X * scale, point.Y * scale, point.Z * scale );

    }

}
