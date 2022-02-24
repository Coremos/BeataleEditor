using System;
using System.Collections.Generic;
using UnityEngine;

namespace Beatale.Route.Curve
{
    public class CurveSample
    {
        public float Length;
        public List<RouteSample> Samples;

        public CurveSample()
        {
            Samples = new List<RouteSample>();
        }

        public RouteSample GetRouteSample(float distance)
        {
            for (int index = 1; index < Samples.Count; index++)
            {
                if (distance < Samples[index].Distance)
                {
                    float t = (distance - Samples[index - 1].Distance) / (Samples[index].Distance - Samples[index - 1].Distance);
                    return RouteSample.Lerp(Samples[index - 1], Samples[index], t);
                }
            }
            throw new Exception("Can't find RouteSample");
        }

        public void Add(RouteSample sample)
        {
            Samples.Add(sample);
        }
    }
}
