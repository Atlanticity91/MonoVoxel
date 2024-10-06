using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MonoVoxel.Inputs {
    
    public sealed class MonoInputMouse : MonoInputDevice {

        private MouseState m_old_state;
        private MouseState m_new_state;

        public MonoInputMouse( ) { }

        public void Tick( ) {
            m_old_state = m_new_state;
            m_new_state = Mouse.GetState( );
        }

        public bool Evaluate( int key, MonoInputStates state ) {
            return false;
        }

        public Vector2 GetAxis( int axis )
            => m_new_state.Position.ToVector2( );

    }

}
