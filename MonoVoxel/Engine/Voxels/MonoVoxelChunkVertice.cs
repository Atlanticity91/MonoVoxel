using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoVoxel.Engine.Voxels {

    public struct MonoVoxelChunkVertice : IVertexType {

        Vector4 Position;
        Vector4 Metadata;

        public MonoVoxelChunkVertice( int x, int y, int z, float uv_x, float uv_y, int block_id, int face_id ) {
            Position = new Vector4( x, y, z, 1.0f );
            Metadata = new Vector4( uv_x, uv_y, block_id, face_id );
        }

        public VertexDeclaration VertexDeclaration => new VertexDeclaration(
            new VertexElement(  0, VertexElementFormat.Vector4, VertexElementUsage.Position, 0 ),
            new VertexElement( 16, VertexElementFormat.Vector4, VertexElementUsage.TextureCoordinate, 0 )
        );

    }

}
