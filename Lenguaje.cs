using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

/*
    Requerimiento 1: Solo la primera produccion es publica, el resto es privada
    Requerimiento 2: Implementar la cerradura Epsilon
    Requerimiento 3: Implementar el operador OR
    Requerimento 4 : Indentar el código
*/
namespace Compilador
{
    public class Lenguaje : Sintaxis
    {
        private bool publicBandera = true;
        private List<string> listaEpsilons = new List<string>();
        private List<string> lenguajeEpsilon = new List<string>();
        private bool banderaEpsilon = false;
        private String space="  ";
        private int saltosLinea = 6;

        public Lenguaje()
        {
        }
        public Lenguaje(string nombre) : base(nombre)
        {
        }
        private void esqueleto(string nspace)
        {
            ImprimePantalla(0,"using System;");
            //lenguajecs.WriteLine("using System;");
            ImprimePantalla(0,"using System.Collections.Generic;");
            ImprimePantalla(0,"using System.Linq;");
            ImprimePantalla(0,"using System.Net.Http.Headers;");
            ImprimePantalla(0,"using System.Reflection.Metadata.Ecma335;");
            ImprimePantalla(0,"using System.Runtime.InteropServices;");
            ImprimePantalla(0,"using System.Threading.Tasks;");
            ImprimePantalla(0,"");
            ImprimePantalla(0,"namespace " + nspace);
            ImprimePantalla(0,"{");
            ImprimePantalla(2,"public class Lenguaje : Sintaxis");
            ImprimePantalla(2,"{");
            ImprimePantalla(4,"public Lenguaje()");
            ImprimePantalla(4,"{");
            ImprimePantalla(4,"}");
            ImprimePantalla(4,"");
            ImprimePantalla(4,"public Lenguaje(string nombre) : base(nombre)");
            ImprimePantalla(4,"{");
            ImprimePantalla(4,"}");
            ImprimePantalla(4,"");
        }
        public void genera()
        {
            match("namespace");
            match(":");
            esqueleto(Contenido);
            if (Clasificacion == Tipos.SNT)
            {
                match(Tipos.SNT);
            }
            else
            {
                match(Tipos.ST);
            }
            match(";");
            producciones();
            ImprimePantalla(2,"}");
            ImprimePantalla(0,"}");
        }

        private void producciones()
        {
            if (publicBandera)
            {
                if (Clasificacion == Tipos.SNT)
                {
                    ImprimePantalla(4,"public void "+Contenido+"()");
                    ImprimePantalla(4,"{");
                    publicBandera = false;
                }
            }
            else
            {
                if (Clasificacion == Tipos.SNT)
                {
                    ImprimePantalla(4,"private void "+Contenido+"()");
                    ImprimePantalla(4,"{");
                }
            }

            match(Tipos.SNT);
            match(Tipos.Flecha);
            conjuntoTokens();
            saltosLinea=6;
            match(Tipos.FinProduccion);
            if (banderaEpsilon)
            {
                Console.WriteLine("Hay un epsilon");
            }
            ImprimePantalla(4,"}");
            ImprimePantalla(0,"");
            banderaEpsilon = false;
            if (Clasificacion == Tipos.SNT)
            {
                producciones();
            }
        }

        private void conjuntoTokens()
        {
            if (Clasificacion == Tipos.SNT)
            {
                ImprimePantalla(saltosLinea,Contenido+"();");
                match(Tipos.SNT);
            }
            else if (Clasificacion == Tipos.ST)
            {
                if (Contenido == "?")
                {
                    //Console.WriteLine(beforeToken);
                    match(Tipos.ST);
                    banderaEpsilon = true;
                    Contenido = beforeToken;
                    //Console.WriteLine(Contenido);
                    //lenguajecs.WriteLine("              if("+Contenido+")");

                }
                else
                {
                    ImprimePantalla(saltosLinea,"match(\""+Contenido+"\");");
                    Console.WriteLine(Contenido);
                    match(Tipos.ST);
                }
            }
            else if (Clasificacion == Tipos.Tipo)
            {
                ImprimePantalla(6,"match(Tipos."+Contenido+");");
                match(Tipos.Tipo);
            }
            else if (Clasificacion == Tipos.Or)
            {
                Console.WriteLine(Clasificacion);
                bool primerCaso = true;  // Bandera para determinar si es el primer caso
                bool contieneOr = false; // Bandera para verificar si existe un operador OR

                // Procesa los tokens mientras no se alcance el fin de la producción
                while (Clasificacion != Tipos.FinProduccion)
                {
                    // Si encontramos un operador OR, significa que se ha encontrado una nueva opción
                    if (Clasificacion == Tipos.Or)
                    {
                        contieneOr = true; // Marca que existe un OR
                        match(Tipos.Or); // Consume el operador OR
                    }
                    else if (Clasificacion == Tipos.ST || Clasificacion == Tipos.SNT)
                    {
                        // Si es el primer caso después de OR, genera un else if
                        if (!primerCaso)
                        {
                            ImprimePantalla(6,"else if (getContenido() == \"");
                        }
                        else
                        {
                            // Si es el primer caso de la regla (antes de OR), genera un if
                            ImprimePantalla(saltosLinea,"if (getContenido() == \"");
                            primerCaso = false; // Cambia la bandera para siguientes tokens
                        }
                        // Escribe el contenido del token actual
                        ImprimePantalla(4, Contenido + "\")");
                        ImprimePantalla(4, "{");
                        // Dependiendo del tipo de token, genera la acción asociada
                        if (Clasificacion == Tipos.ST)
                        {
                            ImprimePantalla(4,"                match(\"" + Contenido + "\");");
                            match(Tipos.ST);
                        }
                        else if (Clasificacion == Tipos.SNT)
                        {
                            ImprimePantalla(6,Contenido + "();");                           
                            match(Tipos.SNT);
                        }
                        ImprimePantalla(6,"}");
                    }
                }
                // Si contiene OR, se genera un bloque `else` al final
                if (contieneOr)
                {
                    ImprimePantalla(6,"else");
                    ImprimePantalla(6,"{");
                    ImprimePantalla(6,"// Acción por defecto");
                    ImprimePantalla(6,"}");
                }
            }
            else if (Clasificacion == Tipos.Izquierdo)
            {
                match(Tipos.Izquierdo);
                ImprimeSinEspacio(saltosLinea,"if(");
                saltosLinea++;
                saltosLinea++;
                if (Clasificacion == Tipos.ST)
                {
                    ImprimePantalla(0,"getContenido = \"" + Contenido + "\")");
                    ImprimePantalla(saltosLinea-2,"{");
                    ImprimePantalla(saltosLinea,"match(\"" + Contenido + "\");");
                    match(Tipos.ST);
                }
                else if (Clasificacion == Tipos.Tipo)
                {
                    ImprimePantalla(0,"getContenido = \"" + Contenido + "\")");
                    ImprimePantalla(saltosLinea-2,"{");
                    ImprimePantalla(saltosLinea,"match(Tipos." + Contenido + ");");
                    match(Tipos.Tipo);
                }
                if (Contenido == "|")
                {
                    ImprimePantalla(saltosLinea-2,"}");
                    bool primerCaso = true;  // Bandera para determinar si es el primer caso
                    bool contieneOr = false; // Bandera para verificar si existe un operador OR
                    // Procesa los tokens mientras no se alcance el fin de la producción
                    while (Clasificacion != Tipos.FinProduccion)
                    {
                        // Si encontramos un operador OR, significa que se ha encontrado una nueva opción
                        if (Contenido == "|")
                        {
                            contieneOr = true; // Marca que existe un OR
                            match(Tipos.Or); // Consume el operador OR
                        }
                        else if (Clasificacion == Tipos.ST || Clasificacion == Tipos.SNT)
                        {
                            // Si es el primer caso después de OR, genera un else if
                            if (!primerCaso)
                            {
                                ImprimePantalla(saltosLinea-2,"else");
                            }
                            else
                            {
                                // Si es el primer caso de la regla (antes de OR), genera un if
                                ImprimePantalla(saltosLinea-2,"else if (AquigetContenido() == \""+Contenido+"\")");
                                primerCaso = false; // Cambia la bandera para siguientes tokens
                                //ImprimePantalla(6,"{");
                            }
                            // Escribe el contenido del token actual          
                            ImprimePantalla(saltosLinea-2,"{");
                            // Dependiendo del tipo de token, genera la acción asociada
                            if (Clasificacion == Tipos.ST)
                            {
                                ImprimePantalla(saltosLinea,"match(\"" + Contenido + "\");");
                                match(Tipos.ST);
                            }
                            else if (Clasificacion == Tipos.SNT)
                            {
                                ImprimePantalla(saltosLinea, Contenido + "();");
                                match(Tipos.SNT);
                            }
                            ImprimePantalla(saltosLinea-2,"}");
                        }
                        else{
                            match(Tipos.Derecho);
                        }
                    }
                }
            }
            else if(Clasificacion==Tipos.Derecho){
                match(Tipos.Derecho);
                ImprimePantalla(saltosLinea-2,"}");
                saltosLinea--;
                saltosLinea--;
            }
            else if(Clasificacion==Tipos.Epsilon){
                nextToken();
            }
            if (Clasificacion != Tipos.FinProduccion)
            {
                conjuntoTokens();
            }
        }

        private void ImprimePantalla(int repeticiones, string texto){
            for(int i = 0; i < repeticiones; i++){
                lenguajecs.Write(space);
            }
            lenguajecs.WriteLine(texto);
        }

        private void ImprimeSinEspacio(int repeticiones, string texto){
            for(int i = 0; i < repeticiones; i++){
                lenguajecs.Write(space);
            }
            lenguajecs.Write(texto);
        }
    }
}