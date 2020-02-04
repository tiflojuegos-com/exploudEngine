using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace tfj.exploudEngine
{
    internal static class eOculusOperations
    {
        [DllImport("plugins/OculusSpatializerFMOD.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl, EntryPoint = "OSP_FMOD_Initialize")]
        internal static extern int oculusInit(int SampleRate, uint BufferLength);

        [DllImport("plugins/OculusSpatializerFMOD.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl, EntryPoint = "OSP_FMOD_SetProfilerEnabled")]
        internal static extern int setProfiler(bool enabled);

        [DllImport("plugins/OculusSpatializerFMOD.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl, EntryPoint = "OSP_FMOD_SetDynamicRoomRaysPerSecond")]
        internal static extern int setDinamicRayRoomsPerSecond(int rays);
    }
}
