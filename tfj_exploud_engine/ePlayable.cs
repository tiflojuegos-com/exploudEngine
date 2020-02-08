using System;
using System.Collections.Generic;
using System.Text;

namespace tfj.exploudEngine
{
    public abstract class ePlayable
    {
        public string id { get; private set; }
        public string path { get; private set; }
        public abstract void update();
        internal abstract void release();
        public abstract void play();

    }
}
