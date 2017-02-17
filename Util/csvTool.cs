using System.Data;
using System.IO;
using System.Linq;

namespace Signal_viewer.Util
{
    class csvTool
    {
        public static DataTable CsvToDataTable(string File, string TableName, string delimiter)
        {
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            StreamReader s = new StreamReader(File, System.Text.Encoding.Default);
            //string ss = s.ReadLine();//skip the first line
            string[] columns = s.ReadLine().Split(delimiter.ToCharArray());
            ds.Tables.Add(TableName);
            foreach (string col in columns)
            {
                bool added = false;
                string next = "";
                int i = 0;
                while (!added)
                {
                    string columnname = col + next;
                    columnname = columnname.Replace("#", "");
                    columnname = columnname.Replace("'", "");
                    columnname = columnname.Replace("&", "");
                    columnname = columnname.Replace(" ", "");

                    if (!ds.Tables[TableName].Columns.Contains(columnname))
                    {
                        ds.Tables[TableName].Columns.Add(columnname.ToUpper(), typeof(int));
                        added = true;
                    }
                    else
                    {
                        i++;
                        next = "_" + i.ToString();
                    }
                }
            }

            string AllData = s.ReadToEnd();
            string[] rows = AllData.Split("\n".ToCharArray());

            /* handle exception case */
            string[] item0 = rows[0].Split(delimiter.ToCharArray());
            string colStr = "Column";
            int colNum = ds.Tables[TableName].Columns.Count;
            while(ds.Tables[TableName].Columns.Count < item0.Count())
            {
                ds.Tables[TableName].Columns.Add(colStr + colNum.ToString(), typeof(int));
                colNum++;
            }

            /* Load row data to table */
            foreach (string r in rows)
            {
                string[] items = r.Split(delimiter.ToCharArray());
                for(int i =0; i<items.Count();i++)
                {
                    if (items[i] == "")
                        items[i] = "0";
                }
                int[] numbers = items.Select(int.Parse).ToArray();
                ds.Tables[TableName].Rows.Add(items);
            }
  

            s.Close();

            dt = ds.Tables[0];

            return dt;
        }
    }
}
