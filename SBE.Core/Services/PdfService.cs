using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using iTextSharp.tool.xml.html;
using iTextSharp.tool.xml.parser;
using iTextSharp.tool.xml.pipeline.css;
using iTextSharp.tool.xml.pipeline.end;
using iTextSharp.tool.xml.pipeline.html;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SBE.Core.Services
{
    internal sealed class PdfService
    {
        private sealed class ImageProviderImpl : AbstractImageProvider
        {
            readonly Dictionary<string, string> images = new Dictionary<string, string>();

            public override string GetImageRootPath()
            {
                return null;
            }

            public override iTextSharp.text.Image Retrieve(string src)
            {
                if (images.ContainsKey(src))
                {
                    var bytes = Convert.FromBase64String(images[src]);
                    return iTextSharp.text.Image.GetInstance(bytes);
                }

                return base.Retrieve(src);
            }

            internal void RegisterImage(string src, string base64)
            {
                images.Add(src, base64);
            }
        }

        private sealed class HeaderFooterHelper
        {
            private PdfContentByte overContent;
            private int pageNumber;
            private int numberOfPages;
            private XMLWorkerHelper xmlWorker;

            internal float HeaderHeight { get; set; }
            internal string Header { private get; set; }
            internal float FooterHeight { get; set; }
            internal string Footer { private get; set; }

            internal bool IsEmpty()
            {
                return string.IsNullOrEmpty(Header) && string.IsNullOrEmpty(Footer);
            }

            internal void Render(PdfContentByte pdfContentByte, int page, int numberOfPages, XMLWorkerHelper xmlWorker)
            {
                overContent = pdfContentByte;
                this.pageNumber = page;
                this.numberOfPages = numberOfPages;
                this.xmlWorker = xmlWorker;

                RenderHeader();
                RenderFooter();
            }

            private void RenderHeader()
            {
                var left = PageSize.A4.GetLeft(10f);
                var top = PageSize.A4.GetTop(10f);
                var right = PageSize.A4.GetRight(10f);

                var rect = new Rectangle(left, top - HeaderHeight, right, top);
                RenderHtml(Header, rect);
            }

            private void RenderFooter()
            {
                var left = PageSize.A4.GetLeft(10f);
                var bottom = PageSize.A4.GetBottom(10f);
                var right = PageSize.A4.GetRight(10f);

                var rect = new Rectangle(left, bottom, right, bottom + FooterHeight);
                RenderHtml(Footer, rect);
            }

            private void RenderHtml(string html, Rectangle rectangle)
            {
                html = html.Replace("@page@", pageNumber.ToString());
                html = html.Replace("@pages@", numberOfPages.ToString());

                if (string.IsNullOrEmpty(html))
                {
                    return;
                }

                var elementHandler = new ElementCollector();
                xmlWorker.ParseXHtml(elementHandler, new StringReader(html));

                var ct = new ColumnText(overContent);
                foreach (var e in elementHandler.Elements)
                {
                    ct.AddElement(e);
                }

                ct.SetSimpleColumn(rectangle);
                ct.Go();
            }
        }

        readonly ImageProviderImpl imageProvider = new ImageProviderImpl();
        readonly HeaderFooterHelper headerFooter = new HeaderFooterHelper();
        string css = string.Empty;

        private sealed class ElementCollector : IElementHandler
        {
            internal ElementList Elements { get; private set; } = new ElementList();

            public void Add(IWritable w)
            {
                Elements.Add(w);
            }
        }

        internal void SetHeader(float height, string xhtml)
        {
            headerFooter.HeaderHeight = height;
            headerFooter.Header = xhtml;
        }

        internal void SetFooter(float height, string xhtml)
        {
            headerFooter.FooterHeight = height;
            headerFooter.Footer = xhtml;
        }

        internal void SetCss(string css)
        {
            this.css = css;
        }

        internal byte[] RenderPdf(string xhtml)
        {
            var bytes = RenderPages(xhtml);

            if (!headerFooter.IsEmpty())
            {
                return bytes;
            }

            return StampPdfWithHeaderFooter(ref bytes);
        }

        private byte[] StampPdfWithHeaderFooter(ref byte[] bytes)
        {
            using (var output = new MemoryStream())
            using (var reader = new PdfReader(bytes))
            {
                using (var stamper = new PdfStamper(reader, output))
                {
                    var xmlWorker = XMLWorkerHelper.GetInstance();

                    for (int page = 1; page <= reader.NumberOfPages; page++)
                    {
                        headerFooter.Render(stamper.GetOverContent(page), page, reader.NumberOfPages, xmlWorker);
                    }
                }

                bytes = output.ToArray();
                return bytes;
            }
        }

        private byte[] RenderPages(string xhtml)
        {
            using (var output = new MemoryStream())
            using (var htmlStream = new MemoryStream(Encoding.UTF8.GetBytes(xhtml)))
            using (var cssStream = new MemoryStream(Encoding.UTF8.GetBytes(css)))
            using (var doc = new Document(PageSize.A4, 10f, 10f, 10f + headerFooter.HeaderHeight, 10f + headerFooter.FooterHeight))
            using (var writer = PdfWriter.GetInstance(doc, output))
            {
                doc.Open();

                XMLWorker worker = CreateXmlWorker(doc, writer);
                var p = new XMLParser(worker);
                p.Parse(new StringReader(xhtml));

                doc.Close();
                return output.ToArray();
            }
        }

        private XMLWorker CreateXmlWorker(Document doc, PdfWriter writer)
        {
            var cssResolver = XMLWorkerHelper.GetInstance().GetDefaultCssResolver(true);
            cssResolver.AddCss(content: css, isPersistent: true);

            var htmlContext = new HtmlPipelineContext(null);
            htmlContext.SetTagFactory(Tags.GetHtmlTagProcessorFactory());
            htmlContext.SetImageProvider(imageProvider);

            var pdfPipeline = new PdfWriterPipeline(doc, writer);
            var htmlPipeline = new HtmlPipeline(htmlContext, pdfPipeline);
            var cssPipeline = new CssResolverPipeline(cssResolver, htmlPipeline);

            var worker = new XMLWorker(cssPipeline, true);
            return worker;
        }

        internal void RegisterImage(string src, string base64)
        {
            imageProvider.RegisterImage(src, base64);
        }
    }
}
