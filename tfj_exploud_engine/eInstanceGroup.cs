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
        

        internal eInstanceGroup(ChannelGroup handle, string name, eSoundEngine engine)
        {
            this.handle = handle;
            this.name = name;
            this.engine = engine;

        }

        public void update()
        {

        }
    }
}
