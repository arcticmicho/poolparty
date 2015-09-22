using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;

public class EnumWriterTool {

	public static bool WriteEnumToFile(string filePath, string[] enumKeys, string enumName)
    {
        try
        {   
            FileStream enumFile = new FileStream(filePath, FileMode.Truncate);

            StringBuilder builder = new StringBuilder();
            builder.Append(string.Format("public enum {0} \n", enumName));
            builder.Append("{ \n");
            for(int i=0; i<enumKeys.Length; i++)
            {
                builder.Append(string.Format("{0} = {1}, \n", enumKeys[i], i));
            }
            builder.Append("} \n");

            StreamWriter enumWriter = new StreamWriter(enumFile);
            enumWriter.Write(builder.ToString());
            enumWriter.Flush();
            enumWriter.Close();
            enumFile.Close();
        }
        catch (System.Exception e)
        {            
            throw e;
        }
        return false;
    }
}
