/*
 * VirtEco
 * By: Mark Ambrocio
 * 
 * Icon from: https://icon-icons.com/icon/desert
 * 
 * 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static VirtualEcoSystem.FileSystem.FileInitializer;

namespace VirtualEcoSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            CheckForSaveFile();
            Console.Title = "VirtEco: Mojave Desert | By: Mark Ambroico";
            new VirtEcoGame();
        }
    }
}
