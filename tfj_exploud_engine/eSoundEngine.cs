using System;
using System.Collections.Generic;
using System.Text;
using FMOD;
using logSystem;
using NLog;

namespace tfj.exploudEngine
{
    public class eSoundEngine
    {
        
        private static eSoundEngine _defaultEngine;
        public static eSoundEngine defaultEngine
        {
            get
            {
                return (_defaultEngine);
            }
            set
            {
                _defaultEngine = value;
            }
        }

        public FMOD.System fmod { get; private set; }
        public uint oculusSourcePluginHandle { get; private set; }
        public uint oculusAmbisonicPluginHangle { get; private set; }
        public uint oculusGlobalSettingsPluginHandle { get; private set; }
        public DSP oculusGlobalSettingsDSP { get; private set; }
        public eInstanceGroup default3dGroup { get; private set; }
        public eInstanceGroup default2dGroup { get; private set; }
        public eInstanceGroup defaultMusicGroup { get; private set;  }
        public eListener listener { get; private set;  }
        public eRoom currentRoom { get; private set;  }
        private Dictionary<string, eSound> soundKache;
        public bool reflections3d
        {
            get
            {
                eUtils.fmodCheck(this.oculusGlobalSettingsDSP.getParameterBool(0, out bool activeReflections));
                return (activeReflections);
            }
            set
            {
                eUtils.fmodCheck(this.oculusGlobalSettingsDSP.setParameterBool(0, value));
            }
        }
        public bool reberberations3d
        {
            get
            {
                eUtils.fmodCheck(this.oculusGlobalSettingsDSP.getParameterBool(1, out bool activeReberb));
                return (activeReberb);
            }
            set
            {
                eUtils.fmodCheck(this.oculusGlobalSettingsDSP.setParameterBool(1, value));
            }
        }

        public eSoundEngine()
        {
            LogWriter.getLog().Debug("starting exploud engine");
            defaultEngine = this;
            eUtils.fmodCheck(Factory.System_Create(out FMOD.System fmod));
            this.fmod = fmod;
            fmod.setSoftwareChannels(10);
            eUtils.fmodCheck(fmod.init(10, INITFLAGS.NORMAL, (IntPtr)OUTPUTTYPE.AUTODETECT));

            loadPlugins();
            setDefaultSettings();

        }

        public void loadPlugins()
        {
            eUtils.fmodCheck(fmod.loadPlugin("plugins/OculusSpatializerFMOD.dll", out uint sourceHandle));
            this.oculusSourcePluginHandle = sourceHandle;
            eUtils.fmodCheck(fmod.getNestedPlugin(oculusSourcePluginHandle, 1, out uint ambisonicHandle));
            this.oculusAmbisonicPluginHangle = ambisonicHandle;
            eUtils.fmodCheck(fmod.getNestedPlugin(oculusSourcePluginHandle, 2, out uint GSHandle));
            this.oculusGlobalSettingsPluginHandle = GSHandle;
            LogWriter.getLog().Debug("plugins loaded");
        }

        public void setDefaultSettings()
        {
            this.listener = new eListener(this);
            string group3dName = "3dGroup";
            string group2dName = "2dGroup";
            string groupMusicName = "musicGroup";
            eUtils.fmodCheck(fmod.createChannelGroup(group3dName, out ChannelGroup group3d));
            this.default3dGroup = new eInstanceGroup(group3d, group3dName, this);
            eUtils.fmodCheck(fmod.createChannelGroup(group2dName, out ChannelGroup group2d));
            this.default2dGroup = new eInstanceGroup(group2d, group2dName, this);
            eUtils.fmodCheck(fmod.createChannelGroup(groupMusicName, out ChannelGroup groupMusic));
            this.defaultMusicGroup = new eInstanceGroup(groupMusic, groupMusicName, this);
            eUtils.fmodCheck(fmod.createDSPByPlugin(this.oculusGlobalSettingsPluginHandle, out DSP globalSettingsDSP));
            this.oculusGlobalSettingsDSP = globalSettingsDSP;
            eUtils.fmodCheck(this.default3dGroup.handle.addDSP(CHANNELCONTROL_DSP_INDEX.TAIL,this.oculusGlobalSettingsDSP));
            this.reberberations3d = true;
            this.reflections3d = false;
            this.currentRoom = new eRoom(this);
            clearSoundCache();

        }

        public eSound loadSound(string path)
        {
          
            string id = path.Replace("\\", ".");
            LogWriter.getLog().Info($"loading {id} sound");
            if(soundKache.ContainsKey(id))
            {
                LogWriter.getLog().Info($"{id} sound was loaded previously. returning kached data");
                return (soundKache[id]);
            }
            eSound sound = new eSound(id, path, this);
            soundKache.Add(id, sound);
            LogWriter.getLog().Info($"{id} sound loaded");
            return (sound);
        }

        public void update()
        {
            this.listener.update();
            foreach(KeyValuePair<string,eSound> k in soundKache)
            {
                k.Value.update();

            }
            this.currentRoom.update();
            fmod.getChannelsPlaying(out int channels);
            
            
            eUtils.fmodCheck(fmod.update());
            

        }

        public void clearSoundCache()
        {
            this.soundKache = new Dictionary<string, eSound>();
        }
    }
}
