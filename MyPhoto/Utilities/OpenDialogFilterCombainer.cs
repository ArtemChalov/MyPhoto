using System.Collections.Generic;
using System.Text;

namespace MyPhoto.Utilities
{
    static class OpenDialogFilterCombainer
    {
        public static string CombainFilter(Dictionary<string, string[]> filters)
        {
            StringBuilder builder = new StringBuilder(200);

            foreach (var item in filters)
            {
                var listOfExt = filters[item.Key];
                builder.Append(item.Key);
                builder.Append('|');

                for (int i = 0; i < listOfExt.Length; i++)
                {
                    builder.Append('*');
                    builder.Append('.');
                    builder.Append(listOfExt[i]);
                    builder.Append(';');
                }
                if (builder.Length > 2)
                    builder.Remove(builder.Length - 1, 1);
                builder.Append('|');
            }
            if (builder.Length > 2)
                builder.Remove(builder.Length - 3, 3);

            if (builder.Length > 0) return builder.ToString();
            return null;
        }
    }
}
