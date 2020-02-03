using System;
using System.Collections.Generic;
using System.Text;
using FMOD;
using logSystem;
using NLog;

namespace tfj.exploudEngine
{
    public class eListener
    {
        private eSoundEngine engine;
        public float x=0;
        public float y=0;
        public float z=0;
        public float velX=0;
        public float velY=0;
        public float velZ = 0;
        private int _verticalRotation = 0;
        public int verticalRotation
        {
            get
            {
                return (_verticalRotation);
            }
        }
        private int _rotation=0;
        public int rotation
        {
            get
            {
                return (_rotation);
            }
            set
            {
                _rotation = value;
                if(_rotation > 360)
                {
                    _rotation = 0;
                }
                if(value <0 )
                {
                    _rotation = 0;
                }
            }
        }

        internal eListener(eSoundEngine engine)
        {
            this.engine = engine;

        }

        public void update()
        {
            VECTOR positionVector = new VECTOR { x = this.x, y = this.y, z = this.z };
            VECTOR velocityVector = new VECTOR { x = this.velX, y = this.velY, z = this.velZ };
            VECTOR upVector = new VECTOR { x = 0, y =(float) Math.Sin((verticalRotation*Math.PI)/180), z = 0 };
            VECTOR upVector2 = new VECTOR { x = 0, y = 1, z = 0 };
            VECTOR forwardVector2 = new VECTOR { x = 0, y = 0, z = 1 };
            VECTOR forwardVector = new VECTOR
            {
                x = (float)Math.Sin((rotation * Math.PI) / 180),
                y = 0,
                z = (float)Math.Cos((rotation * Math.PI) / 180)
            };
            
            eUtils.fmodCheck(engine.fmod.set3DListenerAttributes(0, ref positionVector, ref velocityVector, ref forwardVector, ref upVector2));
        }
    }
}
