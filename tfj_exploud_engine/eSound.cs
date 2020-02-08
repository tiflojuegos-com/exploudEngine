using System;
using System.Collections.Generic;
using System.Text;
using FMOD;
using logSystem;
using NLog;

namespace tfj.exploudEngine
{
    public class eSound : ePlayable
    {
        public Sound handle { get; private set; }
        public new string id { get; private set;  }
        public new string path { get; private set;  }
        public List<eInstance> instances;
        private eSoundEngine engine;

        internal eSound(string id, string path, eSoundEngine engine)
        {
            this.engine = engine;
            this.id = id;
            this.path = path;
            eUtils.fmodCheck(this.engine.fmod.createSound(this.path, MODE.DEFAULT, out Sound handle));
            this.handle = handle;
            this.instances = new List<eInstance>();
        }

        ~eSound()
        {
            this.release();
        }

        public void clearInstances()
        {
            foreach(eInstance instance in this.instances)
            {
                instance.stop();
                instance.release();

            }
            this.instances = new List<eInstance>();
        }

        public eInstance play3d(float x, float y, float z, loopMode loop = loopMode.noLoop, bool paused = false)
        {
            eInstance instance = prePlay();
            instance.x = x;
            instance.y = y;
            instance.z = z;
            instance.setLoop(loop);
            instance.is3d = true;
            instance.paused = paused;
            return(instance);

        }

        public eInstance play(float pan = 0, loopMode loop = loopMode.noLoop, bool paused = false)
        {
            eInstance instance = prePlay();
            instance.pan = pan;
            
            instance.addToGroup(this.engine.default2dGroup);
            instance.is3d = false;
            instance.setLoop(loop);
            instance.paused = paused;
            return (instance);
        }

        private eInstance prePlay()
        {
            eUtils.fmodCheck(this.engine.fmod.playSound(this.handle, this.engine.default3dGroup.handle, true, out Channel channel));
            eInstance instance = new eInstance(channel, this.engine);
            this.instances.Add(instance);
            return (instance);
        }

        public override void update()
        {
            
            List<eInstance> forPop = new List<eInstance>();

            foreach(eInstance instanse in this.instances)
            {
                if(instanse.playing == false)
                {
                    forPop.Add(instanse);
                    continue;
                }
                instanse.update();

            }
            foreach(eInstance i in forPop)
            {
                i.release();                
                instances.Remove(i);

            }
        }

        internal override void release()
        {
            this.clearInstances();
        }

        public override void play()
        {
            this.play(0, loopMode.noLoop);
        }
    }
}
