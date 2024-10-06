namespace MonoVoxel.Engine.Voxels.Generators {
    
    public abstract class MonoVoxelChunkGenerator {

        public abstract void Generate( ref MonoVoxelChunk[] chunks, ref byte[] voxels );

    }

}
