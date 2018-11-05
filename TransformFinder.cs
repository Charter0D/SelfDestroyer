using SDG.Unturned;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SelfDestroyer
{
    public class TransformFinder
    {
        public static List<Transform> Barricades => FindBarricades();
        public static List<Transform> Structures => FindStructures();

        private static List<Transform> FindBarricades()
        {
            List<Transform> res = new List<Transform>();
            for (int k = 0; k < BarricadeManager.BarricadeRegions.GetLength(0); k++)
            {
                for (int l = 0; l < BarricadeManager.BarricadeRegions.GetLength(1); l++)
                {
                    var reg = BarricadeManager.BarricadeRegions[k, l];
                    for (int i = 0; i < reg.drops.Count; i++)
                    {
                        var drop = reg.drops[i];
                        res.Add(drop.model);
                    }
                }
            }
            return res;
        }

        private static List<Transform> FindStructures()
        {
            List<Transform> res = new List<Transform>();
            for (int k = 0; k < StructureManager.regions.GetLength(0); k++)
            {
                for (int l = 0; l < StructureManager.regions.GetLength(1); l++)
                {
                    var reg = StructureManager.regions[k, l];
                    for (int i = 0; i < reg.drops.Count; i++)
                    {
                        var drop = reg.drops[i];
                        res.Add(drop.model);
                    }
                }
            }
            return res;
        }

        public static List<Transform> FindAll()
        {
            var kust = Barricades;
            var suks = Structures;
            foreach (var v in suks)
                kust.Add(v);
            return kust;
        }

        public static List<Transform> Own(CSteamID iD)
        {
            var bars = FindBarricades();
            var structs = FindStructures();
            var res = new List<Transform>();

            foreach (var v in bars)
            {
                BarricadeManager.tryGetInfo(v, out byte x, out byte y, out ushort plant, out ushort index, out BarricadeRegion region);
                if (region.barricades[index].owner == iD.m_SteamID)
                    res.Add(v);
            }

            foreach (var v in structs)
            {
                StructureManager.tryGetInfo(v, out byte x, out byte y, out ushort index, out StructureRegion region);
                if (region.structures[index].owner == iD.m_SteamID)
                    res.Add(v);
            }

            return res;
        }
    }
}
