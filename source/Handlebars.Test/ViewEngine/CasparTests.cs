﻿using System.IO;
using Xunit;

namespace HandlebarsDotNet.Test.ViewEngine
{
    public class CasparTests
    {
#if !netstandard
        [Fact]
        public void CanRenderCasparIndexTemplate()
        {
            var fs = (new DiskFileSystem());
            var handlebars = HandlebarsDotNet.Handlebars.Create(new HandlebarsConfiguration()
            {
                FileSystem = fs
            });
            AddHelpers(handlebars);
            var renderView = handlebars.CompileView("ViewEngine/Casper-master/index.hbs");
            var output = renderView(new
            {
                blog = new
                {
                    url = "http://someblog.com",
                    title = "This is the blog title"
                },
                posts = new[]
                {
                    new
                    {
                        title = "My Post Title",
                        image = "/someimage.png",
                        post_class = "somepostclass"
                    }
                }
            });
            var cq = CsQuery.CQ.CreateDocument(output);
            Assert.Equal("My Post Title", cq["h2.post-title a"].Text());
        }

        [Fact]
        public void CanRenderCasparPostTemplate()
        {
            var fs = (new DiskFileSystem());
            var handlebars = HandlebarsDotNet.Handlebars.Create(new HandlebarsConfiguration()
            {
                FileSystem = fs
            });
            AddHelpers(handlebars);
            var renderView = handlebars.CompileView("ViewEngine/Casper-master/post.hbs");
            var output = renderView(new
            {
                blog = new
                {
                    url = "http://someblog.com",
                    title = "This is the blog title"
                },
                post = new
                {
                    title = "My Post Title",
                    image = "/someimage.png",
                    post_class = "somepostclass"
                }
            });
            var cq = CsQuery.CQ.CreateDocument(output);
            Assert.Equal("My Post Title", cq["h1.post-title"].Html());
        }
#endif
        private static void AddHelpers(IHandlebars handlebars)
        {
            handlebars.RegisterHelper("asset",
                (writer, root, context, arguments) => writer.Write("asset:" + string.Join("|", arguments)));
            handlebars.RegisterHelper("date",
                (writer, root, context, arguments) => writer.Write("date:" + string.Join("|", arguments)));
            handlebars.RegisterHelper("tags",
                (writer, root, context, arguments) => writer.Write("tags:" + string.Join("|", arguments)));
            handlebars.RegisterHelper("encode",
                (writer, root, context, arguments) => writer.Write("encode:" + string.Join("|", arguments)));
            handlebars.RegisterHelper("url", (writer, root, context, arguments) => writer.Write("url:" + string.Join("|", arguments)));
            handlebars.RegisterHelper("excerpt", (writer, root, context, arguments) => writer.Write("url:" + string.Join("|", arguments)));
        }
#if !netstandard
        [Fact]
        public void CanRenderCasparPostNoLayoutTemplate()
        {
            var fs = (new DiskFileSystem());
            var handlebarsConfiguration = new HandlebarsConfiguration() { FileSystem = fs };
            var handlebars = Handlebars.Create(handlebarsConfiguration);

            AddHelpers(handlebars);
            var renderView = handlebars.CompileView("ViewEngine/Casper-master/post-no-layout.hbs");
            var output = renderView(new
            {
                post = new
                {
                    title = "My Post Title",
                    image = "/someimage.png",
                    post_class = "somepostclass"
                }
            });
            var cq = CsQuery.CQ.CreateDocument(output);
            Assert.Equal("My Post Title", cq["h1.post-title"].Html());
        }

        [Fact]
        public void CanRenderCasparIndexTemplateWithStaticInstance()
        {

            Handlebars.RegisterHelper("asset", (writer, root, context, arguments) => writer.Write("asset:" + string.Join("|", arguments)));
            Handlebars.RegisterHelper("date", (writer, root, context, arguments) => writer.Write("date:" + string.Join("|", arguments)));
            Handlebars.RegisterHelper("tags", (writer, root, context, arguments) => writer.Write("tags:" + string.Join("|", arguments)));
            Handlebars.RegisterHelper("encode", (writer, root, context, arguments) => writer.Write("encode:" + string.Join("|", arguments)));
            Handlebars.RegisterHelper("url", (writer, root, context, arguments) => writer.Write("url:" + string.Join("|", arguments)));
            Handlebars.RegisterHelper("excerpt", (writer, root, context, arguments) => writer.Write("url:" + string.Join("|", arguments)));

            Handlebars.Configuration.FileSystem = new DiskFileSystem();

            var renderView = Handlebars.CompileView("ViewEngine/Casper-master/index.hbs");
            var output = renderView(new
            {
                blog = new
                {
                    url = "http://someblog.com",
                    title = "This is the blog title"
                },
                posts = new[]
                {
                    new
                    {
                        title = "My Post Title",
                        image = "/someimage.png",
                        post_class = "somepostclass"
                    }
                }
            });
            var cq = CsQuery.CQ.CreateDocument(output);
            Assert.Equal("My Post Title", cq["h2.post-title a"].Text());
        }
#endif

        class DiskFileSystem : ViewEngineFileSystem
        {
            public override string GetFileContent(string filename)
            {
                return File.ReadAllText(filename);
            }

            protected override string CombinePath(string dir, string otherFileName)
            {
                return Path.Combine(dir, otherFileName);
            }

            public override bool FileExists(string filePath)
            {
                return File.Exists(filePath);
            }
        }
    }
}
