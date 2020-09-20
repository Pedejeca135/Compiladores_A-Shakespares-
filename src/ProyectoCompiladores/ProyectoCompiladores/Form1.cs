using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace ProyectoCompiladores
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        #region tab_PreparacionDelProyecto
        private void BT_SubirArchivoPrep_Click(object sender, EventArgs e)
        {
            OpenFileDialog VentanaCargaArchivo = new OpenFileDialog();
            VentanaCargaArchivo.InitialDirectory = "c:\\";
            VentanaCargaArchivo.Filter = "Archivo de Texto (*.txt)|*.txt";
            if (VentanaCargaArchivo.ShowDialog() == DialogResult.OK)
            {
                string CadenaAux = "";
                string CadenaTextBox = "";
                string RutaArchivo = VentanaCargaArchivo.FileName;
                MessageBox.Show("Ruta Archivo :" + RutaArchivo);
                StreamReader LectorArchivo = new StreamReader(RutaArchivo);
                while ((CadenaAux = LectorArchivo.ReadLine()) != null)
                {
                    CadenaAux += "\n";
                    CadenaTextBox += CadenaAux;
                }
                TB_SubirArchivo.Text = CadenaTextBox;
            }
        }

        private void BT_LimpiarTXPrep_Click(object sender, EventArgs e)
        {
            TB_SubirArchivo.Text = "";
        }
        #endregion

        #region tab_1Avance
        /*

                EVALUACIÓN DE EXPRESIONES EN NOTACIÓN POSFIJA.

                1. Inicializar una pila.
                2. Apuntar al primer carácter de la expresión posfija.
                while ( no ocurra un error && no sea fin de la expresión posfija )
                {
                    switch ( carácter )
                    {
                        Operando:
                        Insertar en la pila;
                        break;
                        Operador:
                        if ( si es “unario”)
                        {
                            Extraer el valor del tope de la pila y aplicar el operador.
                            (Se produce un error en caso de no tener valor).
                            Insertar el resultado en el nuevo tope de la pila.
                        }
                        else ( si es “binario”)
                        {
                            Extraer los 2 valores del tope de la pila y aplicar el operador.
                            (Se produce un error en caso de no tener los 2 valores).
                            Insertar el resultado en el nuevo tope de la pila.
                        }
                        break;
                    }
                    Apuntar al siguiente carácter de la expresión posfija.
                }
        */

        static String alfabeto = "abcdefghijklmnñopqrstuvxyz0123456789";
        static String op_Presedecia1 = "*+?";//jerarquia 1
        static String op_Presedecia2 = "&";//jerarquia 2
        static String op_Presedecia3 = "|";//jerarquia 3

       // static List<String> operadores = new List<String>(op_Presedecia1,op_Presedecia2,op_Presedecia3);

        static String op = op_Presedecia1 + op_Presedecia2 + op_Presedecia3;

        private void BT_SubirArchivo1_Click(object sender, EventArgs e)
        {
            OpenFileDialog VentanaCargaArchivo = new OpenFileDialog();
            VentanaCargaArchivo.InitialDirectory = "c:\\";
            VentanaCargaArchivo.Filter = "Archivo de Texto (*.txt)|*.txt";
            if (VentanaCargaArchivo.ShowDialog() == DialogResult.OK)
            {
                string CadenaAux = "";
                string CadenaTextBox = "";
                string RutaArchivo = VentanaCargaArchivo.FileName;
                MessageBox.Show("Ruta Archivo :" + RutaArchivo);
                StreamReader LectorArchivo = new StreamReader(RutaArchivo);
                while ((CadenaAux = LectorArchivo.ReadLine()) != null)
                {
                    CadenaAux += "\n";
                    CadenaTextBox += CadenaAux;
                }
                inFijaTextBox.Text = CadenaTextBox;
            }
        }
        private void BT_LimpiarTX1_Click(object sender, EventArgs e)
        {
            inFijaTextBox.Text = "";
        }



        private void InToPosBoton_Click(object sender, EventArgs e)
        {
            String posFija = "";

            Stack<char> pila = new Stack<char>();


            String infija = inFijaTextBox.Text;
            //infija = desglosaCorchetes(infija);

            //variables de control para el primer while
            Boolean error = false;
            int contadorInfija = 0;
            Boolean band = false;

            if (infija == "")
            {
                error = true;
            }

            while (!error && contadorInfija < infija.Length)
            {
                //char caracter = infija.Substring(contadorInfija, 1);
                char caracter = infija[contadorInfija];

                switch (caracter)
                {
                    case ' '://en caso de ser un espacio.
                        break;
                    case '\n':
                        while(pila.Any())
                        {
                            if (pila.Peek() != '(' && pila.Peek() != ')')
                            {
                                posFija += pila.Pop();
                            }
                            else
                            {
                                pila.Pop();
                            }
                        }
                        break;
                    case '('://parentesis izquierdo.
                        pila.Push(caracter);
                        break;
                    case ')'://parentesis derecho.
                        while (pila.Peek() != '(')
                        {                            
                            posFija += pila.Pop(); ;//despliega en posFija
                        }
                        pila.Pop();//saca el parentesis izquierdo sin desplegarlo.
                        break;
                    default:
                        if (alfabeto.Contains(caracter))//es un operando.
                        {
                            posFija += caracter;//despliega en posFija
                        }
                        else
                        {
                            if (op.Contains(caracter))//es un operador.
                            {
                                band = true;
                                while (band)
                                {
                                    if (!pila.Any() || pila.Peek() == '(' || prioridad(caracter, pila.Peek()))
                                    {
                                        pila.Push(caracter);
                                        band = false;
                                    }
                                    else
                                    {
                                        posFija += pila.Pop();
                                    }
                                }
                            }
                            else//se introdujo algun caracter invalido.
                            {
                                error = true;
                            }
                        }
                        break;
                }
                contadorInfija++;
            }
            posFijaTextBox.Text = posFija;
        }

        private Boolean prioridad(char mayor, char menor)
        {
            if (mayor == menor || op_Presedecia1.Contains(menor))
            {
                return false;
            }
            if (op_Presedecia1.Contains(mayor) && !op_Presedecia1.Contains(menor))
            {
                return true;
            }
            if (op_Presedecia2.Contains(mayor) && !op_Presedecia2.Contains(menor))
            {
                return true;
            }
            if (op_Presedecia3.Contains(menor))
            {
                return true;
            }
            return false;
        }



        /*
        private String desglosaCorchetes(String infija)
        {
            String res;

            List<int> corchetesIniciales = new List<int>();
            List<int> corchetesFinales = new List<int>();

            Boolean salir = false;
            for (int i = s.IndexOf('['); i > -1; i = s.IndexOf('[', i + 1))
            {
                corchetesIniciales.Add(i);
            }
            for (int i = s.IndexOf(']'); i > -1; i = s.IndexOf(']', i + 1))
            {
                corchetesFinales.Add(i);
            }

            if (corchetesFinales.Count() != corchetesFinales.Count())
            {
                return "";
            }

        }
        */
        #endregion

        #region tab_3avance
        #endregion

        #region tab_4avance
        #endregion

        #region tab_5avance
        #endregion

        #region tab_6avance
        #endregion

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }//Forms END
}//namespace END
