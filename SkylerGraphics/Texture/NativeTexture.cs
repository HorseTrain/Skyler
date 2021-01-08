using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace SkylerGraphics.Texture
{
    public class NativeTexture 
    {
        public int[] Buffer    { get; set; }
        public int Width        { get; set; }
        public int Height       { get; set; }
        public int Handle       { get; set; }

        public NativeTexture(int Width, int Height)
        {
            Handle = -1;

            this.Width = Width;
            this.Height = Height;

            Buffer = new int[Width * Height];
        }
        
        public void UploadTexture()
        {
            DeloadTexture();

            Handle = GL.GenTexture();

            GL.BindTexture(TextureTarget.Texture2D, Handle);

            GL.TexImage2D(TextureTarget.Texture2D,0,PixelInternalFormat.Rgba,Width,Height,0,PixelFormat.Rgba, PixelType.UnsignedByte,Buffer);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMagFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
        }

        public void UseTexture()
        {
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D,Handle);
        }

        public void DeloadTexture()
        {
            if (Handle != -1)
            {
                GL.DeleteTexture(Handle);
            }
        }
    }
}
