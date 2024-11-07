using System;
using System.Collections.Generic;
using System.Net.Http;

namespace Docker.DotNet
{
    internal class JsonRequestContent<T> : IRequestContent
    {
        private readonly T _value;
        private readonly JsonSerializer _serializer;

        public JsonRequestContent(T val, JsonSerializer serializer)
        {
            if (EqualityComparer<T>.Default.Equals(val))
            {
                throw new ArgumentNullException(nameof(val));
            }

            if (serializer == null)
            {
                throw new ArgumentNullException(nameof(serializer));
            }

            _value = val;
            _serializer = serializer;
        }

        public HttpContent GetContent()
        {
            return _serializer.GetJsonContent(_value);
        }
    }
}