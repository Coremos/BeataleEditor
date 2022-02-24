using Beatale.ChartSystem;
using System.Linq;
using UnityEngine;

namespace Beatale.TunnelSystem
{
    public class TunnelManager : MonoBehaviour
    {
        public float BPM;
        public Chart Chart;
        public float PlayTime;
        public MusicManager MusicManager;

        private bool isMove;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A)) isMove = !isMove;

            if (!isMove) return;
            PlayTime += Time.deltaTime;
        }

        public void GetFractionTime()
        {
            int position = 0, numerator = 0, denominator = 0;
            var BPMChanges = Chart.BPMChanges.Where(x => x.BPM > 10);
        }

        public float CalculateFloatTime(int position, int numerator, int denominator, float bpm)
        {
            var oneBeatTime = 60.0f / bpm;
            float time = position * oneBeatTime + (numerator / denominator);
            return time;
        }

        public void CalculateFractionTime(float time, float bpm, float division, out int position, out int numerator, out int denominator)
        {
            var oneBeatTime = 60.0f / bpm;
            var quntitizeTime = oneBeatTime / division;

            var index = time / oneBeatTime; // 몇 번째인지 계산됨
            position = (int)index;
            var difference = index - position;
            numerator = (int)(difference * oneBeatTime / quntitizeTime);
            denominator = (int)division;
        }
    }
}
