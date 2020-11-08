using System;
using System.Collections.Generic;
using System.Text;

namespace TechnicalTest.Common.Models.Response
{
    public class Response<T> where T : class
    {
        public bool RealizadoCorrectamente { get; set; }

        public string Mensaje { get; set; }

        public T Resultado { get; set; }
    }
}
