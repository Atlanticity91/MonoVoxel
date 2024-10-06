using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MonoVoxel.Inputs.Devices {

    public enum MonoInputMouseButtons {

        Left = 0,
        Middle,
        Right

    }

    public sealed class MonoInputMouse : MonoInputDevice {

        private MouseState m_old_state;
        private MouseState m_new_state;

        /// <summary>
        /// Constructor
        /// </summary>
        public MonoInputMouse( ) { }

        /// <summary>
        /// 
        /// </summary>
        public void Tick( ) {
            m_old_state = m_new_state;
            m_new_state = Mouse.GetState();
        }

        /// <summary>
        /// Get if left mouse button state match.
        /// </summary>
        /// <param name="state" >Query state</param>
        /// <returns>True when button state match query value</returns>
        private bool GetIsLeftButton( MonoInputStates state ) {
            var result = false;

            switch ( state ) {
                case MonoInputStates.Pressed  : result = m_old_state.LeftButton == ButtonState.Released && m_new_state.LeftButton == ButtonState.Pressed;  break;
                case MonoInputStates.Released : result = m_old_state.LeftButton == ButtonState.Pressed  && m_new_state.LeftButton == ButtonState.Released; break;
                case MonoInputStates.Down     : result = m_old_state.LeftButton == ButtonState.Pressed  && m_new_state.LeftButton == ButtonState.Pressed;  break;
                case MonoInputStates.Up       : result = m_old_state.LeftButton == ButtonState.Released && m_new_state.LeftButton == ButtonState.Released; break;

                default : break;
            }

            return result;
        }

        /// <summary>
        /// Get if middle mouse button state match.
        /// </summary>
        /// <param name="state" >Query state</param>
        /// <returns>True when button state match query value</returns>
        private bool GetIsMiddleButton( MonoInputStates state ) {
            var result = false;

            switch ( state ) {
                case MonoInputStates.Pressed  : result = m_old_state.MiddleButton == ButtonState.Released && m_new_state.MiddleButton == ButtonState.Pressed;  break;
                case MonoInputStates.Released : result = m_old_state.MiddleButton == ButtonState.Pressed  && m_new_state.MiddleButton == ButtonState.Released; break;
                case MonoInputStates.Down     : result = m_old_state.MiddleButton == ButtonState.Pressed  && m_new_state.MiddleButton == ButtonState.Pressed;  break;
                case MonoInputStates.Up       : result = m_old_state.MiddleButton == ButtonState.Released && m_new_state.MiddleButton == ButtonState.Released; break;

                default : break;
            }

            return result;
        }

        /// <summary>
        /// Get if right mouse button state match.
        /// </summary>
        /// <param name="state" >Query state</param>
        /// <returns>True when button state match query value</returns>
        private bool GetIsRightButton( MonoInputStates state ) {
            var result = false;

            switch ( state ) {
                case MonoInputStates.Pressed  : result = m_old_state.RightButton == ButtonState.Released && m_new_state.RightButton == ButtonState.Pressed;  break;
                case MonoInputStates.Released : result = m_old_state.RightButton == ButtonState.Pressed  && m_new_state.RightButton == ButtonState.Released; break;
                case MonoInputStates.Down     : result = m_old_state.RightButton == ButtonState.Pressed  && m_new_state.RightButton == ButtonState.Pressed;  break;
                case MonoInputStates.Up       : result = m_old_state.RightButton == ButtonState.Released && m_new_state.RightButton == ButtonState.Released; break;

                default : break;
            }

            return result;
        }

        /// <summary>
        /// Evaluate a key for query state.
        /// </summary>
        /// <param name="key" >Query key</param>
        /// <param name="state" >Query key state</param>
        /// <returns>True when key as state value</returns>
        public bool Evaluate( int key, MonoInputStates state ) {
            var result = false;

            switch ( (MonoInputMouseButtons)key ) {
                case MonoInputMouseButtons.Left   : result = GetIsLeftButton( state );   break;
                case MonoInputMouseButtons.Middle : result = GetIsMiddleButton( state ); break;
                case MonoInputMouseButtons.Right  : result = GetIsRightButton( state );  break;

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
            value = GetAxis( axis_id );

            return true;
        }

        /// <summary>
        /// Get axis value
        /// </summary>
        /// <param name="axis" >Query axis index</param>
        /// <returns>Axis x and y value as vector</returns>
        public Vector2 GetAxis( int axis )
            => ( m_new_state.Position - m_old_state.Position ).ToVector2( );

    }

}
