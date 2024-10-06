using Microsoft.Xna.Framework.Graphics;

namespace MonoVoxel.Engine {
    
    public sealed class MonoVoxelGraphics {

        private RasterizerState m_rasterizer;

        public MonoVoxelGraphics( ) {
            m_rasterizer = new RasterizerState( );

            m_rasterizer.CullMode = CullMode.CullCounterClockwiseFace;
        }

        public void Apply( GraphicsDevice device ) {
            device.SamplerStates[ 0 ] = SamplerState.PointClamp;
            device.RasterizerState = m_rasterizer;
        }

    }

}
