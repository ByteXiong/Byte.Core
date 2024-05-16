namespace Byte.Core.Common.Helpers
{
    public static class MaskHelper
    {
        public static int[] GetMaskPositions(int mask)
        {
            var positions = new List<int>();
            for (int i = 0; i < 32; i++)
            {
                if ((mask & (1 << i)) != 0)
                {
                    positions.Add(i);
                }
            }
            return positions.ToArray();
        }

        public static int SetMaskPositions(int[] positions)
        {
            int mask = 0;
            foreach (var position in positions)
            {
                mask |= 1 << position;
            }
            return mask;
        }
    }
}
