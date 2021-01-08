using OpenTK.Graphics.OpenGL;
using SkylerCommon.Debugging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace SkylerShader.NativeShader
{
    public class ShaderSource
    {
        public int Handle       { get; set; }
        public string Source    { get; set; }
        public ShaderType Type  { get; set; }

        public bool Compiled => Handle != -1;

        public ShaderSource(string Source, ShaderType Type)
        {
            Handle = -1;
            this.Source = Source;
            this.Type = Type;
        }

        public void CompileShader()
        {
            if (Compiled)
            {
                GL.DeleteShader(Handle);

                Handle = -1;
            }

            Handle = GL.CreateShader(Type);

            GL.ShaderSource(Handle,Source);
            GL.CompileShader(Handle);

            string Error = GL.GetShaderInfoLog(Handle);

            if (Error != "")
            {
                Debug.LogError(Error,true);
            }
        }
    }
}
