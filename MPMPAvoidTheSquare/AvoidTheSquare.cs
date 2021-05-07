using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Threading;

namespace MPMPAvoidTheSquare
{
    public class AvoidTheSquare
    {
        private int n_;
        private HashSet<long> masks_;
        private SortedDictionary<int, int> results_;

        public AvoidTheSquare(int n)
        {
            n_ = n;
            if (n_ * n_ > 8 * sizeof(ulong))
                throw new Exception("n is too large!");
            masks_ = new HashSet<long>();
            results_ = new SortedDictionary<int, int>();
        }

        private bool isOk(int k)
        {
            return k >= 0 && k < n_;
        }

        private long GetMask(int i, int j)
        {
            return 1L << (i * n_ + j);
        }

        // All masks that represent a square
        public void InitSquares()
        {
            // Point le plus haut du carre a gauche (i,j)
            for (int ai = 0; ai < n_; ai++)
                for (int aj = 0; aj < n_; aj++)
                    for (int zi = 0; zi < n_; zi++)
                        for (int zj = 0; zj < n_; zj++)
                        {
                            if (zi != 0 || zj != 0)
                            {
                                int bi = ai + zi;
                                int bj = aj + zj;

                                int ci = ai - zj;
                                int cj = aj + zi;

                                int di = ai + zi - zj;
                                int dj = aj + zj + zi;

                                if (isOk(bi) && isOk(bj) &&
                                    isOk(ci) && isOk(cj) &&
                                    isOk(di) && isOk(dj))
                                {
                                    Console.WriteLine($"{ai} {aj} | {bi} {bj} |{ci} {cj} |{di} {dj}");
                                    long mask = GetMask(ai, aj) | GetMask(bi, bj) | GetMask(ci, cj) | GetMask(di, dj);
                                    masks_.Add(mask);
                                }
                            }
                        }
            Console.WriteLine($"Masks: {masks_.Count}");
        }

        private bool HasSquare(long value)
        {
            foreach (var mask in masks_)
            {
                var res = mask & value;
                if (res == 0 || res == mask)
                    return true;
            }
            return false;
        }

        public int CountOnes(long v)
        {
            return BitOperations.PopCount((ulong)v);
        }

        public void CountNoSquares()
        {
            long target = 1L << (n_ * n_);
            long mask = (target >> 8) - 1;
            Parallel.For(0, target, new ParallelOptions() { MaxDegreeOfParallelism = Environment.ProcessorCount }, 
                (v, state) =>
           {
               if (!HasSquare(v))
               {
                   int nbOnes = CountOnes(v);
                   lock (results_)
                   {
                       if (results_.ContainsKey(nbOnes))
                           results_[nbOnes]++;
                       else
                           results_.Add(nbOnes, 1);
                   }
               }
               if ((v & mask) == 0)
                   Console.WriteLine($"{DateTime.Now.ToString("HH:mm:ss")} {(((double)v) / target) * 100.0}%");
           });
            foreach (var result in results_)
            {
                Console.WriteLine($"{result.Key} => {result.Value}");
            }
        }
    }
}

