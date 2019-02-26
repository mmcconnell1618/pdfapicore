using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PuppeteerSharp;

namespace PdfApiCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PdfController : ControllerBase
    {

        [HttpGet]
        public async Task<IActionResult> IndexAsync()
        {
            await new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultRevision);
            var browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = true
            });
            //var page = await browser.NewPageAsync();
            //await page.GoToAsync("http://www.google.com");

            FileStreamResult result;

            using (var page = await browser.NewPageAsync())
            {
                string html = @"<div style=""display:flex; flex-direction:row; justify-content:center;""><div style=""border:solid 1px red; margin:2em;padding:2em;background:azure;"">First</div><div style=""border:solid 1px red; margin:2em;padding:2em;background:azure;"">Second</div><div style=""border:solid 1px red; margin:2em;padding:2em;background:azure;"">Third</div></div>";
                await page.SetContentAsync(html);
                //var pageresult = await page.GetContentAsync();
                var stream = await page.PdfStreamAsync();
                result = new FileStreamResult(stream, "application/pdf");
            }

            //await page.PdfAsync(outputFile);

            return result;
        }
    }
}