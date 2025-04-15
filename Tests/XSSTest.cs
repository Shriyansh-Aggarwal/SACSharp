using System;
using System.Web;

namespace Tests
{
    public class XSSTest : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            // ⚠️ Vulnerable: Reflecting unsanitized user input
            string userInput = context.Request.QueryString["search"];
            context.Response.Write(userInput);

            // ✅ Safe: Encoded output (just for contrast, won't be flagged)
            string safeOutput = HttpUtility.HtmlEncode(userInput);
            context.Response.Write(safeOutput);
        }

        public bool IsReusable => false;
    }
}
