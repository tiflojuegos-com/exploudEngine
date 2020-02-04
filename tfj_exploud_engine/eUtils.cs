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

        public static void oculusCheck(int operationResult)
        {
            if(operationResult != 0)
            {
                LogWriter.getLog().Error($"problems calling an internal function of Oculus plugin. Returned an {operationResult} operation result status.");
            }
        }

        public static RESULT fmodCheck(FMOD.RESULT result, string step = "non detailed.")
        {
          if(result != RESULT.OK)
            {
                LogWriter.getLog().Error($"problems excecuting a fmod function at {step} step {result}");
              
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
