using Microsoft.Xna.Framework;
using MonoVoxel.Engine.Ressources;
using MonoVoxel.Engine.Utils;

namespace MonoVoxel.Engine.Voxels {

    public sealed class MonoVoxelChunk {

        private int m_vertice_count;
        private int m_voxel_offset;
        private MonoVoxelPoint m_voxel_grid;
        private Matrix m_location;
        private MonoVoxelChunkVertice[] m_vertices;

        public MonoVoxelPoint VoxelGrid         => m_voxel_grid;
        public Matrix Location                  => m_location;
        public int VerticeCount                 => m_vertice_count;
        public MonoVoxelChunkVertice[] Geometry => m_vertices;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="x" >Chunk X value</param>
        /// <param name="y" >Chunk Y value</param>
        /// <param name="z" >Chunk Z value</param>
        /// <param name="voxel_offset" >Voxel offset from the start of the voxel array</param>
        public MonoVoxelChunk( int x, int y, int z, int voxel_offset ) {
            m_vertice_count = 0;
            m_voxel_offset  = voxel_offset;
            m_voxel_grid    = new MonoVoxelPoint( x, y, z );
            m_location      = Matrix.CreateTranslation( x * MonoVoxelEngine.ChunkSize, y * MonoVoxelEngine.ChunkSize, z * MonoVoxelEngine.ChunkSize );
            m_vertices      = new MonoVoxelChunkVertice[ MonoVoxelEngine.ChunkVolume * 12 ];
        }

        /// <summary>
        /// Get if a value is clamped inside [ 0 : ChunkSize ]
        /// </summary>
        /// <param name="value" >Query value</param>
        /// <returns>True when value is in the range</returns>
        private bool GetIsInChunk( int value )
            => value > -1 && value < MonoVoxelEngine.ChunkSize;

        /// <summary>
        /// Get if a value is clamped inside [ 0 : GridSize ]
        /// </summary>
        /// <param name="value" >Query value</param>
        /// <returns>True when value is in the range</returns>
        private bool GetIsInGrid( int value )
            => value > -1 && value < MonoVoxelEngine.GridSize;

        /// <summary>
        /// Get if a value is 0 or ChunkSize - 1.
        /// </summary>
        /// <param name="value" >Query value</param>
        /// <returns>True when value match</returns>
        private bool GetIsInBorder( int value )
            => value == 0 || value == MonoVoxelEngine.ChunkSize - 1;

        /// <summary>
        /// Get if a voxel position is inside the chunk.
        /// </summary>
        /// <param name="voxel_position" >Query voxel position</param>
        /// <returns>True when the position is inside the chunk</returns>
        private bool GetIsChunkVoxel( MonoVoxelPoint voxel_position )
            => GetIsInChunk( voxel_position.X ) && GetIsInChunk( voxel_position.Y ) && GetIsInChunk( voxel_position.Z );

        /// <summary>
        /// Get if a voxel position is inside the chunk grid.
        /// </summary>
        /// <param name="grid_position" >Query voxel grid position</param>
        /// <returns>True when the voxel is inside the chunk grid</returns>
        private bool GetIsGridVoxel( MonoVoxelPoint grid_position )
            => GetIsInGrid( grid_position.X ) && GetIsInGrid( grid_position.Y ) && GetIsInGrid( grid_position.Z );

        /// <summary>
        /// Calculate the voxel index based on is position.
        /// </summary>
        /// <param name="location" >Voxel position</param>
        /// <returns>Coresponding voxel index</returns>
        private int GetChunkVoxelID( MonoVoxelPoint location )
            => location.Z * MonoVoxelEngine.ChunkArea + location.Y * MonoVoxelEngine.ChunkSize + location.X;

        /// <summary>
        /// Get if a voxel is a chunk border voxel.
        /// </summary>
        /// <param name="voxel_position" >Query voxel position</param>
        /// <returns>True when it's a border voxel</returns>
        private bool GetIsBordelVoxel( MonoVoxelPoint voxel_position )
            => GetIsInBorder( voxel_position.X ) || GetIsInBorder( voxel_position.Y ) || GetIsInBorder( voxel_position.Z );

        /// <summary>
        /// Clamp voxel position component.
        /// </summary>
        /// <param name="value" >Voxel position component</param>
        /// <param name="direction" >Voxel direcrion</param>
        /// <returns>Clamped value</returns>
        private int GetVoxelClamp( int value, int direction ) {
            if ( direction > 0 )
                value = (value + direction) % MonoVoxelEngine.ChunkSize;
            else if ( direction < 0 ) {
                if ( value > 0 )
                    value -= direction;
                else
                    value = MonoVoxelEngine.ChunkSize - direction;
            }

            return value;
        }

        /// <summary>
        /// Get grid voxel position.
        /// </summary>
        /// <param name="local_position" >Current voxel position</param>
        /// <param name="offest" >Query voxel extension direction</param>
        /// <returns>Grid voxel position</returns>
        private MonoVoxelPoint GetWorldVoxelPosition( MonoVoxelPoint local_position, MonoVoxelPoint offest ) {
            local_position.X = GetVoxelClamp( local_position.X, offest.X );
            local_position.Y = GetVoxelClamp( local_position.Y, offest.Y );
            local_position.Z = GetVoxelClamp( local_position.Z, offest.Z );

            return local_position;
        }

        /// <summary>
        /// Get if a grid voxel is empty.
        /// </summary>
        /// <param name="voxels" >Array of voxels</param>
        /// <param name="local_position" >Current voxel position</param>
        /// <param name="offset" >Query voxel extension direction</param>
        /// <returns>True when it's empty</returns>
        private bool GetIsWorldVoxelEmpty( byte[] voxels, MonoVoxelPoint local_position, MonoVoxelPoint offset ) {
            var voxel_position = GetWorldVoxelPosition( local_position, offset );
            var grid_position  = m_voxel_grid + offset;

            if ( GetIsGridVoxel( grid_position ) && GetIsChunkVoxel( voxel_position ) ) {
                var voxel_offset = MonoVoxelEngine.ChunkVolume * ( 
                    grid_position.Z * MonoVoxelEngine.GridArea +
                    grid_position.Y * MonoVoxelEngine.GridSize + 
                    grid_position.X 
                );
                var voxel_id     = GetChunkVoxelID( voxel_position );

                return voxels[ voxel_offset + voxel_id ] == 0;
            }

            return false;
        }

        /// <summary>
        /// Get if a voxel is empty.
        /// </summary>
        /// <param name="voxels" >Array of voxels</param>
        /// <param name="local_position" >Current voxel position</param>
        /// <param name="offset" >Query voxel extension direction</param>
        /// <returns>True when it's empty</returns>
        private bool GetIsVoxelEmpty( byte[] voxels, MonoVoxelPoint local_position, MonoVoxelPoint offset ) {
            var voxel_position = local_position + offset;
            var is_empty       = true;
            
            if ( GetIsChunkVoxel( voxel_position ) ) {
                var voxel_id = GetChunkVoxelID( voxel_position );

                is_empty = voxels[ m_voxel_offset + voxel_id ] == 0;
            } else
                is_empty = GetIsWorldVoxelEmpty( voxels, local_position, offset );

            return is_empty;
        }

        /// <summary>
        /// Push data to vertices.
        /// </summary>
        /// <param name="x" >Voxel X position</param>
        /// <param name="y" >Voxel Y position</param>
        /// <param name="z" >Voxel Z position</param>
        /// <param name="uv_x" >Voxel UV u component</param>
        /// <param name="uv_y" >Voxel UV v component</param>
        /// <param name="block_id" >Block index</param>
        /// <param name="face_id" >Block face index</param>
        private void PushData( int x, int y, int z, float uv_x, float uv_y, int block_id, int face_id )
            => m_vertices[ m_vertice_count++ ] = new( x, y, z, uv_x, uv_y, block_id, face_id );

        /// <summary>
        /// Generate top face geometry.
        /// </summary>
        /// <param name="voxels" >Array of voxels</param>
        /// <param name="block_id" >Block index</param>
        /// <param name="local_position" >Current voxel position</param>
        /// <param name="block_manager" >Current block manager instance</param>
        private void GenerateFaceTop( byte[] voxels, byte block_id, MonoVoxelPoint local_position, MonoVoxelBlockManager block_manager ) {
            if ( GetIsVoxelEmpty( voxels, local_position, new( 0, 1, 0 ) ) ) {
                var uv = block_manager.GetFace( block_id, 0 );

                PushData( local_position.X    , local_position.Y + 1, local_position.Z    , uv.X, uv.Y, block_id, 0 );
                PushData( local_position.X + 1, local_position.Y + 1, local_position.Z    , uv.X, uv.W, block_id, 0 );
                PushData( local_position.X + 1, local_position.Y + 1, local_position.Z + 1, uv.Z, uv.W, block_id, 0 );
                PushData( local_position.X    , local_position.Y + 1, local_position.Z + 1, uv.Z, uv.Y, block_id, 0 );
            }
        }

        /// <summary>
        /// Generate bottom face geometry.
        /// </summary>
        /// <param name="voxels" >Array of voxels</param>
        /// <param name="block_id" >Block index</param>
        /// <param name="local_position" >Current voxel position</param>
        /// <param name="block_manager" >Current block manager instance</param>
        private void GenerateFaceBottom( byte[] voxels, byte block_id, MonoVoxelPoint local_position, MonoVoxelBlockManager block_manager ) {
            if ( GetIsVoxelEmpty( voxels, local_position, new( 0, -1, 0 ) ) ) {
                var uv = block_manager.GetFace( block_id, 1 );

                PushData( local_position.X    , local_position.Y, local_position.Z + 1, uv.Z, uv.W, block_id, 1 );
                PushData( local_position.X + 1, local_position.Y, local_position.Z + 1, uv.X, uv.W, block_id, 1 );
                PushData( local_position.X + 1, local_position.Y, local_position.Z    , uv.X, uv.Y, block_id, 1 );
                PushData( local_position.X    , local_position.Y, local_position.Z    , uv.Z, uv.Y, block_id, 1 );
            }
        }

        /// <summary>
        /// Generate right face geometry.
        /// </summary>
        /// <param name="voxels" >Array of voxels</param>
        /// <param name="block_id" >Block index</param>
        /// <param name="local_position" >Current voxel position</param>
        /// <param name="block_manager" >Current block manager instance</param>
        private void GenerateFaceRight( byte[] voxels, byte block_id, MonoVoxelPoint local_position, MonoVoxelBlockManager block_manager ) {
            if ( GetIsVoxelEmpty( voxels, local_position, new( 1, 0, 0 ) ) ) {
                var uv = block_manager.GetFace( block_id, 2 );

                PushData( local_position.X + 1, local_position.Y + 1, local_position.Z + 1, uv.X, uv.Y, block_id, 2 );
                PushData( local_position.X + 1, local_position.Y + 1, local_position.Z    , uv.Z, uv.Y, block_id, 2 );
                PushData( local_position.X + 1, local_position.Y    , local_position.Z    , uv.Z, uv.W, block_id, 2 );
                PushData( local_position.X + 1, local_position.Y    , local_position.Z + 1, uv.X, uv.W, block_id, 2 );
            }
        }

        /// <summary>
        /// Generate left face geometry.
        /// </summary>
        /// <param name="voxels" >Array of voxels</param>
        /// <param name="block_id" >Block index</param>
        /// <param name="local_position" >Current voxel position</param>
        /// <param name="block_manager" >Current block manager instance</param>
        private void GenerateFaceLeft( byte[] voxels, byte block_id, MonoVoxelPoint local_position, MonoVoxelBlockManager block_manager ) {
            if ( GetIsVoxelEmpty( voxels, local_position, new( -1, 0, 0 ) ) ) {
                var uv = block_manager.GetFace( block_id, 3 );

                PushData( local_position.X, local_position.Y    , local_position.Z    , uv.X, uv.Y, block_id, 3 );
                PushData( local_position.X, local_position.Y + 1, local_position.Z    , uv.Z, uv.Y, block_id, 3 );
                PushData( local_position.X, local_position.Y + 1, local_position.Z + 1, uv.Z, uv.W, block_id, 3 );
                PushData( local_position.X, local_position.Y    , local_position.Z + 1, uv.X, uv.W, block_id, 3 );
            }
        }

        /// <summary>
        /// Generate back face geometry.
        /// </summary>
        /// <param name="voxels" >Array of voxels</param>
        /// <param name="block_id" >Block index</param>
        /// <param name="local_position" >Current voxel position</param>
        /// <param name="block_manager" >Current block manager instance</param>
        private void GenerateFaceBack( byte[] voxels, byte block_id, MonoVoxelPoint local_position, MonoVoxelBlockManager block_manager ) {
            if ( GetIsVoxelEmpty( voxels, local_position, new( 0, 0, 1 ) ) ) {
                var uv = block_manager.GetFace( block_id, 5 );

                PushData( local_position.X + 1, local_position.Y + 1, local_position.Z + 1, uv.X, uv.Y, block_id, 4 );
                PushData( local_position.X + 1, local_position.Y    , local_position.Z + 1, uv.Z, uv.Y, block_id, 4 );
                PushData( local_position.X    , local_position.Y    , local_position.Z + 1, uv.Z, uv.W, block_id, 4 );
                PushData( local_position.X    , local_position.Y + 1, local_position.Z + 1, uv.X, uv.W, block_id, 4 );
            }
        }

        /// <summary>
        /// Generate front face geometry.
        /// </summary>
        /// <param name="voxels" >Array of voxels</param>
        /// <param name="block_id" >Block index</param>
        /// <param name="local_position" >Current voxel position</param>
        /// <param name="block_manager" >Current block manager instance</param>
        private void GenerateFaceFront( byte[] voxels, byte block_id, MonoVoxelPoint local_position, MonoVoxelBlockManager block_manager ) {
            if ( GetIsVoxelEmpty( voxels, local_position, new( 0, 0, -1 ) ) ) {
                var uv = block_manager.GetFace( block_id, 4 );

                PushData( local_position.X    , local_position.Y    , local_position.Z, uv.Z, uv.W, block_id, 5 );
                PushData( local_position.X + 1, local_position.Y    , local_position.Z, uv.X, uv.W, block_id, 5 );
                PushData( local_position.X + 1, local_position.Y + 1, local_position.Z, uv.X, uv.Y, block_id, 5 );
                PushData( local_position.X    , local_position.Y + 1, local_position.Z, uv.Z, uv.Y, block_id, 5 );
            }
        }

        /// <summary>
        /// Build chunk geometry.
        /// </summary>
        /// <param name="block_manager" >Current block manager instance</param>
        /// <param name="voxels" >Voxels array</param>
        public void BuildGeometry( MonoVoxelBlockManager block_manager, byte[] voxels ) {
            m_vertice_count = 0;

            var local_position = new MonoVoxelPoint( );
            
            for ( var z = 0; z < MonoVoxelEngine.ChunkSize; z++ ) {
                local_position.Z = z;

                for ( var y = 0; y < MonoVoxelEngine.ChunkSize; y++ ) {
                    local_position.Y = y;

                    for ( var x = 0; x < MonoVoxelEngine.ChunkSize; x++ ) {
                        var voxel_id = z * MonoVoxelEngine.ChunkArea + y * MonoVoxelEngine.ChunkSize + x;
                        var block_id = voxels[ m_voxel_offset + voxel_id ];

                        if ( block_id == 0 )
                            continue;

                        local_position.X = x;
                        block_id -= 1;

                        GenerateFaceTop(    voxels, block_id, local_position, block_manager );
                        GenerateFaceBottom( voxels, block_id, local_position, block_manager );
                        GenerateFaceRight(  voxels, block_id, local_position, block_manager );
                        GenerateFaceLeft(   voxels, block_id, local_position, block_manager );
                        GenerateFaceBack(   voxels, block_id, local_position, block_manager );
                        GenerateFaceFront(  voxels, block_id, local_position, block_manager );
                    }
                }
            }
        }

    }

}
