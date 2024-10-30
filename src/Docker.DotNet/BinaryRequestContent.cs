using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Docker.DotNet
{
    internal class BinaryRequestContent : IRequestContent
    {
        private readonly Stream _stream;
        private readonly string _mimeType;

        public BinaryRequestContent(Stream stream, string mimeType)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            if (string.IsNullOrEmpty(mimeType))
            {
                throw new ArgumentNullException(nameof(mimeType));
            }

            _stream = stream;
            _mimeType = mimeType;
        }

        public HttpContent GetContent()
        {
            var data = new StreamContent(_stream);
            data.Headers.ContentType = new MediaTypeHeaderValue(_mimeType);
            return data;
        }
    }
}