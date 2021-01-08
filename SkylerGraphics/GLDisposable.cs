using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkylerGraphics
{
    public delegate void GLDesposeFunction(int ID);

    public class GLDisposable
    {
        public static List<GLDisposable> Disposables    { get; set; } = new List<GLDisposable>();
        GLDesposeFunction Dispose                       { get; set; }
        public int Handle                               { get; set; }

        public GLDisposable(GLDesposeFunction Dispose)
        {
            this.Dispose = Dispose;
        }
    }
}
