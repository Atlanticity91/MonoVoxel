using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace MonoVoxel.Engine.Ressources {
    
    public sealed class MonoVoxelBlockManager {

        private List<MonoVoxelBlock> m_blocks;

        /// <summary>
        /// Constructor
        /// </summary>
        public MonoVoxelBlockManager( ) 
            => m_blocks = new List<MonoVoxelBlock>( );

        /// <summary>
        /// Add a new block.
        /// </summary>
        /// <param name="block" >New block</param>
        public void Add( MonoVoxelBlock block ) {
            if ( !m_blocks.Contains( block ) )
                m_blocks.Add( block );
        }

        /// <summary>
        /// Get a block by it's index.
        /// </summary>
        /// <param name="block_id" >Query block index</param>
        /// <returns>Query block</returns>
        public MonoVoxelBlock? Get( int block_id ) {
            var block = (MonoVoxelBlock?)null;

            if ( block_id < m_blocks.Count )
                block = m_blocks[ block_id ];

            return block;
        }

        /// <summary>
        /// Get face of a block.
        /// </summary>
        /// <param name="block_id" >Query block index</param>
        /// <param name="face_id" >Query block face index</param>
        /// <returns>UV min and max packed in a 4 components vector</returns>
        public Vector4 GetFace( int block_id, int face_id ) {
            var uv = Vector4.Zero;

            if ( block_id < m_blocks.Count && face_id < m_blocks[ block_id ].Faces.Length )
                uv = m_blocks[ block_id ].Faces[ face_id ];

            return uv;
        }

        /// <summary>
        /// Caculate UV for a specified column and row combo.
        /// </summary>
        /// <param name="column" >Query block face sprite column</param>
        /// <param name="row" >Query block face sprite row</param>
        /// <returns>UV min and max packed in a 4 components vector</returns>
        public static Vector4 UV( int column, int row ) {
            const float unit = 1.0f / 16.0f;

            return new( column * unit, row * unit, ( column + 1 ) * unit, ( row + 1 ) * unit );
        }

    }

}
