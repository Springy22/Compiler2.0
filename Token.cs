using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Compilador
{
    public class Token
    {
        public enum Tipos
        {
            ST,
            SNT,
            Flecha,
            FinProduccion,
            Epsilon,
            Or,
            Derecho,
            Izquierdo,
            Tipo
        };

        private string contenido;
        private Tipos clasificacion;

        public Token()
        {
            contenido = "";
        }

        public string Contenido
        {
            get => contenido;
            set => contenido = value;
        }

        public Tipos Clasificacion
        {
            get => clasificacion;
            set => clasificacion = value;
        }
    }

    class TestHiding
    {
        public static void Test()
        {
            Token t = new Token();

            // Asignando y mostrando la propiedad de la clase base.
            t.Contenido = "Valor Base";

            Console.WriteLine("Contenido en la clase base: {0}", t.Contenido);
        }
    }
}

        /*public Token()
        {
            contenido = "";
        }
        public void setContenido(string contenido)
        {
            this.contenido = contenido;
        }
        public void setClasificacion(Tipos clasificacion)
        {
            this.clasificacion = clasificacion;
        }
        public string getContenido()
        {
            return this.contenido;
        }
        public Tipos getClasificacion()
        {
            return this.clasificacion;
        }*/