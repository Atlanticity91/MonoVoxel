using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace MonoVoxel.Inputs {

    public sealed class MonoInputManager {

        private List<MonoInputDevice> m_devices;
        private Dictionary<string, MonoInputQueries> m_queries;

        public MonoInputManager( ) { 
            m_devices = new List<MonoInputDevice>( ) { 
                new MonoInputKeyboard( ),
                new MonoInputMouse( )
            };
            m_queries = new Dictionary<string, MonoInputQueries>( );
        }

        public void Register( string name, MonoInputQuery[] queries ) {
            if ( !string.IsNullOrEmpty( name ) ) {
                if ( !m_queries.ContainsKey( name ) ) {
                    var query = new MonoInputQueries( queries );

                    if ( query != null )
                        m_queries.Add( name, query );
                } else
                    m_queries[ name ].Add( queries );
            }
        }

        public void Tick( ) {
            foreach ( var device in m_devices )
                device.Tick( );

            foreach ( var query in m_queries.Values )
                query.Tick( );
        }

        public bool Evaluate( string name, bool consume ) {
            var result = false;

            if ( m_queries.ContainsKey( name ) )
                result = m_queries[ name ].Evaluate( m_devices, consume );

            return result;
        }

        public Vector2 GetAxis( int device, int axis ) {
            var result = Vector2.Zero;

            if ( device < m_devices.Count )
                result = m_devices[ device ].GetAxis( axis );

            return result;
        }

    }

}
