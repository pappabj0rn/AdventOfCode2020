using System;

namespace Core
{
    public class SeatingCalculator
    {
        public (int row, int seat, int seatId) CalculateSeatNo(string binaryInput)
        {
            binaryInput = binaryInput
                .Replace('F', '0')
                .Replace('L', '0')
                .Replace('B', '1')
                .Replace('R', '1');
            
            var row = Convert.ToInt32(binaryInput.Substring(0, 7),2);
            var seat = Convert.ToInt32(binaryInput.Substring(7),2);

            return (row, seat, row * 8 + seat);
        }
    }
}