using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using FMOD;
using NLog;
using logSystem;

namespace tfj.exploudEngine
{
    public enum loopMode
    {
        noLoop,
        simpleLoop,
        bidiLoop
    }
    public static class eUtils
    {

        public static RESULT fmodCheck(FMOD.RESULT result)
        {
          if(result != RESULT.OK)
            {
                LogWriter.getLog().Error($"problems excecuting a fmod function {result}");
              
            }
            return (result);
        }

        public static byte[] generateBytes<t>(t data, Type datatype)
        {
            byte[] databytes = new byte[Marshal.SizeOf(datatype)];
            GCHandle pinStructure = GCHandle.Alloc(data, GCHandleType.Pinned);
            try
            {
                Marshal.Copy(pinStructure.AddrOfPinnedObject(), databytes, 0, databytes.Length);
                
            }
            catch(Exception e)
            {
                LogWriter.getLog().Error($"problems marshalling data. {e.Message}");
                
            }
            finally
            {
                pinStructure.Free();
                
            }
            return (databytes);

        }
    }
}
