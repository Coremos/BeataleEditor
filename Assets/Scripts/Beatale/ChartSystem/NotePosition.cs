namespace Beatale.ChartSystem
{
    public struct NotePosition
    {
        public int Bar;
        public int Numerator;
        public int Denominator;
        public double Time;

        public static NotePosition operator -(NotePosition position1, NotePosition position2)
        {
            var position = new NotePosition();
            position.Bar = position1.Bar - position2.Bar;
            if (position1.Denominator == 0 && position2.Denominator == 0)
            {
                position.Numerator = 0;
                position.Denominator = 0;
            }
            else
            {
                var numerator1 = position1.Numerator;
                var numerator2 = position2.Numerator;

                position.Denominator = (position2.Denominator == 0) ? 1 : position2.Denominator;

                if (position2.Denominator != position1.Denominator)
                {
                    numerator1 *= (position2.Denominator == 0) ? 1 : position2.Denominator;
                    numerator2 *= (position1.Denominator == 0) ? 1 : position1.Denominator;
                    position.Denominator *= (position1.Denominator == 0) ? 1 : position1.Denominator;
                }

                position.Numerator = numerator1 - numerator2;
            }
            return position;
        }

        public static bool operator >(NotePosition position1, NotePosition position2)
        {
            if (position1.Bar >= position2.Bar)
            {
                if (position1.Bar > position2.Bar) return true;
                else
                {
                    var difference = position1 - position2;
                    return difference.Numerator > 0;
                }
            }
            return false;
        }

        public static bool operator <(NotePosition position1, NotePosition position2)
        {
            if (position1.Bar <= position2.Bar)
            {
                if (position1.Bar < position2.Bar) return true;
                else
                {
                    var difference = position1 - position2;
                    return difference.Numerator < 0;
                }
            }
            return false;
        }

        public static bool operator >=(NotePosition position1, NotePosition position2)
        {
            if (position1.Bar >= position2.Bar)
            {
                if (position1.Bar > position2.Bar) return true;
                else
                {
                    var difference = position1 - position2;
                    return difference.Numerator >= 0;
                }
            }
            return false;
        }

        public static bool operator <=(NotePosition position1, NotePosition position2)
        {
            if (position1.Bar <= position2.Bar)
            {
                if (position1.Bar < position2.Bar) return true;
                else
                {
                    var difference = position1 - position2;
                    return difference.Numerator <= 0;
                }
            }
            return false;
        }


    }

}
