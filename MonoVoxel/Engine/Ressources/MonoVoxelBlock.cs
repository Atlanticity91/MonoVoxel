using Microsoft.Xna.Framework;

namespace MonoVoxel.Engine.Ressources {

    public struct MonoVoxelBlock {

        public Vector4[] Faces;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="face" >Face of the block</param>
        public MonoVoxelBlock( Vector4 face ) {
            Faces = new Vector4[ 6 ];
            
            for ( var i = 0; i < 6; i++ )
                Faces[ i ] = face;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="face" >Array of all block face</param>
        public MonoVoxelBlock( params Vector4[] face ) {
            Faces = face;
        }

    }

}
