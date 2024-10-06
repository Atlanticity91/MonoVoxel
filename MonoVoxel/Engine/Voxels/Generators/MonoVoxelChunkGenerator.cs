namespace MonoVoxel.Engine.Voxels.Generators {
    
    public abstract class MonoVoxelChunkGenerator {

        /// <summary>
        /// Generate voxels.
        /// </summary>
        /// <param name="chunks" >Reference to grid chunk array</param>
        /// <param name="voxels" >Reference to voxels array</param>
        public abstract void Generate( ref MonoVoxelChunk[] chunks, ref byte[] voxels );

    }

}
