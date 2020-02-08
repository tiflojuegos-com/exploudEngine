using System;
using System.Collections.Generic;
using System.Text;
using FMOD;

namespace tfj.exploudEngine
{
    public class eInstanceGroup
    {
        public ChannelGroup handle { get; private set; }
        public string name { get; private set; }
        private eSoundEngine engine;
      public float volume
        {
            get
            {
                eUtils.fmodCheck(this.handle.getVolume(out float volume));
                return (volume);
            }
            set
            {
                if(value>1)
                {
                    value = 1;
                }
                else if(value<0)
                {
                    value = 0;
                }
                eUtils.fmodCheck(this.handle.setVolume(value));
            }
        }
        public bool muted
        {
            get
            {
                eUtils.fmodCheck(this.handle.getMute(out bool isMuted));
                return (isMuted);
            }
            set
            {
                eUtils.fmodCheck(this.handle.setMute(value));

            }
        }

        internal eInstanceGroup(ChannelGroup handle, string name, eSoundEngine engine)
        {
            this.handle = handle;
            this.name = name;
            this.engine = engine;
          

        }

        public void stop()
        {
            eUtils.fmodCheck(this.handle.stop(), $"stoping {name} channel ");
        }

        public void update()
        {

        }
    }
}
