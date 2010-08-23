using System;
using System.Collections.Generic;
using System.Text;

namespace JsMinifier
{
    public interface IJavaScriptMinifier
    {
        void Minify(string src, string dst);
        string MinifyString(string src);
    }
}
