using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace MonoVoxel.Engine.Ressources {
    
    public sealed class MonoVoxelBlockManager {

        private List<MonoVoxelBlock> m_blocks;

        public MonoVoxelBlockManager( ) 
            => m_blocks = new List<MonoVoxelBlock>( );

        public void Create( MonoVoxelBlock block ) {
            if ( !m_blocks.Contains( block ) )
                m_blocks.Add( block );
        }

        public MonoVoxelBlock? Get( int block_id ) {
            var block = (MonoVoxelBlock?)null;

            if ( block_id < m_blocks.Count )
                block = m_blocks[ block_id ];

            return block;
        }

        public Vector4 GetFace( int block_id, int face_id ) {
            var uv = Vector4.Zero;

            if ( block_id < m_blocks.Count && face_id < m_blocks[ block_id ].Faces.Length )
                uv = m_blocks[ block_id ].Faces[ face_id ];

            return uv;
        }

        public static Vector4 UV( int column, int row ) {
            const float unit = 1.0f / 16.0f;

            return new( column * unit, row * unit, ( column + 1 ) * unit, ( row + 1 ) * unit );
        }

    }

}
