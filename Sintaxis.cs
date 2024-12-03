using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Compilador
{
    public class Sintaxis : Lexico
    {
        public String beforeToken = "";
        public Sintaxis()
        {
            nextToken();
        }
        public Sintaxis(string nombre) : base(nombre)
        {
            nextToken();
        }
        public void match(string espera)
        {
            if (Contenido == espera)
            {
                beforeToken = espera;
                nextToken();
            }
            else
            {
                throw new Error("Sintaxis: se espera un " + espera + " (" + Contenido + ")" + "en la linea: " + linea, log);
            }
        }
        public void match(Tipos espera)
        {
            if (Clasificacion == espera)
            {
                beforeToken = Contenido;   
                nextToken();
            }
            else
            {
                throw new Error("Sintaxis: se espera un " + espera + " (" + Contenido + ")" + "en la linea: " + linea, log);
            }
        }

        /*
        private int contadorLinea(){
            int laine = new Lexico().linea12;
            laine=laine-10;
            int diferencia;
            if((laine-13)!=0){
                diferencia=(laine-13)/2;
                laine=laine+(-diferencia);
            }
            return laine;
        }
        */
    }
}