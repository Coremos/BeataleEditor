using Beatale.ChartSystem;
using System.Collections.Generic;
using UnityEngine;

namespace Beatale.JudgeSystem
{
    public class Judge : MonoBehaviour
    {
        static readonly float MISS_TIME = 10.0f;
        static readonly float GOOD_TIME = 2.0f;
        static readonly float GREAT_TIME = 0.0f;

        private Queue<JudgeObject> judgeQueue;
        private Chart chart;
        private double currentTime;

        public void OnDownInput(float degree, float width, double time)
        {

        }

        public void OnInput(float degree, float width, double time)
        {

        }
    }
}
