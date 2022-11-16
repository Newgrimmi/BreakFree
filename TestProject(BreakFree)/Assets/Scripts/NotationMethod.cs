using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotationMethod : MonoBehaviour
{

    public string NotationMethods(float x,string y)
    {
        if (x <= 1000) return x.ToString(y);

        var prefixes = new Dictionary<double, string>
        {
            {3," K"},
            {6," M"},
            {9," B"},
            {12," T"},
            {15," Qa"},
            {18," Qi"},
            {21," Sx"},
            {24," Sp"},
            {27," O"},
            {30," N"},
            {33," D"},
            {36," Ud"},
            {39," Dd"},
            {42," Td"},
            {45," Qad"},
            {48," Qid"},
            {51," Sxd"},
            {54," Spd"},
            {57," Od"},
            {60," Nd"},
            {63," V"},
            {66," Uv"},
            {69," Dv"},
            {72," Tv"},
        };

        var exponent = Math.Floor(Math.Log10(x));
        var thirdExponent = 3 * Math.Floor(exponent / 3);
        var mantissa = x / Math.Pow(10, thirdExponent);
        if (x <= 1000)
            return x.ToString("");
        if(x >= 1e75)
        {
            return mantissa.ToString("F2") + "e" + exponent;
        }
        return mantissa.ToString("F2") + prefixes[thirdExponent];
    }
}
