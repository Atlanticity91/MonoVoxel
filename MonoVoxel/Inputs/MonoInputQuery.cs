using Microsoft.Xna.Framework.Input;

namespace MonoVoxel.Inputs {

    public enum MonoInputStates {

        Pressed = 0,
        Released,
        Down,
        Up

    }

    public struct MonoInputQuery {

        public int Device;
        public int Key;
        public MonoInputStates State;

        public MonoInputQuery( Keys key, MonoInputStates state ) {
            Device = 0;
            Key    = (int)key;
            State  = state;
        }

    }

}
