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

        public Lenguaje()
        {
        }
        public Lenguaje(string nombre) : base(nombre)
        {
        }
        private void esqueleto(string nspace)
        {
            lenguajecs.WriteLine("using System;");
            lenguajecs.WriteLine("using System.Collections.Generic;");
            lenguajecs.WriteLine("using System.Linq;");
            lenguajecs.WriteLine("using System.Net.Http.Headers;");
            lenguajecs.WriteLine("using System.Reflection.Metadata.Ecma335;");
            lenguajecs.WriteLine("using System.Runtime.InteropServices;");
            lenguajecs.WriteLine("using System.Threading.Tasks;");
            lenguajecs.WriteLine("\nnamespace " + nspace);
            lenguajecs.WriteLine("{");
            lenguajecs.WriteLine("    public class Lenguaje : Sintaxis");
            lenguajecs.WriteLine("    {");
            lenguajecs.WriteLine("        public Lenguaje()");
            lenguajecs.WriteLine("        {");
            lenguajecs.WriteLine("        }");
            lenguajecs.WriteLine("");
            lenguajecs.WriteLine("        public Lenguaje(string nombre) : base(nombre)");
            lenguajecs.WriteLine("        {");
            lenguajecs.WriteLine("        }");
            lenguajecs.WriteLine("");
            //lenguajecs.WriteLine("        public void ");
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
            lenguajecs.WriteLine("    }");
            lenguajecs.WriteLine("}");
        }

        private void producciones()
        {
            if (publicBandera)
            {
                if (Clasificacion == Tipos.SNT)
                {
                    lenguajecs.WriteLine("        public void " + Contenido + " ()");
                    lenguajecs.WriteLine("        {");
                    publicBandera = false;
                }
            }
            else
            {
                if (Clasificacion == Tipos.SNT)
                {
                    lenguajecs.WriteLine("        private void " + Contenido + " ()");
                    lenguajecs.WriteLine("        {");
                }
            }

            match(Tipos.SNT);
            match(Tipos.Flecha);
            conjuntoTokens();
            match(Tipos.FinProduccion);
            if (banderaEpsilon)
            {
                Console.WriteLine("Hay un epsilon");
            }
            lenguajecs.WriteLine("        }");
            lenguajecs.WriteLine("");
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
                lenguajecs.WriteLine("            " + Contenido + "();");
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
                    lenguajecs.WriteLine("              match(\"" + Contenido + "\");");
                    match(Tipos.ST);
                }
            }
            else if (Clasificacion == Tipos.Tipo)
            {
                lenguajecs.WriteLine("              match(Tipos." + Contenido + ");");
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
                            lenguajecs.Write("            else if (getContenido() == \"");
                        }
                        else
                        {
                            // Si es el primer caso de la regla (antes de OR), genera un if
                            lenguajecs.Write("            if (getContenido() == \"");
                            primerCaso = false; // Cambia la bandera para siguientes tokens
                        }
                        // Escribe el contenido del token actual
                        lenguajecs.Write(Contenido + "\")");
                        lenguajecs.WriteLine("{");
                        // Dependiendo del tipo de token, genera la acción asociada
                        if (Clasificacion == Tipos.ST)
                        {
                            lenguajecs.WriteLine("                match(\"" + Contenido + "\");");
                            match(Tipos.ST);
                        }
                        else if (Clasificacion == Tipos.SNT)
                        {
                            lenguajecs.WriteLine("                " + Contenido + "();");
                            match(Tipos.SNT);
                        }
                        lenguajecs.WriteLine("            }");
                    }
                }
                // Si contiene OR, se genera un bloque `else` al final
                if (contieneOr)
                {
                    lenguajecs.WriteLine("            else");
                    lenguajecs.WriteLine("            {");
                    lenguajecs.WriteLine("                // Acción por defecto");
                    lenguajecs.WriteLine("            }");
                }
            }
            else if (Clasificacion == Tipos.Izquierdo)
            {
                match(Tipos.Izquierdo);
                lenguajecs.Write("            if(");
                if (Clasificacion == Tipos.ST)
                {
                    lenguajecs.WriteLine("Contenido = \"" + Contenido + "\")");
                    lenguajecs.WriteLine("            {");
                    lenguajecs.WriteLine("                match(\"" + Contenido + "\");");
                    match(Tipos.ST);
                }
                else if (Clasificacion == Tipos.Tipo)
                {
                    lenguajecs.WriteLine("Clasificacion == Tipos." + Contenido + ")");
                    lenguajecs.WriteLine("           {");
                    lenguajecs.WriteLine("                match(Tipos." + Contenido + ");");
                    match(Tipos.Tipo);
                }
                if (Contenido == "|")
                {
                    lenguajecs.WriteLine("            }");
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
                                lenguajecs.Write("            else");
                            }
                            else
                            {
                                // Si es el primer caso de la regla (antes de OR), genera un if
                                lenguajecs.Write("            else if (getContenido() == \"");
                                primerCaso = false; // Cambia la bandera para siguientes tokens
                                lenguajecs.Write(Contenido + "\")");
                            }
                            // Escribe el contenido del token actual          
                            lenguajecs.WriteLine("{");
                            // Dependiendo del tipo de token, genera la acción asociada
                            if (Clasificacion == Tipos.ST)
                            {
                                lenguajecs.WriteLine("                match(\"" + Contenido + "\");");
                                match(Tipos.ST);
                            }
                            else if (Clasificacion == Tipos.SNT)
                            {
                                lenguajecs.WriteLine("                " + Contenido + "();");
                                match(Tipos.SNT);
                            }
                            lenguajecs.WriteLine("            }");
                        }
                        else{
                            match(Tipos.Derecho);
                        }
                    }
                }
            }
            else if(Clasificacion==Tipos.Derecho){
                match(Tipos.Derecho);
                lenguajecs.WriteLine("            }");
            }
            else if(Clasificacion==Tipos.Epsilon){
                nextToken();
            }
            if (Clasificacion != Tipos.FinProduccion)
            {
                
                conjuntoTokens();
            }
        }
    }
}