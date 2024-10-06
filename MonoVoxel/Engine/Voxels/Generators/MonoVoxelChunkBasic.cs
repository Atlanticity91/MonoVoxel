using MonoVoxel.Engine.Utils;
using System;

namespace MonoVoxel.Engine.Voxels.Generators {

    public sealed class MonoVoxelChunkBasic : MonoVoxelChunkGenerator {

        /// <summary>
        /// Constructor
        /// </summary>
        public MonoVoxelChunkBasic( ) { }

        /// <summary>
        /// Generate chunk voxels.
        /// </summary>
        /// <param name="noise" >Noise generator</param>
        /// <param name="voxels" >Reference to voxels array</param>
        /// <param name="chunk" >Current chunk</param>
        /// <param name="voxel_offset" >Chunk voxel offset fron voxels array start</param>
        private void GenerateChunk( ref OpenSimplexNoise noise, ref byte[] voxels, MonoVoxelChunk chunk, int voxel_offset ) {
            var grid_position = chunk.VoxelGrid * MonoVoxelEngine.ChunkSize;

            for ( var z = 0; z < MonoVoxelEngine.ChunkSize; z++ ) {
                for ( var x = 0; x < MonoVoxelEngine.ChunkSize; x++ ) {
                    var world_height = (int)( noise.Evaluate( ( grid_position.X + x) * 0.01, ( grid_position.Z + z) * 0.01 ) * MonoVoxelEngine.ChunkSize ) + MonoVoxelEngine.ChunkSize;
                    var local_height = Math.Min( world_height - grid_position.Y, MonoVoxelEngine.ChunkSize );

                    for ( var y = 0; y < local_height; y++ ) {
                        var voxel_id = z * MonoVoxelEngine.ChunkArea + y * MonoVoxelEngine.ChunkSize + x;

                        voxels[ voxel_offset + voxel_id ] = 2;
                    }
                }
            }
        }

        /// <summary>
        /// Generate basic world.
        /// </summary>
        /// <param name="chunks" >Reference to grid chunk array</param>
        /// <param name="voxels" >Reference to voxels array</param>
        public override void Generate( ref MonoVoxelChunk[] chunks, ref byte[] voxels ) {
            var noise = new OpenSimplexNoise( );

            for ( var z = 0; z < MonoVoxelEngine.GridSize; z++ ) {
                for ( var y = 0; y < MonoVoxelEngine.GridSize; y++ ) {
                    for ( var x = 0; x < MonoVoxelEngine.GridSize; x++ ) {
                        var voxel_id     = z * MonoVoxelEngine.GridArea + y * MonoVoxelEngine.GridSize + x;
                        var voxel_offset = voxel_id * MonoVoxelEngine.ChunkVolume;

                        chunks[ voxel_id ] = new MonoVoxelChunk( x, y, z, voxel_offset );

                        GenerateChunk( ref noise, ref voxels, chunks[ voxel_id ], voxel_offset );
                    }
                }
            }
        }

    }

}
