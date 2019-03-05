using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.NodeServices;
using System.Runtime.InteropServices;
using Microsoft.Extensions.Logging;

namespace DatingApp.API.Controllers {

   
    [Route("api/[controller]")]
    [ApiController]
    // contollerBase implemetuje bez View[], controller z View[]
    public class HomeController: ControllerBase  {
         // Insert correct filePath
    //     [DllImport(@"E:\Projekty c#\WindowsNativeCpp\x64\Release\WindowsNativeCpp.dll", EntryPoint = 
    //    "math_add", CallingConvention = CallingConvention.StdCall)]
        [DllImport(@"NodeModule\WindowsNativeCpp.dll", EntryPoint = 
        "math_add", CallingConvention = CallingConvention.StdCall)]
        public static extern int Add(int a, int b);
        // GET api/Home
        [HttpGet("Adds")]
        public async Task < IActionResult > Adds([FromServices] INodeServices nodeServices) {
                var num1 = 10;
                var num2 = 20;
                var result = await nodeServices.InvokeAsync < int > ("NodeModule\\AddModule.js", num1, num2);
                string v = $"Result of {num1} + {num2} is {result}";
                Console.WriteLine(v);
                var result1 = Add(1 ,2);
                Console.WriteLine("result of C++ is {0}", result1);
                // ViewData["ResultFromNode"] = v;
                return Ok(v);
            }
            [HttpGet("qr")]
        public async Task < IActionResult > QR([FromServices] INodeServices nodeServices, string text = "42") {
            var data = await nodeServices.InvokeAsync < byte[] > ("NodeModule/QR.js", text);
            return File(data, "image/png");
        }

        [HttpGet("Preview")]
        public async Task < IActionResult > GenerateUrlPreview([FromServices] INodeServices nodeServices) {
            var url = Request.Form["Url"].ToString();
            var fileName = System.IO.Path.ChangeExtension(DateTime.UtcNow.Ticks.ToString(), "png");
            Console.WriteLine($"({DateTime.UtcNow.ToLongTimeString()}), plik: {fileName}, url: {url}");
            var file = await nodeServices.InvokeAsync < string > ("NodeModule\\UrlPreviewModule.js", url, System.IO.Path.Combine("PreviewImages", fileName));

            return Content($"<a class=\"btn btn-default\" target=\"_blank\" href=\"/Home/Download?img={fileName}\">Download image</a>");
        }

    }
}