using System;
using System.Collections.Generic;
using System.Text;
using FMOD;
using logSystem;
using NLog;

namespace tfj.exploudEngine
{
    public class eMusic : ePlayable
    {
        public Sound handle { get; private set; }
        public new string id { get; private set; }
        public new string path { get; private set; }
        private eInstance instance;
        private eSoundEngine engine;
public bool paused
        {
            get
            {
                return (this.instance.paused);
            }
            set
            {
                this.instance.paused = value;
            }
        }
        public bool playing
        {
            get
            {
                return (this.instance.playing);
            }
        }

        internal eMusic(string id, string path, eSoundEngine engine)
        {
            this.engine = engine;
            this.id = id;
            this.path = path;
            eUtils.fmodCheck(this.engine.fmod.createStream(this.path, MODE.DEFAULT, out Sound handle));
            this.handle = handle;
           
        }

        ~eMusic()
        {
            
            this.release();
        }

        public void stop()
        {
            if(this.instance==null)
            {
                return;
            }
            this.instance.stop();
            this.instance.release();
            this.instance = null;

        }

        public void play(loopMode loop = loopMode.noLoop, bool paused = false)
        {
            eInstance instance = prePlay();
          
            instance.addToGroup(this.engine.defaultMusicGroup);
            instance.is3d = false;
            instance.setLoop(loop);
            instance.paused = paused;
           
        }

        private eInstance prePlay()
        {
            if (this.instance != null)
            {
                this.stop();
            }

            eUtils.fmodCheck(this.engine.fmod.playSound(this.handle, this.engine.defaultMusicGroup.handle, true, out Channel channel));
            eInstance instance = new eInstance(channel, this.engine);
            this.instance = instance;
            return (instance);
        }

        public override void update()
        {
            if(this.instance != null)
            {
                if(this.instance.playing)
                {
                    this.instance.update();
                }
                else
                {
                    this.instance.release();
                    this.instance = null;
                }
            }

        }

        internal override void release()
        {
            this.stop();
        }

        public override void play()
        {
            play(loopMode.noLoop); 
        }
    }
}
