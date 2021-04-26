/*
 * Virtual Ecosystem: Mojave Desery
 * By: Mark Ambrocio
 * 
 * Credits:
 * Using Pastel from nuget packages.
 * XML loading from Vending Machine Exercise
 * Icon from: https://icon-icons.com/icon/desert
 * SaveData/BinaryFormatter help from: @Epitome https://youtu.be/Q2nEsa209ew
 * Inventory with help from: 
 * @CodeMonkey on youtube.com
 * & Kryzel on youtube: https://www.youtube.com/watch?v=gZsJ_rG5hdo
 * Obligate Mutual guidance from: Prof. J. Baxter.
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
            // enable prod in utils for cleaner look
            if (!Utils.__PROD__) Console.WriteLine("Development build detected.".Pastel(Utils.Color["Primary"]));
            new VirtEcoGame();
        }
    }
}
