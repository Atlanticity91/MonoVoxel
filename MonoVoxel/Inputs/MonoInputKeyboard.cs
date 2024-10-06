using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MonoVoxel.Inputs {

    public sealed class MonoInputKeyboard : MonoInputDevice {

        private KeyboardState m_old_state;
        private KeyboardState m_new_state;

        public MonoInputKeyboard( ) { }

        public void Tick( ) {
            m_old_state = m_new_state;
            m_new_state = Keyboard.GetState( );
        }

        public bool Evaluate( int key, MonoInputStates state ) {
            var result = false;

            switch ( state ) {
                case MonoInputStates.Pressed  : result = m_new_state.IsKeyDown( (Keys)key ) && m_old_state.IsKeyUp( (Keys)key );   break;
                case MonoInputStates.Released : result = m_new_state.IsKeyUp( (Keys)key )   && m_old_state.IsKeyDown( (Keys)key ); break;
                case MonoInputStates.Down     : result = m_new_state.IsKeyDown( (Keys)key ) && m_old_state.IsKeyDown( (Keys)key ); break;
                case MonoInputStates.Up       : result = m_new_state.IsKeyUp( (Keys)key )   && m_old_state.IsKeyUp( (Keys)key );   break;

                default : break;
            }

            return result;
        }

        public Vector2 GetAxis( int axis )
            => Vector2.Zero;

    }

}
