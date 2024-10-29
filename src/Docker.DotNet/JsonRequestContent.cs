using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Docker.DotNet
{
    internal class JsonRequestContent<T> : IRequestContent
    {
        private const string JsonMimeType = "application/json";

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
            var serializedObject = _serializer.SerializeObject(_value);
            return new StringContent(serializedObject, Encoding.UTF8, JsonMimeType);
        }
    }
}