using Microsoft.Xna.Framework;
using MonoVoxel.Inputs.Devices;
using MonoVoxel.Inputs.Queries;
using System.Collections.Generic;

namespace MonoVoxel.Inputs {

    public sealed class MonoInputManager {

        private MonoInputDevice[] m_devices;
        private Dictionary<string, MonoInputQueries> m_queries;
        private Dictionary<string, MonoInputAxisQueries> m_axis_queries;

        /// <summary>
        /// Constructor
        /// </summary>
        public MonoInputManager( ) {
            m_devices = new MonoInputDevice[ 3 ] {
                new MonoInputKeyboard( ),
                new MonoInputMouse( ),
                new MonoInputGamepadManager( )
            };
            m_queries = new Dictionary<string, MonoInputQueries>( );
            m_axis_queries = new Dictionary<string, MonoInputAxisQueries>( );
        }

        /// <summary>
        /// Register input queries.
        /// </summary>
        /// <param name="name" >Name of the input query</param>
        /// <param name="queries" >Array of all input query</param>
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

        /// <summary>
        /// Register input axis queries.
        /// </summary>
        /// <param name="name" >Name of the input axis query</param>
        /// <param name="axis_queries" >Array of input axis query</param>
        public void Register( string name, MonoInputAxis[] axis_queries ) {
            if ( !string.IsNullOrEmpty( name ) ) {
                if ( !m_axis_queries.ContainsKey( name ) ) {
                    var query =new MonoInputAxisQueries( axis_queries );

                    if ( query != null )
                        m_axis_queries.Add( name, query );
                } else
                    m_axis_queries[ name ].Add( axis_queries );
            }
        }

        /// <summary>
        /// Tick current input manager.
        /// </summary>
        public void Tick( ) {
            foreach ( var device in m_devices )
                device.Tick( );

            foreach ( var query in m_queries.Values )
                query.Tick( );
        }

        /// <summary>
        /// Evaluate an input query.
        /// </summary>
        /// <param name="name" >Name of the input query</param>
        /// <param name="consume" >
        ///     True for input consumtion aka other call for the query
        ///     with the same name always fail.
        /// </param>
        /// <returns>True for evaluation success</returns>
        public bool Evaluate( string name, bool consume ) {
            var result = false;

            if ( m_queries.ContainsKey( name ) )
                result = m_queries[ name ].Evaluate( m_devices, consume );

            return result;
        }

        /// <summary>
        /// Evaluate the input axis value.
        /// </summary>
        /// <param name="devices" >Device array.</param>
        /// <returns>Axis value for x and y as vector</returns>
        public Vector2 Evaluate( string name ) {
            var result = Vector2.Zero;

            if ( m_axis_queries.ContainsKey( name ) )
                result = m_axis_queries[ name ].Evaluate( m_devices );

            return result;
        }

        /// <summary>
        /// Get axis value.
        /// </summary>
        /// <param name="device" >Query device</param>
        /// <param name="axis_id" >Query axis index</param>
        /// <returns>Axis x and y value as vector</returns>
        public Vector2 GetAxis( int device, int axis_id ) {
            var result = Vector2.Zero;

            if ( device < m_devices.Length )
                result = m_devices[ device ].GetAxis( axis_id );

            return result;
        }

    }

}
