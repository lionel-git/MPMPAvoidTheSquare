using System;
using System.Diagnostics;


namespace MPMPAvoidTheSquare
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine(sizeof(ulong));

                var sq = new AvoidTheSquare(6);
                sq.InitSquares();
                sq.CountNoSquares();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
