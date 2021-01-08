using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkylerShader.NativeShader
{
    public class ShaderProgram
    {
        public List<ShaderSource> Shaders   { get; private set; }
        public int Program                  { get; set; }
        public bool Compiled =>             Program != -1;

        public ShaderProgram()
        {
            Shaders = new List<ShaderSource>();
            Program = -1;
        }

        public void AddShader(ShaderSource source)
        {
            Shaders.Add(source);
        }

        public void RemoveShader(ShaderSource source)
        {
            Shaders.Remove(source);
        }

        public void Compile()
        {
            if (Compiled)
            {
                GL.DeleteProgram(Program);

                Program = -1;
            }

            Program = GL.CreateProgram();

            foreach (ShaderSource source in Shaders)
            {
                source.CompileShader();

                GL.AttachShader(Program,source.Handle);
            }

            GL.LinkProgram(Program);
            GL.ValidateProgram(Program);
        }

        public void Use()
        {
            GL.UseProgram(Program);
        }
    }
}
