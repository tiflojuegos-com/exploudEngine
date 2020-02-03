using System;
using System.Collections.Generic;
using System.Text;
using FMOD;

namespace tfj.exploudEngine
{
    public class eInstance
    {
        private eSoundEngine engine;
        public Channel handle { get; private set; }
        public DSP oculusSourceDSP { get; private set; }
        public float x = 0;
        public float y= 0;
        public float z= 0;
        public int rotation = 0;
        public float velX = 0;
        public float vely = 0;
        public float velZ = 0;
        public float bertical = 0;
        public bool reflections
        {
            get
            {
                eUtils.fmodCheck(this.oculusSourceDSP.getParameterBool(1, out bool activeReflections));
                return (activeReflections);
            }
            set
            {
                eUtils.fmodCheck(this.oculusSourceDSP.setParameterBool(1, value));
            }
        }
        public bool oculusAtenuation
        {
            get
            {
                eUtils.fmodCheck(this.oculusSourceDSP.getParameterBool(2, out bool activeInternalAtenuation));
                return (activeInternalAtenuation);
            }
            set
            {
                eUtils.fmodCheck(this.oculusSourceDSP.setParameterBool(2, value));
            }
        }
                public float radius = 0.2f;
        public float minDistance = 0.2f;
        public float maxDistance = 20.0f;
        public bool paused
        {
            get
            {
                eUtils.fmodCheck(handle.getPaused(out bool isPaused));
                return (isPaused);
            }
            set
            {
                eUtils.fmodCheck(handle.setPaused(value));
                
            }
        }
        public bool muted
        {
            get
            {
                eUtils.fmodCheck(handle.getMute(out bool isMuted));
                return (isMuted);

            }
            set
            {
                eUtils.fmodCheck(handle.setMute(value));
            }
        }
        public bool playing
        {
            get
            {
                RESULT result = eUtils.fmodCheck(handle.isPlaying(out bool isPlaying));
                if(result == RESULT.ERR_INVALID_HANDLE || result == RESULT.ERR_CHANNEL_STOLEN)
                {
                    return (false);
                }
                return (isPlaying);

            }
        }
        private bool _is3d;
        public bool is3d
        {
            get
            {
                return (_is3d);
            }
            set
            {
                this._is3d = value;
                if(value)
                {
                    eUtils.fmodCheck(this.handle.addDSP(CHANNELCONTROL_DSP_INDEX.TAIL, this.oculusSourceDSP));
                }
                else
                {
                    eUtils.fmodCheck(this.handle.removeDSP(this.oculusSourceDSP));
                }
            }
        }
        public loopMode loop { get; private set;  }
      
        internal eInstance(Channel handle, eSoundEngine engine)
        {
            this.engine = engine;
            this.handle = handle;
            eUtils.fmodCheck(engine.fmod.createDSPByPlugin(engine.oculusSourcePluginHandle, out DSP sourceDSP));
            this.oculusSourceDSP = sourceDSP;
            this.is3d = true;
            this.reflections = false;
            this.paused = true;
            this.loop = loopMode.noLoop;
        }

        internal void addToGroup(eInstanceGroup group)
        {
            eUtils.fmodCheck(this.handle.setChannelGroup(group.handle));
        }

        public void setLoop(loopMode mode)
        {
            MODE fmodMode;
            switch(mode)
            {
                case loopMode.noLoop:
                    fmodMode = MODE.LOOP_OFF;
                    break;
                case loopMode.simpleLoop:
                    fmodMode = MODE.LOOP_NORMAL;
                    break;
                case loopMode.bidiLoop:
                    fmodMode = MODE.LOOP_BIDI;
                    break;
                default:
                    fmodMode = MODE.LOOP_OFF;
                    break;
            }
            this.loop = mode;
            eUtils.fmodCheck(this.handle.setMode(fmodMode));

        }

        public void update()
        {
            eUtils.fmodCheck(this.oculusSourceDSP.setParameterFloat(3, this.radius));
            eUtils.fmodCheck(this.oculusSourceDSP.setParameterFloat(4, this.minDistance));
            eUtils.fmodCheck(this.oculusSourceDSP.setParameterFloat(5, this.maxDistance));
            VECTOR positionVector = new VECTOR { x = this.x, y = this.y, z = this.z };
            VECTOR velocityVector = new VECTOR { x = this.velX, y = this.vely, z = this.velZ };
            VECTOR upVector = new VECTOR { x = 0, y = 1, z = 0 };
            VECTOR forwardVector = new VECTOR { x = 0, y = 0, z = 1 };

            DSP_PARAMETER_3DATTRIBUTES attributes3d = new DSP_PARAMETER_3DATTRIBUTES();
            attributes3d.absolute = new ATTRIBUTES_3D
            {
                position = positionVector,
                velocity = velocityVector,
                forward = forwardVector,
                up = upVector
            };

            attributes3d.relative = new ATTRIBUTES_3D
            {
                position = positionVector,
                velocity = velocityVector,
                forward = forwardVector,
                up = upVector
            };

            byte[] attributesData = eUtils.generateBytes(attributes3d, typeof(DSP_PARAMETER_3DATTRIBUTES));
            eUtils.fmodCheck(this.oculusSourceDSP.setParameterData(0, attributesData));




        }
    }
}
