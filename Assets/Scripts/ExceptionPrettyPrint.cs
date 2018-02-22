namespace AssemblyCSharp
{
    using System;
    using System.Text;
    using UnityEngine;

    public static class ExceptionPrettyPrint
    {
        public static string Msg(Exception ex)
        {
            Debug.Log("Exception " + ex);
            var msg = new StringBuilder(ex.Message);
            while (ex.InnerException != null)
            {
                ex = ex.InnerException;
                msg.Append(" " + ex.Message);
            }
            return msg.ToString();
        }
    }
}