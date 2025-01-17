﻿using System;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(FPSCounter))]
public class FPSDisplay : MonoBehaviour
{
    [SerializeField] private Text averageLabel;
    [SerializeField] private Text highestLabel;
    [SerializeField] private Text lowestLabel;
    [SerializeField] private FPSColor[] fpsColor;

    private FPSCounter _fpsCounter;
    private StringBuilder _stringBuilder;
    // private string[] _stringsFrom00To199 = {
    //     "00", "01", "02", "03", "04", "05", "06", "07", "08", "09",
    //     "10", "11", "12", "13", "14", "15", "16", "17", "18", "19",
    //     "20", "21", "22", "23", "24", "25", "26", "27", "28", "29",
    //     "30", "31", "32", "33", "34", "35", "36", "37", "38", "39",
    //     "40", "41", "42", "43", "44", "45", "46", "47", "48", "49",
    //     "50", "51", "52", "53", "54", "55", "56", "57", "58", "59",
    //     "60", "61", "62", "63", "64", "65", "66", "67", "68", "69",
    //     "70", "71", "72", "73", "74", "75", "76", "77", "78", "79",
    //     "80", "81", "82", "83", "84", "85", "86", "87", "88", "89",
    //     "90", "91", "92", "93", "94", "95", "96", "97", "98", "99",
    //     "100", "101", "102", "103", "104", "105", "106", "107", "108", "109",
    //     "110", "111", "112", "113", "114", "115", "116", "117", "118", "119",
    //     "120", "121", "122", "123", "124", "125", "126", "127", "128", "129",
    //     "130", "131", "132", "133", "134", "135", "136", "137", "138", "139",
    //     "140", "141", "142", "143", "144", "145", "146", "147", "148", "149",
    //     "150", "151", "152", "153", "154", "155", "156", "157", "158", "159",
    //     "160", "161", "162", "163", "164", "165", "166", "167", "168", "169",
    //     "170", "171", "172", "173", "174", "175", "176", "177", "178", "179",
    //     "180", "181", "182", "183", "184", "185", "186", "187", "188", "189",
    //     "190", "191", "192", "193", "194", "195", "196", "197", "198", "199"
    // };


    private void Awake()
    {
        _fpsCounter = GetComponent<FPSCounter>();
        _stringBuilder = new StringBuilder(4);
    }

    private void Update()
    {
        Display(averageLabel, _fpsCounter.AverageFps);
        Display(highestLabel, _fpsCounter.HighestFps);
        Display(lowestLabel, _fpsCounter.LowestFps);
    }

    private void Display(Text label, int fps)
    {
        _stringBuilder.Clear();
        _stringBuilder.Insert(0, fps);
        label.text = _stringBuilder.ToString();
        foreach (var colorValue in fpsColor)
        {
            if (fps >= colorValue.minFPS)
            {
                label.color = colorValue.color;
                break;
            }
        }
    }
    
    [Serializable] private struct FPSColor
    {
        public Color color;
        public int minFPS;
    } 
}
