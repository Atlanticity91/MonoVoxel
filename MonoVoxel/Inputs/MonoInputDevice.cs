using Microsoft.Xna.Framework;

namespace MonoVoxel.Inputs {

    public interface MonoInputDevice {

        public void Tick( );

        public bool Evaluate( int key, MonoInputStates state );

        public Vector2 GetAxis( int axis );

    }

}
