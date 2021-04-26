/*
 * VirtEco
 * By: Mark Ambrocio
 * 
 * Icon from: https://icon-icons.com/icon/desert
 * 
 * 
 */

using Pastel;
using System;

namespace VirtualEcoSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "VirtEco: Mojave Desert | By: Mark Ambroico";
            if (!Utils.__PROD__) Console.WriteLine("Development build detected.".Pastel(Utils.Color["Primary"]));
            new VirtEcoGame();
        }
    }
}
