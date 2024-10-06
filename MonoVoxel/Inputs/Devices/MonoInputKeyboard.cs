using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MonoVoxel.Inputs.Devices {

    public sealed class MonoInputKeyboard : MonoInputDevice {

        private KeyboardState m_old_state;
        private KeyboardState m_new_state;

        /// <summary>
        /// Constructor
        /// </summary>
        public MonoInputKeyboard( ) { }

        /// <summary>
        /// Tick the keyboard, savind state and query the new.
        /// </summary>
        public void Tick( ) {
            m_old_state = m_new_state;
            m_new_state = Keyboard.GetState( );
        }

        /// <summary>
        /// Evaluate a key for query state.
        /// </summary>
        /// <param name="key" >Query key</param>
        /// <param name="state" >Query key state</param>
        /// <returns>True when key as state value</returns>
        public bool Evaluate( int key, MonoInputStates state ) {
            var result = false;

            switch ( state ) {
                case MonoInputStates.Pressed  : result = m_new_state.IsKeyDown((Keys)key) && m_old_state.IsKeyUp((Keys)key);   break;
                case MonoInputStates.Released : result = m_new_state.IsKeyUp((Keys)key)   && m_old_state.IsKeyDown((Keys)key); break;
                case MonoInputStates.Down     : result = m_new_state.IsKeyDown((Keys)key) && m_old_state.IsKeyDown((Keys)key); break;
                case MonoInputStates.Up       : result = m_new_state.IsKeyUp((Keys)key)   && m_old_state.IsKeyUp((Keys)key);   break;

                default : break;
            }

            return result;
        }

        /// <summary>
        /// Evaluate axis value.
        /// </summary>
        /// <param name="axis_id" >Index of the axis</param>
        /// <param name="value" >Out axis value as vector</param>
        /// <returns>True for evaluation success</returns>
        public bool Evaluate( int axis_id, out Vector2 value ) {
            value = Vector2.Zero;

            return false;
        }

        /// <summary>
        /// Get axis value
        /// </summary>
        /// <param name="axis" >Query axis index</param>
        /// <returns>Axis x and y value as vector</returns>
        public Vector2 GetAxis( int axis )
            => Vector2.Zero;

    }

}
