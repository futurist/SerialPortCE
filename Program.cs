﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Forms;

namespace meter
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [MTAThread]
        static void Main()
        {
            Form1 f = new Form1();
            Application.Run(f);
        }
    }
}