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
    }
}
