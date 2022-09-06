namespace HoughTransform.Core;

public static class Helper
{
    public static (int x, int y, int z) FindMaximumPosition(int[,,] array)
    {
        var maximumValue = array[0, 0, 0];
        var maximumPosition = (x: 0, y: 0, z: 0);
        
        for (var x = 0; x < array.GetLength(0); x++)
        {
            for (var y = 0; y < array.GetLength(1); y++)
            {
                for (var z = 0; z < array.GetLength(2); z++)
                {
                    if (array[x, y, z] <= maximumValue) continue;
                    
                    maximumValue = array[x, y, z];
                    maximumPosition.x = x;
                    maximumPosition.y = y;
                    maximumPosition.z = z;
                }
            }
        }
        
        return maximumPosition;
    }

    public static void ClearArea(ref int[, ,] array, (int x, int y, int z) position, int threshold)
    {
        var minimumValue = FindMinimumValue(array);
        for (var x = position.x - threshold; x < position.x + threshold; x++)
        {
            for (var y = position.y - threshold; y < position.y + threshold; y++)
            {
                for (var z = 0; z < array.GetLength(2); z++)
                {
                    if (x >= 0 && x < array.GetLength(0) && y >= 0 && y < array.GetLength(1))
                    {
                        array[x, y, z] = minimumValue;
                    }
                }
            }
        }
    }
    
    private static int FindMinimumValue(int[, ,] array)
    {
        var minimumValue = array[0, 0, 0];

        for (var x = 0; x < array.GetLength(0); x++)
        {
            for (var y = 0; y < array.GetLength(1); y++)
            {
                for (var z = 0; z < array.GetLength(2); z++)
                {
                    if (array[x, y, z] < minimumValue)
                    {
                        minimumValue = array[x, y, z];
                    }
                }
            }
        }
        
        return minimumValue;
    }
}