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

        public MonoVoxelChunk( int x, int y, int z, int voxel_offset ) {
            m_vertice_count = 0;
            m_voxel_offset  = voxel_offset;
            m_voxel_grid    = new MonoVoxelPoint( x, y, z );
            m_location      = Matrix.CreateTranslation( x * MonoVoxelEngine.ChunkSize, y * MonoVoxelEngine.ChunkSize, z * MonoVoxelEngine.ChunkSize );
            m_vertices      = new MonoVoxelChunkVertice[ MonoVoxelEngine.ChunkVolume * 12 ];
        }

        private bool GetIsInChunk( int value )
            => value > -1 && value < MonoVoxelEngine.ChunkSize;

        private bool GetIsInGrid( int value )
            => value > -1 && value < MonoVoxelEngine.GridSize;

        private bool GetIsInBorder( int value )
            => value == 0 || value == MonoVoxelEngine.ChunkSize - 1;

        private bool GetIsChunkVoxel( MonoVoxelPoint voxel_position )
            => GetIsInChunk( voxel_position.X ) && GetIsInChunk( voxel_position.Y ) && GetIsInChunk( voxel_position.Z );

        private bool GetIsGridVoxel( MonoVoxelPoint grid_position )
            => GetIsInGrid( grid_position.X ) && GetIsInGrid( grid_position.Y ) && GetIsInGrid( grid_position.Z );

        private int GetChunkVoxelID( MonoVoxelPoint location )
            => location.Z * MonoVoxelEngine.ChunkArea + location.Y * MonoVoxelEngine.ChunkSize + location.X;

        private bool GetIsBordelVoxel( MonoVoxelPoint voxel_position )
            => GetIsInBorder( voxel_position.X ) || GetIsInBorder( voxel_position.Y ) || GetIsInBorder( voxel_position.Z );

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

        private MonoVoxelPoint GetWorldVoxelPosition( MonoVoxelPoint local_position, MonoVoxelPoint offest ) {
            local_position.X = GetVoxelClamp( local_position.X, offest.X );
            local_position.Y = GetVoxelClamp( local_position.Y, offest.Y );
            local_position.Z = GetVoxelClamp( local_position.Z, offest.Z );

            return local_position;
        }

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

        private void PushData( int x, int y, int z, float uv_x, float uv_y, int block_id, int face_id )
            => m_vertices[ m_vertice_count++ ] = new( x, y, z, uv_x, uv_y, block_id, face_id );

        private void GenerateFaceTop( byte[] voxels, byte block_id, MonoVoxelPoint local_position, MonoVoxelBlockManager block_manager ) {
            if ( GetIsVoxelEmpty( voxels, local_position, new( 0, 1, 0 ) ) ) {
                var uv = block_manager.GetFace( block_id, 0 );

                PushData( local_position.X    , local_position.Y + 1, local_position.Z    , uv.X, uv.Y, block_id, 0 );
                PushData( local_position.X + 1, local_position.Y + 1, local_position.Z    , uv.X, uv.W, block_id, 0 );
                PushData( local_position.X + 1, local_position.Y + 1, local_position.Z + 1, uv.Z, uv.W, block_id, 0 );
                PushData( local_position.X    , local_position.Y + 1, local_position.Z + 1, uv.Z, uv.Y, block_id, 0 );
            }
        }

        private void GenerateFaceBottom( byte[] voxels, byte block_id, MonoVoxelPoint local_position, MonoVoxelBlockManager block_manager ) {
            if ( GetIsVoxelEmpty( voxels, local_position, new( 0, -1, 0 ) ) ) {
                var uv = block_manager.GetFace( block_id, 1 );

                PushData( local_position.X    , local_position.Y, local_position.Z + 1, uv.Z, uv.W, block_id, 1 );
                PushData( local_position.X + 1, local_position.Y, local_position.Z + 1, uv.X, uv.W, block_id, 1 );
                PushData( local_position.X + 1, local_position.Y, local_position.Z    , uv.X, uv.Y, block_id, 1 );
                PushData( local_position.X    , local_position.Y, local_position.Z    , uv.Z, uv.Y, block_id, 1 );
            }
        }

        private void GenerateFaceRight( byte[] voxels, byte block_id, MonoVoxelPoint local_position, MonoVoxelBlockManager block_manager ) {
            if ( GetIsVoxelEmpty( voxels, local_position, new( 1, 0, 0 ) ) ) {
                var uv = block_manager.GetFace( block_id, 2 );

                PushData( local_position.X + 1, local_position.Y + 1, local_position.Z + 1, uv.X, uv.Y, block_id, 2 );
                PushData( local_position.X + 1, local_position.Y + 1, local_position.Z    , uv.Z, uv.Y, block_id, 2 );
                PushData( local_position.X + 1, local_position.Y    , local_position.Z    , uv.Z, uv.W, block_id, 2 );
                PushData( local_position.X + 1, local_position.Y    , local_position.Z + 1, uv.X, uv.W, block_id, 2 );
            }
        }

        private void GenerateFaceLeft( byte[] voxels, byte block_id, MonoVoxelPoint local_position, MonoVoxelBlockManager block_manager ) {
            if ( GetIsVoxelEmpty( voxels, local_position, new( -1, 0, 0 ) ) ) {
                var uv = block_manager.GetFace( block_id, 3 );

                PushData( local_position.X, local_position.Y    , local_position.Z    , uv.X, uv.Y, block_id, 3 );
                PushData( local_position.X, local_position.Y + 1, local_position.Z    , uv.Z, uv.Y, block_id, 3 );
                PushData( local_position.X, local_position.Y + 1, local_position.Z + 1, uv.Z, uv.W, block_id, 3 );
                PushData( local_position.X, local_position.Y    , local_position.Z + 1, uv.X, uv.W, block_id, 3 );
            }
        }

        private void GenerateFaceBack( byte[] voxels, byte block_id, MonoVoxelPoint local_position, MonoVoxelBlockManager block_manager ) {
            if ( GetIsVoxelEmpty( voxels, local_position, new( 0, 0, 1 ) ) ) {
                var uv = block_manager.GetFace( block_id, 5 );

                PushData( local_position.X + 1, local_position.Y + 1, local_position.Z + 1, uv.X, uv.Y, block_id, 4 );
                PushData( local_position.X + 1, local_position.Y    , local_position.Z + 1, uv.Z, uv.Y, block_id, 4 );
                PushData( local_position.X    , local_position.Y    , local_position.Z + 1, uv.Z, uv.W, block_id, 4 );
                PushData( local_position.X    , local_position.Y + 1, local_position.Z + 1, uv.X, uv.W, block_id, 4 );
            }
        }

        private void GenerateFaceFront( byte[] voxels, byte block_id, MonoVoxelPoint local_position, MonoVoxelBlockManager block_manager ) {
            if ( GetIsVoxelEmpty( voxels, local_position, new( 0, 0, -1 ) ) ) {
                var uv = block_manager.GetFace( block_id, 4 );

                PushData( local_position.X    , local_position.Y    , local_position.Z, uv.Z, uv.W, block_id, 5 );
                PushData( local_position.X + 1, local_position.Y    , local_position.Z, uv.X, uv.W, block_id, 5 );
                PushData( local_position.X + 1, local_position.Y + 1, local_position.Z, uv.X, uv.Y, block_id, 5 );
                PushData( local_position.X    , local_position.Y + 1, local_position.Z, uv.Z, uv.Y, block_id, 5 );
            }
        }

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
