using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using SkylerCommon.Globals;
using SkylerCommon.Memory;
using SkylerGraphics.ContextHandler;
using SkylerGraphics.Texture;
using SkylerShader.NativeShader;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Diagnostics;

namespace SkylerGraphics.Display
{
    public delegate void ExternalHook();

    public unsafe class TKWindow : GameWindow
    {
        string Vertex = @"
#version 330 core

layout(location = 0) in vec3 iPosition;

out vec2 uv;

void main()
{
    uv = iPosition.xy;

    uv.y = 1- uv.y;

    gl_Position = vec4((iPosition - 0.5) * 2,1);
}
";

        string Fragment = @"
#version 330 core

out vec4 fragColor;

in vec2 uv;

uniform sampler2D texture0;

void main()
{
    fragColor = texture(texture0,uv);
}

";
        public float AspectRatio;

        public NativeTexture FrameBufferTexture { get; set; }

        int VAO;

        public static ulong HidHandle { get; set; }

        public TKWindow() : base(

                new GameWindowSettings()
                {
                    IsMultiThreaded = false,
                    RenderFrequency = 1/60d
                },
                new NativeWindowSettings()
                {
                    Profile = ContextProfile.Compatability,
                    Size = new Vector2i((int)GlobalsGraphics.ScreenWidth, (int)GlobalsGraphics.ScreenHeight)
                }
            )
        {

        }

        public ExternalHook End;
        public ExternalHook ScreenUpdate;

        public void Start()
        {
            OnLoad();

            OnResize(new ResizeEventArgs());

            MakeCurrent();

            CompileAndUseShader();

            Title = "Skyler Nintendo Switch Emulator";

            AspectRatio = (float)ClientSize.Y / (float)ClientSize.X;

            FrameBufferTexture = new NativeTexture((int)GlobalsGraphics.ScreenWidth, (int)GlobalsGraphics.ScreenHeight);

            VAO = GL.GenVertexArray();

            Vector3[] buffer = new Vector3[]
            {
                new Vector3(0,0,0),
                new Vector3(1,0,0),
                new Vector3(1,1,0),
                new Vector3(0,1,0)
            };

            int vbo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer,vbo);
            GL.BufferData(BufferTarget.ArrayBuffer,buffer.Length * sizeof(Vector3),buffer,BufferUsageHint.StaticDraw);

            GL.VertexAttribPointer(0,3,VertexAttribPointerType.Float,false,0,0);
            GL.EnableVertexAttribArray(0);

            VSync = VSyncMode.Off;

            Stopwatch timer = new Stopwatch();

            while (true)
            {             
                timer.Start();

                ProcessEvents();

                ResizeScreen();

                GL.Clear(ClearBufferMask.ColorBufferBit);
                GL.ClearColor(0.2f, 0.2f, 0.2f, 1);

                RenderFrameBuffer();

                SwapBuffers();

                if (ScreenUpdate != null)
                {
                    ScreenUpdate();
                }

                if (!Exists || IsExiting)
                {
                    DestroyWindow();
                    break;
                }

                timer.Stop();

                Title = $"Skyler Nintendo Switch Emulator               Host FrameTime: {timer.ElapsedMilliseconds}";

                timer.Reset();
            }

            End();
        }

        ShaderProgram shader;

        public void CompileAndUseShader()
        {
            shader = new ShaderProgram();

            shader.AddShader(new ShaderSource(Vertex,ShaderType.VertexShader));
            shader.AddShader(new ShaderSource(Fragment,ShaderType.FragmentShader));

            shader.Compile();

            shader.Use();
        }

        public void ResizeScreen()
        {
            Size = new Vector2i(ClientSize.X, (int)(ClientSize.X * AspectRatio));

            GL.Viewport(0, 0, ClientSize.X, ClientSize.Y);
        }

        static int GetSwizzleOffset(int X, int Y)
        {
            int Pos;

            Pos = (Y & 0x7f) >> 4;
            Pos += (X >> 4) << 3;
            Pos += (Y >> 7) * (((int)GlobalsGraphics.ScreenWidth >> 4) << 3);
            Pos *= 1024;
            Pos += ((Y & 0xf) >> 3) << 9;
            Pos += ((X & 0xf) >> 3) << 8;
            Pos += ((Y & 0x7) >> 1) << 6;
            Pos += ((X & 0x7) >> 2) << 5;
            Pos += ((Y & 0x1) >> 0) << 4;
            Pos += ((X & 0x3) >> 0) << 2;

            return Pos;
        }

        public void RenderFrameBuffer()
        {
            MemoryReader reader = new MemoryReader((void*)((ulong)GlobalMemory.BaseMemoryPointer + FrameBuffers.MainFrameBuffer));

            for (int x = 0; x < (int)GlobalsGraphics.ScreenWidth; x++)
            {
                for (int y = 0; y < (int)GlobalsGraphics.ScreenHeight; y++ )
                {
                    //GetSwizzleOffset(x, y);

                    reader.Seek((ulong)GetSwizzleOffset(x,y));

                    FrameBufferTexture.Buffer[(x + y * (int)GlobalsGraphics.ScreenWidth)] = reader.ReadStruct<int>();
                }
            }

            FrameBufferTexture.UploadTexture();
            FrameBufferTexture.UseTexture();

            GL.DrawArrays(BeginMode.Quads,0,12);
        }
    }
}
